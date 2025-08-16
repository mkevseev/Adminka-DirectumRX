using Microsoft.OData.Edm;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Админка_RX.Классы;
using Мониторинг.Resources.Функции;
using static Админка_RX.Классы.MetadataRX;
using static Админка_RX.Классы.Функции;

namespace Админка_RX
{
    public partial class DopChangeForm : Form
    {

        public IDictionary<string, object> Данные = new Dictionary<string, object>();
        public string ОсновнойТип = string.Empty;
        public string Тип = string.Empty;
        public string ОсновноеИмяРеквизита = string.Empty;
        public string Ид = string.Empty;
        public string ИмяРеквизита = string.Empty;
        public static IDictionary<string, object> Реквизиты = new Dictionary<string, object>();
        public ODataClient ODATAClient = ChangePropertyForm.ODATAClient;
        public IEdmModel EDMModel = ChangePropertyForm.EDMModel;

        private IEdmEntityType ТипОбъекта = null;
        private IEdmEntitySet ЭлементEdmМодели = null;

        public DopChangeForm()
        {
            InitializeComponent();
        }

        private void DopChangeForm_Load(object sender, EventArgs e)
        {
            ЗаполнитьТаблицу();
        }

        public void ЗаполнитьТаблицу()
        {
            try
            {
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("Изменить значение ячейки");
                //contextMenu.Items.Add("Вставить");
                // Привязка события для выбора варианта из контекстного меню  
                contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(ContextMenu_ItemClicked);

                List<string> допРеквизиты = new List<string>();
                IDictionary<string, object> всеРеквизиты = new Dictionary<string, object>();

                ЭлементEdmМодели = ПолучитьЭлементEdmМодели(EDMModel, Тип);

                var структурныйТип = ПолучитьСтруктурированныйТип(ЭлементEdmМодели);
                // Получим все имена и типы реквизитов типа сущности для дальнейшего сравнения 
                ТипОбъекта = ЭлементEdmМодели.EntityType();
                var nameType = ТипОбъекта.Name;

                Реквизиты = ПолучитьВсеРеквизитыТипа(структурныйТип, всеРеквизиты);

                // Сначала создадим обязательно колонки
                foreach (var реквизит in Реквизиты)
                {
                    var name = реквизит.Key;

                    var property = ТипОбъекта.FindProperty(name);
                    var типРеквизита = ПолучитьТипРеквизита(property);

                    if (типРеквизита == ТипРеквизита.Обычный)
                    {
                        if (!Таблица.Columns.Contains(name))
                        {
                            if (name != "Id")
                                СоздатьКолонку(Таблица, new DataGridViewTextBoxColumn(), name, name, DataGridViewAutoSizeColumnMode.AllCells, true, false);
                            else
                                СоздатьКолонку(Таблица, new DataGridViewTextBoxColumn(), name, name, DataGridViewAutoSizeColumnMode.AllCells, false, false);
                        }
                    }

                    if (типРеквизита == ТипРеквизита.Сущность)
                    {
                        var NavigProperties = ТипОбъекта.NavigationProperties().Where(s => s.Name == name);
                        if (NavigProperties.Any())
                        {
                            var типСущ = ЭлементEdmМодели.FindNavigationTarget(NavigProperties.First()); // Тип данных

                            //// Удалим из коллекции ссылку на главную сущьность
                            if (типСущ.Name == ОсновнойТип)
                                ОсновноеИмяРеквизита = name;

                            // Создадим колонку
                            if (!Таблица.Columns.Contains(name))
                            {
                                if (типСущ.Name != ОсновнойТип)
                                    СоздатьКолонку(Таблица, new DataGridViewTextBoxColumn(), name, name, DataGridViewAutoSizeColumnMode.AllCells, true, false);
                                else
                                    СоздатьКолонку(Таблица, new DataGridViewTextBoxColumn(), name, name, DataGridViewAutoSizeColumnMode.AllCells, false, false);
                            }

                        }
                    }

                    Таблица.Columns[name].ContextMenuStrip = contextMenu;
                }


                // Получим реквизит и его данные
                Данные = ChangePropertyForm.Данные.Where(z => z.Key == ИмяРеквизита).ToDictionary(k => k.Key, i => i.Value);

                var данные = Данные.First();


                if (данные.Value == null)
                {
                    MessageBox.Show("Нет данных");
                    this.Close();
                    return;
                }


                foreach (var строка in данные.Value as IDictionary<string, object>[])
                {
                    var newСтрока = Таблица.Rows.Add();


                    // Тут перебираются реквизиты
                    foreach (var реквизит in строка)
                    {

                        // Сначала каждая строка (Лист)
                        var name = реквизит.Key;
                        var columIndx = Таблица.Columns[name].Index;
                        var value = реквизит.Value;
                        var cell = Таблица.Rows[newСтрока].Cells[columIndx];
                        // Сразу добавим меню
                        //cell.ContextMenuStrip = contextMenu;

                        var property = ТипОбъекта.FindProperty(name);
                        var типРеквизита = MetadataRX.ПолучитьТипРеквизита(property);

                        if (типРеквизита == MetadataRX.ТипРеквизита.Обычный)
                        {
                            cell.Value = value;
                        }

                        if (типРеквизита == MetadataRX.ТипРеквизита.Сущность)
                        {
                            cell.Value = ((IDictionary<string, object>)value)?["Id"];
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void ИзменитьЗначение()
        {
            var f = Таблица.SelectedCells;

            foreach (DataGridViewCell cell in f)
            {
                int rowInd = cell.RowIndex;
                int colInd = cell.ColumnIndex;
                // Что бы не повторялась строка, а то если выделили много повторяет

                ИзменитьЗначениеРеквизита(rowInd, colInd);
            }

        }

        public async void ИзменитьЗначениеРеквизитаAsync(int rowInd, int colInd)
        {

            await Task.Run(() => ИзменитьЗначениеРеквизита(rowInd, colInd));
        }

        public void ИзменитьЗначениеРеквизита(int rowInd, int colInd)
        {
            try
            {
                var имяРекв = Таблица.Columns[colInd].Name; // Имя реквизита
                                                            // Получим значение выделенной строки
                var значение = Таблица.Rows[rowInd].Cells[colInd].Value; // Значение       

                var property = ТипОбъекта.FindProperty(имяРекв);

                var типРеквизита = ПолучитьТипРеквизита(property);

                if (типРеквизита == ТипРеквизита.Обычный)
                {
                    ТипВвода типВвода = ТипВвода.Все;

                    // Сменим тип ввода
                    ОпределитьТипВвода(((IEdmPrimitiveType)property.Type.Definition).Name, ref типВвода);

                    Диалоги.Показать(имяРекв.ToString(), типВвода, значение, Таблица.Rows[rowInd].Cells[colInd]);
                }

                if (типРеквизита == ТипРеквизита.Сущность)
                {
                    var NavigProperties = ТипОбъекта.NavigationProperties().Where(s => s.Name == имяРекв);

                    if (NavigProperties.Any())
                    {
                        var типСущ = ЭлементEdmМодели.FindNavigationTarget(NavigProperties.First()); // Тип данных

                        using (SelectForm selForm = new SelectForm())
                        {
                            selForm.ТипСущности.Text = типСущ.Name.ToString();
                            selForm.Тип = типСущ.Name.ToString();
                            selForm.ИДЗаписи.Text = Ид;
                            selForm.Ид = Ид;
                            selForm.Cell = Таблица.Rows[rowInd].Cells[colInd];
                            selForm.ShowDialog();
                        }

                        return;

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private async void Изменить_Click(object sender, EventArgs e)
        {
            // Удалим последнюю строку и проставим ИД и Основной реквизит
            ОбработкаСтрок();
            // Передадим данные на уровень ниже.

            var данные = Данные.First(); //.Value as IDictionary<string, object>[];

            var newValue = new List<IDictionary<string, object>>();
            IDictionary<string, object> newРеквизит = new Dictionary<string, object>();

            var rows = Таблица.Rows;

            foreach (DataGridViewRow row in rows)
            {
                IDictionary<string, object> строка = new Dictionary<string, object>();

                // Пропустим последнюю строку
                if (!ПустаяСтрока(row.Index))
                {
                    foreach (var реквизит in Реквизиты)
                    {
                        var name = реквизит.Key;

                        var property = ТипОбъекта.FindProperty(name);

                        var типРеквизита = ПолучитьТипРеквизита(property);
                        // Сразу определим есть ли добавляемые данные
                        if (типРеквизита == ТипРеквизита.Обычный)
                        {
                            var value = row.Cells[name].Value;

                            // Просто вставим значение в новые данные
                            строка.Add(name, value);
                        }

                        if (типРеквизита == ТипРеквизита.Сущность)
                        {
                            var NavigProperties = ТипОбъекта.NavigationProperties().Where(s => s.Name == name);

                            // Если тут, значит должно быть Ид
                            if (NavigProperties.Any())
                            {
                                var типСущ = ЭлементEdmМодели.FindNavigationTarget(NavigProperties.First()); // Тип данных

                                var value = row.Cells[name].Value;


                                // Найдём сущьность
                                if (string.IsNullOrEmpty(типСущ.Name) || value == null)
                                {
                                    строка.Add(name, null);
                                    continue;
                                }


                                var фильтр = $"Id eq {int.Parse(value.ToString())}";

                                var значениеРХ = await DirectumRX.ПолучитьAsync(ODATAClient, типСущ.Name, фильтр);
                                if (значениеРХ != null)
                                {
                                    var сущность = значениеРХ.First();
                                    строка.Add(name, сущность);
                                }

                            }
                        }



                    }

                    newValue.Add(строка);
                }


            }

            // Заменим данные на новые
            KeyValuePair<string, object> keyValuePair = new KeyValuePair<string, object>(данные.Key, (object)newValue.ToArray());
            ChangePropertyForm.Данные.Remove(данные.Key);
            ChangePropertyForm.Данные.Add(keyValuePair);

            this.Close();

        }


        private bool ПустаяСтрока(int index)
        {
            var row = Таблица.Rows[index];

            bool isNull = true;
            for (int i = 0; i < row.Cells.Count; i++)
            {
                var value = row.Cells[i].Value;
                if (value != null)
                    isNull = false;
            }

            return isNull;
        }

        private void ОбработкаСтрок()
        {
            var rows = Таблица.Rows;

            foreach (DataGridViewRow row in rows)
            {
                if (!ПустаяСтрока(row.Index))
                {
                    row.Cells["Id"].Value = row.Cells["Id"].Value ?? -1;
                    row.Cells[ОсновноеИмяРеквизита].Value = row.Cells[ОсновноеИмяРеквизита].Value ?? Ид;
                }
            }


        }


        private void ContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Реализация действий при выборе пункта меню, например, копирование или вставка текста  
            // Произведём выбор строки
            ИзменитьЗначение();
        }

        private void Таблица_MouseDown(object sender, MouseEventArgs e)
        {
            // Выделение ячейки после нажатия ПКМ
            if (e.Button == MouseButtons.Right)
            {
                var f = Таблица.HitTest(e.X, e.Y);

                Таблица.CurrentCell = Таблица[f.ColumnIndex, f.RowIndex];
            }
        }
    }
}

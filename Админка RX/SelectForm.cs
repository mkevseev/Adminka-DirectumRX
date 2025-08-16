using Microsoft.OData.Edm;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Админка_RX.Классы;
using static Админка_RX.Классы.MetadataRX;
using static Админка_RX.Классы.Функции;

namespace Админка_RX
{
    public partial class SelectForm : Form
    {

        public string Тип = string.Empty;
        public string Ид = string.Empty;
        public DataGridViewCell Cell = null;
        private IDictionary<string, object> ЛистФильтра = new Dictionary<string, object>();
        private static IEnumerable<IDictionary<string, object>> Данные { get; set; }
        private IDictionary<string, object> Реквизиты = new Dictionary<string, object>();
        private ODataClient ODATAClient = ChangePropertyForm.ODATAClient;
        private IEdmModel EDMModel = ChangePropertyForm.EDMModel;
        private IEdmEntityType ТипОбъекта = null;
        private IEdmEntitySet ЭлементEdmМодели = null;

        public SelectForm()
        {
            InitializeComponent();
        }

        private void SelectForm_Load(object sender, EventArgs e)
        {
            // Заполняем таблицу данными
            ЗаполнитьТаблицу(null);
        }




        public async void ЗаполнитьТаблицу(string фильтр)
        {
            try
            {
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("Выбрать запись");
                //contextMenu.Items.Add("Вставить");
                // Привязка события для выбора варианта из контекстного меню  
                contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(ContextMenu_ItemClicked);

                //Таблица.Columns[3].ContextMenuStrip = contextMenu;

                bool isFiltr = !string.IsNullOrEmpty(фильтр);
                List<string> допРеквизиты = new List<string>();
                IDictionary<string, object> всеРеквизиты = new Dictionary<string, object>();

                ЭлементEdmМодели = ПолучитьЭлементEdmМодели(EDMModel, Тип);

                var структурныйТип = ПолучитьСтруктурированныйТип(ЭлементEdmМодели);
                // Получим все имена и типы реквизитов типа сущности для дальнейшего сравнения 
                ТипОбъекта = ЭлементEdmМодели.EntityType();
                var nameType = ТипОбъекта.Name;


                Реквизиты = ПолучитьВсеРеквизитыТипа(структурныйТип, всеРеквизиты);

                foreach (var реквизит in Реквизиты)
                    if (!БазовыйТип(реквизит.Value.ToString()) && !допРеквизиты.Contains(реквизит.Key))
                        допРеквизиты.Add(реквизит.Key);

                if (isFiltr)
                    Данные = await DirectumRX.ПолучитьAsync(ODATAClient, Тип, допРеквизиты.ToArray(), 100, фильтр);
                else
                    Данные = await DirectumRX.ПолучитьAsync(ODATAClient, Тип, допРеквизиты.ToArray(), 100);


                if (Данные == null && !isFiltr)
                {
                    MessageBox.Show("Нет данных");
                    this.Close();
                    return;
                }

                if (Данные == null)
                {
                    MessageBox.Show("Нет данных");
                    return;
                }

                foreach (var строка in Данные)
                {

                    if (!Таблица.Columns.Contains("ВременнаяКолонка"))
                        СоздатьКолонку(Таблица, new DataGridViewTextBoxColumn(), "ВременнаяКолонка", "ВременнаяКолонка", DataGridViewAutoSizeColumnMode.None, false, false);
                    if (!isFiltr && !Фильтр.Columns.Contains("ВременнаяКолонка"))
                        СоздатьКолонку(Фильтр, new DataGridViewTextBoxColumn(), "ВременнаяКолонка", "ВременнаяКолонка", DataGridViewAutoSizeColumnMode.None, false, false);

                    var строкаInd = Таблица.Rows.Add();
                    foreach (var line in строка)
                    {

                        var name = line.Key;
                        var value = line.Value;
                        var property = ТипОбъекта.FindProperty(name);
                        var типРеквизита = ПолучитьТипРеквизита(property);

                        if (типРеквизита == ТипРеквизита.Коллекция)
                            continue;

                        if (!Таблица.Columns.Contains(name))
                            СоздатьКолонку(Таблица, new DataGridViewTextBoxColumn(), name, name, DataGridViewAutoSizeColumnMode.None, true, false);


                        var columIndx = Таблица.Columns[name].Index;
                        var cell = Таблица.Rows[строкаInd].Cells[columIndx];

                        // Сразу добавим меню
                        cell.ContextMenuStrip = contextMenu;


                        if (типРеквизита == MetadataRX.ТипРеквизита.Обычный)
                        {

                            cell.Value = line.Value;

                            // Создадим такие же колонки для фильтра
                            if (!isFiltr && !Фильтр.Columns.Contains(name))
                                СоздатьКолонку(Фильтр, new DataGridViewTextBoxColumn(), name, name, DataGridViewAutoSizeColumnMode.None, true, true);

                        }

                        if (типРеквизита == MetadataRX.ТипРеквизита.Сущность)
                        {
                            IDictionary<string, object> val = value as IDictionary<string, object>;
                            if (val != null)
                                cell.Value = val.ContainsKey("Name") ? val["Name"] : val["Id"];



                            // Создадим такие же колонки для фильтра
                            if (!isFiltr && !Фильтр.Columns.Contains(name))
                                СоздатьКолонку(Фильтр, new DataGridViewTextBoxColumn(), name, name, DataGridViewAutoSizeColumnMode.None, true, true);
                        }

                        //if (типРеквизита == MetadataRX.ТипРеквизита.Коллекция)
                        //{

                        //    foreach (var val in ((IDictionary<string, object>[])value))
                        //    {
                        //        cell.Value += string.Join("\n", val.Select(kvp => string.Format("{0}={1}", kvp.Key, kvp.Value == null ? "" : kvp.Value.ToString())));
                        //    }
                        //}

                    }

                }

                // Удалим временную колонку
                if (Таблица.Columns.Contains("ВременнаяКолонка"))
                    Таблица.Columns.Remove("ВременнаяКолонка");

                // Удалим временную колонку
                if (!isFiltr && Фильтр.Columns.Contains("ВременнаяКолонка"))
                    Фильтр.Columns.Remove("ВременнаяКолонка");

                if (!isFiltr)
                    // Добавим одну строку в фильтр
                    Фильтр.Rows.Add();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }

        }

        private void Выбрать_Click(object sender, EventArgs e)
        {
            var selects = Таблица.SelectedCells;
            var rowsInd = new List<int>();

            foreach (DataGridViewCell cell in selects)
            {
                if (Таблица.Columns.Contains("Id"))
                {
                    var columIndx = Таблица.Columns["Id"].Index;
                    int rowInd = cell.RowIndex;

                    Cell.Value = Таблица.Rows[rowInd].Cells[columIndx].Value;
                    // Берём первую, потом всё
                    this.Close();
                    continue;
                }

            }

        }

        private void Таблица_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                Фильтр.HorizontalScrollingOffset = e.NewValue;
            }
        }

        private void Фильтр_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                Таблица.HorizontalScrollingOffset = e.NewValue;
            }
        }


        private void Фильтр_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int indColumn = e.ColumnIndex;
            int indRow = e.RowIndex;
            СобратьФильтр(indColumn, indRow);
        }

        private void СобратьФильтр(int indColumn, int indRow)
        {
            string key = Фильтр.Columns[indColumn].Name;
            bool isAny = ЛистФильтра.ContainsKey(key);
            object value = Фильтр.Rows[indRow].Cells[indColumn].Value;
            bool change = isAny && ЛистФильтра.TryGetValue(key, out object oldValue) && value != null && oldValue != value;

            if (change)
            {
                // Удаляем старое значение
                ЛистФильтра.Remove(key);
                ЛистФильтра.Add(key, value);
            }

            // Если найдено в листе и значение null
            if (isAny && value == null)
            {
                // Удаляем старое значение
                ЛистФильтра.Remove(key);
                return;
            }

            // Если не найдено в листе и значение не null
            if (!isAny && value != null)
            {
                ЛистФильтра.Add(key, value);
            }

            var фильтр = new List<string>();

            // Соберём строку фильтра
            foreach (var line in ЛистФильтра)
            {
                var name = line.Key;
                var val = line.Value;

                var property = ТипОбъекта.FindProperty(name);

                var типРеквизита = MetadataRX.ПолучитьТипРеквизита(property);

                if (типРеквизита == MetadataRX.ТипРеквизита.Обычный)
                {
                    MetadataRX.ТипВвода типВвода = MetadataRX.ТипВвода.Все;
                    MetadataRX.ОпределитьТипВвода(((IEdmPrimitiveType)property.Type.Definition).Name, ref типВвода);
                    //// Дата
                    //if (типВвода == MetadataRX.ТипВвода.DateTimeOffset)
                    //{
                    //    // Дата обрабатывается отдельно   и Enumerlable не работал при фильтре contains({0},'{1}') нужно всё расписать по одному для контроля

                    //}
                    //else
                    // Строка
                    фильтр.Add($"{string.Format("contains({0},'{1}')", name, val)}");




                }

                if (типРеквизита == MetadataRX.ТипРеквизита.Сущность)
                {
                    bool isID = true;
                    if (val != null)
                    {
                        foreach (char c in val.ToString())
                        {
                            if (!char.IsDigit(c))
                                isID = false;
                        }
                    }
                    // Если ИД
                    if (isID)
                    {
                        фильтр.Add($"{string.Format("{0}/{1} eq {2}", name, "Id", val)}");
                    }
                    else
                    // А иначе Имя
                    {
                        фильтр.Add($"{string.Format("contains({0}/{1},'{2}')", name, "Name", val)}");
                    }

                }

            }

            ФильтрТекст.Text = string.Join(" and ", фильтр);

        }

        private void ФильтрТекст_TextChanged(object sender, EventArgs e)
        {
            var фильтр = ((TextBox)sender).Text;

            if (string.IsNullOrEmpty(фильтр))
                return;

            Таблица.DataSource = null;
            Таблица.Rows.Clear();
            Таблица.Columns.Clear();

            Таблица.Refresh();

            // Заполняем таблицу данными
            ЗаполнитьТаблицу(фильтр);


        }

        private void SelectForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Данные = null;
            Таблица.Dispose();
        }

        private void Таблица_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Произведём выбор строки
            Выбрать_Click(sender, e);
        }

        private void ContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Реализация действий при выборе пункта меню, например, копирование или вставка текста  
            // Произведём выбор строки
            Выбрать_Click(sender, e);
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

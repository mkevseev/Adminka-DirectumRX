using Microsoft.OData.Edm;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Админка_RX.Классы;
using Мониторинг.Resources.Функции;
using static Админка_RX.Классы.MetadataRX;
using static Админка_RX.Классы.Функции;

namespace Админка_RX
{
    public partial class ChangePropertyForm : Form
    {
        public string Ид = string.Empty;
        public string Тип = string.Empty;
        public static ODataClient ODATAClient = MainForm.ODATAClient;
        public static IEdmModel EDMModel = MainForm.EDMModel;
        public static List<IEdmProperty> ВсеРеквизиты = new List<IEdmProperty>();
        public static List<IEdmNavigationProperty> НавигационныеРеквизиты = new List<IEdmNavigationProperty>();
        public List<string> ИменаНавигРеквиз = new List<string>();
        public static IDictionary<string, object> Данные = new Dictionary<string, object>();
        private IEdmEntityType ТипОбъекта = null;
        private IEdmEntitySet ЭлементEdmМодели = null;



        public List<string> ТипыСущностей = MainForm.ТипыСущностей;
        
        private bool newOpen = true;

        public ChangePropertyForm()
        {
            InitializeComponent();

        }

        private void ChangePropertyForm_Load(object sender, EventArgs e)
        {
            ЗаполнитьТаблицу();

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Изменить значение");
            //contextMenu.Items.Add("Вставить");
            // Привязка события для выбора варианта из контекстного меню  
            contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(ContextMenu_ItemClicked);

            Таблица.Columns[4].ContextMenuStrip = contextMenu;

        }

        public async void ЗаполнитьТаблицу()
        {
            try
            {
                ЭлементEdmМодели = ПолучитьЭлементEdmМодели(EDMModel, Тип);
                ТипОбъекта = ЭлементEdmМодели.EntityType();
                ВсеРеквизиты = ТипОбъекта.Properties().ToList();
                НавигационныеРеквизиты = ТипОбъекта.NavigationProperties().ToList();
                ИменаНавигРеквиз = НавигационныеРеквизиты.Select(i => i.Name).ToList();

                var фильтр = $"Id eq {int.Parse(Ид)}";

                // Создадим колонки
                СоздатьКолонки();

                if (ИменаНавигРеквиз.Any())
                {
                    var записи = await DirectumRX.ПолучитьAsync(ODATAClient, Тип, фильтр, ИменаНавигРеквиз.ToArray());
                    Данные = записи.First();
                }
                else
                {
                    var записи = await DirectumRX.ПолучитьAsync(ODATAClient, Тип, фильтр);
                    Данные = записи.First();
                }

                foreach (var line in Данные)
                {
                    var name = line.Key;
                    var value = line.Value;

                    var property = ВсеРеквизиты.Where(r => r.Name == name).First();

                    var типРеквизита = ПолучитьТипРеквизита(property);

                    if (типРеквизита == ТипРеквизита.Обычный)
                    {
                        if (property.IsKey())
                            continue;

                        var имяТипа = ((IEdmPrimitiveType)property.Type.Definition).Name;

                        var строка = Таблица.Rows.Add();

                        Таблица.Rows[строка].Cells[0].Value = имяТипа;   // Тип реквизита
                        Таблица.Rows[строка].Cells[1].Value = имяТипа; // Тип
                        Таблица.Rows[строка].Cells[2].Value = property.Name;   // Имя реквизита
                        Таблица.Rows[строка].Cells[3].Value = DirectumRX.РусскоеИмяРеквизита(EDMModel, name, имяТипа.ToString(), Тип);   // Русское имя реквизита
                        Таблица.Rows[строка].Cells[4].Value = value; // Тут всегда обычные реквизиты

                    }

                    if (типРеквизита == ТипРеквизита.Сущность || типРеквизита == ТипРеквизита.Коллекция)
                    {
                        var navProperty = (IEdmNavigationProperty)property;
                        var имяТипа = navProperty.ToEntityType().Name;
                        var типСущ = ЭлементEdmМодели.FindNavigationTarget(navProperty); // Тип данных

                        var строка = Таблица.Rows.Add();

                        Таблица.Rows[строка].Cells[0].Value = имяТипа;   // Тип реквизита
                        Таблица.Rows[строка].Cells[1].Value = типСущ.Name; // Тип
                        Таблица.Rows[строка].Cells[2].Value = property.Name;   // Имя реквизита
                        Таблица.Rows[строка].Cells[3].Value = DirectumRX.РусскоеИмяРеквизита(EDMModel, name, имяТипа.ToString(), Тип);   // Русское имя реквизита
                        Таблица.Rows[строка].Cells[4].Value = типРеквизита == ТипРеквизита.Сущность ? ((IDictionary<string, object>)value)?["Id"] : "[Просмотр при изменении значений]"; // Значение

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            newOpen = false;
        }



        private void ИзменитьЗначение()
        {
            var f = Таблица.SelectedCells;
            var rowsInd = new List<int>();

            foreach (DataGridViewCell cell in f)
            {
                int rowInd = cell.RowIndex;
                // Что бы не повторялась строка, а то если выделили много повторяет
                if (rowsInd.Contains(rowInd))
                    continue;

                rowsInd.Add(rowInd);

                ИзменитьЗначениеРеквизита(rowInd);
            }

        }

        public async void ИзменитьЗначениеРеквизитаAsync(int rowInd)
        {

            await Task.Run(() => ИзменитьЗначениеРеквизита(rowInd));
        }

        public void ИзменитьЗначениеРеквизита(int rowInd)
        {
            try
            {

                // Получим значение выделенной строки
                var типРекв = Таблица.Rows[rowInd].Cells[0].Value; // Тип реквизита
                var типСущ = Таблица.Rows[rowInd].Cells[1].Value; // Тип
                var имяРекв = Таблица.Rows[rowInd].Cells[2].Value; // Имя реквизита
                var значение = Таблица.Rows[rowInd].Cells[4].Value; // Значение                



                var property = ТипОбъекта.FindProperty(имяРекв.ToString());

                var типРеквизита = ПолучитьТипРеквизита(property);

                if (типРеквизита == ТипРеквизита.Обычный)
                {
                    ТипВвода типВвода = ТипВвода.Все;

                    // Сменим тип ввода
                    ОпределитьТипВвода(типСущ.ToString(), ref типВвода);

                    Диалоги.Показать(имяРекв.ToString(), типВвода, значение, Таблица.Rows[rowInd].Cells[4]);


                }

                if (типРеквизита == ТипРеквизита.Сущность)
                {
                    using (SelectForm selForm = new SelectForm())
                    {
                        selForm.ТипСущности.Text = типСущ.ToString();
                        selForm.Тип = типСущ.ToString();
                        selForm.ИДЗаписи.Text = Ид;
                        selForm.Ид = Ид;
                        selForm.Cell = Таблица.Rows[rowInd].Cells[4];
                        selForm.ShowDialog();
                    }


                    return;
                }

                if (типРеквизита == ТипРеквизита.Коллекция)
                {
                    using (DopChangeForm changeForm = new DopChangeForm())
                    {
                        changeForm.ТипСущности.Text = типСущ.ToString();
                        changeForm.Тип = типСущ.ToString();
                        changeForm.ОсновнойТип = Тип;
                        changeForm.ИДЗаписи.Text = Ид;
                        changeForm.Ид = Ид;
                        changeForm.ИмяРеквизита = имяРекв.ToString();
                        changeForm.Text += " " + имяРекв.ToString();

                        changeForm.ShowDialog();
                    }


                    return;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }






        private void СоздатьКолонки()
        {

            СоздатьКолонку(Таблица, new DataGridViewTextBoxColumn(), "ТипРеквизита", "Тип реквизита", DataGridViewAutoSizeColumnMode.AllCells, false, false);
            СоздатьКолонку(Таблица, new DataGridViewTextBoxColumn(), "Тип", "Тип", DataGridViewAutoSizeColumnMode.AllCells, true, false);
            СоздатьКолонку(Таблица, new DataGridViewTextBoxColumn(), "ИмяРекв", "Имя реквизита", DataGridViewAutoSizeColumnMode.AllCells, true, false);
            СоздатьКолонку(Таблица, new DataGridViewTextBoxColumn(), "РусИмяРекв", "Русское имя реквизита", DataGridViewAutoSizeColumnMode.AllCells, true, false);
            СоздатьКолонку(Таблица, new DataGridViewTextBoxColumn(), "Значение", "Значение", DataGridViewAutoSizeColumnMode.Fill, true, false);

        }


        private async void Изменить_Click(object sender, EventArgs e)
        {
            var изменения = await DirectumRX.Проверить(ODATAClient, EDMModel, ТипыСущностей, Тип, Ид, ИменаНавигРеквиз.ToArray(), Данные);

            try
            {
                DirectumRX.ИзменитьПакетно(ODATAClient, EDMModel, Тип, Ид, ИменаНавигРеквиз.ToArray(), изменения);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void ContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Реализация действий при выборе пункта меню, например, копирование или вставка текста  
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

        private async void Таблица_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var columInd = e.ColumnIndex;
            var rowInd = e.RowIndex;

            // Всегда только если изменяется колонка со значением
            if (columInd == 4 && !newOpen)
            {
                try
                {


                    // Получим значение изменённой строки
                    var типРекв = Таблица.Rows[rowInd].Cells[0].Value; // Тип реквизита
                    var типСущ = Таблица.Rows[rowInd].Cells[1].Value; // Тип
                    var имяРекв = Таблица.Rows[rowInd].Cells[2].Value; // Имя реквизита
                    var значение = Таблица.Rows[rowInd].Cells[4].Value; // Значение                
                    var name = имяРекв.ToString();


                    var property = ТипОбъекта.FindProperty(name);

                    var типРеквизита = ПолучитьТипРеквизита(property);

                    if (типРеквизита == ТипРеквизита.Обычный)
                    {
                        // Заменим данные на новые
                        KeyValuePair<string, object> keyValuePair = new KeyValuePair<string, object>(name, значение);
                        Данные.Remove(name);
                        Данные.Add(keyValuePair);
                        return;
                    }

                    if (типРеквизита == ТипРеквизита.Сущность)
                    {

                        // Найдём сущьность
                        if (значение == null)
                        {
                            // Заменим данные на новые
                            KeyValuePair<string, object> keyValuePair = new KeyValuePair<string, object>(name, значение);
                            Данные.Remove(name);
                            Данные.Add(keyValuePair);
                            return;
                        }


                        var фильтр = $"Id eq {int.Parse(значение.ToString())}";

                        var значениеРХ = await DirectumRX.ПолучитьAsync(ODATAClient, типСущ.ToString(), фильтр);
                        if (значениеРХ != null)
                        {
                            var сущность = значениеРХ.First();

                            // Заменим данные на новые
                            KeyValuePair<string, object> keyValuePair = new KeyValuePair<string, object>(name, сущность);
                            Данные.Remove(name);
                            Данные.Add(keyValuePair);
                            return;
                        }


                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }


        }
    }
}

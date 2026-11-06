using Microsoft.OData.Edm;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public IDictionary<string, string> ВариантыИсторииРабот = new Dictionary<string, string>();
        public DataGridViewCell Cell = null;
        public static List<IEdmProperty> ВсеРеквизиты = new List<IEdmProperty>();
        public static List<IEdmNavigationProperty> НавигационныеРеквизиты = new List<IEdmNavigationProperty>();
        public List<string> ИменаНавигРеквиз = new List<string>();
        private static IEnumerable<IDictionary<string, object>> Данные { get; set; }
        private IEdmEntityType ТипОбъекта = null;
        private IEdmEntitySet ЭлементEdmМодели = null;
        private ODataClient ODATAClient = ChangePropertyForm.ODATAClient;
        private IEdmModel EDMModel = ChangePropertyForm.EDMModel;
        private IDictionary<string, object> ЛистФильтра = new Dictionary<string, object>();
        public ТипЗапускаФормы ТипЗапуска;



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
                // создаем элементы меню
                ToolStripMenuItem MassiveFillMenuItem = new ToolStripMenuItem("Массово заполнить из...");
                contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(ContextMenu_ItemClicked);

                ToolStripMenuItem createMenuItem = new ToolStripMenuItem("Изменить");
                createMenuItem.Click += CreateMenuItem_Click;

                ToolStripMenuItem whereUsedMenuItem = new ToolStripMenuItem("Где используется");
                whereUsedMenuItem.Click += WhereUsedMenuItem_Click;

                if (ТипЗапуска == ТипЗапускаФормы.ИзменениеРеквизитов)
                {
                    contextMenu.Items.Add("Выбрать запись");
                    //contextMenu.Items.Add("Вставить");
                    // Привязка события для выбора варианта из контекстного меню  
                }
                else if (ТипЗапуска == ТипЗапускаФормы.МассовоеИзменениеРеквизитов)
                {
                    Выбрать.Visible = false;                   

                    // добавляем элементы в меню
                    contextMenu.Items.AddRange(new[] { MassiveFillMenuItem, createMenuItem , whereUsedMenuItem });
                }
                else if (ТипЗапуска == ТипЗапускаФормы.ИсторияРаботы)
                {
                    Выбрать.Visible = false;
                    if (!ЛистФильтра.ContainsKey("UserId"))
                        ЛистФильтра.Add("UserId", long.Parse(Ид));

                    var startDate = DateTime.UtcNow.AddDays(-1);
                    var endDate = DateTime.UtcNow; //new DateTime(2026, 10, 29, 0, 0, 0, DateTimeKind.Utc); // верхняя граница (не включительно)

                    // Важно: ToString("o") даёт формат ISO 8601
                    var filter = $"HistoryDate ge {startDate.ToString("o")} and HistoryDate lt {endDate.ToString("o")}";


                    if (string.IsNullOrEmpty(фильтр))
                        фильтр = $"UserId eq {long.Parse(Ид)} and {filter}";
                }
                else if (ТипЗапуска == ТипЗапускаФормы.Просмотр)
                {
                    Выбрать.Visible = false;

                    // добавляем элементы в меню
                    contextMenu.Items.AddRange(new[] { createMenuItem , whereUsedMenuItem });
                }


                bool isFiltr = !string.IsNullOrEmpty(фильтр);

                ЭлементEdmМодели = ПолучитьЭлементEdmМодели(EDMModel, Тип);
                ТипОбъекта = ЭлементEdmМодели.EntityType();
                ВсеРеквизиты = ТипОбъекта.Properties().ToList();
                НавигационныеРеквизиты = ТипОбъекта.NavigationProperties().ToList();
                ИменаНавигРеквиз = НавигационныеРеквизиты.Select(i => i.Name).ToList();

                if (isFiltr)
                    Данные = await DirectumRX.ПолучитьПакетноAsync(ODATAClient, Тип, ИменаНавигРеквиз.ToArray(), фильтр);
                else
                    Данные = await DirectumRX.ПолучитьПакетноAsync(ODATAClient, Тип, ИменаНавигРеквиз.ToArray());


                if (Данные == null && !isFiltr)
                {
                    MessageBox.Show("Нет данных");
                    this.Close();
                    return;
                }

                if (Данные == null || Данные.Count() == 0)
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
                        {
                            СоздатьКолонку(Таблица, new DataGridViewTextBoxColumn(), name, name, DataGridViewAutoSizeColumnMode.None, true, false);

                            if (ТипЗапуска == ТипЗапускаФормы.МассовоеИзменениеРеквизитов)
                            {
                                ToolStripMenuItem menuItem = new ToolStripMenuItem(name);
                                MassiveFillMenuItem.DropDownItems.Add(menuItem);

                                // Получим все реквизиты 
                                if (типРеквизита == MetadataRX.ТипРеквизита.Сущность)
                                {
                                    var NavigProperties = ТипОбъекта.NavigationProperties().Where(s => s.Name == name);

                                    var типСущ = ЭлементEdmМодели.FindNavigationTarget(NavigProperties.First()); // Тип данных

                                    var propertys = MetadataRX.ПолучитьВсеРеквизитыТипа(типСущ.EntityType(), new Dictionary<string, object>());

                                    foreach (var proper in propertys)
                                    {
                                        ToolStripMenuItem menuDropItem = new ToolStripMenuItem(proper.Key);
                                        menuItem.DropDownItems.Add(menuDropItem);
                                        // устанавливаем обработчики событий для меню
                                        menuDropItem.Click += MassiveFillContextMenu_Click;
                                    }

                                }
                                else
                                {
                                    // устанавливаем обработчики событий для меню
                                    menuItem.Click += MassiveFillContextMenu_Click;
                                }
                            }

                        }

                        var columIndx = Таблица.Columns[name].Index;
                        var cell = Таблица.Rows[строкаInd].Cells[columIndx];

                        // Сразу добавим меню
                        cell.ContextMenuStrip = contextMenu;

                        if (типРеквизита == MetadataRX.ТипРеквизита.Обычный)
                        {
                            cell.Value = value;

                            // Создадим такие же колонки для фильтра
                            if ((!isFiltr || ТипЗапуска == ТипЗапускаФормы.ИсторияРаботы) && !Фильтр.Columns.Contains(name))
                                СоздатьКолонку(Фильтр, new DataGridViewTextBoxColumn(), name, name, DataGridViewAutoSizeColumnMode.None, true, false);

                        }

                        if (типРеквизита == MetadataRX.ТипРеквизита.Сущность)
                        {
                            IDictionary<string, object> val = value as IDictionary<string, object>;
                            if (val != null)
                                cell.Value = val.ContainsKey("Name") ? val["Name"] : val["Id"];

                            // Создадим такие же колонки для фильтра
                            if ((!isFiltr || ТипЗапуска == ТипЗапускаФормы.ИсторияРаботы) && !Фильтр.Columns.Contains(name))
                                СоздатьКолонку(Фильтр, new DataGridViewTextBoxColumn(), name, name, DataGridViewAutoSizeColumnMode.None, true, false);
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

                if ((!isFiltr || ТипЗапуска == ТипЗапускаФормы.ИсторияРаботы) && Фильтр.Rows.Count == 0 && Таблица.Rows.Count > 0)
                    // Добавим одну строку в фильтр
                    Фильтр.Rows.Add();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }

        }

        private void WhereUsedMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #region Изменение реквизитов

        private void Выбрать_Click(object sender, EventArgs e)
        {
            if (ТипЗапуска != ТипЗапускаФормы.ИсторияРаботы)
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


        }

        #endregion

        #region Массовое заполнение

        private void CreateMenuItem_Click(object sender, EventArgs e)
        {
            var selects = Таблица.SelectedCells;
            var rowsInd = new List<int>();

            foreach (DataGridViewCell cell in selects)
            {
                if (Таблица.Columns.Contains("Id"))
                {
                    var columIndx = Таблица.Columns["Id"].Index;
                    int rowInd = cell.RowIndex;

                    using (ChangePropertyForm propertyForm = new ChangePropertyForm())
                    {
                        propertyForm.ТипСущности.Text = Тип;
                        propertyForm.Тип = Тип;
                        propertyForm.ИДЗаписи.Text = Таблица.Rows[rowInd].Cells[columIndx].Value.ToString();
                        propertyForm.Ид = Таблица.Rows[rowInd].Cells[columIndx].Value.ToString();
                        propertyForm.ShowDialog();

                    }
                    ;
                    // Берём первую, потом всё

                    continue;
                }

            }


        }

        private void MassiveFillContextMenu_Click(object sender, EventArgs e)
        {

            var menu = (ToolStripMenuItem)sender;
            var nameProperty = menu.Text;


            var ownerItem = menu.OwnerItem;

            // Если нет родительского меню, то обычный реквизит
            if (ownerItem == null)
            {
                // Получим реквизит
                var property = ТипОбъекта.FindProperty(nameProperty);
                var типРеквизита = MetadataRX.ПолучитьТипРеквизита(property);

                var type = ((IEdmPrimitiveType)property.Type.Definition).Name; // Тип
                var d = type;
            }
            else
            {
                var namePropertyOwner = ownerItem.Text;

                var selCells = Таблица.SelectedCells;
                int colInd = -1;

                foreach (DataGridViewCell cell in selCells)
                {
                    int rowInd = cell.RowIndex;
                    // Определим какую калонку изменяем
                    colInd = cell.ColumnIndex;

                }

                var cellName = Таблица.Columns[colInd].Name;

                // Получим реквизит
                var propertyOwner = ТипОбъекта.FindProperty(namePropertyOwner);
                var типРеквизитаOwner = MetadataRX.ПолучитьТипРеквизита(propertyOwner);


                // Не должно быть
                if (типРеквизитаOwner == MetadataRX.ТипРеквизита.Обычный)
                    return;
                if (типРеквизитаOwner == MetadataRX.ТипРеквизита.Коллекция)
                    return;




                if (типРеквизитаOwner == MetadataRX.ТипРеквизита.Сущность)
                {
                    var NavigProperties = ТипОбъекта.NavigationProperties().Where(s => s.Name == namePropertyOwner);

                    var типСущ = ЭлементEdmМодели.FindNavigationTarget(NavigProperties.First()); // Тип данных

                    // Реализация действий при выборе пункта меню
                    if (Таблица.Columns.Contains("Id") && colInd != -1)
                    {
                        var columIdIndx = Таблица.Columns["Id"].Index;

                        foreach (DataGridViewRow row in Таблица.Rows)
                        {
                            var id = row.Cells[columIdIndx].Value.ToString();
                            ИзменитьЗаписьСущностиAsync(id, namePropertyOwner, типСущ.Name, nameProperty, cellName);
                        }


                    }

                    var ff = типСущ;

                }

            }

        }

        private void ИзменитьЗаписьСущностиAsync(string id, string namePropertyOwner, string типСущ, string nameProperty, string cellName)
        {

            Task.Run(() => ИзменитьЗаписьСущности(id, namePropertyOwner, типСущ, nameProperty, cellName));   // выполняется асинхронно

        }

        async void ИзменитьЗаписьСущности(string id, string namePropertyOwner, string типСущ, string nameProperty, string cellName)
        {
            var записи = await DirectumRX.ПолучитьAsync(ODATAClient, Тип, $"Id eq {long.Parse(id)}", ИменаНавигРеквиз.ToArray());

            if (записи != null)
            {
                var запись = записи.First();
                var реквизиты = запись.Where(r => r.Key == namePropertyOwner);
                if (реквизиты.Any())
                {
                    var реквизит = реквизиты.First();

                    if (реквизит.Value != null)
                    {
                        var записьЗамена = ((IDictionary<string, object>)реквизит.Value)[nameProperty];
                        await DirectumRX.Изменить(ODATAClient, Тип, id, new Dictionary<string, object> { { cellName, записьЗамена } });
                    }
                }

            }

            //

        }

        #endregion

        #region Общие

        #region Фильтр


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
                //return;
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
                    фильтр.Add($"{DirectumRX.ПолучитьСтрокуФильтра(типВвода, name, val)}");

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
            {
                Фильтр.DataSource = null;
                Фильтр.Rows.Clear();
                Фильтр.Columns.Clear();
                Фильтр.Refresh();
            }

            Таблица.DataSource = null;
            Таблица.Rows.Clear();
            Таблица.Columns.Clear();

            Таблица.Refresh();

            // Заполняем таблицу данными
            ЗаполнитьТаблицу(фильтр);

        }

        #endregion

        private void Таблица_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                Фильтр.HorizontalScrollingOffset = e.NewValue;
            }
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

        #endregion


    }
}

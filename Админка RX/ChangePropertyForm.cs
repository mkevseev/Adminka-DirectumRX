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
        public ТипЗапускаФормы ТипЗапускаФормы;
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

            //ContextMenuStrip contextMenu = new ContextMenuStrip();
            //contextMenu.Items.Add("Изменить значение");
            ////contextMenu.Items.Add("Вставить");
            //// Привязка события для выбора варианта из контекстного меню  
            //contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(ContextMenu_ItemClicked);

            //Таблица.Columns[4].ContextMenuStrip = contextMenu;

        }

        public async void ЗаполнитьТаблицу()
        {
            try
            {
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("Изменить значение ячейки");
                //contextMenu.Items.Add("Вставить");
                // Привязка события для выбора варианта из контекстного меню  
                contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(ContextMenu_ItemClicked);



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

                        var имяТипа = ПолучитьИмяТипаРеквизита(property);

                        Функции.ДобавитьСтроку(Таблица, имяТипа, имяТипа, name, value, Тип);

                    }

                    if (типРеквизита == ТипРеквизита.Сущность || типРеквизита == ТипРеквизита.Коллекция)
                    {
                        var navProperty = (IEdmNavigationProperty)property;
                        var имяТипа = navProperty.ToEntityType().Name;
                        var типСущ = ЭлементEdmМодели.FindNavigationTarget(navProperty); // Тип данных
                        var значение = типРеквизита == ТипРеквизита.Сущность ? ((IDictionary<string, object>)value)?["Id"] : "[Просмотр при изменении значений]"; // Значение

                        Функции.ДобавитьСтроку(Таблица, имяТипа, типСущ.Name, name, значение, Тип);

                    }

                }

                Таблица.Columns[4].ContextMenuStrip = contextMenu;


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

                    Диалоги.Выбрать("Выбор", типВвода, значение, Таблица.Rows[rowInd].Cells[4]);


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
            var изменения = await Проверить(ODATAClient, EDMModel, ТипыСущностей, Тип, Ид, ИменаНавигРеквиз.ToArray(), Данные);

            try
            {
                
                DirectumRX.ИзменитьПакетно(ODATAClient, /*EDMModel,*/ Тип, Ид, ИменаНавигРеквиз.ToArray(), Данные);
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



        #region Проверка

        public async Task<IDictionary<string, object>> Проверить(ODataClient oDataClient, IEdmModel edmModel, List<string> типыСущностей, string Имя, string ИД, string[] ДопРеквизиты, IDictionary<string, object> Изменения)
        {
            if (string.IsNullOrEmpty(ИД))
                return null;

            IDictionary<string, object> изменения = new Dictionary<string, object>();

            //var элементEdmМодели = MetadataRX.ПолучитьЭлементEdmМодели(edmModel, Имя);

            //var структурныйТип = MetadataRX.ПолучитьСтруктурированныйТип(элементEdmМодели);
            //// Получим все имена и типы реквизитов типа сущности для дальнейшего сравнения 
            //var типОбъекта = элементEdmМодели.EntityType();
            //var nameType = типОбъекта.Name;

            //IDictionary<string, object> всеРеквизиты = new Dictionary<string, object>();
            //var реквизиты = MetadataRX.ПолучитьВсеРеквизитыТипа(структурныйТип, всеРеквизиты);

            // Перепроверим все реквизиты на сходство.
            // Изменяем только если не равны значения
            // Получим сущьность
            var данные = await DirectumRX.ПолучитьAsync(oDataClient, Имя, ДопРеквизиты, ИД);
            var запись = данные.First();
            if (запись.Any())
            {

                foreach (var item in Изменения)
                {
                    try
                    {

                        var name = item.Key;



                        var property = ТипОбъекта.FindProperty(name);
                        var старыеДанныеРеквизита = запись.Where(r => r.Key == name).First();

                        var типРеквизита = MetadataRX.ПолучитьТипРеквизита(property);

                        if (типРеквизита == MetadataRX.ТипРеквизита.Обычный)
                        {
                            var val = item.Value;
                            var oldVal = старыеДанныеРеквизита.Value;

                            // Всегда был ноль, значит равны, по другому не хочет 
                            if (oldVal == null && val == null)
                                continue;
                            // Старое значение null, а новое нет, тогда запишем на изменение
                            if (oldVal == null && val != null)
                            {
                                // Запишем на изменение
                                изменения.Add(item.Key, val);

                            }
                            // Старое значение null, а новое нет, тогда запишем на изменение
                            else if (oldVal != null && val == null)
                            {
                                // Запишем на изменение
                                изменения.Add(item.Key, val);

                            }
                            else if (oldVal.ToString() != val.ToString())
                            {
                                // Есди оба были не пустые, то запишем при расхождению
                                // Запишем на изменение
                                изменения.Add(item.Key, val);

                            }

                        }

                        if (типРеквизита == MetadataRX.ТипРеквизита.Сущность)
                        {

                            var val = item.Value;
                            var oldVal = старыеДанныеРеквизита.Value;

                            // Всегда был ноль, значит равны, по другому не хочет 
                            if (oldVal == null && val == null)
                                continue;
                            // Старое значение null, а новое нет, тогда запишем на изменение
                            if (oldVal == null && val != null)
                            {
                                // Запишем на изменение
                                изменения.Add(item.Key, val);

                            }
                            // Старое значение null, а новое нет, тогда запишем на изменение
                            else if (oldVal != null && val == null)
                            {
                                // Запишем на изменение
                                изменения.Add(item.Key, val);

                            }
                            else
                            {
                                // Есди оба были не пустые, то запишем при расхождению
                                var id = ((IDictionary<string, object>)val)["Id"];
                                var idOld = ((IDictionary<string, object>)oldVal)["Id"];
                                if (!idOld.Equals(id))
                                {
                                    // Запишем на изменение
                                    изменения.Add(item.Key, val);
                                }
                            }

                        }

                        if (типРеквизита == MetadataRX.ТипРеквизита.Коллекция)
                        {
                            var типСущ = ЭлементEdmМодели.FindNavigationTarget(property as IEdmNavigationProperty); // Тип данных

                            var элементEdmМоделиКоллекции = MetadataRX.ПолучитьЭлементEdmМодели(edmModel, типСущ.Name);

                            var структурныйТипКоллекции = MetadataRX.ПолучитьСтруктурированныйТип(элементEdmМоделиКоллекции);
                            // Получим все имена и типы реквизитов типа сущности для дальнейшего сравнения 
                            var типОбъектаКоллекции = элементEdmМоделиКоллекции.EntityType();
                            var nameTypeКоллекции = типОбъектаКоллекции.Name;

                            IDictionary<string, object> всеРеквизитыКоллекции = new Dictionary<string, object>();
                            var реквизитыКоллекции = MetadataRX.ПолучитьВсеРеквизитыТипа(структурныйТипКоллекции, всеРеквизитыКоллекции);


                            var arrs = (IDictionary<string, object>[])item.Value;
                            var arrsOld = (IDictionary<string, object>[])старыеДанныеРеквизита.Value;
                            if (arrs != null)
                            {
                                var newArrs = new List<IDictionary<string, object>>();
                                foreach (var arr in arrs)
                                {
                                    long idstr = 0;

                                    IDictionary<string, object> newArr = new Dictionary<string, object>();
                                    // Или ИД или -1 у новой строки коллекции обработка -1 в изменениях
                                    idstr = long.Parse(arr["Id"].ToString());

                                    bool writeId = false;

                                    // Не новое значение. Новое всегда записываем на изменнение
                                    if (idstr != -1)
                                    {
                                        foreach (var it in arr)
                                        {
                                            bool change = false;
                                            var реквизитКоллекции = реквизитыКоллекции.Where(r => r.Key == it.Key).First();

                                            var NavigPropertiesКоллекции = типОбъектаКоллекции.NavigationProperties().Where(s => s.Name == it.Key);

                                            if (NavigPropertiesКоллекции.Any())
                                            {
                                                var val = it.Value;


                                                var whe = arrsOld.Where(i => i["Id"].Equals(idstr)); // && !((IDictionary<string, object>)i[it.Key])["Id"].Equals(id)


                                                if (whe.Any())
                                                {
                                                    var oldVal = whe.Select(i => i[it.Key]).First();

                                                    // Всегда был ноль, значит равны, по другому не хочет 
                                                    if (oldVal == null && val == null)
                                                        continue;
                                                    // Старое значение null, а новое нет, тогда запишем на изменение
                                                    if (oldVal == null && val != null)
                                                    {
                                                        // Запишем на изменение
                                                        newArr.Add(it.Key, val);
                                                        change = true;

                                                    }
                                                    // Старое значение null, а новое нет, тогда запишем на изменение
                                                    else if (oldVal != null && val == null)
                                                    {
                                                        // Запишем на изменение
                                                        newArr.Add(it.Key, val);
                                                        change = true;

                                                    }
                                                    else
                                                    {
                                                        // Есди оба были не пустые, то запишем при расхождению
                                                        var id = ((IDictionary<string, object>)it.Value)["Id"];
                                                        var idOld = ((IDictionary<string, object>)oldVal)["Id"];
                                                        if (!idOld.Equals(id))
                                                        {
                                                            // Запишем на изменение
                                                            newArr.Add(it.Key, val);
                                                            change = true;
                                                        }
                                                    }


                                                }
                                            }
                                            else
                                            {
                                                // Обычный реквизит

                                                var val = it.Value;

                                                var whe = arrsOld.Where(i => i["Id"].Equals(idstr));

                                                if (whe.Any())
                                                {
                                                    var oldVal = whe.Select(i => i[it.Key]).First();

                                                    // Всегда был ноль, значит равны, по другому не хочет 
                                                    if (oldVal == null && val == null)
                                                        continue;
                                                    // Старое значение null, а новое нет, тогда запишем на изменение
                                                    if (oldVal == null && val != null)
                                                    {
                                                        // Запишем на изменение
                                                        newArr.Add(it.Key, val);
                                                        change = true;

                                                    }
                                                    // Старое значение null, а новое нет, тогда запишем на изменение
                                                    else if (oldVal != null && val == null)
                                                    {
                                                        // Запишем на изменение
                                                        newArr.Add(it.Key, val);
                                                        change = true;

                                                    }
                                                    // Есди оба были не пустые, то запишем при расхождении
                                                    else if (!oldVal.Equals(val))
                                                    {
                                                        // Запишем на изменение
                                                        newArr.Add(it.Key, val);
                                                        change = true;

                                                    }

                                                }

                                            }

                                            // Запишем ИД строки коллекции в которой было изменение если не записывали
                                            if (change && !writeId)
                                            {
                                                newArr.Add("Id", idstr);
                                                writeId = true;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (var it in arr)
                                        {
                                            if (it.Key != "MemoIT")
                                                // Тут новая строка коллекции
                                                newArr.Add(it.Key, it.Value);
                                        }

                                    }

                                    // Сложим если были изменения
                                    if (newArr.Any())
                                        newArrs.Add(newArr);
                                }

                                // Запишем изменения если были
                                if (newArrs.Any())
                                    изменения.Add(item.Key, newArrs.ToArray());
                            }


                        }

                    }
                    catch (Exception ex)
                    {
                        var d = ex.Message;
                    }

                }

            }

            return изменения;

        }

        #endregion

    }
}

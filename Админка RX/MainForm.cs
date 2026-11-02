using Microsoft.OData.Edm;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Админка_RX.Properties;
using Админка_RX.Классы;
using Мониторинг.Resources.Функции;
using static Админка_RX.Классы.MetadataRX;
using static Админка_RX.Классы.Функции;

namespace Админка_RX
{
    public partial class MainForm : Form
    {
        readonly Properties.Settings Параметры = Properties.Settings.Default;

        Dictionary<IEdmEntityType, IEdmEntitySet> ТипИТипСущности = new Dictionary<IEdmEntityType, IEdmEntitySet>();
        Dictionary<IEdmEntityType, IEdmEntitySet> ВсеТипИТипСущности = new Dictionary<IEdmEntityType, IEdmEntitySet>();
        public static IEnumerable<XElement> ТипыКоллекции = null;
        public static List<IEdmStructuredType> БазовыеТипы = new List<IEdmStructuredType>();
        public static List<string> ТипыСущностей = new List<string>();
        public static DataSet МАКСИД = null;
        public static IEdmModel EDMModel = null;
        public static ODataClient ODATAClient = null;


        public MainForm()
        {

            InitializeComponent();
            // Сразузагрузим данные для быстрого поиска
            ЗагрузитьДанныеAsync();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ИД.Enabled = Параметры.Администратор;
            //Поиск.Enabled = Параметры.Администратор;
            //ИзменРеквКартинка.Enabled = Параметры.Администратор;

            //Height = 95;


            //ИД.Text = "25635";


            // Загрузим начальные данные
            if (!Параметры.ПерваяЗагрузка)
            {
                var Начальные_данные = Resources.Начальные_данные;

                Параметры.Файлы = ToDataSet(Начальные_данные);

                Параметры.ПерваяЗагрузка = true;
                Параметры.Save();

            }

        }


        // определение асинхронного метода
        async void ЗагрузитьДанныеAsync()
        {
            bool OK = false;

            // Намомент загрузки отключим реквизиты
            ВсёОК.Checked = OK;
            Поиск.Enabled = OK;
            ИзменитьРеквизиты.Enabled = OK;
            МассовоеИзменениеРеквизитов.Enabled = OK;
            СтатусЦвет.Value = 0;

            OK = await Task.Run(ЗагрузитьДанные);                // выполняется асинхронно

            ВсёОК.Checked = OK;
            Поиск.Enabled = OK;
            ИзменитьРеквизиты.Enabled = OK;
            МассовоеИзменениеРеквизитов.Enabled = OK;

            if (OK)
                СтатусЦвет.Value = 1;
        }

        public bool ЗагрузитьДанные()
        {
            bool OK = false;


            if (!string.IsNullOrEmpty(Параметры.Пароль) && !string.IsNullOrEmpty(Параметры.Логин) && !string.IsNullOrEmpty(Параметры.Ссылка))
            {
                try
                {
                    // ЕДМ модель для сравнений и получения реквизитов
                    EDMModel = MetadataRX.ПолучитьEdmМодель();

                    if (EDMModel == null)
                        return OK;

                    // ОДата для поискоа и изменений
                    ODATAClient = DirectumRX.ODataClient();
                    // Найдём вообще все типы по которым можно работать
                    ВсеТипИТипСущности = MetadataRX.GetAllEntityTypesAndEntitySet(EDMModel);
                    // Найдём реквизиты коллекции, правятся они в основной сущности
                    ТипыКоллекции = MetadataRX.ПолучитьНавигационноеСвойствоКоллекцию();
                    // Уберём всё лишнее
                    ТипИТипСущности = MetadataRX.УбратьЛишниеТипы(ВсеТипИТипСущности, ТипыКоллекции);
                    БазовыеТипы = MetadataRX.ПолучитьВсеБазовыеТипы(ТипИТипСущности);

                    МАКСИД = Параметры.МаксИД;

                    if (МАКСИД == null || МАКСИД.Tables.Count != ТипИТипСущности.Count)
                        МАКСИД = new DataSet();

                    // Русификация
                    var Файлы = Параметры.Файлы;

                    if (Файлы == null)
                        Файлы = new DataSet();

                    // Для массового изменения
                    foreach (var item in ТипИТипСущности)
                    {
                        #region Заполнение таблиц

                        if (!МАКСИД.Tables.Contains(item.Value.Name))
                        {
                            // Создать новую таблицу  
                            DataTable dataTable = new DataTable(item.Value.Name);
                            dataTable.Columns.Add("MaxId", typeof(long));

                            DataRow _ravi = dataTable.NewRow();
                            _ravi["MaxId"] = -1;
                            dataTable.Rows.Add(_ravi);


                            МАКСИД.Tables.Add(dataTable);

                            Параметры.ЗагрузкаИд = true;
                            Параметры.Save();
                        }

                        #endregion

                        #region Русификация

                        if (!Файлы.Tables.Contains(item.Key.Name))
                        {
                            // Создать новую таблицу  
                            DataTable dataTable = new DataTable(item.Key.Name);
                            dataTable.Columns.Add("Type", typeof(string));
                            // Добавим ТипСущности Для удобства и создадим первую и последнюю строку таблицы 
                            if (dataTable.Rows.Count == 0)
                                dataTable.Rows.Add();
                            dataTable.Rows[0][0] = item.Value;
                            Файлы.Tables.Add(dataTable);

                        }

                        #endregion

                    }

                    Параметры.Файлы = Файлы;
                    Параметры.МаксИД = МАКСИД;
                    Параметры.Save();

                    OK = true;

                }
                catch (Exception ex)
                {
                    var result = MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    if (result == DialogResult.OK)
                        ЗагрузитьДанные();
                    else
                        OK = false;

                }
            }

            return OK;
            // ТУТ МОЖНО СДЕЛАТЬ ДИАЛОГ ИЛИ ЧТО ТО, ПОКАЖЕТ НЕ ЗАБИРАЯ ДОСТУП
            //MessageBox.Show("");
        }


        private void ЗагрузитьВсеИДОбязательно_Click(object sender, EventArgs e)
        {
            //Загрузим последнее ИД
            ЗагрузитьПоследнееИД();
        }

        public async void ЗагрузитьПоследнееИД()
        {
            var bar = this.ОбработкаProgressBar;
            bar.Minimum = 0;
            bar.Value = 0;
            bar.Maximum = МАКСИД.Tables.Count;


            foreach (DataTable table in МАКСИД.Tables)
            {
                bar.Value += 1;

                var tab = МАКСИД.Tables[table.TableName];

                if (tab == null)
                    continue;

                //Thread.Sleep(100);

                try
                {
                    var ids = await DirectumRX.ПолучитьПоследнееИД(ODATAClient, table.TableName);
                    // Если нет Идишек
                    if (!ids.Any())
                        continue;

                    // Всегда 1 ид а иначе очень долго
                    tab.Rows[0][0] = ids.First().Values.First();
                    Параметры.ЗагрузкаИд = false;
                }
                catch (Exception ex)
                {
                    var f = ex.Message;
                }

            }

            Параметры.МаксИД = МАКСИД;
            Параметры.Save();

        }



        #region Меню

        private void КнопкаОбновить_Click(object sender, EventArgs e)
        {
            EDMModel = null;
            ODATAClient = null;

            ЗагрузитьДанныеAsync();
        }

        private void КнопкаНастройка_Click(object sender, EventArgs e)
        {
            SettingForm settingForm = new SettingForm
            {
                ODATAClient = ODATAClient
            };
            settingForm.ShowDialog();
        }


        private void ПоискПоИД_Click(object sender, EventArgs e)
        {
            var ид = ИД.Text;

            if (string.IsNullOrEmpty(ид))
                MessageBox.Show("Укажите ИД");
            else if (Параметры.ЗагрузкаИд)
                MessageBox.Show("Сделайте загрузку ИД");
            else
                ПоискПоИД(ид);
        }

        private void ИзменитьРеквизиты_Click(object sender, EventArgs e)
        {
            var ид = ИД.Text;

            if (string.IsNullOrEmpty(ид))
                MessageBox.Show("Укажите ИД");
            else if (Параметры.ЗагрузкаИд)
                MessageBox.Show("Сделайте загрузку ИД");
            else
                ИзменениеРеквизитов(ид);
        }

        private void МассовоеИзменениеРеквизитов_Click(object sender, EventArgs e)
        {
            var names = ТипИТипСущности.Select(s => s.Value.Name);
            var тип = Диалоги.ЗапроситьВыбор(names);

            if (string.IsNullOrEmpty(тип))
                MessageBox.Show("Укажите Тип сущности");
            else
            {
                SelectForm changeForm = new SelectForm
                {
                    Тип = тип,
                    ТипЗапуска = ТипЗапускаФормы.МассовоеИзменениеРеквизитов
                };
                changeForm.ShowDialog();
            }
        }

        private void МиграцияЗаписей_Click(object sender, EventArgs e)
        {
            var тип = "";

            if (string.IsNullOrEmpty(тип))
                MessageBox.Show("Укажите Тип сущности");
            else
            {
                RecordMigrationForm changeForm = new RecordMigrationForm
                {
                    Тип = тип,
                    Ссылка = Параметры.Ссылка,
                    Ссылка2 = Параметры.Ссылка2
                };
                changeForm.ShowDialog();
            }
        }

        private void ИсторияРаботы_Click(object sender, EventArgs e)
        {
            var ид = ИД.Text;

            ИсторияРаботыПользователя(ид);
            //ПолучитьВсехПользователей();
        }


        private void Поиск_Click(object sender, EventArgs e)
        {
            var names = ТипИТипСущности.Select(s => s.Value.Name);
            var тип = Диалоги.ЗапроситьВыбор(names);

            if (string.IsNullOrEmpty(тип))
                MessageBox.Show("Укажите Тип сущности");
            else
            {
                SelectForm changeForm = new SelectForm
                {
                    Тип = тип,
                    ТипЗапуска = ТипЗапускаФормы.Просмотр
                };
                changeForm.ShowDialog();
            }
        }


        private void ГдеИспользуется_Click(object sender, EventArgs e)
        {
            var ид = ИД.Text;

            if (string.IsNullOrEmpty(ид))
                MessageBox.Show("Укажите ИД");
            else if (Параметры.ЗагрузкаИд)
                MessageBox.Show("Сделайте загрузку ИД");
            else
                ГдеИспользуется(ид);

        }

        #region Функции меню

        private async void ПоискПоИД(string ид)
        {
            var лист = new List<string>();

            string кнопка = string.Empty;

            var bar = ОбработкаProgressBar;
            bar.Minimum = 0;
            bar.Value = 0;
            bar.Maximum = МАКСИД.Tables.Count;

            foreach (DataTable table in МАКСИД.Tables)
            {
                bar.Value += 1;


                if (long.Parse(table.Rows[0][0].ToString()) == -1)
                    continue;

                // Если ИД больше МАКСИД, то и искать не стоит
                if (long.Parse(ид) > long.Parse(table.Rows[0][0].ToString()))
                    continue;

                var тип = table.TableName;

                var значениеРХ = await DirectumRX.ПолучитьAsync(ODATAClient, тип, $"Id eq {long.Parse(ид)}");
                if (значениеРХ != null)
                    лист.Add(тип);


            }


            var типы = MetadataRX.УбратьЛишнихНаследников(ТипИТипСущности, БазовыеТипы, лист);


            // Создадим диалог и получим нажатую кнопку
            кнопка = Диалоги.Показать(типы);



        }

        private async void ИзменениеРеквизитов(string ид)
        {

            var лист = new List<string>();

            string кнопка = string.Empty;

            var bar = ОбработкаProgressBar;
            bar.Minimum = 0;
            bar.Value = 0;
            bar.Maximum = МАКСИД.Tables.Count;

            foreach (DataTable table in МАКСИД.Tables)
            {
                bar.Value += 1;


                if (long.Parse(table.Rows[0][0].ToString()) == -1)
                    continue;

                // Если ИД больше МАКСИД, то и искать не стоит
                if (long.Parse(ид) > long.Parse(table.Rows[0][0].ToString()))
                    continue;

                var тип = table.TableName;

                var значениеРХ = await DirectumRX.ПолучитьAsync(ODATAClient, тип, $"Id eq {long.Parse(ид)}");
                if (значениеРХ != null)
                    лист.Add(тип);


            }


            var типы = MetadataRX.УбратьЛишнихНаследников(ТипИТипСущности, БазовыеТипы, лист);


            // Создадим диалог и получим нажатую кнопку
            кнопка = Диалоги.Показать(типы);

            if (!string.IsNullOrEmpty(кнопка))
            {
                using (ChangePropertyForm propertyForm = new ChangePropertyForm())
                {
                    propertyForm.ТипСущности.Text = кнопка;
                    propertyForm.Тип = кнопка;
                    propertyForm.ТипЗапускаФормы = ТипЗапускаФормы.ИзменениеРеквизитов;
                    propertyForm.ИДЗаписи.Text = ид;
                    propertyForm.Ид = ид;
                    propertyForm.ShowDialog();
                    ИД.Text = string.Empty;
                }

            }

        }

        private async void ИсторияРаботыПользователя(string ид)
        {
            var names = new List<string>();
            var name = string.Empty;

            // Предоставим выбор пользователя
            if (string.IsNullOrEmpty(ид))
            {
                var значения = await DirectumRX.ПолучитьПакетноAsync(ODATAClient, "IUsers", new string[] { });
                foreach (var item in значения)
                    names.Add(string.Format("{0}(Id:{1})", item["Name"], item["Id"]));


                ////var data = await OData.GetDataAsync(entityName: "IUsers",  top: 50);
                //var data2 = await OData.GetDataAsync(entityName: "IUsers", top: 500);
                //var data3 = await OData.GetAllDataAsync(entityName: "IUsers", pageSize: 50);

                names.Sort();

                name = Диалоги.ЗапроситьВыбор(names);

                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Укажите пользователя");
                    return;
                }

                ид = name.Split(new string[] { "(Id:" }, StringSplitOptions.None)[1].TrimEnd(')');
            }


            var лист = new Dictionary<string, string>();

            лист.Add("Документ", "IDocumentHistories");
            лист.Add("Справочник", "IDatabookHistories");
            лист.Add("Процесс", "IWorkflowHistories");
            лист.Add("По всем", "IDocumentHistories,IDatabookHistories,IWorkflowHistories");

            var значениеРХ = await DirectumRX.ПолучитьAsync(ODATAClient, "IUsers", $"Id eq {long.Parse(ид)}");
            if (значениеРХ != null)
            {
                // Создадим диалог и получим нажатую кнопку
                var кнопка = Диалоги.Показать(лист.Select(l => l.Key).ToList());

                if (!string.IsNullOrEmpty(кнопка))
                {
                    if (кнопка == "По всем")
                    {
                        foreach (var имя in лист["По всем"].Split(new char[] { ',' }))
                        {
                            SelectForm changeForm = new SelectForm
                            {
                                Тип = имя,
                                ВариантыИсторииРабот = лист,
                                Ид = ид,
                                ТипЗапуска = ТипЗапускаФормы.ИсторияРаботы
                            };
                            changeForm.Show();
                        }
                    }
                    else
                    {
                        SelectForm changeForm = new SelectForm
                        {
                            Тип = лист[кнопка],
                            ВариантыИсторииРабот = лист,
                            Ид = ид,
                            ТипЗапуска = ТипЗапускаФормы.ИсторияРаботы
                        };
                        changeForm.Show();
                    }
                }
            }
            else
                MessageBox.Show("Пользователь не найден");

        }


        private async void ГдеИспользуется(string ид)
        {
            var лист = new List<string>();

            string кнопка = string.Empty;

            var bar = ОбработкаProgressBar;
            bar.Minimum = 0;
            bar.Value = 0;
            bar.Maximum = МАКСИД.Tables.Count;

            foreach (DataTable table in МАКСИД.Tables)
            {
                bar.Value += 1;


                if (long.Parse(table.Rows[0][0].ToString()) == -1)
                    continue;

                // Если ИД больше МАКСИД, то и искать не стоит
                if (long.Parse(ид) > long.Parse(table.Rows[0][0].ToString()))
                    continue;

                var тип = table.TableName;

                var значениеРХ = await DirectumRX.ПолучитьAsync(ODATAClient, тип, $"Id eq {long.Parse(ид)}");
                if (значениеРХ != null)
                    лист.Add(тип);


            }


            var типы = MetadataRX.УбратьЛишнихНаследников(ТипИТипСущности, БазовыеТипы, лист);


            // Создадим диалог и получим нажатую кнопку
            кнопка = Диалоги.Показать(типы);

            if (!string.IsNullOrEmpty(кнопка))
            {
                var a = MetadataRX.FindPropertiesReferencingEntity(EDMModel, кнопка);

                var fffff = new List<IDictionary<string, object>>();
                var значениеРХ = await DirectumRX.ПолучитьAsync(ODATAClient, кнопка, $"Id eq {long.Parse(ид)}");
                if (значениеРХ != null)
                {

                    var bar2 = ОбработкаProgressBar;
                    bar2.Minimum = 0;
                    bar2.Value = 0;
                    bar2.Maximum = a.Count;

                    foreach (var item in a)
                    {
                        bar2.Value += 1;
                        var d = item.Split(new char[] { '|' }, StringSplitOptions.None);
                        var ЭлементEdmМодели = ПолучитьЭлементEdmМодели(EDMModel, d[0]);
                        var ТипОбъекта = ЭлементEdmМодели.EntityType();
                        var НавигационныеРеквизиты = ТипОбъекта.NavigationProperties().ToList();
                        var ИменаНавигРеквиз = НавигационныеРеквизиты.Select(i => i.Name).ToList();
                        var n = d[0];
                        var r = d[1];
                        var f = $"{r}/Id eq {значениеРХ.First()["Id"].ToString()}";
                        var значениея = await DirectumRX.ПолучитьAsync(ODATAClient, n, f, ИменаНавигРеквиз.ToArray());
                        if (значениея != null)
                            fffff.AddRange(значениея);
                    }
                }


                var ff = fffff;

            }

        }

        #endregion

        #endregion


    }
}

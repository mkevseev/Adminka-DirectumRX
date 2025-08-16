using Microsoft.OData.Edm;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Админка_RX.Properties;
using Админка_RX.Классы;
using Мониторинг.Resources.Функции;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Админка_RX
{
    public partial class MainForm : Form
    {
        readonly Properties.Settings Параметры = Properties.Settings.Default;

        public static List<string> ТипыСущностей = new List<string>();
        public static List<string> ТипыБезНаследников = new List<string>();
        public static DataSet МАКСИД = null;
        public static IEdmModel EDMModel = null;
        public static ODataClient ODATAClient = null;
        public static IDictionary<string, string> ТипИТипСущности = new Dictionary<string, string>();


        public MainForm()
        {

            InitializeComponent();
            // Автообновление
            ВремяАвтоОбновл.Interval = 60000; // Установите интервал в 60 000 миллисекунд (60 секунда)
            ВремяАвтоОбновл.Start(); // Запустите таймер
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
            ПоискПоИДПунктДействия.Enabled = OK;
            ИзменитьРеквизитыПунктДействия.Enabled = OK;
            МассовоеИзменениеРеквизитовПунктДействия.Enabled = OK;
            СтатусЦвет.Value = 0;

            OK = await Task.Run(ЗагрузитьДанные);                // выполняется асинхронно

            ВсёОК.Checked = OK;
            ПоискПоИДПунктДействия.Enabled = OK;
            ИзменитьРеквизитыПунктДействия.Enabled = OK;
            МассовоеИзменениеРеквизитовПунктДействия.Enabled = OK;

            if (OK)
                СтатусЦвет.Value = 1;
        }

        public bool ЗагрузитьДанные()
        {

            //long rfr = 169;
            //KeyValuePair<string, object> keyValuePair = new KeyValuePair<string, object>("Id", rfr);
            //var fff = f.Where(d => d.Contains(keyValuePair));
            //var ffff = fff;
            bool OK = false;


            if (!string.IsNullOrEmpty(Параметры.Пароль) && !string.IsNullOrEmpty(Параметры.Логин) && !string.IsNullOrEmpty(Параметры.Ссылка))
            {
                try
                {
                    var сравнениеНаслед = new Dictionary<string, List<string>>();
                    // ЕДМ модель для сравнений и получения реквизитов
                    EDMModel = MetadataRX.ПолучитьEdmМодель();
                    // ОДата для поискоа и изменений
                    ODATAClient = DirectumRX.ODataClient();
                    // Получим типы сущностей
                    ТипыСущностей = MetadataRX.ПолучитьТипыСущностей(EDMModel);
                    // Тип и тип сущности 
                    ТипИТипСущности = MetadataRX.ПолучитьТипыИТипыСущности(EDMModel, ТипыСущностей);

                    foreach (var тип in ТипыСущностей)
                    {
                        List<string> d = new List<string>();
                        if (!сравнениеНаслед.ContainsKey(тип))
                            сравнениеНаслед.Add(тип, MetadataRX.ПолучитьИменаНаследников(EDMModel, тип, тип, d, ТипИТипСущности));

                    }



                    var имена = сравнениеНаслед.Select(d => d.Key);

                    ТипыБезНаследников = имена.Where(d => !сравнениеНаслед.Any(n => n.Value.Contains(d))).ToList();

                    МАКСИД = Параметры.МаксИД;



                    if (МАКСИД == null)
                    МАКСИД = new DataSet();

                    // Для массового изменения
                    foreach (var item in ТипыБезНаследников)
                    {

                        if (!ТипыСущностейПодпунктМИРДействия.Items.Contains(item))
                            ТипыСущностейПодпунктМИРДействия.Items.Add(item);

                        #region Заполнение таблиц


                        if (!МАКСИД.Tables.Contains(item))
                        {
                            // Создать новую таблицу  
                            DataTable dataTable = new DataTable(item);
                            dataTable.Columns.Add("MaxId", typeof(long));

                            DataRow _ravi = dataTable.NewRow();
                            _ravi["MaxId"] = -1;
                            dataTable.Rows.Add(_ravi);


                            МАКСИД.Tables.Add(dataTable);

                            Параметры.ЗагрузкаИд = true;
                            Параметры.Save();
                        }

                        #endregion
                    }




                    // Русификация
                    var Файлы = Параметры.Файлы;


                    if (Файлы == null)
                        Файлы = new DataSet();

                    foreach (var pair in ТипИТипСущности)
                    {

                        #region Русификация
                        if (!Файлы.Tables.Contains(pair.Key))
                        {

                            // Создать новую таблицу  
                            DataTable dataTable = new DataTable(pair.Key);
                            dataTable.Columns.Add("Type", typeof(string));
                            // Добавим ТипСущности Для удобства и создадим первую и последнюю строку таблицы 
                            if (dataTable.Rows.Count == 0)
                                dataTable.Rows.Add();
                            dataTable.Rows[0][0] = pair.Value;
                            Файлы.Tables.Add(dataTable);



                        }
                        #endregion

                    }


                    Параметры.Файлы = Файлы;
                    Параметры.МаксИД = МАКСИД;
                    Параметры.Save();




                    OK = true;
                    //  НУЖНА ЕЩЁ ПРОВЕРКА НА ЗАГРУЖЕННОСТЬ ДАННЫХ И НА ВРЕМЯ ЗАГРУЗКИ

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


        private void ВремяАвтоОбновл_Tick(object sender, EventArgs e)
        {
            ЗагрузитьДанныеAsync();
        }


        private async void ПоискПоИД(string ид)
        {
            var типыСущностей = MetadataRX.ПолучитьТипыСущностей(EDMModel);
            var типыСущностей2 = new List<string>();
            var наследники = new List<string>();
            string кнопка = string.Empty;


            foreach (var тип in типыСущностей)
            {
                наследники.AddRange(MetadataRX.ПолучитьИменаНаследников(EDMModel, тип));

                if (!наследники.Any(н => н == тип))
                {
                    var значениеРХ = await DirectumRX.ПолучитьAsync(ODATAClient, тип, $"Id eq {int.Parse(ид)}");
                    if (значениеРХ != null)
                    {
                        типыСущностей2.Add(тип);
                        //var d = значениеРХ.First();
                        //var dd = d["Id"];
                        //if (!результат.Result)
                        //    типыСущностей.Remove(тип);
                        значениеРХ = null;
                    }
                }

            }
            // Создадим диалог и получим нажатую кнопку
            кнопка = Диалоги.Показать(типыСущностей2);


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


            // Создадим диалог и получим нажатую кнопку
            кнопка = Диалоги.Показать(лист);

            if (!string.IsNullOrEmpty(кнопка))
            {
                using (ChangePropertyForm propertyForm = new ChangePropertyForm())
                {
                    propertyForm.ТипСущности.Text = кнопка;
                    propertyForm.Тип = кнопка;
                    propertyForm.ИДЗаписи.Text = ид;
                    propertyForm.Ид = ид;
                    propertyForm.ShowDialog();
                    ИД.Text = string.Empty;
                }

            }



        }



        public string GetXml(DataSet dataSet)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                dataSet.WriteXml(memStream);
                return Encoding.UTF8.GetString(memStream.ToArray());
            }
        }

        public DataSet ToDataSet(string xml)
        {
            var byteData = Encoding.UTF8.GetBytes(xml);
            using (MemoryStream memStream = new MemoryStream(byteData))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(memStream);
                return ds;
            }
        }

        #region Меню

        private void КнопкаОбновить_Click(object sender, EventArgs e)
        {
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


        private void ПоискПоИДПунктДействия_Click(object sender, EventArgs e)
        {
            var ид = ИД.Text;

            if (string.IsNullOrEmpty(ид))
                MessageBox.Show("Укажите ИД");
            else
                ПоискПоИД(ид);
        }

        private void ИзменитьРеквизитыПунктДействия_Click(object sender, EventArgs e)
        {
            var ид = ИД.Text;

            if (string.IsNullOrEmpty(ид))
                MessageBox.Show("Укажите ИД");
            else if (Параметры.ЗагрузкаИд)
                MessageBox.Show("Сделайте загрузку ИД");
            else
                ИзменениеРеквизитов(ид);
        }

        private void ИзменитьПодпунктМИРДействия_Click(object sender, EventArgs e)
        {
            var тип = ТипыСущностейПодпунктМИРДействия.Text;

            if (string.IsNullOrEmpty(тип))
                MessageBox.Show("Укажите Тип сущности");
            else
            {
                MassiveChangeForm changeForm = new MassiveChangeForm
                {
                    Тип = тип
                };
                changeForm.ShowDialog();
            }
        }


        #endregion

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

        private void ПодгрузитьВсеИД_Click(object sender, EventArgs e)
        {
            var d = GetXml(МАКСИД);
            var dd = d;
        }


        
    }
}

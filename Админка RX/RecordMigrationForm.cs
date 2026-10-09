using Microsoft.OData.Edm;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Админка_RX.Классы;
using static Админка_RX.Классы.MetadataRX;
using static Админка_RX.Классы.Функции;

namespace Админка_RX
{
    public partial class RecordMigrationForm : Form
    {
        public string Тип = string.Empty;
        public string Ссылка = string.Empty;
        public string Ссылка2 = string.Empty;
        public static IDictionary<string, object> Данные = new Dictionary<string, object>();
        public static IDictionary<string, object> Реквизиты = new Dictionary<string, object>();
        public List<string> ДопРеквизиты = new List<string>();
        public static ODataClient ODATAClient = MainForm.ODATAClient;
        public static IEdmModel EDMModel = MainForm.EDMModel;
        public List<string> ТипыСущностей = MainForm.ТипыСущностей;
        public static IEnumerable<XElement> ТипыКоллекции = MainForm.ТипыКоллекции;
        private IEdmEntityType ТипОбъекта = null;
        private IEdmEntitySet ЭлементEdmМодели = null;
        private bool newOpen = true;

        public RecordMigrationForm()
        {
            InitializeComponent();
        }

        private void RecordMigrationForm_Load(object sender, EventArgs e)
        {
            ТипtoolStripLabel.Text = Тип;
            СсылкаtoolStripLabel.Text = Ссылка;
            Ссылка2toolStripLabel.Text = Ссылка2;
            // Заполняем таблицу данными
            ЗаполнитьТаблицу(newOpen);
        }

        public async void ЗаполнитьТаблицу(bool newOpen)
        {
            try
            {




                IDictionary<string, object> всеРеквизиты = new Dictionary<string, object>();

                ЭлементEdmМодели = ПолучитьЭлементEdmМодели(EDMModel, Тип);


                var структурныйТип = ПолучитьСтруктурированныйТип(ЭлементEdmМодели);
                // Получим все имена и типы реквизитов типа сущности для дальнейшего сравнения 
                ТипОбъекта = ЭлементEdmМодели.EntityType();
                var nameType = ТипОбъекта.Name;


                Реквизиты = ПолучитьВсеРеквизитыТипа(структурныйТип, всеРеквизиты);


                foreach (var реквизит in Реквизиты)
                    if (!БазовыйТип(реквизит.Value.ToString()) && !ДопРеквизиты.Contains(реквизит.Key))
                        ДопРеквизиты.Add(реквизит.Key);

                // Создадим колонки
                СоздатьКолонки();

                //if (ДопРеквизиты.Any())
                //{
                //    var записи = await DirectumRX.ПолучитьAsync(ODATAClient, Тип, ДопРеквизиты.ToArray(), 1);

                //    Данные = записи.First();
                //}
                //else
                //{
                //    var записи = await DirectumRX.ПолучитьAsync(ODATAClient, Тип, 1);

                //    Данные = записи.First();
                //}


                // Подписываемся на RowChanged
                Таблица.CellValueChanged += dataGridView1_CellContentClick;

                foreach (var line in Реквизиты)
                {

                    var name = line.Key;
                    var value = line.Value;
                    var реквизит = Реквизиты.Where(r => r.Key == name).First();
                    var property = ТипОбъекта.FindProperty(name);

                    if (property.IsKey())
                        continue;

                    var строка = Таблица.Rows.Add();

                    var типРеквизита = ПолучитьТипРеквизита(property);

                    if (типРеквизита == ТипРеквизита.Обычный)
                    {
                        Таблица.Rows[строка].Cells[0].Value = реквизит.Value;   // Тип реквизита
                        Таблица.Rows[строка].Cells[1].Value = ((IEdmPrimitiveType)property.Type.Definition).Name; // Тип
                        Таблица.Rows[строка].Cells[2].Value = реквизит.Key;   // Имя реквизита
                        Таблица.Rows[строка].Cells[3].Value = DirectumRX.РусскоеИмяРеквизита(EDMModel, name, реквизит.Value.ToString(), Тип);   // Русское имя реквизита
                    }

                    if (типРеквизита == ТипРеквизита.Сущность)
                    {
                        var NavigProperties = ТипОбъекта.NavigationProperties().Where(s => s.Name == name);

                        if (NavigProperties.Any())
                        {
                            var типСущ = ЭлементEdmМодели.FindNavigationTarget(NavigProperties.First()); // Тип данных

                            Таблица.Rows[строка].Cells[0].Value = реквизит.Value;   // Тип реквизита
                            Таблица.Rows[строка].Cells[1].Value = типСущ.Name; // Тип
                            Таблица.Rows[строка].Cells[2].Value = реквизит.Key;   // Имя реквизита
                            Таблица.Rows[строка].Cells[3].Value = DirectumRX.РусскоеИмяРеквизита(EDMModel, name, реквизит.Value.ToString(), Тип);   // Русское имя реквизита

                        }
                    }

                    if (типРеквизита == ТипРеквизита.Коллекция)
                    {
                        var NavigProperties = ТипОбъекта.NavigationProperties().Where(s => s.Name == name);

                        if (NavigProperties.Any())
                        {
                            var типСущ = ЭлементEdmМодели.FindNavigationTarget(NavigProperties.First()); // Тип данных

                            Таблица.Rows[строка].Cells[0].Value = реквизит.Value;   // Тип реквизита
                            Таблица.Rows[строка].Cells[1].Value = типСущ.Name; // Тип
                            Таблица.Rows[строка].Cells[2].Value = реквизит.Key;   // Имя реквизита
                            Таблица.Rows[строка].Cells[3].Value = DirectumRX.РусскоеИмяРеквизита(EDMModel, name, реквизит.Value.ToString(), Тип);   // Русское имя реквизита

                        }
                    }
                    if (!property.IsKey())
                        Таблица.Rows[строка].Cells[4].Value = false; // Реквизит сравнения
                }





            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            newOpen = false;
        }

        private void СоздатьКолонки()
        {

            СоздатьКолонку(Таблица, new DataGridViewTextBoxColumn(), "ТипРеквизита", "Тип реквизита", DataGridViewAutoSizeColumnMode.AllCells, false, true);
            СоздатьКолонку(Таблица, new DataGridViewTextBoxColumn(), "Тип", "Тип", DataGridViewAutoSizeColumnMode.AllCells, true, true);
            СоздатьКолонку(Таблица, new DataGridViewTextBoxColumn(), "ИмяРекв", "Имя реквизита", DataGridViewAutoSizeColumnMode.AllCells, true, true);
            СоздатьКолонку(Таблица, new DataGridViewTextBoxColumn(), "РусИмяРекв", "Русское имя реквизита", DataGridViewAutoSizeColumnMode.AllCells, true, true);
            СоздатьКолонку(Таблица, new DataGridViewCheckBoxColumn(), "РекСравнения", "Реквизит сравнения", DataGridViewAutoSizeColumnMode.AllCells, true, false);
            if (newOpen)
                СоздатьКолонку(Таблица, new DataGridViewTextBoxColumn(), "СравнениеКоллекции", "Сравнение коллекции", DataGridViewAutoSizeColumnMode.Fill, true, true);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что клик был именно в ячейке с флажком
            if (e.ColumnIndex == 4)
            {
                // Получаем текущую ячейку
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)Таблица.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Проверяем, что флажок действительно был установлен (Value == true)
                if (cell.Value != null && (bool)cell.Value == true)
                {
                    var property = ТипОбъекта.FindProperty(Таблица.Rows[e.RowIndex].Cells[2].Value.ToString());
                    var строка = Таблица.Rows.Add();

                    var типРеквизита = ПолучитьТипРеквизита(property);
                    if (типРеквизита == ТипРеквизита.Коллекция || типРеквизита == ТипРеквизита.Сущность)
                    {
                        // Сдесь обрабатываем коллекции и запрашиваем доп. поиск
                        RecordMigrationForm changeForm = new RecordMigrationForm
                        {
                            Тип = Таблица.Rows[e.RowIndex].Cells[1].Value.ToString(),
                            Ссылка = Ссылка,
                            Ссылка2 = Ссылка2,
                            newOpen = false
                        };
                        changeForm.ShowDialog();
                    }

                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Ссылка = Ссылка2toolStripLabel.Text;
            Ссылка2 = СсылкаtoolStripLabel.Text;
            СсылкаtoolStripLabel.Text = Ссылка;
            Ссылка2toolStripLabel.Text = Ссылка2;

            //EDMModel = MetadataRX.ПолучитьEdmМодель();

            //if (EDMModel == null)
            //    return ;
            //// ОДата для поискоа и изменений
            //ODATAClient = DirectumRX.ODataClient();
        }

        private async void ПеренестиКнопка_Click(object sender, EventArgs e)
        {
            //var изменения = await DirectumRX.Проверить(ODATAClient, EDMModel, ТипыСущностей, Тип, Ид, ДопРеквизиты.ToArray(), Данные);

            //try
            //{
            //    DirectumRX.ИзменитьПакетно(ODATAClient, EDMModel, Тип, Ид, ДопРеквизиты.ToArray(), изменения);
            //    this.Close();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
    }
}

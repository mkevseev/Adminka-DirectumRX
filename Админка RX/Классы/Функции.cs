using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Админка_RX.Классы
{
    public class Функции
    {
        public static IEdmModel EDMModel = MainForm.EDMModel;
        public static void СоздатьКолонку(DataGridView таблица, DataGridViewColumn column, string name, string headerText, DataGridViewAutoSizeColumnMode autoSizeColumnMode, bool visible, bool readOnly)
        {
            column.AutoSizeMode = autoSizeColumnMode;
            column.Name = name;
            column.HeaderText = headerText;
            column.Visible = visible;
            column.ReadOnly = readOnly;
            таблица.Columns.Add(column);
        }

        public static void ДобавитьСтроку(DataGridView таблица, string ИмяТипа, string Тип, string ИмяРеквизита, object Значение, string ТипСущности)
        {

            var строка = таблица.Rows.Add();

            таблица.Rows[строка].Cells[0].Value = ИмяТипа;   // Тип реквизита
            таблица.Rows[строка].Cells[1].Value = Тип; // Тип
            таблица.Rows[строка].Cells[2].Value = ИмяРеквизита;   // Имя реквизита
            таблица.Rows[строка].Cells[3].Value = MetadataRX.РусскоеИмяРеквизита(EDMModel, ИмяРеквизита, ИмяТипа, ТипСущности);   // Русское имя реквизита
            таблица.Rows[строка].Cells[4].Value = Значение; // Тут всегда обычные реквизиты
        }

        public enum ТипЗапускаФормы
        {
            ИзменениеРеквизитов,
            ИзменениеКоллекции,
            МассовоеИзменениеРеквизитов,
            МиграцияЗаписей,
            ИсторияРаботы,
            Просмотр
        }

        public string GetXml(DataSet dataSet)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                dataSet.WriteXml(memStream);
                return Encoding.UTF8.GetString(memStream.ToArray());
            }
        }

        public static DataSet ToDataSet(string xml)
        {
            var byteData = Encoding.UTF8.GetBytes(xml);
            using (MemoryStream memStream = new MemoryStream(byteData))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(memStream);
                return ds;
            }
        }
    }
}

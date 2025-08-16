using Simple.OData.Client;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Админка_RX.Классы;

namespace Админка_RX
{
    public partial class SettingForm : Form
    {
        readonly Properties.Settings Параметры = Properties.Settings.Default;
        public ODataClient ODATAClient = null;



        public SettingForm()
        {
            InitializeComponent();
            Ссылка.Text = Параметры.Ссылка;
            Логин.Text = Параметры.Логин;
            Пароль.Text = Параметры.Пароль;
            Администратор.Checked = Параметры.Администратор;
        }

        private void Ссылка_TextChanged(object sender, EventArgs e)
        {
            Параметры.Ссылка = Ссылка.Text;
            Параметры.Save();
        }

        private void Логин_TextChanged(object sender, EventArgs e)
        {
            Параметры.Логин = Логин.Text;
            Параметры.Save();
        }

        private void Пароль_TextChanged(object sender, EventArgs e)
        {
            Параметры.Пароль = Пароль.Text;
            Параметры.Save();
        }

        private void SettingForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (!string.IsNullOrEmpty(Ссылка.Text) && !string.IsNullOrEmpty(Логин.Text) && !string.IsNullOrEmpty(Пароль.Text))
            {
                ЭтоАдмин();
            }

        }

        private async void ЭтоАдмин()
        {


            var ответ = await DirectumRX.IsCurrentUserAdmin(DirectumRX.ODataClient());

            Параметры.Администратор = (bool)ответ.First().Value;
            Параметры.Save();

        }


        //public DataSet ЗаполнитьТаблицу(DataTable Файлы)
        //{


        //    return Файлы;
        //}

        private void ЗагрузитьФайлы_Click(object sender, EventArgs e)
        {

            DataSet Файлы = Параметры.Файлы;

            //DataTable dataTable = new DataTable(pair.Key);
            //dataTable.Columns.Add("ТипСущности", typeof(string));
            //// Добавим ТипСущности Для удобства и создадим первую и последнюю строку таблицы 
            //if (dataTable.Rows.Count == 0)
            //    dataTable.Rows.Add();
            //dataTable.Rows[0][0] = pair.Value;
            //Файлы.Tables.Add(dataTable);


            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    // Начнём искать все файлы в выбранной папке

                    ЗагрузкаBar.Value = 0;
                    ЗагрузкаBar.Minimum = 0;
                    ЗагрузкаBar.Maximum = Файлы.Tables.Count;
                    int v = 0;
                    foreach (DataTable table in Файлы.Tables)
                    {
                        v++;
                        ЗагрузкаBar.Value = v;

                        var tableName = table.TableName;


                        string filePath = string.Empty;
                        string[] filesMTD = Directory.GetFiles(fbd.SelectedPath, DirectumRX.ПолучитьИмяТипаСущности(tableName) + ".mtd", SearchOption.AllDirectories);

                        if (filesMTD.Count() != 0)
                        {
                            filePath = filesMTD[0];

                            //byte[] bytes = File.ReadAllBytes(filePath);
                            //string fileBase64 = Convert.ToBase64String(bytes);


                            #region MTD файл

                            //// MTD файл
                            using (StreamReader sr = new StreamReader(filePath, System.Text.Encoding.UTF8))
                            {
                                string line;
                                string lines = string.Empty;
                                while ((line = sr.ReadLine()) != null)
                                {
                                    lines += line.Replace("\"", "");
                                }

                                foreach (var items in lines.Split(','))
                                {
                                    var item = items.Replace('"', ' ').Trim().Split(':');
                                    try
                                    {
                                        var name = item[0];
                                        var val = item[1];

                                        if (!string.IsNullOrEmpty(name) && name.Trim() == "NameGuid")
                                        {
                                            if (!table.Columns.Contains("NameGuid"))
                                                table.Columns.Add("NameGuid");

                                            var ind = table.Columns.IndexOf("NameGuid");

                                            table.Rows[0][ind] = val.Trim();


                                            break;
                                        }
                                    }
                                    catch { }


                                }
                                sr.Close();

                                //// Добавим колонку с файлом на всякий случай, вдруг потом что ещё пригодится, что бы не запрашивать
                                //if (!table.Columns.Contains("ФайлMTD"))
                                //    table.Columns.Add("ФайлMTD");

                                //var i = table.Columns.IndexOf("ФайлMTD");
                                //table.Rows[0][i] = fileBase64;
                            }

                            #endregion


                        }

                        string[] filesXML = Directory.GetFiles(fbd.SelectedPath, DirectumRX.ПолучитьИмяТипаСущности(tableName) + "System.ru.resx", SearchOption.AllDirectories);

                        if (filesXML.Count() != 0)
                        {
                            filePath = filesXML[0];

                            //byte[] bytes = File.ReadAllBytes(filePath);
                            //string fileBase64 = Convert.ToBase64String(bytes);

                            #region XML(resx) файл

                            ////// XML(resx) файл
                            XmlDocument xDoc = new XmlDocument();
                            xDoc.Load(filePath);
                            // получим корневой элемент
                            XmlElement xRoot = xDoc.DocumentElement;
                            if (xRoot != null)
                            {
                                try
                                {
                                    // обход всех узлов в корневом элементе
                                    foreach (XmlElement xnode in xRoot)
                                    {
                                        if (xnode.Name == "data")
                                        {
                                            // получаем атрибут name
                                            XmlNode attr = xnode.Attributes.GetNamedItem("name");
                                            if (attr.Value.Contains("Property_") || attr.Value.Contains("Enum_"))
                                            {

                                                if (!table.Columns.Contains(attr.Value))
                                                    table.Columns.Add(attr.Value);

                                                var ind = table.Columns.IndexOf(attr.Value);


                                                // обходим все дочерние узлы элемента user
                                                foreach (XmlNode childnode in xnode.ChildNodes)
                                                {

                                                    // если узел - value
                                                    if (childnode.Name == "value")
                                                    {
                                                        table.Rows[0][ind] = childnode.InnerText;
                                                    }

                                                }


                                            }

                                        }


                                    }
                                }
                                catch { }


                                //// Добавим колонку с файлом на всякий случай, вдруг потом что ещё пригодится, что бы не запрашивать
                                //if (!table.Columns.Contains("ФайлRESX"))
                                //    table.Columns.Add("ФайлRESX");

                                //var i = table.Columns.IndexOf("ФайлRESX");
                                //table.Rows[0][i] = fileBase64;
                            }

                            #endregion

                        }

                    }



                }
            }

            //// запись в файл
            //using (FileStream fstream = new FileStream(@"C:\Users\maksim.evseev\Desktop\Админка РХ\xmls.xml", FileMode.OpenOrCreate))
            //{
            //    Файлы.WriteXml(fstream);
            //}


            Параметры.Файлы = Файлы;

            Параметры.Save();


        }

        private void ЗагрузитьФайлы_DragDrop(object sender, DragEventArgs e)
        {
            DataSet Файлы = Параметры.Файлы;
            ЗагрузкаBar.Value = 0;
            ЗагрузкаBar.Minimum = 0;
            ЗагрузкаBar.Maximum = 1;

            ((Button)sender).DoDragDrop(((Button)sender).Text, DragDropEffects.Move);

            var fil = e.Data.GetData(DataFormats.FileDrop);

            var files = fil as Array;
            if (files != null)
            {
                foreach (string filePath in files)
                {

                    var extension = Path.GetExtension(filePath);

                    var nameFile = Path.GetFileName(filePath).Replace(extension, "");

                    if (Файлы.Tables.Contains("I" + nameFile + "Dto"))
                    {
                        var table = Файлы.Tables["I" + nameFile + "Dto"];


                        //byte[] bytes = File.ReadAllBytes(filePath);
                        //string fileBase64 = Convert.ToBase64String(bytes);

                        if (extension == ".mtd")
                        {

                            #region MTD файл

                            //// MTD файл
                            using (StreamReader sr = new StreamReader(filePath, System.Text.Encoding.UTF8))
                            {
                                string line;
                                string lines = string.Empty;
                                while ((line = sr.ReadLine()) != null)
                                {
                                    lines += line.Replace("\"", "");
                                }

                                foreach (var items in lines.Split(','))
                                {
                                    var item = items.Replace('"', ' ').Trim().Split(':');
                                    try
                                    {
                                        var name = item[0];
                                        var val = item[1];

                                        if (!string.IsNullOrEmpty(name) && name.Trim() == "NameGuid")
                                        {
                                            if (!table.Columns.Contains("NameGuid"))
                                                table.Columns.Add("NameGuid");

                                            var ind = table.Columns.IndexOf("NameGuid");

                                            table.Rows[0][ind] = val.Trim();


                                            break;
                                        }
                                    }
                                    catch { }


                                }
                                sr.Close();

                                //// Добавим колонку с файлом на всякий случай, вдруг потом что ещё пригодится, что бы не запрашивать
                                //if (!table.Columns.Contains("ФайлMTD"))
                                //    table.Columns.Add("ФайлMTD");

                                //var i = table.Columns.IndexOf("ФайлMTD");
                                //table.Rows[0][i] = fileBase64;
                            }

                            #endregion
                        }
                        else if (extension == ".resx")
                        {

                            #region XML(resx) файл

                            ////// XML(resx) файл
                            XmlDocument xDoc = new XmlDocument();
                            xDoc.Load(filePath);
                            // получим корневой элемент
                            XmlElement xRoot = xDoc.DocumentElement;
                            if (xRoot != null)
                            {
                                try
                                {
                                    // обход всех узлов в корневом элементе
                                    foreach (XmlElement xnode in xRoot)
                                    {
                                        if (xnode.Name == "data")
                                        {
                                            // получаем атрибут name
                                            XmlNode attr = xnode.Attributes.GetNamedItem("name");
                                            if (attr.Value.Contains("Property_") || attr.Value.Contains("Enum_"))
                                            {

                                                if (!table.Columns.Contains(attr.Value))
                                                    table.Columns.Add(attr.Value);

                                                var ind = table.Columns.IndexOf(attr.Value);


                                                // обходим все дочерние узлы элемента user
                                                foreach (XmlNode childnode in xnode.ChildNodes)
                                                {

                                                    // если узел - value
                                                    if (childnode.Name == "value")
                                                    {
                                                        table.Rows[0][ind] = childnode.InnerText;
                                                    }

                                                }


                                            }

                                        }


                                    }
                                }
                                catch { }


                                //// Добавим колонку с файлом на всякий случай, вдруг потом что ещё пригодится, что бы не запрашивать
                                //if (!table.Columns.Contains("ФайлRESX"))
                                //    table.Columns.Add("ФайлRESX");

                                //var i = table.Columns.IndexOf("ФайлRESX");
                                //table.Rows[0][i] = fileBase64;
                            }

                            #endregion

                        }


                    }

                }
            }




            Параметры.Файлы = Файлы;

            Параметры.Save();

            ЗагрузкаBar.Value = 1;
        }

        private void ЗагрузитьФайлы_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void button1_Click(object sender, EventArgs e)
        {



            using (Form form = new Form())
            {
                DataGridView dataGridView = new DataGridView
                {
                    Top = 40,
                    Left = 10,

                };
                ComboBox comboBox = new ComboBox
                {
                    Top = 10,
                    Left = 10,
                };

                foreach (DataTable table in Параметры.Файлы.Tables)
                {
                    comboBox.Items.Add(table.TableName);
                }

                comboBox.SelectionChangeCommitted += (sender2, e2) =>
                {
                    dataGridView.DataSource = Параметры.Файлы.Tables[comboBox.SelectedItem.ToString()].DefaultView;

                    dataGridView.Refresh();
                };

                form.Controls.Add(dataGridView);
                form.Controls.Add(comboBox);
                form.ShowDialog();
            }

        }

    }
}

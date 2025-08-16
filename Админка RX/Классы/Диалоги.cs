using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Админка_RX.Классы;
using static Админка_RX.Классы.MetadataRX;



namespace Мониторинг.Resources.Функции
{
    internal class Диалоги
    {
        readonly Админка_RX.Properties.Settings Параметры = Админка_RX.Properties.Settings.Default;


        //СОЗДАНИЕ ДИАЛОГА (и пример)
        public static string Показать(List<string> именаКнопок)
        {

            Form prompt = new Form
            {
                Width = 120,
                Height = 80,
                Text = "Выбор",
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true
            };
            Label textLabel = new Label() { Left = 10, Top = 10, Text = "Выбор:" };
            //CheckedListBox inputBox = new CheckedListBox() { Left = 50, Top = 50, Width = 400 };
            ////if (inputBox.CheckedItems.Contains(textBox1.Text) == false)
            //inputBox.Items.Add("Справочник", CheckState.Checked);
            //inputBox.Items.Add("Документ", CheckState.Checked);
            //inputBox.Items.Add("Задача", CheckState.Checked);

            Создать.Кнопки кнопки = new Создать.Кнопки(prompt, именаКнопок); ;
            prompt.Controls.Add(textLabel);
            //prompt.Controls.Add(inputBox);
            prompt.ShowDialog();
            return кнопки.Кнопка;
        }

        private static class Создать
        {

            public class Кнопки
            {
                public string Кнопка = "";

                public Кнопки(Form form, List<string> значения)
                {
                    int maxLength = значения.Max(x => x.Length);
                    int width = TextRenderer.MeasureText(значения.OrderByDescending(r => r.Length).FirstOrDefault(), new Font("Arial", 9)).Width;
                    form.Width = width + 35;
                    int top = 30;
                    foreach (var значение in значения)
                    {
                        Button СамаКнопка = new Button() { Text = значение, Left = 10, Width = width, Top = top };
                        СамаКнопка.Click += (sender, e) => { Кнопка = значение; };
                        СамаКнопка.Click += (sender, e) => { form.Close(); };
                        form.Controls.Add(СамаКнопка);
                        top += 30;
                        form.Height += 30;
                    }

                }
            }

            public class Реквизит
            {

                public Реквизит(Form form, ТипВвода типВвода, object oldValue, DataGridViewCell Cell)
                {
                    switch (типВвода)
                    {
                        case ТипВвода.Boolean:

                            CheckBox checkBox = new CheckBox
                            {
                                Left = 20,
                                Top = 20,
                                ThreeState = true,

                            };

                            bool chec = false;
                            if (oldValue != null)
                            {
                                chec = bool.Parse(oldValue.ToString());
                                checkBox.CheckState = chec ? CheckState.Checked : CheckState.Unchecked;
                                checkBox.Checked = chec;
                            }
                            else
                            {
                                checkBox.CheckState = CheckState.Indeterminate;
                                checkBox.Checked = chec;
                            }

                            checkBox.CheckStateChanged += (sender, e) =>
                            {
                                if (checkBox.CheckState == CheckState.Indeterminate)
                                    Cell.Value = null;

                                if (checkBox.CheckState == CheckState.Checked)
                                    Cell.Value = true;

                                if (checkBox.CheckState == CheckState.Unchecked)
                                    Cell.Value = false;

                            };

                            form.Controls.Add(checkBox);
                            form.Height += 30;
                            form.Width = 30;
                            break;

                        case ТипВвода.Int32:

                            TextBox textBox = new TextBox
                            {
                                Left = 10,
                                Top = 30,                             // Запретить отступы
                                Margin = new Padding(0, 0, 0, 0),
                                Text = oldValue?.ToString(),
                                Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left
                            };

                            textBox.KeyPress += (sender, e) =>
                            {
                                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                                {
                                    e.Handled = true;
                                }
                            };


                            textBox.TextChanged += (sender, e) => { Cell.Value = textBox.Text; };

                            form.Controls.Add(textBox);
                            form.Height += 30;

                            break;

                        case ТипВвода.Int64:

                            TextBox textBox2 = new TextBox
                            {
                                Left = 10,
                                Top = 30,                             // Запретить отступы
                                Margin = new Padding(0, 0, 0, 0),
                                Text = oldValue?.ToString(),
                                Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left
                            };

                            textBox2.KeyPress += (sender, e) =>
                            {
                                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                                {
                                    e.Handled = true;
                                }
                            };

                            textBox2.TextChanged += (sender, e) => { Cell.Value = textBox2.Text; };

                            form.Controls.Add(textBox2);
                            form.Height += 30;

                            break;

                        case ТипВвода.DateTimeOffset:

                            DateTimeOffset dateTimeOffset = DateTimeOffset.Now;

                            if (oldValue != null)
                                dateTimeOffset = DateTimeOffset.Parse(oldValue.ToString());


                            NewDateTimePicker dateTime = new NewDateTimePicker
                            {
                                Top = 20,
                                Left = 20,
                                Format = DateTimePickerFormat.Custom,
                                CustomFormat = "dd.MM.yyyy hh:mm:ss",
                                DateTimeOffsetValue = dateTimeOffset,
                                OffsetValue = dateTimeOffset.Offset,
                                Value = dateTimeOffset.DateTime,
                                Width = 170,
                            };



                            DateTimePicker time = new DateTimePicker
                            {
                                Top = 20,
                                Format = DateTimePickerFormat.Time,
                                ShowUpDown = true,
                                Left = 30 + dateTime.Width,
                                Width = 70,
                                Value = dateTimeOffset.DateTime,

                            };

                            time.ValueChanged += (sender2, e2) =>
                            {
                                var val = dateTime.Value;
                                var t = ((DateTimePicker)sender2).Value;
                                var newDate = new DateTime(val.Year, val.Month, val.Day, t.Hour, t.Minute, t.Second);
                                if (dateTime.Value != newDate)
                                    dateTime.Value = newDate;

                            };

                            dateTime.ValueChanged += (sender2, e2) =>
                            {
                                var val = ((DateTimePicker)sender2).Value;
                                var t = time.Value;
                                var newDate = new DateTime(val.Year, val.Month, val.Day, val.Hour, val.Minute, val.Second);
                                if (time.Value != newDate)
                                    time.Value = newDate;

                                Cell.Value = dateTime.DateTimeOffsetValue;
                            };

                            form.Controls.Add(dateTime);
                            form.Controls.Add(time);
                            form.Height += 20;
                            form.Width = dateTime.Width + time.Width + 70;

                            break;


                        case ТипВвода.Guid:

                            MaskedTextBox maskedText = new MaskedTextBox
                            {
                                Left = 10,
                                Top = 30,
                                Mask = "&&&&&&&&-&&&&-&&&&-&&&&-&&&&&&&&&&&&",
                                Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left,
                                Text = oldValue?.ToString()
                            };

                            //maskedText.TextChanged += (sender, e) =>
                            //{
                                //if (e.IsValidInput)
                                //{
                                    maskedText.TextChanged += (sender2, e2) => { Cell.Value = maskedText.Text; };
                                //}

                            //};

                            form.Controls.Add(maskedText);
                            form.Height += 30;
                            form.Width = 50;

                            break;

                        case ТипВвода.Double:


                            TextBox textBox3 = new TextBox
                            {
                                Left = 10,
                                Top = 30,                             // Запретить отступы
                                Margin = new Padding(0, 0, 0, 0),
                                Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left,
                                Text = oldValue?.ToString()
                            };

                            textBox3.KeyPress += (sender, e) =>
                            {
                                if (e.KeyChar == '.')
                                    e.KeyChar = ',';
                                if (e.KeyChar != 22)
                                    e.Handled = !Char.IsDigit(e.KeyChar) && (e.KeyChar != ',' || (((TextBox)sender).Text.Contains(",") && !((TextBox)sender).SelectedText.Contains(","))) && e.KeyChar != (char)Keys.Back && (e.KeyChar != '-' || ((TextBox)sender).SelectionStart != 0 || (((TextBox)sender).Text.Contains("-") && !((TextBox)sender).SelectedText.Contains("-")));
                                else
                                {
                                    double d;
                                    e.Handled = !double.TryParse(Clipboard.GetText(), out d) || (d < 0 && (((TextBox)sender).SelectionStart != 0 || ((TextBox)sender).Text.Contains("-") && !((TextBox)sender).SelectedText.Contains("-"))) || ((d - (int)d) != 0 && ((TextBox)sender).Text.Contains(",") && !((TextBox)sender).SelectedText.Contains(","));
                                    MessageBox.Show("Не удалось вставить содержимое буфера обмена");
                                }
                            };

                            textBox3.TextChanged += (sender, e) => { Cell.Value = textBox3.Text; };

                            form.Controls.Add(textBox3);
                            form.Height += 30;
                            form.Width = 50;



                            break;

                        case ТипВвода.String:


                            TextBox textBox4 = new TextBox
                            {
                                Left = 10,
                                Top = 30,                             // Запретить отступы
                                Margin = new Padding(0, 0, 0, 0),
                                Anchor =  AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left,
                                Text = oldValue?.ToString()

                            };

                            textBox4.KeyPress += (sender, e) =>
                            {

                            };

                            textBox4.TextChanged += (sender, e) => { Cell.Value = textBox4.Text; };

                            form.Controls.Add(textBox4);
                            form.Height += 30;
                            form.Width = 50;
                            break;
                    }


                }
            }
        }

        public static void Показать(string имяРеквизита, ТипВвода типВвода, object oldValue, DataGridViewCell Cell)
        {

            Form prompt = new Form
            {
                Width = 120,
                Height = 80,
                Text = "Выбор",
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true
            };
            //Label textLabel = new Label() { Left = 10, Top = 10, Text = "Выбор:" };
            //CheckedListBox inputBox = new CheckedListBox() { Left = 50, Top = 50, Width = 400 };
            ////if (inputBox.CheckedItems.Contains(textBox1.Text) == false)
            //inputBox.Items.Add("Справочник", CheckState.Checked);
            //inputBox.Items.Add("Документ", CheckState.Checked);
            //inputBox.Items.Add("Задача", CheckState.Checked);

            Создать.Реквизит реквизит = new Создать.Реквизит(prompt, типВвода, oldValue, Cell); ;
            //prompt.Controls.Add(textLabel);
            //prompt.Controls.Add(inputBox);
            prompt.ShowDialog();
            //return реквизит.Значение;
            prompt.Dispose();
        }



    }
}

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

        public static string _Значение;

        //СОЗДАНИЕ ДИАЛОГА (и пример)
        public static string Показать(List<string> именаКнопок)
        {
            string Кнопка = string.Empty;
            if (именаКнопок.Any())
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

                Кнопка = кнопки.Кнопка;
            }

            return Кнопка;
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
                                Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left,
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

        public static string ПоказатьВыбор(Control реквизит, string имяКнопки)
        {
            Form prompt = new Form
            {
                Width = реквизит.Width + 30,
                Height = реквизит.Height + 80,
                Text = "Выбор",
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true
            };

            prompt.Controls.Add(реквизит);

            if (!string.IsNullOrEmpty(имяКнопки))
            {
                Button button = new Button
                {
                    Left = prompt.Width / 2 - (имяКнопки.Length * 6),
                    Top = 30,
                    Text = имяКнопки,
                };
                prompt.Controls.Add(button);

                button.Click += (sender, e) =>
                {
                    prompt.Close();
                };

                prompt.FormClosing += (sender, e) =>
                {
                    _Значение = реквизит.Text;
                };
            }

            prompt.ShowDialog();
            //return реквизит.Значение;
            prompt.Dispose();

            return _Значение;
        }

        public static string ЗапроситьВыбор(IEnumerable<string> names)
        {
            var тип = string.Empty;
            var maxLength = names.Max(s => s.Length);
            var реквизит = new ComboBox
            {
                Width = TextRenderer.MeasureText(names.OrderByDescending(r => r.Length).FirstOrDefault(), new Font("Arial", 9)).Width,
                Height = 80,
                Top = 5,
                Left = 5,
            };

            реквизит.Items.AddRange(names.ToArray());

            тип = ПоказатьВыбор(реквизит, "Выбрать");

            return тип;
        }





        private static string result;

        public static string Показать(IEnumerable<string> entities)
        {
            Form form = new Form();
            form.Text = "Выбор";
            form.Size = new Size(700, 450);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.BackColor = Color.FromArgb(240, 240, 245);
            InitializeComponent(form);
            GenerateButtons(form, entities);
            SetDynamicMinSize(form);
            form.Controls.Add(flowPanel);
            form.ShowDialog();
            return result;
        }
        private static FlowLayoutPanel flowPanel;

        private static void GenerateButtons(Form form, IEnumerable<string> entities)
        {
            flowPanel.Controls.Clear();

            foreach (var text in entities)
            {
                var btn = new Button
                {
                    Text = text,
                    AutoSize = true,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Margin = new Padding(8),
                    Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                    TextAlign = ContentAlignment.MiddleCenter,

                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(60, 60, 60),
                    FlatAppearance =
                    {
                        BorderSize = 1,
                        BorderColor = Color.FromArgb(200, 200, 200)
                    }
                };

                // Защита кнопки от сплющивания (минимальный размер самой кнопки)
                btn.MinimumSize = new Size(120, 45);

                btn.MouseEnter += (s, e) =>
                {
                    btn.FlatAppearance.BorderColor = Color.FromArgb(100, 149, 237);
                    btn.BackColor = Color.FromArgb(245, 250, 255);
                };

                btn.MouseLeave += (s, e) =>
                {
                    btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                    btn.BackColor = Color.White;
                };

                btn.Click += (s, e) =>
                {
                    result = text;
                    form.Close();
                };


                flowPanel.Controls.Add(btn);
            }
        }

        // --- ГЛАВНОЕ: Динамический расчет минимального размера окна ---
        private static void SetDynamicMinSize(Form form)
        {
            // 1. Принудительно отрисовываем панель, чтобы кнопки получили реальные размеры
            flowPanel.PerformLayout();

            // 2. Ищем самую широкую кнопку (чтобы окно нельзя было сжать уже, чем она)
            int maxBtnWidth = 0;
            int btnHeight = 0;

            foreach (Control c in flowPanel.Controls)
            {
                if (c.Width > maxBtnWidth)
                    maxBtnWidth = c.Width;
                btnHeight = c.Height; // Высота у всех примерно одинаковая
            }

            // 3. Считаем минимальную ширину окна:
            // Ширина самой длинной кнопки + отступы панели (20 слева + 20 справа) + запас 20px
            int minFormWidth = maxBtnWidth + 60;

            // 4. Считаем минимальную высоту окна:
            // Высота одной кнопки + отступы панели (20 сверху + 20 снизу) + запас 40px на заголовок
            int minFormHeight = btnHeight + 80;

            // 5. Устанавливаем границу
            form.MinimumSize = new Size(minFormWidth, minFormHeight);
            if (flowPanel.Controls.Count < 20)
                form.Size = new Size(minFormWidth * flowPanel.Controls.Count / 2, minFormHeight * 2);
            else if (flowPanel.Controls.Count > 20)
                form.Size = new Size(minFormWidth * 5, minFormHeight * 5);
            else if (flowPanel.Controls.Count == 1)
                form.Size = form.MinimumSize;
        }

        private static void InitializeComponent(Form form)
        {
            form.Text = "Выбор";
            form.Size = new Size(700, 450);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.BackColor = Color.FromArgb(240, 240, 245);

            flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                WrapContents = true,      // Кнопки уходят вниз при сужении
                AutoScroll = true,       // Скролл появится, если кнопок ОЧЕНЬ много
                Padding = new Padding(20),
                FlowDirection = FlowDirection.LeftToRight
            };

            form.Controls.Add(flowPanel);
        }


        internal class Ожидание
        {
            public Form form = new Form();
            public ProgressBar progressBar = new ProgressBar();
            public bool close = false;
            public Label label = new Label();

            public Ожидание(int count)
            {
                Создать(count);
            }

            public Ожидание Создать(int count)
            {
                form.Text = "Выбор";
                form.Size = new Size(500, 100);
                form.StartPosition = FormStartPosition.CenterScreen;
                form.BackColor = Color.FromArgb(240, 240, 245);
                form.FormClosed += Form_FormClosed;
                form.FormBorderStyle = FormBorderStyle.FixedSingle;

                progressBar.Minimum = 0;
                progressBar.Value = 0;
                progressBar.Maximum = count;
                progressBar.Size = new Size(500, 40);
                progressBar.Location = new Point(50, 0);

                label.Size = new Size(500, 20);

                form.Controls.Add(label);
                form.Controls.Add(progressBar);

                form.Show();

                return this;
            }

            private void Form_FormClosed(object sender, FormClosedEventArgs e)
            {
                close = true;
            }

            public Ожидание Добавить(int count)
            {
                progressBar.Value += count;
                return this;
            }

            public void Закрыть()
            {
                form.Close();
            }

            public Ожидание Текст(string text)
            {
                label.Text = text;
                return this;
            }
        }




    }
}

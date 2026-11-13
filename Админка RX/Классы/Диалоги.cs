using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Админка_RX.Классы;
using static System.Net.Mime.MediaTypeNames;
using static Админка_RX.Классы.MetadataRX;


namespace Мониторинг.Resources.Функции
{
    internal class Диалоги
    {
        readonly Админка_RX.Properties.Settings Параметры = Админка_RX.Properties.Settings.Default;

        private static string _результат;
        public string Результат
        {
            get
            {
                return _результат;    // возвращаем значение свойства
            }

        }

        private static Form _form;
        public Form Form
        {
            get
            {
                return _form;    // возвращаем значение свойства
            }
            set
            {
                _form = value;   // устанавливаем новое значение свойства
            }
        }

        public static Диалоги Создать(string nameForm)
        {
            var dialog = new Диалоги();
            Form form = new Form
            {
                Size = new Size(400, 450),
                Text = nameForm,
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true,
                BackColor = Color.FromArgb(240, 240, 245)
            };

            _form = form;

            return dialog;
        }


        public static string Выбрать(string nameForm, IEnumerable<string> entities)
        {
            Создать(nameForm);

            // Добавим панель
            InitializeFlowPanel();
            // Создадим кнопки
            GenerateButtonsFlowPanel(entities);
            // Автоматический минимальный размер
            SetDynamicMinSizeFlowPanel();

            _form.Controls.Add(flowPanel);
            _form.ShowDialog();
            //_form.Dispose();
            return _результат;
        }


        public static void Выбрать(string nameForm, ТипВвода типВвода, object oldValue, DataGridViewCell Cell, string имяКнопки = "Изменить")
        {
            Создать(nameForm);

            _результат = string.Empty;

            bool isEdit = false;

            Реквизиты реквизит = new Реквизиты(типВвода, oldValue, Cell);

            // Автоматический минимальный размер
            SetDynamicMinSizeForm();

            if (!string.IsNullOrEmpty(имяКнопки))
            {
                Button button = new Button
                {
                    Left = _form.Width / 2 - (имяКнопки.Length * 6),
                    Top = _form.Controls[0].Top + 40,
                    Text = имяКнопки,
                    Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left
                };
                _form.Controls.Add(button);



                button.Click += (sender, e) =>
                {
                    _результат = Cell.Value != null ? Cell.Value.ToString() : string.Empty;
                    isEdit = true;
                    _form.Close();
                };

                _form.FormClosing += (sender, e) =>
                {
                    if (!isEdit)
                        Cell.Value = oldValue;
                };
            }

            // Автоматический минимальный размер
            SetDynamicMinSizeForm();

            _form.ShowDialog();
            //return реквизит.Значение;
            _form.Dispose();

        }

        public static string Выбрать(Control реквизит, string имяКнопки)
        {
            Создать("Выбор");
            _form.Width = реквизит.Width + 30;
            _form.Height = реквизит.Height + 80;

            _form.Controls.Add(реквизит);

            if (!string.IsNullOrEmpty(имяКнопки))
            {
                Button button = new Button
                {
                    Left = _form.Width / 2 - (имяКнопки.Length * 6),
                    Top = 30,
                    Text = имяКнопки,
                };
                //button.Width = TextRenderer.MeasureText(имяКнопки, new Font(button.Font.Name, button.Font.Size)).Width;

                _form.Controls.Add(button);

                button.Click += (sender, e) =>
                {
                    _form.Close();
                };

                _form.FormClosing += (sender, e) =>
                {
                    _результат = реквизит.Text;
                };
            }

            _form.ShowDialog();
            //return реквизит.Значение;
            _form.Dispose();

            return _результат;
        }

        private static void InitializeFlowPanel()
        {

            flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                WrapContents = true,      // Кнопки уходят вниз при сужении
                AutoScroll = true,       // Скролл появится, если кнопок ОЧЕНЬ много
                Padding = new Padding(20),
                FlowDirection = FlowDirection.LeftToRight
            };

            _form.Controls.Add(flowPanel);
        }


        private static FlowLayoutPanel flowPanel;

        private static void GenerateButtonsFlowPanel(IEnumerable<string> entities)
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
                    _результат = text;
                    _form.Close();
                };


                flowPanel.Controls.Add(btn);
            }
        }

        // --- ГЛАВНОЕ: Динамический расчет минимального размера окна ---
        private static void SetDynamicMinSizeFlowPanel()
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
            _form.MinimumSize = new Size(minFormWidth, minFormHeight);
            if (flowPanel.Controls.Count < 20)
                _form.Size = new Size(minFormWidth * flowPanel.Controls.Count / 2, minFormHeight * 2);
            else if (flowPanel.Controls.Count > 20)
                _form.Size = new Size(minFormWidth * 5, minFormHeight * 5);
            else if (flowPanel.Controls.Count == 1)
                _form.Size = _form.MinimumSize;

        }

        private static void SetDynamicMinSizeForm()
        {
            // 1. Принудительно отрисовываем панель, чтобы кнопки получили реальные размеры
            _form.PerformLayout();

            // 2. Ищем самую широкую кнопку (чтобы окно нельзя было сжать уже, чем она)
            int maxBtnWidth = 0;
            int btnHeight = 0;

            foreach (Control c in _form.Controls)
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
            _form.MinimumSize = new Size(minFormWidth, minFormHeight);
            if (_form.Controls.Count < 20)
                _form.Size = new Size(minFormWidth * _form.Controls.Count / 2, minFormHeight * 2);
            else if (_form.Controls.Count > 20)
                _form.Size = new Size(minFormWidth * 5, minFormHeight * 5);
            else if (_form.Controls.Count == 1)
                _form.Size = _form.MinimumSize;


            if (_form.Controls.Count == 2)
                _form.Height = _form.Controls[0].Top + _form.Controls[0].Height + _form.Controls[1].Top + _form.Controls[1].Height + 30;
            else if (_form.Controls.Count == 3)
            {
                _form.Height = _form.Controls[0].Top + _form.Controls[0].Height + _form.Controls[2].Top + _form.Controls[2].Height + 30;
                _form.Width = _form.Controls[0].Width + _form.Controls[1].Width + 65;
            }
                

            _form.MinimumSize = new Size(_form.Width, _form.Height);
        }



        public class Реквизиты
        {

            public Реквизиты(ТипВвода типВвода, object oldValue, DataGridViewCell Cell)
            {
                switch (типВвода)
                {
                    case ТипВвода.Int32:
                    case ТипВвода.Int64:
                    case ТипВвода.String:
                    case ТипВвода.Double:
                    case ТипВвода.Guid:

                        var control = new Control();
                        if (типВвода == ТипВвода.Guid)
                        {
                            control = new MaskedTextBox
                            {
                                Left = 10,
                                Top = 30,
                                Width = _form.Width - 35,
                                Height = 30,
                                Mask = "&&&&&&&&-&&&&-&&&&-&&&&-&&&&&&&&&&&&",
                                Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left,
                                Text = oldValue?.ToString()
                            };
                        }
                        else
                        {
                            control = new TextBox
                            {
                                Left = 10,
                                Top = 30,
                                Width = _form.Width - 35,
                                Height = 30,
                                Margin = new Padding(0, 0, 0, 0),// Запретить отступы
                                Text = oldValue?.ToString(),
                                Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left
                            };
                        }


                        control.KeyPress += (sender, e) =>
                        {

                            if ((типВвода == ТипВвода.Int32 || типВвода == ТипВвода.Int64) && !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                            {
                                e.Handled = true;
                            }

                            if (типВвода == ТипВвода.Double)
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
                            }

                        };


                        control.TextChanged += (sender, e) => { Cell.Value = control.Text; };

                        _form.Controls.Add(control);

                        break;

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

                        _form.Controls.Add(checkBox);

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

                        _form.Controls.Add(dateTime);
                        _form.Controls.Add(time);

                        break;

                }
            }
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





        /*
         * АРХИВ
         

        /// <summary>
        /// Старый выбор
        /// </summary>
        /// <param name="именаКнопок"></param>
        /// <returns></returns>
        public string ВыбратьСтарый(List<string> именаКнопок)
        {
            if (_form == null)
                return null;

            Кнопки кнопки = new Кнопки(именаКнопок);

            _form.ShowDialog();

            _результат = кнопки.НажатаяКнопка;
            _form.Dispose();
            return _результат;
        }

        public class Кнопки
        {
            public string НажатаяКнопка = "";

            public Кнопки(List<string> значения)
            {
                int maxLength = значения.Max(x => x.Length);
                int width = TextRenderer.MeasureText(значения.OrderByDescending(r => r.Length).FirstOrDefault(), new Font("Arial", 9)).Width;
                _form.Width = width + 35;
                int top = 30;
                foreach (var значение in значения)
                {
                    Button СамаКнопка = new Button() { Text = значение, Left = 10, Width = width, Top = top };
                    СамаКнопка.Click += (sender, e) => { НажатаяКнопка = значение; };
                    СамаКнопка.Click += (sender, e) => { _form.Close(); };
                    _form.Controls.Add(СамаКнопка);
                    top += 30;
                    _form.Height += 30;
                }

            }
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

            тип = Выбрать(реквизит, "Выбрать");

            return тип;
        }
         

         */






    }
}

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Админка_RX.Классы
{
    internal class NewDateTimePicker : DateTimePicker
    {


        private TextBox editbox;
        private int buttonWidth;
        private Color fillColor = Color.LightSeaGreen;
        private Image CalenderImg = Properties.Resources.Календарь;
        private DateTimeOffset newValue;
        private TimeSpan newOffsetValue;
        private RectangleF iconButton;
        private const int iconWidth = 34;
        private const int arrowWidth = 17;




        public NewDateTimePicker()
        {
            editbox = new TextBox
            {
                BorderStyle = BorderStyle.None,
                ReadOnly = true
            };
            //editbox.BackColor = Color.Gold;   // debugging
            this.Controls.Add(editbox);
            this.SetStyle(ControlStyles.UserPaint, true);

            ValueChanged += (sender2, e2) =>
            {
                var newDateOffset = new DateTimeOffset(Value, newOffsetValue);
                newValue = newDateOffset;
                editbox.Text = newValue.ToString();

            };

        }

        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = editbox.Font = value; }
        }


        public DateTimeOffset DateTimeOffsetValue
        {

            get
            {
                return newValue;
            }
            set
            {
                newValue = value;
            }
        }

        public TimeSpan OffsetValue
        {

            get
            {
                return newOffsetValue;
            }
            set
            {
                newOffsetValue = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            base.OnPaint(e);

            //e.Graphics.FillRectangle(new SolidBrush(Color.Green), this.ClientRectangle);

            e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(Color.Blue), 0, 0);

            //Первым аргументом вам нужно указать картинку выпадающего списка.
            e.Graphics.DrawImage(CalenderImg, new Point(this.ClientRectangle.X + this.ClientRectangle.Width - 20, this.ClientRectangle.Y));
        }


        protected override void OnResize(EventArgs e)
        {
            if (buttonWidth == 0)
                measureButtonWidth();
            var margin = (this.ClientSize.Height - editbox.PreferredHeight) / 2;
            editbox.Location = new Point(margin - 1, margin - 2);
            editbox.Width = this.ClientSize.Width - margin - buttonWidth;
            base.OnResize(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (iconButton.Contains(e.Location))
                this.Cursor = Cursors.Hand;
            else this.Cursor = Cursors.Default;
        }
        private void measureButtonWidth()
        {
            if (!Application.RenderWithVisualStyles)
                buttonWidth = 21;
            else
            {
                var renderer = new VisualStyleRenderer("DATEPICKER", 3, 1);
                using (var gr = CreateGraphics())
                {
                    buttonWidth = renderer.GetPartSize(gr, ThemeSizeType.True).Height;
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            int iconWidth = GetIconWidth();
            iconButton = new RectangleF(this.Width - iconWidth, 0, iconWidth, this.Height);
        }

        private int GetIconWidth()
        {
            int textwidth = TextRenderer.MeasureText(this.Text, this.Font).Width;
            if (textwidth <= this.Width - (iconWidth + 20))
                return iconWidth;
            else return arrowWidth;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) editbox.Dispose();
            base.Dispose(disposing);
        }


    }
}

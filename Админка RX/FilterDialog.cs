using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Vocabularies;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Админка_RX.FilterDialog;

namespace Админка_RX
{
    

    public partial class FilterDialog : Form
    {
        private readonly IEdmProperty _property;
        private Control _editor;
        private Control _editor2;          // второе поле для Between

        public FilterCondition Result { get; private set; }


        public enum FilterOp
        {
            Eq, Ne, Gt, Ge, Lt, Le,          // сравнения
            Between,                          // диапазон: ge value1 and le value2
            Contains, StartsWith, EndsWith,   // строковые функции
            IsNull, IsNotNull                 // для nullable-полей
        }

        public class FilterCondition
        {
            public string PropertyName { get; set; }
            public IEdmTypeReference Type { get; set; }  // тип свойства из EDM
            public FilterOp Op { get; set; }
            public object Value { get; set; }
            public object Value2 { get; set; }           // только для Between
        }

        public static class OpMap
        {
            private static readonly FilterOp[] EqualityOnly =
                { FilterOp.Eq, FilterOp.Ne };
            private static readonly FilterOp[] Comparable =
                { FilterOp.Eq, FilterOp.Ne, FilterOp.Gt, FilterOp.Ge, FilterOp.Lt, FilterOp.Le, FilterOp.Between };
            private static readonly FilterOp[] StringOps =
                { FilterOp.Eq, FilterOp.Ne, FilterOp.Contains, FilterOp.StartsWith, FilterOp.EndsWith };

            public static FilterOp[] Get(IEdmTypeReference type)
            {
                var primitive = type.AsPrimitive();
                var kind = primitive != null ? primitive.PrimitiveKind() : (EdmPrimitiveTypeKind?)null;

                if (kind == EdmPrimitiveTypeKind.String)
                    return StringOps;

                switch (kind)
                {
                    case EdmPrimitiveTypeKind.Int16:
                    case EdmPrimitiveTypeKind.Int32:
                    case EdmPrimitiveTypeKind.Int64:
                    case EdmPrimitiveTypeKind.Single:
                    case EdmPrimitiveTypeKind.Double:
                    case EdmPrimitiveTypeKind.Decimal:
                    case EdmPrimitiveTypeKind.Date:
                    case EdmPrimitiveTypeKind.DateTimeOffset:
                        return Comparable;
                }

                if (type.IsEnum())
                    return EqualityOnly;

                return EqualityOnly;
            }
        }


        public FilterDialog(IEdmProperty property)
        {
            InitializeComponent();
            _property = property;
            Text = $"Фильтр: {property.Name}";

            cmbOperator.Items.AddRange(
                OpMap.Get(property.Type).Cast<object>().Select(op => op.ToString()).ToArray());

            if (property.Type.IsNullable)
                cmbOperator.Items.Add(nameof(FilterOp.IsNull));   // + IsNotNull, если хотите

            cmbOperator.SelectedIndexChanged += (_, __) => BuildEditor();
            cmbOperator.SelectedIndex = 0;
        }


        private void BuildEditor()
        {
            panelEditor.Controls.Clear();
            _editor = _editor2 = null;
            var op = (FilterOp)Enum.Parse(typeof(FilterOp), (string)cmbOperator.SelectedItem);
            if (op == FilterOp.IsNull || op == FilterOp.IsNotNull) return;

            var t = _property.Type;
            _editor = CreateEditor(t);

            if (_editor != null) panelEditor.Controls.Add(_editor);

            if (op == FilterOp.Between)
            {
                _editor2 = CreateEditor(t);
                panelEditor.Controls.Add(_editor2);
            }
        }

        private Control CreateEditor(IEdmTypeReference t)
        {
            if (t.IsString() || t.IsGuid())
            {
                return new TextBox { Width = 240 };
            }

            if (t.IsInt32() || t.IsInt16() || t.IsInt64())
            {
                return new NumericUpDown
                {
                    Width = 240,
                    Minimum = decimal.MinValue,
                    Maximum = decimal.MaxValue
                };
            }

            if (t.IsDecimal())
            {
                return new NumericUpDown
                {
                    Width = 240,
                    DecimalPlaces = 2,
                    Minimum = decimal.MinValue,
                    Maximum = decimal.MaxValue
                };
            }

            if (t.IsDate() || t.IsDateTimeOffset())
            {
                return new DateTimePicker { Width = 240, Format = DateTimePickerFormat.Short };
            }

            if (t.IsBoolean())
            {
                var cb = new ComboBox { Width = 240, DropDownStyle = ComboBoxStyle.DropDownList };
                cb.Items.AddRange(new object[] { "true", "false" });
                cb.SelectedIndex = 0;
                return cb;
            }

            if (t.IsEnum())
            {
                var cb = new ComboBox { Width = 240, DropDownStyle = ComboBoxStyle.DropDownList };
                foreach (var m in t.AsEnum().EnumDefinition().Members)
                    cb.Items.Add(m.Name);
                if (cb.Items.Count > 0)
                    cb.SelectedIndex = 0;
                return cb;
            }

            return null;
        }

        private object ReadValue(Control c)
        {
            if (c is NumericUpDown)
                return ((NumericUpDown)c).Value;

            if (c is DateTimePicker)
                return ((DateTimePicker)c).Value;

            if (c is TextBox)
            {
                var text = ((TextBox)c).Text;
                return string.IsNullOrWhiteSpace(text) ? (object)null : text;
            }

            if (c is ComboBox)
            {
                var cb = (ComboBox)c;
                if (_property.Type.IsBoolean())
                    return (object)bool.Parse((string)cb.SelectedItem);
                return cb.SelectedItem;
            }

            return null;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var op = (FilterOp)Enum.Parse(typeof(FilterOp), (string)cmbOperator.SelectedItem);
            Result = new FilterCondition
            {
                PropertyName = _property.Name,
                Type = _property.Type,
                Op = op,
                Value = ReadValue(_editor),
                Value2 = op == FilterOp.Between ? ReadValue(_editor2) : null
            };
            if (op != FilterOp.IsNull && op != FilterOp.IsNotNull && Result.Value == null)
            {
                MessageBox.Show("Укажите значение");
                DialogResult = DialogResult.None;
                return;
            }
            DialogResult = DialogResult.OK;
        }

        
    }

    public static class ODataFilterBuilder
    {
        public static string Build(IReadOnlyList<FilterCondition> conditions)
        {
            var parts = new List<string>();
            foreach (var c in conditions)
            {
                var s = BuildOne(c);
                if (s != null) parts.Add(s);
            }
            return string.Join(" and ", parts);
        }

        static string BuildOne(FilterCondition c)
        {
            if (c.Op == FilterOp.IsNull) return c.PropertyName + " eq null";
            if (c.Op == FilterOp.IsNotNull) return c.PropertyName + " ne null";
            if (c.Value == null) return null;

            switch (c.Op)
            {
                case FilterOp.Between:
                    return string.Format("{0} ge {1} and {0} le {2}",
                        c.PropertyName, Format(c.Type, c.Value), Format(c.Type, c.Value2));
                case FilterOp.Contains:
                    return string.Format("contains({0}, {1})", c.PropertyName, Format(c.Type, c.Value));
                case FilterOp.StartsWith:
                    return string.Format("startswith({0}, {1})", c.PropertyName, Format(c.Type, c.Value));
                case FilterOp.EndsWith:
                    return string.Format("endswith({0}, {1})", c.PropertyName, Format(c.Type, c.Value));
                default:
                    return string.Format("{0} {1} {2}", c.PropertyName, OpToStr(c.Op), Format(c.Type, c.Value));
            }
        }

        static string OpToStr(FilterOp op)
        {
            switch (op)
            {
                case FilterOp.Eq: return "eq";
                case FilterOp.Ne: return "ne";
                case FilterOp.Gt: return "gt";
                case FilterOp.Ge: return "ge";
                case FilterOp.Lt: return "lt";
                case FilterOp.Le: return "le";
                default: throw new ArgumentOutOfRangeException("op");
            }
        }

        static string Format(IEdmTypeReference type, object value)
        {
            var prim = type.AsPrimitive();
            var kind = prim.PrimitiveKind();

            switch (kind)
            {
                case EdmPrimitiveTypeKind.String:
                    return "'" + ((string)value).Replace("'", "''") + "'";
                case EdmPrimitiveTypeKind.Boolean:
                    return (bool)value ? "true" : "false";
                case EdmPrimitiveTypeKind.Guid:
                    return "guid'" + value + "'";
                case EdmPrimitiveTypeKind.Date:
                    return "date'" + ((DateTime)value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "'";
                case EdmPrimitiveTypeKind.DateTimeOffset:
                    return "datetimeoffset'" + ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture) + "'";
                case EdmPrimitiveTypeKind.Decimal:
                    return ((decimal)value).ToString(CultureInfo.InvariantCulture);
                case EdmPrimitiveTypeKind.Double:
                case EdmPrimitiveTypeKind.Single:
                    return Convert.ToDouble(value).ToString("R", CultureInfo.InvariantCulture);
                default:
                    return Convert.ToString(value, CultureInfo.InvariantCulture);
            }
        }
    }
}

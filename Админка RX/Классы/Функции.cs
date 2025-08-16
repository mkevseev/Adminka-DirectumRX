using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Админка_RX.Классы
{
    public class Функции
    {
        
        public static void СоздатьКолонку(DataGridView таблица, DataGridViewColumn column, string name, string headerText, DataGridViewAutoSizeColumnMode autoSizeColumnMode, bool visible, bool readOnly)
        {
            column.AutoSizeMode = autoSizeColumnMode;
            column.Name = name;
            column.HeaderText = headerText;
            column.Visible = visible;
            column.ReadOnly = readOnly;
            таблица.Columns.Add(column);

        }

            
    }
}

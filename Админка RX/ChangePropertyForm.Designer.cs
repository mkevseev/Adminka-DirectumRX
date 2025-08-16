namespace Админка_RX
{
    partial class ChangePropertyForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Таблица = new System.Windows.Forms.DataGridView();
            this.ТипСущности = new System.Windows.Forms.Label();
            this.ИДЗаписи = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Изменить = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Таблица)).BeginInit();
            this.SuspendLayout();
            // 
            // Таблица
            // 
            this.Таблица.AllowUserToAddRows = false;
            this.Таблица.AllowUserToDeleteRows = false;
            this.Таблица.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Таблица.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Таблица.Location = new System.Drawing.Point(1, 0);
            this.Таблица.Name = "Таблица";
            this.Таблица.ReadOnly = true;
            this.Таблица.Size = new System.Drawing.Size(795, 599);
            this.Таблица.TabIndex = 0;
            this.Таблица.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.Таблица_CellValueChanged);
            this.Таблица.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Таблица_MouseDown);
            // 
            // ТипСущности
            // 
            this.ТипСущности.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ТипСущности.AutoSize = true;
            this.ТипСущности.Location = new System.Drawing.Point(11, 603);
            this.ТипСущности.Name = "ТипСущности";
            this.ТипСущности.Size = new System.Drawing.Size(26, 13);
            this.ТипСущности.TabIndex = 1;
            this.ТипСущности.Text = "Тип";
            // 
            // ИДЗаписи
            // 
            this.ИДЗаписи.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ИДЗаписи.AutoSize = true;
            this.ИДЗаписи.Location = new System.Drawing.Point(40, 621);
            this.ИДЗаписи.Name = "ИДЗаписи";
            this.ИДЗаписи.Size = new System.Drawing.Size(19, 13);
            this.ИДЗаписи.TabIndex = 2;
            this.ИДЗаписи.Text = "ид";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 621);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "ИД:";
            // 
            // Изменить
            // 
            this.Изменить.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Изменить.Location = new System.Drawing.Point(321, 608);
            this.Изменить.Name = "Изменить";
            this.Изменить.Size = new System.Drawing.Size(153, 23);
            this.Изменить.TabIndex = 4;
            this.Изменить.Text = "Сохранить все изменения";
            this.Изменить.UseVisualStyleBackColor = true;
            this.Изменить.Click += new System.EventHandler(this.button1_Click);
            // 
            // ChangePropertyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 640);
            this.Controls.Add(this.Изменить);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ИДЗаписи);
            this.Controls.Add(this.ТипСущности);
            this.Controls.Add(this.Таблица);
            this.Name = "ChangePropertyForm";
            this.Text = "Изменение реквизитов";
            this.Load += new System.EventHandler(this.ChangePropertyForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Таблица)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView Таблица;
        public System.Windows.Forms.Label ТипСущности;
        public System.Windows.Forms.Label ИДЗаписи;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Изменить;
    }
}
namespace Админка_RX
{
    partial class SelectForm
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
            this.Выбрать = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ИДЗаписи = new System.Windows.Forms.Label();
            this.ТипСущности = new System.Windows.Forms.Label();
            this.Таблица = new System.Windows.Forms.DataGridView();
            this.Фильтр = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ФильтрТекст = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Таблица)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Фильтр)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Выбрать
            // 
            this.Выбрать.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Выбрать.Location = new System.Drawing.Point(541, 649);
            this.Выбрать.Name = "Выбрать";
            this.Выбрать.Size = new System.Drawing.Size(75, 23);
            this.Выбрать.TabIndex = 21;
            this.Выбрать.Text = "Выбрать";
            this.Выбрать.UseVisualStyleBackColor = true;
            this.Выбрать.Click += new System.EventHandler(this.Выбрать_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 663);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "ИД:";
            // 
            // ИДЗаписи
            // 
            this.ИДЗаписи.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ИДЗаписи.AutoSize = true;
            this.ИДЗаписи.Location = new System.Drawing.Point(40, 663);
            this.ИДЗаписи.Name = "ИДЗаписи";
            this.ИДЗаписи.Size = new System.Drawing.Size(19, 13);
            this.ИДЗаписи.TabIndex = 19;
            this.ИДЗаписи.Text = "ид";
            // 
            // ТипСущности
            // 
            this.ТипСущности.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ТипСущности.AutoSize = true;
            this.ТипСущности.Location = new System.Drawing.Point(13, 644);
            this.ТипСущности.Name = "ТипСущности";
            this.ТипСущности.Size = new System.Drawing.Size(26, 13);
            this.ТипСущности.TabIndex = 18;
            this.ТипСущности.Text = "Тип";
            // 
            // Таблица
            // 
            this.Таблица.AllowUserToAddRows = false;
            this.Таблица.AllowUserToDeleteRows = false;
            this.Таблица.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Таблица.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Таблица.Location = new System.Drawing.Point(1, 94);
            this.Таблица.Name = "Таблица";
            this.Таблица.ReadOnly = true;
            this.Таблица.Size = new System.Drawing.Size(1162, 542);
            this.Таблица.TabIndex = 17;
            this.Таблица.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.Таблица_CellMouseDoubleClick);
            this.Таблица.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Таблица_Scroll);
            this.Таблица.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Таблица_MouseDown);
            // 
            // Фильтр
            // 
            this.Фильтр.AllowUserToAddRows = false;
            this.Фильтр.AllowUserToDeleteRows = false;
            this.Фильтр.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Фильтр.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Фильтр.Location = new System.Drawing.Point(0, 19);
            this.Фильтр.Name = "Фильтр";
            this.Фильтр.Size = new System.Drawing.Size(1162, 68);
            this.Фильтр.TabIndex = 22;
            this.Фильтр.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.Фильтр_CellValueChanged);
            this.Фильтр.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Фильтр_Scroll);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ФильтрТекст);
            this.groupBox1.Controls.Add(this.Фильтр);
            this.groupBox1.Location = new System.Drawing.Point(1, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1162, 100);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Фильтр:";
            // 
            // ФильтрТекст
            // 
            this.ФильтрТекст.Location = new System.Drawing.Point(58, -2);
            this.ФильтрТекст.Name = "ФильтрТекст";
            this.ФильтрТекст.ReadOnly = true;
            this.ФильтрТекст.Size = new System.Drawing.Size(1104, 20);
            this.ФильтрТекст.TabIndex = 23;
            this.ФильтрТекст.TextChanged += new System.EventHandler(this.ФильтрТекст_TextChanged);
            // 
            // SelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1165, 684);
            this.Controls.Add(this.Выбрать);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ИДЗаписи);
            this.Controls.Add(this.ТипСущности);
            this.Controls.Add(this.Таблица);
            this.Controls.Add(this.groupBox1);
            this.Name = "SelectForm";
            this.Text = "SelectForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SelectForm_FormClosed);
            this.Load += new System.EventHandler(this.SelectForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Таблица)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Фильтр)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Выбрать;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label ИДЗаписи;
        public System.Windows.Forms.Label ТипСущности;
        private System.Windows.Forms.DataGridView Таблица;
        private System.Windows.Forms.DataGridView Фильтр;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox ФильтрТекст;
    }
}
namespace Админка_RX
{
    partial class DopChangeForm
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
            this.Изменить = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ИДЗаписи = new System.Windows.Forms.Label();
            this.ТипСущности = new System.Windows.Forms.Label();
            this.Таблица = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.Таблица)).BeginInit();
            this.SuspendLayout();
            // 
            // Изменить
            // 
            this.Изменить.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Изменить.Location = new System.Drawing.Point(328, 606);
            this.Изменить.Name = "Изменить";
            this.Изменить.Size = new System.Drawing.Size(220, 23);
            this.Изменить.TabIndex = 10;
            this.Изменить.Text = "Подтвердить изменения коллекции";
            this.Изменить.UseVisualStyleBackColor = true;
            this.Изменить.Click += new System.EventHandler(this.Изменить_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 616);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "ИД:";
            // 
            // ИДЗаписи
            // 
            this.ИДЗаписи.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ИДЗаписи.AutoSize = true;
            this.ИДЗаписи.Location = new System.Drawing.Point(40, 616);
            this.ИДЗаписи.Name = "ИДЗаписи";
            this.ИДЗаписи.Size = new System.Drawing.Size(19, 13);
            this.ИДЗаписи.TabIndex = 8;
            this.ИДЗаписи.Text = "ид";
            // 
            // ТипСущности
            // 
            this.ТипСущности.AutoSize = true;
            this.ТипСущности.Location = new System.Drawing.Point(13, 594);
            this.ТипСущности.Name = "ТипСущности";
            this.ТипСущности.Size = new System.Drawing.Size(26, 13);
            this.ТипСущности.TabIndex = 7;
            this.ТипСущности.Text = "Тип";
            // 
            // Таблица
            // 
            this.Таблица.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Таблица.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Таблица.Location = new System.Drawing.Point(2, 1);
            this.Таблица.Name = "Таблица";
            this.Таблица.Size = new System.Drawing.Size(877, 587);
            this.Таблица.TabIndex = 6;
            this.Таблица.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Таблица_MouseDown);
            // 
            // DopChangeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 637);
            this.Controls.Add(this.Изменить);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ИДЗаписи);
            this.Controls.Add(this.ТипСущности);
            this.Controls.Add(this.Таблица);
            this.Name = "DopChangeForm";
            this.Text = "Изменение реквизита:";
            this.Load += new System.EventHandler(this.DopChangeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Таблица)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Изменить;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label ИДЗаписи;
        public System.Windows.Forms.Label ТипСущности;
        private System.Windows.Forms.DataGridView Таблица;
    }
}
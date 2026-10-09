namespace Админка_RX
{
    partial class RecordMigrationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecordMigrationForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ТипtoolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ИЗtoolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.СсылкаtoolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.ВtoolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.Ссылка2toolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.Таблица = new System.Windows.Forms.DataGridView();
            this.ПеренестиКнопка = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Таблица)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ТипtoolStripLabel,
            this.toolStripSeparator1,
            this.ИЗtoolStripLabel,
            this.СсылкаtoolStripLabel,
            this.toolStripButton1,
            this.ВtoolStripLabel,
            this.Ссылка2toolStripLabel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(712, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ТипtoolStripLabel
            // 
            this.ТипtoolStripLabel.Name = "ТипtoolStripLabel";
            this.ТипtoolStripLabel.Size = new System.Drawing.Size(31, 22);
            this.ТипtoolStripLabel.Text = "Тип:";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ИЗtoolStripLabel
            // 
            this.ИЗtoolStripLabel.Name = "ИЗtoolStripLabel";
            this.ИЗtoolStripLabel.Size = new System.Drawing.Size(89, 22);
            this.ИЗtoolStripLabel.Text = "Переносим из:";
            // 
            // СсылкаtoolStripLabel
            // 
            this.СсылкаtoolStripLabel.Name = "СсылкаtoolStripLabel";
            this.СсылкаtoolStripLabel.Size = new System.Drawing.Size(49, 22);
            this.СсылкаtoolStripLabel.Text = "Ссылка";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(32, 22);
            this.toolStripButton1.Text = "<->";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // ВtoolStripLabel
            // 
            this.ВtoolStripLabel.Name = "ВtoolStripLabel";
            this.ВtoolStripLabel.Size = new System.Drawing.Size(16, 22);
            this.ВtoolStripLabel.Text = "в:";
            // 
            // Ссылка2toolStripLabel
            // 
            this.Ссылка2toolStripLabel.Name = "Ссылка2toolStripLabel";
            this.Ссылка2toolStripLabel.Size = new System.Drawing.Size(49, 22);
            this.Ссылка2toolStripLabel.Text = "Ссылка";
            // 
            // Таблица
            // 
            this.Таблица.AllowUserToAddRows = false;
            this.Таблица.AllowUserToDeleteRows = false;
            this.Таблица.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Таблица.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Таблица.Location = new System.Drawing.Point(0, 28);
            this.Таблица.Name = "Таблица";
            this.Таблица.Size = new System.Drawing.Size(712, 422);
            this.Таблица.TabIndex = 18;
            // 
            // ПеренестиКнопка
            // 
            this.ПеренестиКнопка.Location = new System.Drawing.Point(306, 456);
            this.ПеренестиКнопка.Name = "ПеренестиКнопка";
            this.ПеренестиКнопка.Size = new System.Drawing.Size(75, 23);
            this.ПеренестиКнопка.TabIndex = 19;
            this.ПеренестиКнопка.Text = "Перенести";
            this.ПеренестиКнопка.UseVisualStyleBackColor = true;
            this.ПеренестиКнопка.Click += new System.EventHandler(this.ПеренестиКнопка_Click);
            // 
            // RecordMigrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 487);
            this.Controls.Add(this.ПеренестиКнопка);
            this.Controls.Add(this.Таблица);
            this.Controls.Add(this.toolStrip1);
            this.Name = "RecordMigrationForm";
            this.Text = "RecordMigrationForm";
            this.Load += new System.EventHandler(this.RecordMigrationForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Таблица)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel ТипtoolStripLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.DataGridView Таблица;
        private System.Windows.Forms.ToolStripLabel ИЗtoolStripLabel;
        private System.Windows.Forms.ToolStripLabel СсылкаtoolStripLabel;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripLabel ВtoolStripLabel;
        private System.Windows.Forms.ToolStripLabel Ссылка2toolStripLabel;
        private System.Windows.Forms.Button ПеренестиКнопка;
    }
}
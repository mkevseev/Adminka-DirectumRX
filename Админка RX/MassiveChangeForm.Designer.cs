namespace Админка_RX
{
    partial class MassiveChangeForm
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
            this.Фильтр = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ТипtoolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ФильтрtoolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.ФитьтрТекстtoolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.Таблица)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Фильтр)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
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
            this.Таблица.Location = new System.Drawing.Point(1, 111);
            this.Таблица.Name = "Таблица";
            this.Таблица.ReadOnly = true;
            this.Таблица.Size = new System.Drawing.Size(1162, 437);
            this.Таблица.TabIndex = 17;
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
            this.Фильтр.Location = new System.Drawing.Point(0, 17);
            this.Фильтр.Name = "Фильтр";
            this.Фильтр.Size = new System.Drawing.Size(1161, 56);
            this.Фильтр.TabIndex = 22;
            this.Фильтр.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.Фильтр_CellValueChanged);
            this.Фильтр.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Фильтр_Scroll);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.Фильтр);
            this.groupBox1.Location = new System.Drawing.Point(1, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1164, 81);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Фильтр:";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ТипtoolStripLabel,
            this.toolStripSeparator1,
            this.ФильтрtoolStripLabel,
            this.ФитьтрТекстtoolStripLabel,
            this.toolStripSeparator2,
            this.toolStripProgressBar1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1165, 25);
            this.toolStrip1.TabIndex = 24;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ТипtoolStripLabel
            // 
            this.ТипtoolStripLabel.Name = "ТипtoolStripLabel";
            this.ТипtoolStripLabel.Size = new System.Drawing.Size(33, 22);
            this.ТипtoolStripLabel.Text = "Тип: ";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ФильтрtoolStripLabel
            // 
            this.ФильтрtoolStripLabel.Name = "ФильтрtoolStripLabel";
            this.ФильтрtoolStripLabel.Size = new System.Drawing.Size(51, 22);
            this.ФильтрtoolStripLabel.Text = "Фильтр:";
            // 
            // ФитьтрТекстtoolStripLabel
            // 
            this.ФитьтрТекстtoolStripLabel.Name = "ФитьтрТекстtoolStripLabel";
            this.ФитьтрТекстtoolStripLabel.Size = new System.Drawing.Size(0, 22);
            this.ФитьтрТекстtoolStripLabel.TextChanged += new System.EventHandler(this.ФильтрТекст_TextChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 22);
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // MassiveChangeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1165, 550);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.Таблица);
            this.Controls.Add(this.groupBox1);
            this.Name = "MassiveChangeForm";
            this.Text = "SelectForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SelectForm_FormClosed);
            this.Load += new System.EventHandler(this.SelectForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Таблица)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Фильтр)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView Таблица;
        private System.Windows.Forms.DataGridView Фильтр;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel ФильтрtoolStripLabel;
        private System.Windows.Forms.ToolStripLabel ФитьтрТекстtoolStripLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        public System.Windows.Forms.ToolStripLabel ТипtoolStripLabel;
    }
}
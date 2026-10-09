namespace Админка_RX
{
    partial class SettingForm
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
            this.Ссылка = new System.Windows.Forms.TextBox();
            this.Пароль = new System.Windows.Forms.TextBox();
            this.Логин = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Администратор = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.ЗагрузкаBar = new System.Windows.Forms.ProgressBar();
            this.ЗагрузитьФайлы = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Ссылка2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Администратор2 = new System.Windows.Forms.CheckBox();
            this.Логин2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Пароль2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // Ссылка
            // 
            this.Ссылка.Location = new System.Drawing.Point(60, 19);
            this.Ссылка.Name = "Ссылка";
            this.Ссылка.Size = new System.Drawing.Size(269, 20);
            this.Ссылка.TabIndex = 0;
            this.Ссылка.TextChanged += new System.EventHandler(this.Ссылка_TextChanged);
            // 
            // Пароль
            // 
            this.Пароль.Location = new System.Drawing.Point(223, 45);
            this.Пароль.Name = "Пароль";
            this.Пароль.Size = new System.Drawing.Size(100, 20);
            this.Пароль.TabIndex = 1;
            this.Пароль.TextChanged += new System.EventHandler(this.Пароль_TextChanged);
            // 
            // Логин
            // 
            this.Логин.Location = new System.Drawing.Point(60, 45);
            this.Логин.Name = "Логин";
            this.Логин.Size = new System.Drawing.Size(100, 20);
            this.Логин.TabIndex = 2;
            this.Логин.TextChanged += new System.EventHandler(this.Логин_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ссылка:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Логин:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(169, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Пароль:";
            // 
            // Администратор
            // 
            this.Администратор.AutoSize = true;
            this.Администратор.Enabled = false;
            this.Администратор.Location = new System.Drawing.Point(8, 71);
            this.Администратор.Name = "Администратор";
            this.Администратор.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Администратор.Size = new System.Drawing.Size(105, 17);
            this.Администратор.TabIndex = 7;
            this.Администратор.Text = "Администратор";
            this.Администратор.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.ЗагрузкаBar);
            this.groupBox1.Controls.Add(this.ЗагрузитьФайлы);
            this.groupBox1.Location = new System.Drawing.Point(3, 203);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(345, 72);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Загрузка:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(174, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(165, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Поправить файлы";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ЗагрузкаBar
            // 
            this.ЗагрузкаBar.Location = new System.Drawing.Point(6, 43);
            this.ЗагрузкаBar.Name = "ЗагрузкаBar";
            this.ЗагрузкаBar.Size = new System.Drawing.Size(333, 23);
            this.ЗагрузкаBar.TabIndex = 1;
            // 
            // ЗагрузитьФайлы
            // 
            this.ЗагрузитьФайлы.AllowDrop = true;
            this.ЗагрузитьФайлы.Location = new System.Drawing.Point(6, 19);
            this.ЗагрузитьФайлы.Name = "ЗагрузитьФайлы";
            this.ЗагрузитьФайлы.Size = new System.Drawing.Size(165, 23);
            this.ЗагрузитьФайлы.TabIndex = 0;
            this.ЗагрузитьФайлы.Text = "Загрузить файлы";
            this.ЗагрузитьФайлы.UseVisualStyleBackColor = true;
            this.ЗагрузитьФайлы.Click += new System.EventHandler(this.ЗагрузитьФайлы_Click);
            this.ЗагрузитьФайлы.DragDrop += new System.Windows.Forms.DragEventHandler(this.ЗагрузитьФайлы_DragDrop);
            this.ЗагрузитьФайлы.DragEnter += new System.Windows.Forms.DragEventHandler(this.ЗагрузитьФайлы_DragEnter);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Ссылка);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.Администратор);
            this.groupBox2.Controls.Add(this.Логин);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.Пароль);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(3, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(345, 93);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Система 1";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Ссылка2);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.Администратор2);
            this.groupBox3.Controls.Add(this.Логин2);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.Пароль2);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(3, 106);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(345, 91);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Система 2";
            // 
            // Ссылка2
            // 
            this.Ссылка2.Location = new System.Drawing.Point(60, 19);
            this.Ссылка2.Name = "Ссылка2";
            this.Ссылка2.Size = new System.Drawing.Size(269, 20);
            this.Ссылка2.TabIndex = 0;
            this.Ссылка2.TextChanged += new System.EventHandler(this.Ссылка2_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Ссылка:";
            // 
            // Администратор2
            // 
            this.Администратор2.AutoSize = true;
            this.Администратор2.Enabled = false;
            this.Администратор2.Location = new System.Drawing.Point(8, 71);
            this.Администратор2.Name = "Администратор2";
            this.Администратор2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Администратор2.Size = new System.Drawing.Size(105, 17);
            this.Администратор2.TabIndex = 7;
            this.Администратор2.Text = "Администратор";
            this.Администратор2.UseVisualStyleBackColor = true;
            // 
            // Логин2
            // 
            this.Логин2.Location = new System.Drawing.Point(60, 45);
            this.Логин2.Name = "Логин2";
            this.Логин2.Size = new System.Drawing.Size(100, 20);
            this.Логин2.TabIndex = 2;
            this.Логин2.TextChanged += new System.EventHandler(this.Логин2_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(169, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Пароль:";
            // 
            // Пароль2
            // 
            this.Пароль2.Location = new System.Drawing.Point(223, 45);
            this.Пароль2.Name = "Пароль2";
            this.Пароль2.Size = new System.Drawing.Size(100, 20);
            this.Пароль2.TabIndex = 1;
            this.Пароль2.TextChanged += new System.EventHandler(this.Пароль2_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Логин:";
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 279);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "SettingForm";
            this.Text = "Настройка";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingForm_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox Ссылка;
        private System.Windows.Forms.TextBox Пароль;
        private System.Windows.Forms.TextBox Логин;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.CheckBox Администратор;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ProgressBar ЗагрузкаBar;
        private System.Windows.Forms.Button ЗагрузитьФайлы;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox Ссылка2;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.CheckBox Администратор2;
        private System.Windows.Forms.TextBox Логин2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Пароль2;
        private System.Windows.Forms.Label label6;
    }
}
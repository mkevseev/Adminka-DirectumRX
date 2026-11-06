namespace Админка_RX
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ИД = new System.Windows.Forms.TextBox();
            this.ВсёОК = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.Статус = new System.Windows.Forms.ToolStripStatusLabel();
            this.СтатусЦвет = new System.Windows.Forms.ToolStripProgressBar();
            this.ОжиданиеtoolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ОбработкаProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.КнопкаОбновить = new System.Windows.Forms.ToolStripButton();
            this.ЗагрузитьВсеИДОбязательно = new System.Windows.Forms.ToolStripButton();
            this.КнопкаНастройка = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Действия = new System.Windows.Forms.ToolStripDropDownButton();
            this.Поиск = new System.Windows.Forms.ToolStripMenuItem();
            this.ПоискПоИДПунктДействия = new System.Windows.Forms.ToolStripMenuItem();
            this.ГдеИспользуетсяПунктДействия = new System.Windows.Forms.ToolStripMenuItem();
            this.ИзменитьРеквизиты = new System.Windows.Forms.ToolStripMenuItem();
            this.МассовоеИзменениеРеквизитов = new System.Windows.Forms.ToolStripMenuItem();
            this.ИсторияРаботы = new System.Windows.Forms.ToolStripMenuItem();
            this.МиграцияЗаписей = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ИД
            // 
            this.ИД.Location = new System.Drawing.Point(13, 28);
            this.ИД.Name = "ИД";
            this.ИД.Size = new System.Drawing.Size(214, 20);
            this.ИД.TabIndex = 2;
            // 
            // ВсёОК
            // 
            this.ВсёОК.AutoSize = true;
            this.ВсёОК.Location = new System.Drawing.Point(-94, 12);
            this.ВсёОК.Name = "ВсёОК";
            this.ВсёОК.Size = new System.Drawing.Size(80, 17);
            this.ВсёОК.TabIndex = 4;
            this.ВсёОК.Text = "checkBox1";
            this.ВсёОК.UseVisualStyleBackColor = true;
            this.ВсёОК.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.ИД);
            this.panel1.Location = new System.Drawing.Point(-1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(238, 71);
            this.panel1.TabIndex = 6;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Статус,
            this.СтатусЦвет,
            this.ОжиданиеtoolStripStatusLabel1,
            this.ОбработкаProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 49);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(238, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // Статус
            // 
            this.Статус.Name = "Статус";
            this.Статус.Size = new System.Drawing.Size(49, 17);
            this.Статус.Text = "Статус: ";
            // 
            // СтатусЦвет
            // 
            this.СтатусЦвет.BackColor = System.Drawing.SystemColors.Control;
            this.СтатусЦвет.MarqueeAnimationSpeed = 1;
            this.СтатусЦвет.Maximum = 1;
            this.СтатусЦвет.Name = "СтатусЦвет";
            this.СтатусЦвет.Size = new System.Drawing.Size(10, 16);
            // 
            // ОжиданиеtoolStripStatusLabel1
            // 
            this.ОжиданиеtoolStripStatusLabel1.Name = "ОжиданиеtoolStripStatusLabel1";
            this.ОжиданиеtoolStripStatusLabel1.Size = new System.Drawing.Size(67, 17);
            this.ОжиданиеtoolStripStatusLabel1.Text = "Ожидание:";
            // 
            // ОбработкаProgressBar
            // 
            this.ОбработкаProgressBar.Name = "ОбработкаProgressBar";
            this.ОбработкаProgressBar.Size = new System.Drawing.Size(80, 16);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.КнопкаОбновить,
            this.ЗагрузитьВсеИДОбязательно,
            this.КнопкаНастройка,
            this.toolStripSeparator1,
            this.Действия});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(238, 25);
            this.toolStrip1.TabIndex = 9;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // КнопкаОбновить
            // 
            this.КнопкаОбновить.AutoToolTip = false;
            this.КнопкаОбновить.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.КнопкаОбновить.Image = global::Админка_RX.Properties.Resources.Обновить;
            this.КнопкаОбновить.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.КнопкаОбновить.Name = "КнопкаОбновить";
            this.КнопкаОбновить.Size = new System.Drawing.Size(23, 22);
            this.КнопкаОбновить.Text = "toolStripButton1";
            this.КнопкаОбновить.ToolTipText = "Обновить данные";
            this.КнопкаОбновить.Click += new System.EventHandler(this.КнопкаОбновить_Click);
            // 
            // ЗагрузитьВсеИДОбязательно
            // 
            this.ЗагрузитьВсеИДОбязательно.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ЗагрузитьВсеИДОбязательно.Image = global::Админка_RX.Properties.Resources.Стрелка_вниз;
            this.ЗагрузитьВсеИДОбязательно.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ЗагрузитьВсеИДОбязательно.Name = "ЗагрузитьВсеИДОбязательно";
            this.ЗагрузитьВсеИДОбязательно.Size = new System.Drawing.Size(23, 22);
            this.ЗагрузитьВсеИДОбязательно.ToolTipText = "Загрузить ИД";
            this.ЗагрузитьВсеИДОбязательно.Click += new System.EventHandler(this.ЗагрузитьВсеИДОбязательно_Click);
            // 
            // КнопкаНастройка
            // 
            this.КнопкаНастройка.AutoToolTip = false;
            this.КнопкаНастройка.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.КнопкаНастройка.Image = global::Админка_RX.Properties.Resources.Настройка;
            this.КнопкаНастройка.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.КнопкаНастройка.Name = "КнопкаНастройка";
            this.КнопкаНастройка.Size = new System.Drawing.Size(23, 22);
            this.КнопкаНастройка.Text = "toolStripButton2";
            this.КнопкаНастройка.ToolTipText = "Настройка";
            this.КнопкаНастройка.Click += new System.EventHandler(this.КнопкаНастройка_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // Действия
            // 
            this.Действия.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Поиск,
            this.ИзменитьРеквизиты,
            this.ИсторияРаботы,
            this.МиграцияЗаписей});
            this.Действия.Image = global::Админка_RX.Properties.Resources.Меню;
            this.Действия.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Действия.Name = "Действия";
            this.Действия.Size = new System.Drawing.Size(87, 22);
            this.Действия.Text = "Действия";
            // 
            // Поиск
            // 
            this.Поиск.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ПоискПоИДПунктДействия,
            this.ГдеИспользуетсяПунктДействия});
            this.Поиск.Image = global::Админка_RX.Properties.Resources.Поиск;
            this.Поиск.Name = "Поиск";
            this.Поиск.Size = new System.Drawing.Size(189, 22);
            this.Поиск.Text = "Поиск";
            this.Поиск.Click += new System.EventHandler(this.Поиск_Click);
            // 
            // ПоискПоИДПунктДействия
            // 
            this.ПоискПоИДПунктДействия.Image = global::Админка_RX.Properties.Resources.Поиск_по_ИД;
            this.ПоискПоИДПунктДействия.Name = "ПоискПоИДПунктДействия";
            this.ПоискПоИДПунктДействия.Size = new System.Drawing.Size(180, 22);
            this.ПоискПоИДПунктДействия.Text = "Поиск по ИД";
            this.ПоискПоИДПунктДействия.Click += new System.EventHandler(this.ПоискПоИД_Click);
            // 
            // ГдеИспользуетсяПунктДействия
            // 
            this.ГдеИспользуетсяПунктДействия.Image = global::Админка_RX.Properties.Resources.Где_используется;
            this.ГдеИспользуетсяПунктДействия.Name = "ГдеИспользуетсяПунктДействия";
            this.ГдеИспользуетсяПунктДействия.Size = new System.Drawing.Size(180, 22);
            this.ГдеИспользуетсяПунктДействия.Text = "Где используется";
            this.ГдеИспользуетсяПунктДействия.Click += new System.EventHandler(this.ГдеИспользуется_Click);
            // 
            // ИзменитьРеквизиты
            // 
            this.ИзменитьРеквизиты.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.МассовоеИзменениеРеквизитов});
            this.ИзменитьРеквизиты.Image = global::Админка_RX.Properties.Resources.Редактировать_реквизиты;
            this.ИзменитьРеквизиты.Name = "ИзменитьРеквизиты";
            this.ИзменитьРеквизиты.Size = new System.Drawing.Size(189, 22);
            this.ИзменитьРеквизиты.Text = "Изменить реквизиты";
            this.ИзменитьРеквизиты.Click += new System.EventHandler(this.ИзменитьРеквизиты_Click);
            // 
            // МассовоеИзменениеРеквизитов
            // 
            this.МассовоеИзменениеРеквизитов.Image = global::Админка_RX.Properties.Resources.Массовое_изменение_реквизитов;
            this.МассовоеИзменениеРеквизитов.Name = "МассовоеИзменениеРеквизитов";
            this.МассовоеИзменениеРеквизитов.Size = new System.Drawing.Size(257, 22);
            this.МассовоеИзменениеРеквизитов.Text = "Массовое изменение реквизитов";
            this.МассовоеИзменениеРеквизитов.Click += new System.EventHandler(this.МассовоеИзменениеРеквизитов_Click);
            // 
            // ИсторияРаботы
            // 
            this.ИсторияРаботы.Image = global::Админка_RX.Properties.Resources.История_работы;
            this.ИсторияРаботы.Name = "ИсторияРаботы";
            this.ИсторияРаботы.Size = new System.Drawing.Size(189, 22);
            this.ИсторияРаботы.Text = "История работы";
            this.ИсторияРаботы.Click += new System.EventHandler(this.ИсторияРаботы_Click);
            // 
            // МиграцияЗаписей
            // 
            this.МиграцияЗаписей.Name = "МиграцияЗаписей";
            this.МиграцияЗаписей.Size = new System.Drawing.Size(189, 22);
            this.МиграцияЗаписей.Text = "Миграция записей";
            this.МиграцияЗаписей.Click += new System.EventHandler(this.МиграцияЗаписей_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(238, 72);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ВсёОК);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Админка RX";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox ИД;
        private System.Windows.Forms.CheckBox ВсёОК;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton КнопкаОбновить;
        private System.Windows.Forms.ToolStripButton КнопкаНастройка;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel Статус;
        private System.Windows.Forms.ToolStripProgressBar СтатусЦвет;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripStatusLabel ОжиданиеtoolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar ОбработкаProgressBar;
        private System.Windows.Forms.ToolStripDropDownButton Действия;
        private System.Windows.Forms.ToolStripMenuItem Поиск;
        private System.Windows.Forms.ToolStripMenuItem ИзменитьРеквизиты;
        private System.Windows.Forms.ToolStripButton ЗагрузитьВсеИДОбязательно;
        private System.Windows.Forms.ToolStripMenuItem МиграцияЗаписей;
        private System.Windows.Forms.ToolStripMenuItem МассовоеИзменениеРеквизитов;
        private System.Windows.Forms.ToolStripMenuItem ИсторияРаботы;
        private System.Windows.Forms.ToolStripMenuItem ПоискПоИДПунктДействия;
        private System.Windows.Forms.ToolStripMenuItem ГдеИспользуетсяПунктДействия;
    }
}


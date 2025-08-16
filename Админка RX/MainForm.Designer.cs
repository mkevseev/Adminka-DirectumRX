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
            this.components = new System.ComponentModel.Container();
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
            this.ПоискПоИДПунктДействия = new System.Windows.Forms.ToolStripMenuItem();
            this.ИзменитьРеквизитыПунктДействия = new System.Windows.Forms.ToolStripMenuItem();
            this.МассовоеИзменениеРеквизитовПунктДействия = new System.Windows.Forms.ToolStripMenuItem();
            this.ТипыСущностейПодпунктМИРДействия = new System.Windows.Forms.ToolStripComboBox();
            this.ИзменитьПодпунктМИРДействия = new System.Windows.Forms.ToolStripMenuItem();
            this.ВремяАвтоОбновл = new System.Windows.Forms.Timer(this.components);
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
            this.ПоискПоИДПунктДействия,
            this.ИзменитьРеквизитыПунктДействия,
            this.МассовоеИзменениеРеквизитовПунктДействия});
            this.Действия.Image = global::Админка_RX.Properties.Resources.Меню;
            this.Действия.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Действия.Name = "Действия";
            this.Действия.Size = new System.Drawing.Size(87, 22);
            this.Действия.Text = "Действия";
            // 
            // ПоискПоИДПунктДействия
            // 
            this.ПоискПоИДПунктДействия.Name = "ПоискПоИДПунктДействия";
            this.ПоискПоИДПунктДействия.Size = new System.Drawing.Size(257, 22);
            this.ПоискПоИДПунктДействия.Text = "Поиск по ИД";
            this.ПоискПоИДПунктДействия.Click += new System.EventHandler(this.ПоискПоИДПунктДействия_Click);
            // 
            // ИзменитьРеквизитыПунктДействия
            // 
            this.ИзменитьРеквизитыПунктДействия.Image = global::Админка_RX.Properties.Resources.Редактировать_реквизиты;
            this.ИзменитьРеквизитыПунктДействия.Name = "ИзменитьРеквизитыПунктДействия";
            this.ИзменитьРеквизитыПунктДействия.Size = new System.Drawing.Size(257, 22);
            this.ИзменитьРеквизитыПунктДействия.Text = "Изменить реквизиты";
            this.ИзменитьРеквизитыПунктДействия.Click += new System.EventHandler(this.ИзменитьРеквизитыПунктДействия_Click);
            // 
            // МассовоеИзменениеРеквизитовПунктДействия
            // 
            this.МассовоеИзменениеРеквизитовПунктДействия.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ТипыСущностейПодпунктМИРДействия,
            this.ИзменитьПодпунктМИРДействия});
            this.МассовоеИзменениеРеквизитовПунктДействия.Name = "МассовоеИзменениеРеквизитовПунктДействия";
            this.МассовоеИзменениеРеквизитовПунктДействия.Size = new System.Drawing.Size(257, 22);
            this.МассовоеИзменениеРеквизитовПунктДействия.Text = "Массовое изменение реквизитов";
            // 
            // ТипыСущностейПодпунктМИРДействия
            // 
            this.ТипыСущностейПодпунктМИРДействия.Name = "ТипыСущностейПодпунктМИРДействия";
            this.ТипыСущностейПодпунктМИРДействия.Size = new System.Drawing.Size(300, 23);
            // 
            // ИзменитьПодпунктМИРДействия
            // 
            this.ИзменитьПодпунктМИРДействия.Name = "ИзменитьПодпунктМИРДействия";
            this.ИзменитьПодпунктМИРДействия.Size = new System.Drawing.Size(360, 22);
            this.ИзменитьПодпунктМИРДействия.Text = "Изменить";
            this.ИзменитьПодпунктМИРДействия.Click += new System.EventHandler(this.ИзменитьПодпунктМИРДействия_Click);
            // 
            // ВремяАвтоОбновл
            // 
            this.ВремяАвтоОбновл.Tick += new System.EventHandler(this.ВремяАвтоОбновл_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(238, 72);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ВсёОК);
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
        private System.Windows.Forms.Timer ВремяАвтоОбновл;
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
        private System.Windows.Forms.ToolStripMenuItem ПоискПоИДПунктДействия;
        private System.Windows.Forms.ToolStripMenuItem ИзменитьРеквизитыПунктДействия;
        private System.Windows.Forms.ToolStripMenuItem МассовоеИзменениеРеквизитовПунктДействия;
        private System.Windows.Forms.ToolStripComboBox ТипыСущностейПодпунктМИРДействия;
        private System.Windows.Forms.ToolStripMenuItem ИзменитьПодпунктМИРДействия;
        private System.Windows.Forms.ToolStripButton ЗагрузитьВсеИДОбязательно;
    }
}


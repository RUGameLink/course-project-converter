namespace xmlReader
{
    partial class Form1
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
            this.textXml = new System.Windows.Forms.TextBox();
            this.reeadBtn = new System.Windows.Forms.Button();
            this.statusLbl = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textList = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textXml
            // 
            this.textXml.Location = new System.Drawing.Point(12, 12);
            this.textXml.Multiline = true;
            this.textXml.Name = "textXml";
            this.textXml.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textXml.Size = new System.Drawing.Size(499, 426);
            this.textXml.TabIndex = 0;
            // 
            // reeadBtn
            // 
            this.reeadBtn.Location = new System.Drawing.Point(635, 50);
            this.reeadBtn.Name = "reeadBtn";
            this.reeadBtn.Size = new System.Drawing.Size(75, 23);
            this.reeadBtn.TabIndex = 1;
            this.reeadBtn.Text = "Стартуем";
            this.reeadBtn.UseVisualStyleBackColor = true;
            this.reeadBtn.Click += new System.EventHandler(this.reeadBtn_Click);
            // 
            // statusLbl
            // 
            this.statusLbl.AutoSize = true;
            this.statusLbl.Location = new System.Drawing.Point(536, 103);
            this.statusLbl.Name = "statusLbl";
            this.statusLbl.Size = new System.Drawing.Size(0, 13);
            this.statusLbl.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // textList
            // 
            this.textList.Location = new System.Drawing.Point(517, 177);
            this.textList.Multiline = true;
            this.textList.Name = "textList";
            this.textList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textList.Size = new System.Drawing.Size(277, 261);
            this.textList.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textList);
            this.Controls.Add(this.statusLbl);
            this.Controls.Add(this.reeadBtn);
            this.Controls.Add(this.textXml);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textXml;
        private System.Windows.Forms.Button reeadBtn;
        private System.Windows.Forms.Label statusLbl;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox textList;
    }
}


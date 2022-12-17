namespace ekbToDraw
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
            this.fileBtn = new System.Windows.Forms.Button();
            this.pathBtn = new System.Windows.Forms.Button();
            this.fileImg = new System.Windows.Forms.PictureBox();
            this.pathImg = new System.Windows.Forms.PictureBox();
            this.convertBtn = new System.Windows.Forms.Button();
            this.fileNameText = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.fileImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pathImg)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileBtn
            // 
            this.fileBtn.Location = new System.Drawing.Point(24, 38);
            this.fileBtn.Name = "fileBtn";
            this.fileBtn.Size = new System.Drawing.Size(210, 68);
            this.fileBtn.TabIndex = 0;
            this.fileBtn.Text = "Выберите файл";
            this.fileBtn.UseVisualStyleBackColor = true;
            this.fileBtn.Click += new System.EventHandler(this.fileBtn_Click);
            // 
            // pathBtn
            // 
            this.pathBtn.Location = new System.Drawing.Point(24, 121);
            this.pathBtn.Name = "pathBtn";
            this.pathBtn.Size = new System.Drawing.Size(210, 68);
            this.pathBtn.TabIndex = 1;
            this.pathBtn.Text = "Выберите место\n для сохранения";
            this.pathBtn.UseVisualStyleBackColor = true;
            this.pathBtn.Click += new System.EventHandler(this.pathBtn_Click);
            // 
            // fileImg
            // 
            this.fileImg.Location = new System.Drawing.Point(261, 38);
            this.fileImg.Name = "fileImg";
            this.fileImg.Size = new System.Drawing.Size(68, 68);
            this.fileImg.TabIndex = 2;
            this.fileImg.TabStop = false;
            // 
            // pathImg
            // 
            this.pathImg.Location = new System.Drawing.Point(261, 121);
            this.pathImg.Name = "pathImg";
            this.pathImg.Size = new System.Drawing.Size(68, 68);
            this.pathImg.TabIndex = 3;
            this.pathImg.TabStop = false;
            // 
            // convertBtn
            // 
            this.convertBtn.Location = new System.Drawing.Point(24, 195);
            this.convertBtn.Name = "convertBtn";
            this.convertBtn.Size = new System.Drawing.Size(116, 68);
            this.convertBtn.TabIndex = 4;
            this.convertBtn.Text = "Конвертировать";
            this.convertBtn.UseVisualStyleBackColor = true;
            this.convertBtn.Click += new System.EventHandler(this.convertBtn_Click);
            // 
            // fileNameText
            // 
            this.fileNameText.Location = new System.Drawing.Point(6, 25);
            this.fileNameText.Name = "fileNameText";
            this.fileNameText.Size = new System.Drawing.Size(172, 20);
            this.fileNameText.TabIndex = 5;
            this.fileNameText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.fileNameText);
            this.groupBox1.Location = new System.Drawing.Point(146, 195);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(183, 68);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Имя файла";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 276);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.convertBtn);
            this.Controls.Add(this.pathImg);
            this.Controls.Add(this.fileImg);
            this.Controls.Add(this.pathBtn);
            this.Controls.Add(this.fileBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.fileImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pathImg)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button fileBtn;
        private System.Windows.Forms.Button pathBtn;
        private System.Windows.Forms.PictureBox fileImg;
        private System.Windows.Forms.PictureBox pathImg;
        private System.Windows.Forms.Button convertBtn;
        private System.Windows.Forms.TextBox fileNameText;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}


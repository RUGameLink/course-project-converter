using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace converterEKB
{
    public partial class Form1 : Form
    {
        string filename; //Путь к файлу
        string pathToDid; //Путь к директории
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Файл xml (*.xml)|*.xml|All files(*.*)|*.*";
            convertBtn.Enabled = false;
        }

        private void selectFileBtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                fileLabel.Text = "Ошибка загрузки файла!";
                fileLabel.ForeColor = Color.Red;
                filename = null;
            }
            else
            {
                // получаем выбранный файл
                filename = openFileDialog1.FileName;
                fileLabel.Text = "Файл успешно загружен";
                fileLabel.ForeColor = Color.Green;

            }
            checkToReady();
        }

        private void selectPathBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            FBD.ShowNewFolderButton = false;
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                pathLabel.Text = "Директория для сохранения успешно выбрана!";
                pathLabel.ForeColor = Color.Green;
                pathToDid = FBD.SelectedPath;
            }
            else
            {
                pathLabel.Text = "Ошибка выбора \r\nдиректории для сохранения";
                pathToDid = null;
                pathLabel.ForeColor = Color.Red;
            }
            checkToReady();
        }

        private void checkToReady()
        {
            if(filename != null && pathToDid != null)
            {
                convertBtn.Enabled = true;
            }
            else
            {
                convertBtn.Enabled = false;
            }
        }
    }
}

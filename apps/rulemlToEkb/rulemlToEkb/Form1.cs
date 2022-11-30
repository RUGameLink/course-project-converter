using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rulemlToEkb
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

        private void loadFileBtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                fileLbl.Text = "Ошибка загрузки файла!";
                fileLbl.ForeColor = Color.Red;
                filename = null;
            }
            else
            {
                // получаем выбранный файл
                filename = openFileDialog1.FileName;
                fileLbl.Text = "Файл успешно загружен";
                fileLbl.ForeColor = Color.Green;
            }
            checkToReady();
        }

        private void checkToReady()
        {
            if (filename != null && pathToDid != null)
            {
                convertBtn.Enabled = true;
            }
            else
            {
                convertBtn.Enabled = false;
            }
        }

        private void saveFileBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            FBD.ShowNewFolderButton = false;
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                pathLbl.Text = "Директория для сохранения успешно выбрана!";
                pathLbl.ForeColor = Color.Green;
                pathToDid = FBD.SelectedPath;
            }
            else
            {
                pathLbl.Text = "Ошибка выбора \r\nдиректории для сохранения";
                pathToDid = null;
                pathLbl.ForeColor = Color.Red;
            }
            checkToReady();
        }
    }
}

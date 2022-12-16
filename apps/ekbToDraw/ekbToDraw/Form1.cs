using ekbToDraw.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ekbToDraw
{
    public partial class Form1 : Form
    {
        string filename; //Путь к файлу
        string pathToDid; //Путь к директории
        bool status = false;

        //Листы объектов классов
        List<EKB> ekb;
        List<Association> associations;

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Файл ekb (*.ekb)|*.ekb|All files(*.*)|*.*"; //Фильтр файлов
            convertBtn.Enabled = false;

            ekb = new List<EKB>();
            associations = new List<Association>();
        }

        private void fileBtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                fileBtn.Text = "Ошибка загрузки файла!";
                fileBtn.ForeColor = Color.Red;
                fileImg.Image = ekbToDraw.Properties.Resources.error;
                filename = null;
            }
            else
            {
                // получаем выбранный файл
                filename = openFileDialog1.FileName;
                fileBtn.Text = "Файл успешно загружен";
                fileImg.Image = ekbToDraw.Properties.Resources.ok;
                fileBtn.ForeColor = Color.Green;

            }
            checkToReady();
        }

        private void pathBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            FBD.ShowNewFolderButton = false;
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                pathBtn.Text = "Директория для \r\nсохранения успешно выбрана!";
                pathImg.Image = ekbToDraw.Properties.Resources.ok;
                pathBtn.ForeColor = Color.Green;
                pathToDid = FBD.SelectedPath;
            }
            else
            {
                pathBtn.Text = "Ошибка выбора \r\nдиректории для сохранения";
                pathImg.Image = ekbToDraw.Properties.Resources.error;
                pathToDid = null;
                pathBtn.ForeColor = Color.Red;
            }
            checkToReady();
        }

        private void checkToReady() //Метод проверки выбора директории и файла
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

        private void convertBtn_Click(object sender, EventArgs e)
        {
            parseTheFile();
        //    convertToEKB();
            if (status)
            {
        //        saveToEkb();
            }
            else
            {
                MessageBox.Show("Проверьте файл для конвертации.", "Произошла ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void parseTheFile() //Метод парсинга файла
        {
            try
            {
                string attr = "";
                string className = "";

                string start = "";
                string end = "";
                string typeText = "";
                string assocName = "";

                List<(string, string)> Attribute;

                filename = openFileDialog1.FileName;

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(filename);
                XmlElement xRoot = xDoc.DocumentElement;
                if (xRoot != null) //Проверка корня файла на пустоту
                {
                    var child = xRoot.ChildNodes; //Начало парсинга
                    //if (child.Item(1) == null)
                    //{
                    //    status = false;
                    //    return;
                    //}
                    var know = child.Item(0).ChildNodes;
                    var templates = know.Item(6).ChildNodes;
                    for(int i = 0; i < templates.Count; i++)
                    {
                        var template = templates[i];
                        className = template.ChildNodes[1].InnerText;
                        MessageBox.Show(className);
                        var templateInside = template.ChildNodes;
                        var slots = templateInside.Item(7).ChildNodes;
                        for(int j = 0; j < slots.Count; j ++)
                        {
                            var slot = slots[j];
                            attr = slot.ChildNodes[0].InnerText;
                            typeText = slot.ChildNodes[4].InnerText;
                            Attribute.Add((attr, typeText));
                        }
                        ekb.Add(new EKB(className, Attribute));
                    }
                    Console.WriteLine();
                }
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                return;
            }
        }
    }
}

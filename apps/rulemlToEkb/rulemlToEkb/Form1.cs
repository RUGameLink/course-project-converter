using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace rulemlToEkb
{
    public partial class Form1 : Form
    {
        string filename; //Путь к файлу
        string pathToDid; //Путь к директории

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Файл ruleml (*.ruleml)|*.ruleml|All files(*.*)|*.*";
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

        private void convertBtn_Click(object sender, EventArgs e)
        {
            parseTheFile();
        }

        private void parseTheFile()
        {
            try
            {
                string attr = "";
                string func = "";

                string start = "";
                string end = "";
                string typeText = "";

                List<(string, string, string)> Attribute;

                filename = openFileDialog1.FileName;

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(filename);
                XmlElement xRoot = xDoc.DocumentElement;
                if (xRoot != null)
                {
                    var child = xRoot.ChildNodes;
                    var assert = child.Item(2);
                    var imp = assert.FirstChild;
                    var _head = imp.FirstChild;
                    var atoms = _head.ChildNodes;
                    for (int i = 0; i < atoms.Count; i++)
                    {
                        Attribute = new List<(string, string, string)>();

                        var inside = atoms[i];
                        var atom = inside.ChildNodes;
                        var op = atom.Item(0);
                        var className = op.ChildNodes.Item(0).InnerText;
                        for(int j = 1; j < atom.Count; j++)
                        {
                            typeText = atom.Item(j).Attributes["type"].Value;
                            attr = atom.Item(j).FirstChild.InnerText;
                        }
                            
                        MessageBox.Show($"{attr} {typeText}");
                            //var className = rel.Item(0).InnerText;
                            //for (int j = 1; j < inside.Count; j++)
                            //{
                            //    var param = inside[j];
                            //    var atomOp = param.ChildNodes;
                            //    var type = param.FirstChild.InnerText;
                            //    var paramName = atomOp.Item(0).ChildNodes.Item(0).InnerText;
                            //    MessageBox.Show($"{type} {paramName}");
                            //}

                            //var attribute = childClasses.Attributes["name"];
                            //if (attribute != null)
                            //{
                            //    string className = attribute.Value;

                            ////    eAs.Add(new EA(className, Attribute));

                            //}
                            attr = "";
                            func = "";

                    }
                    Console.WriteLine("efefefef");

                    int l = 0;
                    int p = 0;
                    //while (l < eAs.Count)
                    //{
                    //    p = 0;
                    //    textList.Text += $"\r\n==========\r\n" +
                    //    $"\r\n{eAs[l].ClassName}";
                    //    while (p < eAs[l].Attribute.Count)
                    //    {
                    //        textList.Text += $"\r\n{eAs[l].Attribute[p]}";

                    //        p++;
                    //    }

                    //    l++;

                    //}
                    //int q = 0;
                    //while (q < associations.Count)
                    //{
                    //    textList.Text += $"\r\n==========\r\n" +
                    //        $"{associations[q].AssotionName}" +
                    //        $"\r\n{associations[q].SourceName}" +
                    //        $"\r\n{associations[q].TargetName}";
                    //    q++;
                    //}

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

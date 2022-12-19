using ekbToDraw.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
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

        string drawText = "";

        //Листы объектов классов
        List<EKB> ekb;
        List<Association> associations;
        string deepId = "";

        List<(string, string, int, int)> dataList;

        int sourseX = 0;
        int sourseY = 0;

        int targetX = 0;
        int targetY = 0;

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
            drawText = "";

            parseTheFile();
            convertToDraw(); //Метод преобразования файла в формат EKB
            if (status)
            {
                 saveToDraw();
            }
            else
            {
                MessageBox.Show("Проверьте файл для конвертации.", "Произошла ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ekb.Clear();
            associations.Clear();
            dataList.Clear();
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

                    var know = child.Item(0).ChildNodes;
                    
                    if(know.Item(6) == null)
                    {
                        return;
                    }
                    var templates = know.Item(6).ChildNodes;
                    for (int i = 0; i < templates.Count; i++)
                    {
                        Attribute = new List<(string, string)>();
                        var template = templates[i];
                        className = template.ChildNodes[1].InnerText;
                        var templateInside = template.ChildNodes;
                            var check = templateInside.Item(7);
                            if(check == null)
                            {
                                return;
                            }

                        var slots = templateInside.Item(7).ChildNodes;
                                for (int j = 0; j < slots.Count; j++)
                                {
                                    var slot = slots[j];
                                    attr = slot.ChildNodes[0].InnerText;
                                    typeText = slot.ChildNodes[4].InnerText;
                                    Attribute.Add((attr, typeText));
                                }
                                ekb.Add(new EKB(className, Attribute));
                    }
                    var grules = know.Item(8).ChildNodes;
                    for(int i = 0; i < grules.Count; i++)
                    {
                        var grule = grules[i];
                        assocName = grule.ChildNodes[1].InnerText;
                        start = grule.ChildNodes[7].InnerText;
                        end = grule.ChildNodes[8].InnerText;
                        associations.Add(new Association(assocName, start, end));
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

        private void convertToDraw() //Метод преобразования файла в формат EKB
        {
            dataList = new List<(string, string, int, int)>();
            var drawHeader = $"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                $"\r\n<mxfile host=\"app.diagrams.net\" modified=\"2022-12-12T10:43:19.882Z\" agent=\"5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 YaBrowser/22.11.2.807 Yowser/2.5 Safari/537.36\" etag=\"o0eCUbzZ9Tfc7CzTUTEk\" version=\"20.6.2\" type=\"device\">" +
                $"\r\n  <diagram id=\"C5RBs43oDa-KdzZeNtuy\" name=\"Page-1\">" +
                $"\r\n    <mxGraphModel dx=\"1247\" dy=\"694\" grid=\"1\" gridSize=\"10\" guides=\"1\" tooltips=\"1\" connect=\"1\" arrows=\"1\" fold=\"1\" page=\"1\" pageScale=\"1\" pageWidth=\"827\" pageHeight=\"1169\" math=\"0\" shadow=\"0\">" +
                $"\r\n      <root>";
            drawText += drawHeader;
            
            var drawBody = $"        \r\n<mxCell id=\"WIyWlLk6GJQsqaUBKTNV-0\" />" +
                $"\r\n        <mxCell id=\"WIyWlLk6GJQsqaUBKTNV-1\" parent=\"WIyWlLk6GJQsqaUBKTNV-0\" />";
            drawText += drawBody;
            int count = 0;
            var x = 0;
            var y = 0;
            Random rnd = new Random();
            for (int i = 0; i < ekb.Count; i++)
            {
               
                x = rnd.Next(500) + rnd.Next(-300, 100);
                
                y = rnd.Next(400) + rnd.Next(-400, 323);

                string id = $"x-kl-r7OIvYpWUdUMo4z-{count}";
                dataList.Add((id, ekb[i].ClassName, x, y));
                int paramCount = 26;
                var classText = $"\r\n<mxCell id=\"{id}\" value=\"{ekb[i].ClassName}\" style=\"swimlane;fontStyle=0;childLayout=stackLayout;horizontal=1;startSize=26;fillColor=none;horizontalStack=0;resizeParent=1;resizeParentMax=0;resizeLast=0;collapsible=1;marginBottom=0;\" vertex=\"1\" parent=\"WIyWlLk6GJQsqaUBKTNV-1\">" +
    $"\r\n          <mxGeometry x=\"{x}\" y=\"{y}\" width=\"140\" height=\"104\" as=\"geometry\" />" +
    $"\r\n        </mxCell>";
                drawText += classText;
                count++;




                getClassName(ekb[i].ClassName);
                for (int j = 0; j < ekb[i].Attribute.Count; j++)
                {
                    var paramText = $"\r\n<mxCell id=\"x-kl-r7OIvYpWUdUMo4z-{count}\" value=\"- {ekb[i].Attribute[j].Item1}: {ekb[i].Attribute[j].Item2}\" style=\"text;strokeColor=none;fillColor=none;align=left;verticalAlign=top;spacingLeft=4;spacingRight=4;overflow=hidden;rotatable=0;points=[[0,0.5],[1,0.5]];portConstraint=eastwest;\" vertex=\"1\" parent=\"{deepId}\">" +
                        $"\r\n          <mxGeometry y=\"{paramCount}\" width=\"140\" height=\"26\" as=\"geometry\" />" +
                        $"\r\n</mxCell>";
                    count++;
                    paramCount += 26;
                    drawText += paramText;
                }
            }
            for (int j = 0; j < associations.Count; j++)
            {
                getClassCoord(associations[j].SourceName, associations[j].TargetName);
                var assocText = $"        \r\n<mxCell id=\"x-kl-r7OIvYpWUdUMo4z-{count}\" value=\"\" style=\"endArrow=open;endFill=1;endSize=12;html=1;rounded=0;exitX=1.019;exitY=0.108;exitDx=0;exitDy=0;exitPerimeter=0;entryX=-0.02;entryY=0.13;entryDx=0;entryDy=0;entryPerimeter=0;\" edge=\"1\" parent=\"WIyWlLk6GJQsqaUBKTNV-1\" source=\"{associations[j].SourceName}\" target=\"{associations[j].TargetName}\">" +
                    $"\r\n          <mxGeometry width=\"160\" relative=\"1\" as=\"geometry\">" +
                    $"\r\n            <mxPoint x=\"{sourseX}\" y=\"{sourseY}\" as=\"sourcePoint\" />" +
                    $"\r\n            <mxPoint x=\"{targetX}\" y=\"{targetY}\" as=\"targetPoint\" />" +
                    $"\r\n          </mxGeometry>" +
                    $"\r\n        </mxCell>";
                count++;
                drawText += assocText;
            }
            var drawFooter = $"      \r\n</root>" +
                $"\r\n    </mxGraphModel>" +
                $"\r\n  </diagram>" +
                $"\r\n</mxfile>";

            drawText+= drawFooter;
        }
        
        private void getClassName(string className)
        {
            foreach(var data in dataList)
            {
                if(data.Item2 == className)
                {
                    deepId =  data.Item1;
                }
            }
        }

        private void getClassCoord(string className, string classNameTarget)
        {
            foreach (var data in dataList)
            {
                if (data.Item2 == className)
                {
                    sourseX = data.Item3;
                    sourseY = data.Item4;
                }
                if (data.Item2 == classNameTarget)
                {
                    targetX = data.Item3;
                    targetY = data.Item4;
                }
            }
        }

        private void saveToDraw() //Метод сохранения файла
        {
            if(fileNameText.Text.Length == 0)
            {
                string path = $"{pathToDid}/file.xml";
                System.IO.File.WriteAllText(path, drawText);
            }
            else
            {
                string path = $"{pathToDid}/{fileNameText.Text}.xml";
                System.IO.File.WriteAllText(path, drawText);
            }
            MessageBox.Show("Файл сохранен");
        }
    }
}

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
using System.Xml.Linq;

namespace xmlReader
{
    public partial class Form1 : Form
    {
        string filename; //Путь к файлу
        List<EA> eAs;
        List<Association> associations;
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Файл xml (*.xml)|*.xml|All files(*.*)|*.*";
            eAs= new List<EA>();
            associations= new List<Association>();
        }

        private void reeadBtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                statusLbl.Text = "Ошибка загрузки файла!";
                statusLbl.ForeColor = Color.Red;
                filename = null;
            }
            else
            {
                try
                {
                    string attr = "";
                    string func = "";

                    string start = "";
                    string end = "";
                    string typeText = "";

                    List<(string, string, string, string)> Attribute = new List<(string, string, string, string)>();
                    List<(string, string, string, string)> Operation = new List<(string, string, string, string)>();

                    filename = openFileDialog1.FileName;
                    textXml.Text = filename;
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(filename);
                    XmlElement xRoot = xDoc.DocumentElement;
                    if (xRoot != null)
                    {
                        var child = xRoot.ChildNodes;
                        var content = child.Item(1);
                        var model = content.FirstChild;
                        var ownedElement = model.FirstChild;
                        var packagesTemp = ownedElement.ChildNodes;
                        var packages = packagesTemp.Item(1);
                        var ownedElementTemp = packages.ChildNodes;
                        var ownedElementClasses = ownedElementTemp.Item(1);
                        var classes = ownedElementClasses.ChildNodes;
                        for(int i = 0; i < classes.Count; i++)
                        {
                            Operation.Clear();
                            Attribute.Clear();
                            var childClasses = classes.Item(i);
                            if (childClasses.Name == "UML:Class")
                            {
                                var inside = childClasses.ChildNodes;
                                var feature = inside.Item(1);
                                var fields = feature.ChildNodes;
                                for(int j = 0; j < fields.Count; j++)
                                {
                                    var operation = fields[j];
                                    if(operation.Name == "UML:Attribute")
                                    {
                                        var typeList = operation.ChildNodes;
                                        var typeTemp = typeList.Item(2);
                                        var type = typeTemp.ChildNodes;
                                        for(int k = 0; k < type.Count; k++)
                                        {
                                            var typeAt = type.Item(k);
                                            if (typeAt.Attributes["tag"].Value == "type")
                                            {
                                                typeText = typeAt.Attributes["value"].Value;
                                            }
                                        }
                                        attr += $"\r\n{operation.Attributes["visibility"].Value} {typeText} {operation.Attributes["name"].Value} ";
                                        Attribute.Add(("attribute", operation.Attributes["visibility"].Value, 
                                            typeText, 
                                            operation.Attributes["name"].Value));
                                    }
                                    if(operation.Name == "UML:Operation")
                                    {
                                        var typeList = operation.ChildNodes;
                                        var typeTemp = typeList.Item(0);
                                        var type = typeTemp.ChildNodes;
                                        for (int k = 0; k < type.Count; k++)
                                        {
                                            var typeAt = type.Item(k);
                                            if (typeAt.Attributes["tag"].Value == "type")
                                            {
                                                typeText = typeAt.Attributes["value"].Value;
                                            }
                                        }
                                        func += $"\r\n{operation.Attributes["visibility"].Value} {typeText} {operation.Attributes["name"].Value}";
                                        Operation.Add(("operation", operation.Attributes["visibility"].Value,
                                            typeText,
                                            operation.Attributes["name"].Value));
                                    }
                                }
                                var attribute = childClasses.Attributes["name"];
                                if (attribute != null)
                                {
                                    string className = attribute.Value;
                                    textXml.Text += ($"\r\n\r\n========\r\n\r\n" +
                                        $"Я класс {className}" +
                                    $"\r\nИмею поля {attr}" +
                                    $"\r\nИ функции {func}");

                                    eAs.Add(new EA(className, Attribute, Operation));

                                }
                                attr = "";
                                func = "";

                            }
                            else
                            {
                                var inside = childClasses.ChildNodes;
                                var feature = inside.Item(0);
                                var deepInside = feature.ChildNodes;
                                for(int k = 0; k < deepInside.Count; k++)
                                {
                                    var links = deepInside.Item(k);
                                    if (links.Attributes["tag"].Value == "ea_sourceName")
                                    {
                                        start = links.Attributes["value"].Value;
                                    }
                                    if (links.Attributes["tag"].Value == "ea_targetName")
                                    {
                                        end = links.Attributes["value"].Value;
                                    }
                                }
                                var attribute = childClasses.Attributes["name"];
                                string assocName = attribute.Value;
                                textXml.Text += ($"\r\n\r\n========\r\n\r\n" +
                                    $"Я ассоциация {assocName}" +
                                $"\r\nИз класса {start}" +
                                $"\r\nВ класс {end}");
                                associations.Add(new Association(assocName, start, end));
                            }
                        }
                        Console.WriteLine("efefefef");

                        int l = 0;
                        int p = 0;
                        while(l < eAs.Count)
                        {
                            p = 0;
                            textList.Text += $"\r\n==========\r\n" +
                            $"\r\n{eAs[l].ClassName}";
                            while(p < eAs[l].Attribute.Count)
                            {
                                textList.Text += $"\r\n{eAs[l].Attribute[p]}" +
                                $"\r\n{eAs[l].Operation[p]}";

                                p++;
                            }
                            
                            l++;
                            statusLbl.Text = l.ToString();
                        }
                        int q = 0;
                        while(q < associations.Count)
                        {
                            textList.Text += $"\r\n==========\r\n" +
                                $"{associations[q].AssotionName}" +
                                $"\r\n{associations[q].SourceName}" +
                                $"\r\n{associations[q].TargetName}";
                            q++;
                        }

                    }
                }
                catch(Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}

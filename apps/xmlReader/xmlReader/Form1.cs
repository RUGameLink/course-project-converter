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
        string test;
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Файл xml (*.xml)|*.xml|All files(*.*)|*.*";
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
                            var childClasses = classes.Item(i);
                            if (childClasses.Name == "UML:Class")
                            {
                                var inside = childClasses.ChildNodes;
                                var feature = inside.Item(1);
                                var fields = feature.ChildNodes;
                             //   MessageBox.Show(fields.ToString());
                                for(int j = 0; j < fields.Count; j++)
                                {
                                    var operation = fields[j];
                                    if(operation.Name == "UML:Attribute")
                                    {
                                        attr += $"\n{operation.Attributes["name"].Value} {operation.Attributes["visibility"].Value}";
                                    //    MessageBox.Show($"{attr} {func}");
                                    }
                                    if(operation.Name == "UML:Operation")
                                    {
                                        func += $"\n{operation.Attributes["name"].Value} {operation.Attributes["visibility"].Value}";
                                    //    MessageBox.Show($"{attr} {func}");
                                    }
                                 //   MessageBox.Show($"{attr} {func}");
                                }
                            //    MessageBox.Show("Я класс, сучка");
                                var attribute = childClasses.Attributes["name"];
                                if (attribute != null)
                                {
                                    string className = attribute.Value;
                                    MessageBox.Show($"Я класс {className}" +
                                        $"\nИмею поля {attr}" +
                                        $"\nИ функции {func}");
                                    // Process the value here
                                }
                                
                            }
                            else
                            {
                                //   MessageBox.Show("Ебана, я ассоциация...");
                                var inside = childClasses.ChildNodes;
                                var feature = inside.Item(0);


                                if (feature.Attributes["tag"].Value == "ea_sourceName")
                                {
                                    start = feature.Attributes["value"].Value;
                                }
                                if (feature.Attributes["tag"].Value == "ea_targetName")
                                {
                                    end = feature.Attributes["value"].Value;
                                }
                                var attribute = childClasses.Attributes["name"];
                                string assocName = attribute.Value;
                               MessageBox.Show($"Я ассоциация {assocName}" +
                                    $"\nИз класса {start}" +
                                        $"\nВ класс {end}");
                        // Process the value here
                    
                            }
                            //MessageBox.Show(childClasses.Name);
                            
                        }
                        
                        Console.WriteLine("efefefef");
                    }
                    

                }
                catch {}
            }
        }
    }
}

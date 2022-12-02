using converterEKB.Model;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace converterEKB
{
    public partial class Form1 : Form
    {
        string filename; //Путь к файлу
        string pathToDid; //Путь к директории

        List<EA> eAs;
        List<Association> associations;

        string ekbText = "";

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Файл xml (*.xml)|*.xml|All files(*.*)|*.*";
            convertBtn.Enabled = false;

            eAs = new List<EA>();
            associations = new List<Association>();
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

        private void convertBtn_Click(object sender, EventArgs e)
        {
            parseTheFile();
            convertToEKB();
        }

        private void convertToEKB()
        {
            Random random= new Random();
            string id = $"{random.Next(1000000000)}{random.Next(100)}";
            string header = $"<Structure>"
                + $"\r\n\t<KnowledgeBase>"
                + $"\r\n\t\t<ID>{id}</ID>"
                + $"\r\n\t\t<Name>База знаний 1</Name>"
                + $"\r\n\t\t<ShortName>Baza-znaniy-1</ShortName>"
                + $"\r\n\t\t<Kind>0</Kind>"
                + $"\r\n\t\t<Description></Description>"
                + $"\r\n\t\t<Vars/>"
                + $"\r\n\t\t<Templates>";

            string templates = "";
            string slots = "";
            string templateEnd = "";
            string templatesEnd = "";
            string grules = "";

            int l = 0;

            ekbText = header;
            int count = 1;
            while (l < eAs.Count)
            {
                string idTempStr = "";
                if(count < 10)
                {
                    idTempStr = "";
                    idTempStr = $"00{count}";
                }
                if (count >= 10)
                {
                    idTempStr = "";
                    idTempStr = $"0{count}";
                }
                templates = $"<Template>" +
                $"\r\n\t\t\t\t<ID>T{idTempStr}</ID>" +
                $"\r\n\t\t\t\t<Name>{eAs[l].ClassName}</Name>" +
                $"\r\n\t\t\t\t<ShortName>{eAs[l].ClassName}</ShortName>" +
                $"\r\n\t\t\t\t<Description>Описание шаблона T{idTempStr}</Description>" +
                $"\r\n\t\t\t\t<PackageName></PackageName>" +
                $"\r\n\t\t\t\t<RootPackageName></RootPackageName>" +
                $"\r\n\t\t\t\t<DrawParams>xT{idTempStr}=15" +
                $"\r\n\t\t\t\tyT{idTempStr}=15" +
                $"\r\n\t\t\t\tw=265" +
                $"\r\n\t\t\t\th=65" +
                $"\r\n\t\t\t\t</DrawParams>" +
                $"\r\n\t\t\t\t<Slots>";
                int p = 0;
                ekbText += templates;
                while (p < eAs[l].Attribute.Count)
                {
                    textList.Text += $"\r\n{eAs[l].Attribute[p]}";

                    slots = $"\r\n\t\t\t\t\t<Slot>" +
                    $"\r\n\t\t\t\t\t\t<Name>{eAs[l].Attribute[p].Item3}</Name>" +
                    $"\r\n\t\t\t\t\t\t<ShortName>{eAs[l].Attribute[p].Item3}</ShortName>" +
                    $"\r\n\t\t\t\t\t\t<Description>{eAs[l].Attribute[p].Item3}</Description>" +
                    $"\r\n\t\t\t\t\t\t<Value></Value>" +
                    $"\r\n\t\t\t\t\t\t<DataType>{eAs[l].Attribute[p].Item2}</DataType>" +
                    $"\r\n\t\t\t\t\t\t<Constraint></Constraint>" +
                    $"\r\n\t\t\t\t\t</Slot>";
                    ekbText += slots;
                    p++;
                }

                templateEnd = $"\r\n\t\t\t</Template>";
                ekbText += templateEnd;
                l++;
                count++;
            }

            templatesEnd = templatesEnd = $"\r\n\t\t\t</Templates>";
            ekbText += templatesEnd;

            string facts = "\r\n\t\t\t<Facts/>" +
            "\\t\\t<GRules>\"";
            ekbText += facts;

            int q = 0;
            count = 1;
            while (q < associations.Count)
            {
                string idTempStr = "";
                if (count < 10)
                {
                    idTempStr = "";
                    idTempStr = $"00{count}";
                }
                if (count >= 10)
                {
                    idTempStr = "";
                    idTempStr = $"0{count}";
                }
                grules =
                    $"\r\n\t\t\t<GRule>" +
                    $"\r\n\t\t\t\t<ID>G{idTempStr}</ID>" +
                    $"\r\n\t\t\t\t<Name>{associations[q].AssotionName}</Name>" +
                    $"\r\n\t\t\t\t<ShortName>{associations[q].AssotionName}</ShortName>" +
                    $"\r\n\t\t\t\t<Description>{associations[q].AssotionName}</Description>" +
                    $"\r\n\t\t\t\t<PackageName></PackageName>" +
                    $"\r\n\t\t\t\t<RootPackageName></RootPackageName>" +
                    $"\r\n\t\t\t\t<DrawParams>xG{idTempStr}=26" +
                    $"\r\n\t\t\t\tyG{idTempStr}=105" +
                    $"\r\n\t\t\t\tw=170" +
                    $"\r\n\t\t\t\th=34" +
                    $"\r\n\t\t\t\t</DrawParams>" +
                    $"\r\n\t\t\t\t<Conditions>" +
                    $"\r\n\t\t\t\t\t<C0>{associations[q].SourceName}</C0>" +
                    $"\r\n\t\t\t\t</Conditions>" +
                    $"\r\n\t\t\t\t<Actions>" +
                    $"\r\n\t\t\t\t\t<A0>{associations[q].TargetName}</A0>" +
                    $"\r\n\t\t\t\t</Actions>" +
                    $"\r\n\t\t\t</GRule>";
                    ekbText += grules;
                q++;
                count++;
            }

            string ekbEnd = $"\t\t</GRules>" +
                $"\r\n\t\t<Rules/>" +
                $"\r\n\t\t<Functions/>" +
                $"\r\n\t\t<Tasks/>" +
                $"\r\n\t\t<FScales/>" +
                $"\r\n\t\t<TempPackageList/>" +
                $"\r\n\t\t<FactPackageList/>" +
                $"\r\n\t\t<RulePackageList/>" +
                $"\r\n\t\t<GRulePackageList/>" +
                $"\r\n\t</KnowledgeBase>" +
                $"\r\n</Structure>";

            ekbText+= ekbEnd;
            textList.Text = ekbText;
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

                List<( string, string, string)> Attribute; 

                filename = openFileDialog1.FileName;

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
                    for (int i = 0; i < classes.Count; i++)
                    {
                        Attribute = new List<( string, string, string)>();
                        var childClasses = classes.Item(i);
                        if (childClasses.Name == "UML:Class")
                        {
                            var inside = childClasses.ChildNodes;
                            var feature = inside.Item(1);
                            var fields = feature.ChildNodes;
                            for (int j = 0; j < fields.Count; j++)
                            {
                                var operation = fields[j];
                                if (operation.Name == "UML:Attribute")
                                {
                                    var typeList = operation.ChildNodes;
                                    var typeTemp = typeList.Item(2);
                                    var type = typeTemp.ChildNodes;
                                    for (int k = 0; k < type.Count; k++)
                                    {
                                        var typeAt = type.Item(k);
                                        if (typeAt.Attributes["tag"].Value == "type")
                                        {
                                            typeText = typeAt.Attributes["value"].Value;
                                        }
                                    }
                                    attr += $"\r\n{operation.Attributes["visibility"].Value} {typeText} {operation.Attributes["name"].Value} ";
                                    
                                    Attribute.Add((operation.Attributes["visibility"].Value,
                                        typeText,
                                        operation.Attributes["name"].Value));
                                }
                            }
                            var attribute = childClasses.Attributes["name"];
                            if (attribute != null)
                            {
                                string className = attribute.Value;

                                eAs.Add(new EA(className, Attribute));

                            }
                            attr = "";
                            func = "";

                        }
                        else
                        {
                            var inside = childClasses.ChildNodes;
                            var feature = inside.Item(0);
                            var deepInside = feature.ChildNodes;
                            for (int k = 0; k < deepInside.Count; k++)
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
                            associations.Add(new Association(assocName, start, end));
                        }
                    }
                    Console.WriteLine("efefefef");

                    int l = 0;
                    int p = 0;
                    while (l < eAs.Count)
                    {
                        p = 0;
                        textList.Text += $"\r\n==========\r\n" +
                        $"\r\n{eAs[l].ClassName}";
                        while (p < eAs[l].Attribute.Count)
                        {
                            textList.Text += $"\r\n{eAs[l].Attribute[p]}";

                            p++;
                        }

                        l++;

                    }
                    int q = 0;
                    while (q < associations.Count)
                    {
                        textList.Text += $"\r\n==========\r\n" +
                            $"{associations[q].AssotionName}" +
                            $"\r\n{associations[q].SourceName}" +
                            $"\r\n{associations[q].TargetName}";
                        q++;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

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

        bool status = false;

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
            convertProgress.Value = 0;
            parseTheFile();
            convertToEKB();
            if (status)
            {
                saveToEkb();
                convertProgress.Value = 100;
            }
            else
            {
                convertProgress.Value = 0;
                MessageBox.Show("Проверьте файл для конвертации.", "Произошла ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void saveToEkb()
        {
            string path = $"{pathToDid}/file.ekb";
            System.IO.File.WriteAllText(path, ekbText);
            MessageBox.Show("Файл сохранен");
        }

        private void convertToEKB()
        {
            Random random = new Random();
            string id = $"{random.Next(1000000000)}{random.Next(100)}";
            string header = $"\r\n<Structure>"
                + $"\r\n<KnowledgeBase>"
                + $"\r\n<ID>{id}</ID>"
                + $"\r\n<Name>База знаний 1</Name>"
                + $"\r\n<ShortName>Baza-znaniy-1</ShortName>"
                + $"\r\n<Kind>0</Kind>"
                + $"\r\n<Description></Description>"
                + $"\r\n<Vars/>"
                + $"\r\n<Templates>";

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
                templates = $"\r\n<Template>" +
                $"\r\n<ID>T{idTempStr}</ID>" +
                $"\r\n<Name>{eAs[l].ClassName}</Name>" +
                $"\r\n<ShortName>{eAs[l].ClassName}</ShortName>" +
                $"\r\n<Description>Описание шаблона T{idTempStr}</Description>" +
                $"\r\n<PackageName></PackageName>" +
                $"\r\n<RootPackageName></RootPackageName>" +
                $"\r\n<DrawParams>xT{idTempStr}=15" +
                $"\r\nyT{idTempStr}=15" +
                $"\r\nw=265" +
                $"\r\nh=65" +
                $"\r\n</DrawParams>" +
                $"\r\n<Slots>";
                int p = 0;
                ekbText += templates;
                while (p < eAs[l].Attribute.Count)
                {
                    slots = $"\r\n<Slot>" +
                    $"\r\n<Name>{eAs[l].Attribute[p].Item3}</Name>" +
                    $"\r\n<ShortName>{eAs[l].Attribute[p].Item3}</ShortName>" +
                    $"\r\n<Description>{eAs[l].Attribute[p].Item3}</Description>" +
                    $"\r\n<Value></Value>" +
                    $"\r\n<DataType>{eAs[l].Attribute[p].Item2}</DataType>" +
                    $"\r\n<Constraint></Constraint>" +
                    $"\r\n</Slot>";
                    ekbText += slots;
                    p++;
                }

                templateEnd = $"\r\n</Slots>" +
                    "\r\n</Template>";
                ekbText += templateEnd;
                l++;
                count++;
            }

            templatesEnd = templatesEnd = $"\r\n</Templates>";
            ekbText += templatesEnd;

            string facts = "\r\n<Facts/>" +
            "\r\n<GRules>";
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
                    $"\r\n<GRule>" +
                    $"\r\n<ID>G{idTempStr}</ID>" +
                    $"\r\n<Name>{associations[q].AssotionName}</Name>" +
                    $"\r\n<ShortName>{associations[q].AssotionName}</ShortName>" +
                    $"\r\n<Description>{associations[q].AssotionName}</Description>" +
                    $"\r\n<PackageName></PackageName>" +
                    $"\r\n<RootPackageName></RootPackageName>" +
                    $"\r\n<DrawParams>xG{idTempStr}=26" +
                    $"\r\nyG{idTempStr}=105" +
                    $"\r\nw=170" +
                    $"\r\nh=34" +
                    $"\r\n</DrawParams>" +
                    $"\r\n<Conditions>" +
                    $"\r\n<C0>{associations[q].SourceName}</C0>" +
                    $"\r\n</Conditions>" +
                    $"\r\n<Actions>" +
                    $"\r\n<A0>{associations[q].TargetName}</A0>" +
                    $"\r\n</Actions>" +
                    $"\r\n</GRule>";
                ekbText += grules;
                q++;
                count++;
            }

            string ekbEnd = $"\r\n</GRules>" +
                $"\r\n<Rules/>" +
                $"\r\n<Functions/>" +
                $"\r\n<Tasks/>" +
                $"\r\n<FScales/>" +
                $"\r\n<TempPackageList/>" +
                $"\r\n<FactPackageList/>" +
                $"\r\n<RulePackageList/>" +
                $"\r\n<GRulePackageList/>" +
                $"\r\n</KnowledgeBase>" +
                $"\r\n</Structure>";

            ekbText += ekbEnd;
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
                try
                {
                    xDoc.Load(filename);
                }
                catch(System.Xml.XmlException ex)
                {
                    status = false;
                    return;
                }
                XmlElement xRoot = xDoc.DocumentElement;
                if (xRoot != null)
                {

                    var child = xRoot.ChildNodes;
                    var content = child.Item(1);
                    if (content == null)
                    {
                        status = false;
                        return;
                    }
                    var model = content.FirstChild;

                    var ownedElement = model.FirstChild;
                    var packagesTemp = ownedElement.ChildNodes;
                    var packages = packagesTemp.Item(1);
                    var ownedElementTemp = packages.ChildNodes;
                    var ownedElementClasses = ownedElementTemp.Item(1);
                    var classes = ownedElementClasses.ChildNodes;
                    for (int i = 0; i < classes.Count; i++)
                    {
                        Attribute = new List<(string, string, string)>();
                        var childClasses = classes.Item(i);
                        if (childClasses.Name == "UML:Class")
                        {
                            var inside = childClasses.ChildNodes;
                            var feature = inside.Item(1);
                            if(feature == null)
                            {
                                status = false;
                                return;
                            }
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
                }
                status = true;
            }
            catch (System.NullReferenceException ex)
            {
                status = false;
                return;
            }
        }
    }
}

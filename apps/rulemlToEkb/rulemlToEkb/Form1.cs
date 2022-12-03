using rulemlToEkb.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        string ekbText = "";

        List<RuleMl> ruleMl;
        List<Association> associations;

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Файл ruleml (*.ruleml)|*.ruleml|All files(*.*)|*.*";
            convertBtn.Enabled = false;

            ruleMl = new List<RuleMl>();
            associations = new List<Association>();
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
            associations.Clear();
            ruleMl.Clear();

            parseTheFile();
            convertToEKB();
            saveToEkb();
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

                List<(string, string)> Attribute;

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
                        Attribute = new List<(string, string)>();

                        var inside = atoms[i];
                        var atom = inside.ChildNodes;
                        var op = atom.Item(0);
                        var className = op.ChildNodes.Item(0).InnerText;
                        for (int j = 0; j < atom.Count; j++)
                        {
                            var temp = atom[j];
                            if (temp.Attributes["type"] != null)
                            {
                                typeText = temp.Attributes["type"].Value;
                                attr = temp.FirstChild.InnerText;
                                Attribute.Add((typeText, attr));
                            }
                           
                        }
                        ruleMl.Add(new RuleMl(className, Attribute));
                        attr = "";
                            func = "";

                    }
                    var bodyChilds = imp.ChildNodes;
                    var body = bodyChilds.Item(1);
                    var bodyAtoms = body.ChildNodes;
                    for (int i = 0; i < bodyAtoms.Count; i++)
                    {
                        Attribute = new List<(string, string)>();

                        var inside = bodyAtoms[i];
                        var atom = inside.ChildNodes;
                        var op = atom.Item(0);
                        var className = op.ChildNodes.Item(0).InnerText;

                        start = inside.ChildNodes.Item(1).InnerText;
                        end = inside.ChildNodes.Item(2).InnerText;

                        associations.Add(new Association(className, start, end));
                    }

                    Console.WriteLine("efefefef");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            while (l < ruleMl.Count)
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
                $"\r\n<Name>{ruleMl[l].ClassName}</Name>" +
                $"\r\n<ShortName>{ruleMl[l].ClassName}</ShortName>" +
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
                while (p < ruleMl[l].Attribute.Count)
                {
                    textList.Text += $"{ruleMl[l].Attribute[p]}";

                    slots = $"\r\n<Slot>" +
                    $"\r\n<Name>{ruleMl[l].Attribute[p].Item2}</Name>" +
                    $"\r\n<ShortName>{ruleMl[l].Attribute[p].Item2}</ShortName>" +
                    $"\r\n<Description>{ruleMl[l].Attribute[p].Item2}</Description>" +
                    $"\r\n<Value></Value>" +
                    $"\r\n<DataType>{ruleMl[l].Attribute[p].Item1}</DataType>" +
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
            textList.Text = ekbText;

            textList.Text += $"\r\r\n{ruleMl.Count}";
        }
    }
}

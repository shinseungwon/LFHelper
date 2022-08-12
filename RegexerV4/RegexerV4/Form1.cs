using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace RegexerV4
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetConsoleWindow();

        public static readonly List<string> Templates = new List<string>();

        public Form1()
        {
            InitializeComponent();
            AllocConsole();
            SetWindowPos(GetConsoleWindow(), 0, -3000, 0, 0, 0, 0x0001);

            Templates.AddRange(File.ReadAllText("Templates.txt").Replace("\r", "")
                .Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries));

            cbRegexFormula.Items.AddRange(Templates.ToArray());
            if (cbRegexFormula.Items.Count > 0)
            {
                cbRegexFormula.SelectedIndex = 0;
            }
        }

        private string RegexBuild(string template, string content)
        {
            StringBuilder sb = new StringBuilder();

            string[] contentLines = content.Replace("\r", "").Replace("\t", "")
                .Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in contentLines)
            {
                string target = template;
                string[] contents = s.Split(new string[] { " " }
                , StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < contents.Length; i++)
                {
                    target = target.Replace("{" + i + "}", contents[i]);
                }
                sb.Append(target + Environment.NewLine);
            }

            return sb.ToString();
        }

        private string RegexLtoR(string content)
        {
            return content.Replace("\r", "").Replace("\n", " ");
        }

        private DataTable TxtToGrid(string content, string starts, string seq)
        {
            DataTable result = new DataTable();

            string[] seqItems = seq.Split(new string[] { " " }
            , StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in seqItems)
            {
                string[] se = s.Split(new string[] { "\t" }
                , StringSplitOptions.RemoveEmptyEntries);

                result.Columns.Add(se[0] + "-" + se[1]);
            }

            string[] contentLines = content.Replace("\r", "").Split(new string[] { "\n" }
            , StringSplitOptions.RemoveEmptyEntries);

            int[] empty = new int[result.Columns.Count];
            int x = 0;

            foreach (string contentLine in contentLines)
            {
                if (contentLine.StartsWith(starts))
                {
                    DataRow row = result.NewRow();
                    x = 0;
                    foreach (string seqItem in seqItems)
                    {
                        string[] se = seqItem.Split(new string[] { "\t" }
                        , StringSplitOptions.RemoveEmptyEntries);

                        int s = int.Parse(se[0]), e = int.Parse(se[1]);
                        string columnName = s + "-" + e;
                        string itemStr = contentLine.Substring(s - 1, e - s + 1).Trim();
                        if (itemStr.Length > 0)
                        {
                            row[columnName] = itemStr;
                            empty[x] = 1;
                        }
                        x++;
                    }
                    result.Rows.Add(row);
                }
            }
            Console.WriteLine(empty.Length);
            Console.WriteLine(result.Columns.Count);

            List<string> columnNames = new List<string>();

            for (int i = 0; i < empty.Length; i++)
            {
                if (empty[i] == 0)
                {
                    columnNames.Add(result.Columns[i].ColumnName);
                }
            }

            foreach (string columnName in columnNames)
            {
                result.Columns.Remove(columnName);
            }

            return result;
        }

        private DataTable TxtToGridSpace(string content, string starts)
        {
            DataTable result = new DataTable();

            string[] contentLines = content.Replace("\r", "").Split(new string[] { "\n" }
                , StringSplitOptions.RemoveEmptyEntries);

            foreach (string contentLine in contentLines)
            {
                List<KeyValuePair<int, string>> items = new List<KeyValuePair<int, string>>();
                if (contentLine.StartsWith(starts))
                {
                    char[] arr = contentLine.ToCharArray();
                    int p = -1, tp = 0, n = arr.Length, x = 0;
                    StringBuilder sb = new StringBuilder();

                    while (++p < n)
                    {
                        if (arr[p] == ' ')
                        {
                            if (x == 1)
                            {
                                items.Add(new KeyValuePair<int, string>(tp, sb.ToString()));
                                sb.Clear();
                            }
                            x++;
                        }
                        else
                        {
                            if (x > 1)
                            {
                                tp = p;
                            }
                            x = 0;
                            sb.Append(arr[p]);
                        }
                    }

                    DataRow row = result.NewRow();
                    foreach (KeyValuePair<int, string> kv in items)
                    {
                        if (!result.Columns.Contains(kv.Key + ""))
                        {
                            result.Columns.Add(kv.Key + "");
                        }

                        row[kv.Key + ""] = kv.Value;
                    }
                    result.Rows.Add(row);
                }
            }

            return result;
        }

        private DataTable XmlToGrid(string s, string directory)
        {
            DataTable result = new DataTable();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(s);
            XmlNodeList rows = xml.SelectNodes(directory);

            if (rows.Count > 0 && rows[0].ChildNodes.Count > 0)
            {
                foreach (XmlNode n in rows[0].ChildNodes)
                {
                    result.Columns.Add(n.Name);
                }

                foreach (XmlNode n in rows)
                {
                    DataRow row = result.NewRow();
                    foreach (XmlNode nn in n.ChildNodes)
                    {
                        if (!result.Columns.Contains(nn.Name))
                        {
                            break;
                        }
                        row[nn.Name] = nn.InnerText;
                    }
                    result.Rows.Add(row);
                }
            }

            return result;
        }

        private void btRegexGo_Click(object sender, EventArgs e)
        {
            tbRegexOutput.Text = RegexBuild(cbRegexFormula.Text, tbRegexInput.Text);
        }

        private void btRegexLtoR_Click(object sender, EventArgs e)
        {
            tbRegexOutput.Text = RegexLtoR(tbRegexInput.Text);
        }

        private void btTxtCheck_Click(object sender, EventArgs e)
        {
            string text = tbTxtContent.Text;
            string starts = tbTxtStarts.Text;
            string seq = tbTxtSeq.Text;

            dgTxt.Rows.Clear();
            dgTxt.Columns.Clear();

            if (seq == "")
            {
                dgTxt.DataSource = TxtToGridSpace(text, starts);
            }
            else
            {
                dgTxt.DataSource = TxtToGrid(text, starts, seq);
            }
        }

        private void btXmlCheck_Click(object sender, EventArgs e)
        {
            string text = tbXmlContent.Text;
            string directory = tbXmlDirectory.Text;

            dgXml.Rows.Clear();
            dgXml.Columns.Clear();

            dgXml.DataSource = XmlToGrid(text, directory);
        }
    }
}

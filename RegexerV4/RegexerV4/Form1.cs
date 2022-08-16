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

        public static readonly Stack<string> Undo = new Stack<string>();
        public static readonly Stack<string> Redo = new Stack<string>();

        public Form1()
        {
            InitializeComponent();
            AllocConsole();
            SetWindowPos(GetConsoleWindow(), 0, -3000, 0, 0, 0, 0x0001);

            Templates.AddRange(File.ReadAllText("Templates.txt").Replace("\r", "")
                .Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries));

            cbRegexFormula.Items.AddRange(Templates.ToArray());
            cbRegexFormula.SelectedIndex = 0;

            tbRegexDelimeter.Text = "\t";
            lbRegexDelimeter.Text = "{TAB}";
        }

        private string RegexBuild(string template, string content, string delimeter)
        {
            StringBuilder sb = new StringBuilder();

            string[] contentLines = content.Replace("\r", "")
                .Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in contentLines)
            {
                string target = template;
                string[] contents = s.Split(new string[] { delimeter }
                , StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < contents.Length; i++)
                {
                    target = target.Replace("{" + i + "}", contents[i]);
                }
                sb.Append(target + Environment.NewLine);
            }

            return sb.ToString();
        }

        private string PrintCode(string input)
        {
            StringBuilder sb = new StringBuilder();

            char[] arr = input.ToCharArray();

            foreach (char c in arr)
            {
                if (c == ' ')
                {
                    sb.Append("{SPACE}");
                }
                else if (c == '\t')
                {
                    sb.Append("{TAB}");
                }
                else if (c == '\r')
                {
                    sb.Append("{CR}");
                }
                else if (c == '\n')
                {
                    sb.Append("{LF}");
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        //private string[] GetResultArray(string input, string delimeter)
        //{
        //    List<string> result = new List<string>();

        //    string[] splitted = input.Split(new string[] { delimeter }
        //    , StringSplitOptions.RemoveEmptyEntries);

        //    foreach (string s in splitted)
        //    {
        //        string trimmed = s.Trim();
        //        if (trimmed.Length > 0)
        //        {
        //            result.Add(trimmed);
        //        }
        //    }

        //    return result.ToArray();
        //}

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
            int x;

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
            tbRegexOutput.Text = RegexBuild(cbRegexFormula.Text, tbRegexInput.Text, tbRegexDelimeter.Text);
        }

        private void btRegexReplace_Click(object sender, EventArgs e)
        {
            Undo.Push(tbRegexInput.Text);
            Redo.Clear();

            //Text
            string content = tbRegexInput.Text;
            tbRegexInput.Text = tbRegexInput.Text + "\r\n changed";
            //~Test

            btRegexUndo.Text = "btRegexUndo (" + Undo.Count() + ")";
            btRegexRedo.Text = "btRegexRedo (" + Redo.Count() + ")";
        }

        private void tbRegexDelimeter_TextChanged(object sender, EventArgs e)
        {
            lbRegexDelimeter.Text = PrintCode(tbRegexDelimeter.Text);
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

        private void dgRegexReplace_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                dgRegexReplace.Rows.Clear();

                string content = Clipboard.GetText();

                string[] contentLines = content.Replace("\r", "")
                    .Split(new string[] { "\n" }
                    , StringSplitOptions.RemoveEmptyEntries);

                foreach (string contentLine in contentLines)
                {
                    string[] contentItems = contentLine
                        .Split(new string[] { "\t" }
                        , StringSplitOptions.RemoveEmptyEntries);

                    if (contentItems.Length == 2)
                    {
                        dgRegexReplace.Rows.Add(contentItems[0], contentItems[1]);
                    }
                }
            }
        }

        private void btRegexUndo_Click(object sender, EventArgs e)
        {
            if (Undo.Count > 0)
            {
                Redo.Push(tbRegexInput.Text);
                tbRegexInput.Text = Undo.Pop();

                btRegexUndo.Text = "btRegexUndo (" + Undo.Count() + ")";
                btRegexRedo.Text = "btRegexRedo (" + Redo.Count() + ")";
            }
        }

        private void btRegexRedo_Click(object sender, EventArgs e)
        {
            if (Redo.Count > 0)
            {
                Undo.Push(tbRegexInput.Text);
                tbRegexInput.Text = Redo.Pop();

                btRegexUndo.Text = "btRegexUndo (" + Undo.Count() + ")";
                btRegexRedo.Text = "btRegexRedo (" + Redo.Count() + ")";
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Helper;

namespace FileSeekerV2
{
    public partial class FileSeekerV2 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private delegate void SetControlPropertyThreadSafeDelegate(Control control, string propertyName, object propertyValue);

        static bool Stop = false;
        static readonly List<string> Targets = new List<string>();
        static readonly List<string> Results = new List<string>();

        static readonly StringBuilder LogText = new StringBuilder();

        static readonly LogHelper logHelper = new LogHelper("Logs");
        static readonly ConfigHelper configHelper = new ConfigHelper("Configs.txt");

        public FileSeekerV2()
        {
            InitializeComponent();

            if (configHelper.GetConfig("Console") == "1")
            {
                AllocConsole();
            }

            string preset = File.ReadAllText("Preset.txt");
            string[] dirs = preset.Split('\n');
            foreach (string dir in dirs)
            {
                if (!dir.StartsWith("'"))
                {
                    ListViewItem l = listView1.Items.Add(dir);
                    if (Directory.Exists(l.Text))
                    {
                        l.BackColor = Color.Green;
                    }
                    else
                    {
                        l.BackColor = Color.Red;
                    }
                }
            }
        }

        private void BaseKeyEvent(object sender, KeyEventArgs args)
        {
            if (args.KeyCode == Keys.Enter)
            {
                button1_Click(sender, args);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "GO")
            {
                Targets.Clear();
                Results.Clear();
                LogText.Clear();
                textBox1.Text = "";
                button1.Text = "STOP";

                if (textBox4.Text != "")
                {
                    if (Directory.Exists(textBox4.Text))
                    {
                        Targets.Add(textBox4.Text);
                    }
                    else
                    {
                        Print("Invalid target directory");
                    }
                }
                else
                {
                    if (listView1.SelectedItems.Count == 0)
                    {
                        foreach (ListViewItem l in listView1.Items)
                        {
                            if (Directory.Exists(l.Text))
                            {
                                Targets.Add(l.Text);
                            }
                        }
                    }
                    else
                    {
                        foreach (ListViewItem l in listView1.SelectedItems)
                        {
                            if (Directory.Exists(l.Text))
                            {
                                Targets.Add(l.Text);
                            }
                        }
                    }
                }

                Thread thread = new Thread(Search);
                thread.Start();
            }
            else if (button1.Text == "STOP")
            {
                Stop = true;
                button1.Text = "...";
                button1.Enabled = false;
            }
        }

        private void Search()
        {
            SetControlPropertyThreadSafe(button1, "Text", "STOP");
            SetControlPropertyThreadSafe(checkBox1, "Enabled", false);
            SetControlPropertyThreadSafe(checkBox2, "Enabled", false);

            string resDir = @"res_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            if (checkBox2.Checked)
            {
                Directory.CreateDirectory(resDir);
            }

            foreach (string s in Targets)
            {
                try
                {
                    CheckDir(new DirectoryInfo(s), resDir);
                }
                catch (Exception e)
                {
                    logHelper.WriteText(e.ToString());
                    Print(e.ToString());
                }
            }

            if (Stop)
            {
                Stop = false;
            }

            SetControlPropertyThreadSafe(button1, "Text", "GO");
            SetControlPropertyThreadSafe(button1, "Enabled", true);
            SetControlPropertyThreadSafe(checkBox1, "Enabled", true);
            SetControlPropertyThreadSafe(checkBox2, "Enabled", true);

            StringBuilder result = new StringBuilder();
            foreach (string s in Results)
            {
                result.Append(s + Environment.NewLine);
            }
            Print(result.ToString() + Results.Count + " Results");
        }

        private void CheckDir(DirectoryInfo d, string dir)
        {
            string directoryLike = textBox7.Text;
            string titleLike = textBox2.Text;
            string contentLike = textBox3.Text;

            string directoryDateTime = textBox5.Text;
            string fileDateTime = textBox6.Text;

            Console.WriteLine("CheckDir : " + dir);
            if (Stop)
            {
                return;
            }

            foreach (FileInfo f in d.GetFiles())
            {
                if (f.Extension == ".zip" && checkBox1.Checked && f.Name.Contains(directoryLike)
                    && CheckDateTime(f.CreationTime, directoryDateTime))
                {
                    using (ZipArchive za = ZipFile.OpenRead(f.FullName))
                    {
                        foreach (ZipArchiveEntry zae in za.Entries)
                        {
                            if (zae.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)
                                || zae.FullName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                            {
                                if (zae.Name.Contains(titleLike))
                                {
                                    using (Stream s = zae.Open())
                                    {
                                        using (StreamReader sr = new StreamReader(s))
                                        {
                                            string content = sr.ReadToEnd();
                                            if (content.Contains(contentLike))
                                            {
                                                Print("\n----------------------------------------\n"
                                                    + "File Found From Zip : \n" + zae.Name + "\n From : \n" + f.FullName
                                                    + "\n----------------------------------------\n");
                                                Results.Add(zae.FullName);
                                                if (checkBox2.Checked)
                                                {
                                                    zae.ExtractToFile(dir + @"\" + zae.Name);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (f.Extension == ".txt" || f.Extension == ".csv")
                {
                    if (f.Name.Contains(titleLike) && CheckDateTime(f.CreationTime, fileDateTime))
                    {
                        string content = File.ReadAllText(f.FullName);
                        if (content.Contains(contentLike))
                        {
                            Print("\n--------------------------------------------------\n"
                                + "File Found : \n" + dir + @"\" + f.Name + "\n From : \n" + f.FullName
                                + "\n--------------------------------------------------\n");
                            Results.Add(f.FullName);
                            if (checkBox2.Checked)
                            {
                                File.Copy(f.FullName, dir + @"\" + f.Name);
                            }
                        }
                    }
                }
                else
                {
                    if (f.Name.Contains(titleLike) && CheckDateTime(f.CreationTime, fileDateTime))
                    {
                        Print("\n--------------------------------------------------\n"
                            + "File Found : \n" + dir + @"\" + f.Name + "\n From : \n" + f.FullName
                            + "\n--------------------------------------------------\n");
                        Results.Add(f.FullName);
                        if (checkBox2.Checked)
                        {
                            File.Copy(f.FullName, dir + @"\" + f.Name);
                        }
                    }
                }
            }

            foreach (DirectoryInfo dd in d.GetDirectories())
            {
                if (dd.Name.Contains(directoryLike)
                    && CheckDateTime(dd.CreationTime, directoryDateTime))
                {
                    CheckDir(dd, dir);
                }
            }
        }

        private bool CheckDateTime(DateTime dt, string compare)
        {
            if (compare.Length == 8)//yyyymmdd
            {
                try
                {
                    DateTime comp1 = DateTime.Parse(compare);
                    DateTime comp2 = comp1.AddDays(1);
                    return comp1 <= dt && comp2 >= dt;
                }
                catch
                {
                    return false;
                }
            }
            else if (compare.Length == 17 || compare.Length == 31)//yyyymmdd/yyyymmdd or yyyymmdd hhmmss/yyyymmddhhmmss
            {

                try
                {
                    string[] dates = compare.Split(new string[] { "/" }
                    , StringSplitOptions.RemoveEmptyEntries);
                    DateTime comp1 = DateTime.Parse(dates[0]);
                    DateTime comp2 = DateTime.Parse(dates[1]);

                    return comp1 <= dt && comp2 >= dt;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        private void Print(string s)
        {
            Console.WriteLine(s);
            LogText.Append(Environment.NewLine + s.Replace("\n", Environment.NewLine));
            SetControlPropertyThreadSafe(textBox1, "Text", LogText.ToString());
        }

        public static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate(SetControlPropertyThreadSafe)
                    , new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(
                    propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue });
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            ListView lv = (ListView)sender;
            if (lv.SelectedItems.Count == 1)
            {
                textBox4.Text = lv.SelectedItems[0].Text;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = checkBox3.Checked;
        }
    }
}

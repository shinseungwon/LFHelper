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

                if (textBox3.Text != "")
                {
                    if (Directory.Exists(textBox3.Text))
                    {
                        Targets.Add(textBox3.Text);
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
                    CheckDir(new DirectoryInfo(s), resDir, textBox2.Text, textBox3.Text);
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

        private void CheckDir(DirectoryInfo d, string dir, string th, string ch)
        {
            if (Stop)
            {
                return;
            }

            foreach (FileInfo f in d.GetFiles())
            {
                if (f.Extension == ".zip" && checkBox1.Checked)
                {
                    using (ZipArchive za = ZipFile.OpenRead(f.FullName))
                    {
                        foreach (ZipArchiveEntry zae in za.Entries)
                        {
                            if (zae.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)
                                || zae.FullName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                            {
                                //if (!File.Exists(dir + @"\" + zae.Name) && zae.Name.Contains(th))
                                if (zae.Name.Contains(th))
                                {
                                    using (Stream s = zae.Open())
                                    {
                                        using (StreamReader sr = new StreamReader(s))
                                        {
                                            string content = sr.ReadToEnd();
                                            if (content.Contains(ch))
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
                    //if (!File.Exists(dir + @"\" + f.Name) && f.Name.Contains(th))
                    if (f.Name.Contains(th))
                    {
                        string content = File.ReadAllText(f.FullName);
                        if (content.Contains(ch))
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
                    if (f.Name.Contains(th))
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
                CheckDir(dd, dir, th, ch);
            }
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
    }
}

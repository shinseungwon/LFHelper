using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Helper;

namespace WebShooterV2
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

        private delegate void SetControlPropertyThreadSafeDelegate(Control control, string propertyName, object propertyValue);

        public static readonly List<string> tNames = new List<string>();
        public static readonly List<string> tContents = new List<string>();

        public static readonly List<string> tUrl = new List<string>();
        public static readonly List<string> tHeader = new List<string>();
        public static readonly List<string> tBody = new List<string>();

        public static readonly List<string> wRequest = new List<string>();
        public static readonly List<string> wResponse = new List<string>();

        public Form1()
        {
            AllocConsole();
            InitializeComponent();
            SetWindowPos(GetConsoleWindow(), 0, -3000, 0, 0, 0, 0x0001);
            LoadTemplates();

            if (lvTemplate.Items.Count > 0)
            {
                int index = lvTemplate.Items[0].Index;

                tbUrl.Text = tUrl[index];
                tbHeader.Text = tHeader[index];
                tbBody.Text = System.Xml.Linq.XDocument.Parse(tBody[index]).ToString();
            }
        }

        private void LoadTemplates()
        {
            string dir = Directory.GetCurrentDirectory() + @"\templates\";
            DirectoryInfo di = new DirectoryInfo(dir);

            tNames.Clear();
            tContents.Clear();
            lvTemplate.Items.Clear();

            tUrl.Clear();
            tHeader.Clear();
            tBody.Clear();

            foreach (FileInfo fi in di.GetFiles())
            {
                tNames.Add(fi.Name);
                string content = File.ReadAllText(fi.FullName);
                tContents.Add(content);

                string[] parts = content.Split(new string[] { "\r\n\r\n" }
                , StringSplitOptions.RemoveEmptyEntries);

                tUrl.Add(parts[0]);
                tHeader.Add(parts[1]);
                tBody.Add(parts[2]);
            }

            foreach (string s in tNames)
            {
                lvTemplate.Items.Add(new ListViewItem(s.Split('.')[0]));
            }
        }

        private string WebCommunication(string url, string header, string content)
        {
            //Create Request Format
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.UserAgent = "LFL Jeffrey";
            req.ContentLength = content.Length;

            string[] headerLines = header.Split(new string[] { "\r\n" }
                , StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in headerLines)
            {
                string[] headerLine = s.Split(new string[] { " " }
                    , StringSplitOptions.RemoveEmptyEntries);

                req.Headers[headerLine[0]] = headerLine[1];
            }

            using (Stream stream = req.GetRequestStream())
            using (StreamWriter sw = new StreamWriter(stream))
            {
                sw.Write(content);
            }

            //Get Response
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            string result = "";

            using (Stream stream = res.GetResponseStream())
            using (StreamReader sr = new StreamReader(stream))
            {
                result = sr.ReadToEnd();
            }

            return result;
        }

        private string[][] GetDataList(string str)
        {
            string[] lines = str.Replace("\r", "").Replace("\t", "").Split('\n');
            List<string[]> tmp = new List<string[]>();
            foreach (string s in lines)
            {
                if (s.Length > 0 && !s.StartsWith("'"))
                {
                    string[] items = s.Split(new string[] { " " }
                    , StringSplitOptions.RemoveEmptyEntries);
                    List<string> stacks = new List<string>();
                    foreach (string ss in items)
                    {
                        if (ss.Length > 0)
                        {
                            stacks.Add(ss.Replace("\t", ""));
                        }
                    }
                    tmp.Add(stacks.ToArray());
                }
            }

            return tmp.ToArray();
        }

        private void Execute()
        {
            string[][] data = GetDataList(tbData.Text);
            int i, j;

            for (i = 0; i < data.Length; i++)
            {
                string request = tbBody.Text;
                for (j = 0; j < data[i].Length; j++)
                {
                    request = request.Replace("{" + j + "}", data[i][j]);
                }
                string response = WebCommunication(tbUrl.Text, tbHeader.Text, request);
                Console.WriteLine("Result " + i + " Content(" + response.Length + ") : " + response.Substring(0, 10) + " ...");

                wRequest.Add(request);
                wResponse.Add(response);

                lvResult.Items[i].SubItems[0].Text = request.Substring(0, 10);
                lvResult.Items[i].SubItems[1].Text = response.Substring(0, 10);
                lvResult.Items[i].SubItems[2].Text = response.Length + "";

                if (response.Contains(tbResultLike.Text))
                {
                    lvResult.Items[i].BackColor = Color.LightGreen;
                }
            }

            Console.WriteLine("Pull Done");
        }

        private void bGo_Click(object sender, EventArgs e)
        {
            tbData.Enabled = false;
            tbUrl.Enabled = false;
            tbHeader.Enabled = false;
            tbBody.Enabled = false;
            lvResult.Enabled = false;
            tbResponse.Enabled = false;
            tbResultLike.Enabled = false;
            bLoad.Enabled = false;
            bGo.Enabled = false;
            bGo.Text = "Running";
            bExport.Enabled = false;

            lvResult.Items.Clear();
            tbResponse.Text = "";

            wRequest.Clear();
            wResponse.Clear();

            string[] dataLines = tbData.Text.Split(new string[] { "\r\n" }
                , StringSplitOptions.RemoveEmptyEntries);
            int i;

            for (i = 0; i < dataLines.Length; i++)
            {
                ListViewItem lvi = new ListViewItem(new string[] { "Request " + i, "Response " + i, "ready" });
                lvResult.Items.Add(lvi);
            }

            new Thread(Execute).Start();

            tbData.Enabled = true;
            tbUrl.Enabled = true;
            tbHeader.Enabled = true;
            tbBody.Enabled = true;
            lvResult.Enabled = true;
            tbResponse.Enabled = true;
            tbResultLike.Enabled = true;
            bLoad.Enabled = true;
            bGo.Enabled = true;
            bGo.Text = "Go";
            bExport.Enabled = true;
        }

        private void bExport_Click(object sender, EventArgs e)
        {
            if (tbRequest.Text.Length > 0 && tbResponse.Text.Length > 0)
            {
                File.WriteAllText("result_request.txt", tbRequest.Text);
                File.WriteAllText("result_response.txt", tbResponse.Text);
            }
        }

        private void bLoad_Click(object sender, EventArgs e)
        {
            LoadTemplates();
        }

        private void lvTemplate_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvTemplate.SelectedItems.Count == 1)
            {
                int index = lvTemplate.SelectedItems[0].Index;

                tbUrl.Text = tUrl[index];
                tbHeader.Text = tHeader[index];
                tbBody.Text = System.Xml.Linq.XDocument.Parse(tBody[index]).ToString();
            }
        }

        private void lvResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i, t = lvResult.Items.Count;
            for (i = 0; i < t; i++)
            {
                if (lvResult.Items[i].Selected)
                {
                    tbRequest.Text = System.Xml.Linq.XDocument.Parse(wRequest[i]).ToString();
                    tbResponse.Text = System.Xml.Linq.XDocument.Parse(wResponse[i]).ToString();
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
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

        public static readonly List<string> tNames = new List<string>();
        public static readonly List<string> tContents = new List<string>();

        public static readonly List<string> tUrl = new List<string>();
        public static readonly List<string> tHeader = new List<string>();
        public static readonly List<string> tBody = new List<string>();

        public Form1()
        {
            AllocConsole();
            InitializeComponent();
            SetWindowPos(GetConsoleWindow(), 0, -3000, 0, 0, 0, 0x0001);
            LoadTemplates();
        }

        private void LoadTemplates()
        {
            string dir = Directory.GetCurrentDirectory() + @"\templates\";
            DirectoryInfo di = new DirectoryInfo(dir);

            tNames.Clear();
            tContents.Clear();

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

        static string WebCommunication(string url, string header, string content)
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

        private void bApply_Click(object sender, EventArgs e)
        {

        }

        private void bGo_Click(object sender, EventArgs e)
        {

        }

        private void bExport_Click(object sender, EventArgs e)
        {

        }

        private void bLoad_Click(object sender, EventArgs e)
        {
            LoadTemplates();
        }

        private void lvTemplate_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvTemplate.SelectedItems.Count == 1)
            {
                string target = lvTemplate.SelectedItems[0].Text;
                int index = lvTemplate.SelectedItems[0].Index;

                tbUrl.Text = tUrl[index];
                tbHeader.Text = tHeader[index];
                tbBody.Text = tBody[index];
            }
        }
    }
}

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
using System.Xml;

namespace HyundaiMan
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

        public static string[][] CustomerInfo;
        public static string[][] WebInfo;

        public static List<string> Request = new List<string>();
        public static List<string> Response = new List<string>();

        public Form1()
        {
            InitializeComponent();
            AllocConsole();
            SetWindowPos(GetConsoleWindow(), 0, -3000, 0, 0, 0, 0x0001);

            CustomerInfo = GetDataList(File.ReadAllText(@"Data\Customers.txt"));
            foreach (string[] s in CustomerInfo)
            {
                cbCustomer.Items.Add(s[0]);
            }
            cbCustomer.SelectedIndex = 0;

            WebInfo = GetDataList(File.ReadAllText(@"Data\Webs.txt"));
            cbType.SelectedIndex = 0;

            //TestCase
            //tbInput.Text = "22080800805080" + Environment.NewLine + "22080800805077"; //cos order
            //tbInput.Text = "22080800805399" + Environment.NewLine + "22080800805287"; //aos order
            //tbInput.Text = "22080800805510" + Environment.NewLine + "22080800805258"; //ark order

            tbInput.Text = "22080400720498" + Environment.NewLine + "22080300717681"; //cos order
            //tbInput.Text = "22073000717030" + Environment.NewLine + "22073000716815"; //aos order
            //tbInput.Text = "22080600769076" + Environment.NewLine + "22072800669273"; //ark order
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
                            stacks.Add(ss);
                        }
                    }
                    tmp.Add(stacks.ToArray());
                }
            }

            return tmp.ToArray();
        }

        static List<string[]> xmlParse(string s)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(s);
            XmlNodeList rowList = xml.SelectNodes("//Response2XML/Dataset/rows/row");
            List<string[]> result = new List<string[]>();


            foreach (XmlNode n in rowList)
            {
                string[] resultLine = new string[4];
                foreach (XmlNode nn in n.ChildNodes)
                {
                    if (nn.Name == "ordNo")
                    {
                        resultLine[0] = nn.InnerText;
                    }
                    else if (nn.Name == "ordPtcSeq")
                    {
                        resultLine[1] = nn.InnerText;
                    }
                    else if (nn.Name == "itemSkuMngNo")
                    {
                        resultLine[2] = nn.InnerText;
                    }
                    else if (nn.Name == "lastDlvstPrgrGbcd")
                    {
                        resultLine[3] = nn.InnerText;
                    }
                }
                if (!string.IsNullOrEmpty(resultLine[0]))
                {
                    result.Add(resultLine);
                }
            }

            return result;
        }

        private void cbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void bSearch_Click(object sender, EventArgs e)
        {
            lvResult.Items.Clear();
            Request.Clear();
            Response.Clear();
            Console.WriteLine(cbCustomer.SelectedIndex + " - " + cbCustomer.Text);
            Console.WriteLine(cbType.SelectedIndex + " - " + cbType.Text);
            int customer = cbCustomer.SelectedIndex, type = cbType.SelectedIndex;
            string[] customerInfo = CustomerInfo[customer];
            string[] webInfo = null;

            foreach (string[] webInfoLine in WebInfo)
            {
                if (type == 0 && webInfoLine[0] == "OrderSelect")
                {
                    webInfo = webInfoLine;
                    break;
                }

                if (type == 1 && webInfoLine[0] == "ReturnSelect")
                {
                    webInfo = webInfoLine;
                    break;
                }
            }

            if (webInfo != null)
            {
                string[] inputLines = tbInput.Text.Replace("\r", "").Split(
                new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                string header = "oauserid " + customerInfo[1] + customerInfo[2]
                    + Environment.NewLine + "oausekey " + customerInfo[3];

                StringBuilder sb = new StringBuilder();
                for (int i = 2; i < webInfo.Length; i++)
                {
                    sb.Append(webInfo[i] + " ");
                }
                string contentBase = sb.ToString()
                    .Replace("{0}", customerInfo[1])
                    .Replace("{1}", customerInfo[2]);

                foreach (string input in inputLines)
                {
                    string content = contentBase.Replace("{2}", input);
                    string[] steps = null;

                    if (type == 0) //orders
                    {
                        steps = new string[] { "25", "30", "45", "50" };
                    }
                    else if (type == 1) //returns
                    {
                        steps = new string[] { "55", "63" };
                    }

                    foreach (string step in steps)
                    {
                        string contentStep = content.Replace("{3}", step);
                        string result = WebCommunication(webInfo[1], header, contentStep);
                        Request.Add(webInfo[1] + Environment.NewLine + Environment.NewLine
                            + header + Environment.NewLine + Environment.NewLine + content);
                        Response.Add(result);
                        xmlParse(result);

                        List<string[]> resultLines = xmlParse(result);
                        foreach (string[] sa in resultLines)
                        {
                            lvResult.Items.Add(new ListViewItem(sa));
                        }
                    }
                }
            }
        }
    }
}

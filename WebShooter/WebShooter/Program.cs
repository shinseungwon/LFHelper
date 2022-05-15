using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace WebShooter
{
    internal class Program
    {
        //static void Main(string[] args)
        static void Main()
        {
            string[] args = { "1" };
            if (args.Length == 1)
            {
                string header = File.ReadAllText(@"Header" + args[0] + ".txt");
                string[][] headerLines = Trimming(header);

                if (headerLines.Length > 0)
                {
                    string url = headerLines[0][0];
                    string xmlAddr = headerLines[1][0];
                    string body = File.ReadAllText(@"Body" + args[0] + ".txt");

                    //batch
                    string batchDir = @"batch" + args[0] + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    Directory.CreateDirectory(batchDir);

                    string batch = File.ReadAllText(@"Batch" + args[0] + ".txt");
                    string[][] batches = Trimming(batch);

                    int bLines = 0;
                    foreach (string[] s in batches)
                    {
                        if (!s[0].StartsWith("'"))
                        {
                            string content = body;
                            for (int i = 0; i < s.Length; i++)
                            {
                                string rep = Trim(s[i]);
                                if (rep == "-")
                                {
                                    int repPos = content.IndexOf("{" + i + "}");
                                    int tagEndPos = content.IndexOf(">", repPos);
                                    string lineTag = content.Substring(repPos + 5, tagEndPos - repPos - 5);
                                    content = content.Replace("<" + lineTag + ">" + "{" + i + "}</" + lineTag + ">", "");
                                }
                                else
                                {
                                    content = content.Replace("{" + i + "}", rep);
                                }
                            }
                            string result = WebCommunication(headerLines, content);

                            string code = "data";
                            if (result.IndexOf("<code>0000</code>") > 0)
                            {
                                code = "Success";
                            }

                            string bRecordName = batchDir + @"\line" + bLines++ + "_";
                            File.WriteAllText(bRecordName + "req_" + code + "_" + content.Length + ".txt", content);
                            File.WriteAllText(bRecordName + "res_" + code + "_" + result.Length + ".txt", result);
                            File.WriteAllText(bRecordName + "csv.csv", XmlToCsv(result, xmlAddr), Encoding.UTF8);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("No Storer Seq");
            }
        }

        static string XmlToCsv(string s, string addr)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(s);
            XmlNodeList rows = xml.SelectNodes(addr);
            StringBuilder sb = new StringBuilder();

            foreach (XmlNode n in rows)
            {
                if (sb.Length == 0)
                {
                    foreach (XmlNode nn in n.ChildNodes)
                    {
                        sb.Append(nn.Name + ",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append("\n");
                }
                foreach (XmlNode nn in n.ChildNodes)
                {
                    sb.Append("=\"" + nn.InnerText.Replace(",", "") + "\",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("\n");
            }

            return sb.ToString();
        }

        static string WebCommunication(string[][] headerLines, string content)
        {
            //Create Request Format
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(headerLines[0][0]);
            req.Method = "POST";
            req.UserAgent = "LFL Jeffrey";
            req.ContentLength = content.Length;

            for (int i = 2; i < headerLines.Length; i++)
            {
                req.Headers[headerLines[i][0]] = Trim(headerLines[i][1]);
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

        static string[][] Trimming(string str)
        {
            string[] lines = str.Replace("\r", "").Split('\n');
            List<string[]> tmp = new List<string[]>();
            foreach (string s in lines)
            {
                if (s.Length > 0)
                {
                    string[] items = s.Split(' ');
                    List<string> stacks = new List<string>();
                    foreach (string ss in items)
                    {
                        if (ss.Length > 0)
                        {
                            stacks.Add(Trim(ss));
                        }
                    }
                    tmp.Add(stacks.ToArray());
                }
            }

            return tmp.ToArray();
        }

        static string Trim(string s)
        {
            return s.Replace("\r", "")
                .Replace("\n", "")
                .Replace("\t", "")
                .Replace(" ", "");
        }
    }
}

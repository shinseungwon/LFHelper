using System;
using HelperDotNet;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace PoGroupUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            //1.Get Config
            ConfigHelper ch = new ConfigHelper("Config.txt");

            string id = ch.Configs["id"];
            string password = ch.Configs["password"];

            string ip = ch.Configs["ip"];
            string catalog = ch.Configs["catalog"];

            //2.Connect DB + Call SP
            string connectionString = "Data Source=" + ip + ",1433; Initial Catalog=" + catalog
                + "; User id=" + id + "; Password=" + password + ";";

            string query = "";

            DbHelper dh = new DbHelper(connectionString);
            Logger l = new Logger(Directory.GetCurrentDirectory() + @"\Logger");
            dh.SetLogger(l);
            DataSet ds = new DataSet();
            dh.CallQuery(query, ref ds);

            string err;
            string target = File.ReadAllText(@"Target.txt");
            List<string> targets = SplitLine(target);

            foreach (string s in targets)
            {
                //Console.WriteLine(s);
                string[] elements = s.Split(' ');

                if (elements.Length != 2)
                {
                    err = "Each line should have 2 elements (text : " + s + ")";
                    Console.WriteLine(err);
                    l.WriteText(err);
                }
                else
                {
                    if (elements[1].Length != 10)
                    {
                        err = "line value check -> 1st value : pogroup, 2nd value : pokey " + elements[1].Length;
                        Console.WriteLine(err);
                        l.WriteText(err);
                    }
                    else
                    {
                        DataSet dsCnt = new DataSet();
                        dh.CallQuery("select count(1) as cnt from po(nolock) where pokey = '" + elements[1] + "'", ref dsCnt);

                        if (dsCnt.Tables[0].Rows[0]["cnt"].ToString() != "1")
                        {
                            err = "line value check -> 1st value : pogroup, 2nd value : pokey / or PO not exists!";
                            Console.WriteLine(err);
                            l.WriteText(err);
                        }
                        else
                        {
                            Console.WriteLine("PO Exists, Pokey : " + elements[1] + " set pogroup as " + elements[0]);
                            //Console.Write(" update po set pogroup = '" + elements[0] + "' where pokey = '" + elements[1] + "'");
                            query += " update po set pogroup = '" + elements[0] + "' where pokey = '" + elements[1] + "'" + Environment.NewLine;
                        }
                    }
                }
            }

            Console.Write("Confirm update?(Y/N) : ");
            string res = Console.ReadLine();
            if (res == "Y" || res == "y")
            {
                Console.WriteLine(query);
            }
        }

        static string TrimStr(string s)
        {
            StringBuilder sb = new StringBuilder();
            short space = 0; //0 initial, 1 inserted one time
            for (int i = 0; i < s.Length; i++)
            {
                char x = s[i];
                if (x >= 32 && x < 127) //is visible character
                {
                    if (x == ' ')
                    {
                        if (space == 0)
                        {
                            sb.Append(s[i]);
                            space = 1;
                        }
                    }
                    else
                    {
                        if (space == 1)
                        {
                            space = 0;
                        }
                        sb.Append(s[i]);
                    }
                }
            }

            return sb.ToString();
        }

        static List<string> SplitLine(string s)
        {
            string[] sa = s.Split('\n');
            List<string> l = new List<string>();

            for (int i = 0; i < sa.Length; i++)
            {
                sa[i] = TrimStr(sa[i].Replace('\t', ' '));
                if (!l.Contains(sa[i]) && sa[i].Length > 3)
                {
                    l.Add(sa[i]);
                }
            }

            return l;
        }
    }
}

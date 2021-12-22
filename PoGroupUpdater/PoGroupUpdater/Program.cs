using System;
using HelperDotNet;
using System.IO;
using System.Data;
using System.Collections.Generic;

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

            string err;
            string target = File.ReadAllText(@"Target.txt");
            List<string> targets = Tools.SplitLine(target);

            bool error = false;
            foreach (string s in targets)
            {
                string[] elements = s.Split(' ');

                if (elements.Length != 2)
                {
                    err = "Each line should have 2 elements (text : " + s + ") -> this line will be excluded";
                    Console.WriteLine(err);
                    l.WriteText(err);
                }
                else
                {
                    if (elements[1].Length != 10)
                    {
                        err = "Please check text -> 1st value : pogroup, 2nd value : pokey, text -> " + s;
                        Console.WriteLine(err);
                        l.WriteText(err);
                        error = true;
                        break;
                    }
                    else
                    {
                        DataSet dsCnt = new DataSet();
                        dh.CallQuery("select PoGroup from po(nolock) where pokey = '" + elements[1] + "'", ref dsCnt);

                        if (dsCnt.Tables[0].Rows.Count != 1)
                        {
                            err = "line value check -> 1st value : pogroup, 2nd value : pokey / or PO not exists!";
                            Console.WriteLine(err);
                            l.WriteText(err);
                        }
                        else
                        {
                            string poGroup = dsCnt.Tables[0].Rows[0]["PoGroup"].ToString();
                            if(poGroup.Length == 0)
                            {
                                Console.WriteLine("PO " + elements[1] + " already has PoGroup(" + poGroup + ") please check");
                            }
                            Console.WriteLine("PO Exists, Pokey : " + elements[1] + " set pogroup as " + elements[0]);
                            //Console.Write(" update po set pogroup = '" + elements[0] + "' where pokey = '" + elements[1] + "'");
                            query += "update po set pogroup = '" + elements[0] + "' where pokey = '" + elements[1] + "' " + Environment.NewLine;
                        }
                    }
                }
            }

            if (!error)
            {
                Console.Write("Confirm update?(Y/N) : ");
                string res = Console.ReadLine();
                if (res == "Y" || res == "y")
                {
                    //dh.CallQuery(query);
                    Console.WriteLine(query);
                    Console.WriteLine("Done!");
                }
            }
        }
    }
}
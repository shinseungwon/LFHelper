using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Regexer
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            List<string> ops = new List<string>();
            int menu = 0;
            foreach (string fs in Directory.GetFiles(Directory.GetCurrentDirectory()))
            {
                if (fs.EndsWith(".txt"))
                {
                    Console.WriteLine((++menu) + " : " + Path.GetFileName(fs));
                    ops.Add(fs);
                }
            }
            Console.WriteLine();
            
            while (true)
            {
                Console.WriteLine("Input Command ...");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int num))
                {
                    if (num == 0)
                    {
                        break;
                    }

                    if (num > 0 && num <= menu)
                    {
                        string command = File.ReadAllText(ops[num - 1]);
                        string[] commands = command.Split('\n');
                        Console.WriteLine(commands.Length);
                        if (commands.Length == 3)
                        {
                            string res = commands[0].Trim() + Environment.NewLine;
                            string s = Clipboard.GetText();
                            string[] sa = s.Split('\n');
                            int sc = sa.Length, i, j;
                            for (i = 0; i < sc; i++)
                            {
                                string regex = commands[1].Trim();
                                string sas = sa[i].Trim();
                                if (sas.Length > 0)
                                {
                                    Console.WriteLine(sas);
                                    string[] sasa = sas.Split(' ');
                                    int sasac = sasa.Length;

                                    for (j = 0; j < sasac; j++)
                                    {
                                        regex = regex.Replace("{" + j + "}", sasa[j]);
                                    }
                                    res += regex + Environment.NewLine;
                                }
                            }
                            res += commands[2].Trim() + Environment.NewLine;
                            Clipboard.SetText(res);
                        }
                        else
                        {
                            Console.WriteLine("Wrong Command");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Menu in List");
                    }
                }
            }
        }
    }
}

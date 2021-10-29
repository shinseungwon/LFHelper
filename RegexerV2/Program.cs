using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace RegexerV2
{
    class Program
    {
        static List<string> ops = new List<string>();
        static List<string> opsStr = new List<string>();
        static int menu;

        static void Main()
        {
            menu = Load();

            while (true)
            {
                Console.WriteLine("Input Command ...");
                string input = Console.ReadLine();

                if (input == "r")
                {
                    menu = Load();
                    continue;
                }                

                if (int.TryParse(input, out int num))
                {
                    if (num == 0)
                    {
                        break;
                    }

                    if (num > 0 && num <= menu)
                    {
                        string command = opsStr[num - 1];
                        string[] commands = command.Split('\n');
                        if (commands.Length == 3)
                        {
                            string res = commands[0].Trim() + Environment.NewLine;                            
                            int i, j;
                            
                            List<string> sa = Trimming(Clipboard.GetText());

                            for (i = 0; i < sa.Count; i++)
                            {
                                string regex = commands[1].Trim();
                                string[] sasa = sa[i].Split(" ");
                                List<string> sasal = new List<string>();

                                for (j = 0; j < sasa.Length; j++)
                                {
                                    if (sasa[j].Length > 0)
                                    {
                                        sasal.Add(sasa[j].Trim());
                                    } 
                                }                                
                                
                                for (j = 0; j < sasal.Count; j++)
                                {                                    
                                    regex = regex.Replace("{" + j + "}", sasal[j]);
                                }

                                res += regex + Environment.NewLine;
                                
                            }
                            res += commands[2].Trim() + Environment.NewLine;
                            
                            Clipboard.SetText(res);
                            Console.WriteLine(sa.Count + "Lines");
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

        static List<string> Trimming(string s)
        {            
            string[] sa = s.Split('\n');
            List<string> l = new List<string>();            

            for(int i = 0;i < sa.Length;i++){
                sa[i] = sa[i].Replace('\t', ' ').Replace("     ", " ").Replace("    ", " ").Replace("   ", " ").Replace("  ", " ");
                if(!l.Contains(sa[i]) && sa[i].Length > 3){
                    l.Add(sa[i]);
                }
            }

            return l;
        }

        static int Load()
        {
            menu = 0;
            ops.Clear();
            opsStr.Clear();
            foreach (string fs in Directory.GetFiles(Directory.GetCurrentDirectory()))
            {
                if (fs.EndsWith(".txt"))
                {
                    Console.WriteLine((++menu) + " : " + Path.GetFileName(fs));
                    ops.Add(fs);
                    opsStr.Add(File.ReadAllText(fs));
                }
            }
            Console.WriteLine();

            return menu;
        }
    }

    public static class Clipboard
    {
        public static void SetText(string text)
        {
            var powershell = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = $"-command \"Set-Clipboard -Value \\\"{text}\\\"\""
                }
            };
            powershell.Start();
            powershell.WaitForExit();
        }

        public static string GetText()
        {
            var powershell = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    RedirectStandardOutput = true,
                    FileName = "powershell",
                    Arguments = "-command \"Get-Clipboard\""
                }
            };

            powershell.Start();
            string text = powershell.StandardOutput.ReadToEnd();
            powershell.StandardOutput.Close();
            powershell.WaitForExit();
            return text.TrimEnd();
        }
    }
}

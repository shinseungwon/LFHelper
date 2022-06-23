using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace FileSeeker
{
    internal class Program
    {
        static readonly List<string> Commands = new List<string>();
        static readonly List<string> CommandStrs = new List<string>();

        static void Main(string[] args)
        {
            Load();

            while (true)
            {
                string input = Console.ReadLine();
                string[][] inputArr = Trimming(input);

                if (inputArr.Length == 1 && inputArr[0].Length == 3)
                {
                    string command = inputArr[0][0];
                    string titleHint = inputArr[0][1];
                    string contentHint = inputArr[0][2];

                    if (int.TryParse(command, out int num) && num >= 1 && num <= Commands.Count)
                    {
                        string resDir = @"res_command_" + num + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        Directory.CreateDirectory(resDir);
                        CheckDir(new DirectoryInfo(CommandStrs[num - 1]), resDir, titleHint, contentHint);
                    }
                    else
                    {
                        Console.WriteLine("Wrong Command");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Wrong Input");
                    continue;
                }

                Console.WriteLine("Done");
            }
        }

        private static void CheckDir(DirectoryInfo d, string dir, string th, string ch)
        {
            foreach (FileInfo f in d.GetFiles())
            {
                if (f.Extension == ".zip")
                {
                    using (ZipArchive za = ZipFile.OpenRead(f.FullName))
                    {
                        foreach (ZipArchiveEntry zae in za.Entries)
                        {
                            if (zae.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)
                                || zae.FullName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                            {
                                if (!File.Exists(dir + @"\" + zae.Name) && zae.Name.Contains(th))
                                {
                                    using (Stream s = zae.Open())
                                    {
                                        using (StreamReader sr = new StreamReader(s))
                                        {
                                            string content = sr.ReadToEnd();
                                            if (content.Contains(ch))
                                            {
                                                zae.ExtractToFile(dir + @"\" + zae.Name);
                                                Console.WriteLine("\n----------------------------------------\n" 
                                                    + "File Out From Zip : \n" + zae.Name + "\n From : \n" + f.FullName
                                                    + "\n----------------------------------------\n");
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
                    if (!File.Exists(dir + @"\" + f.Name) && f.Name.Contains(th))
                    {
                        string content = File.ReadAllText(f.FullName);
                        if (content.Contains(ch))
                        {
                            File.Copy(f.FullName, dir + @"\" + f.Name);
                            Console.WriteLine("\n----------------------------------------\n" 
                                + "File Copied : \n" + dir + @"\" + f.Name + "\n From : \n" + f.FullName
                                + "\n--------------------------------------------------\n");
                        }
                    }
                }
            }

            foreach (DirectoryInfo dd in d.GetDirectories())
            {
                CheckDir(dd, dir, th, ch);
            }
        }

        private static string[][] Trimming(string str)
        {
            string[] lines = str.Replace("\r", "").Split('\n');
            List<string> lineList = new List<string>();
            foreach (string s in lines)
            {
                if (!lineList.Contains(s))
                {
                    lineList.Add(s);
                }
            }

            List<string[]> tmp = new List<string[]>();
            foreach (string s in lineList)
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

        private static string Trim(string s)
        {
            return s.Replace("\r", "")
                .Replace("\n", "")
                .Replace("\t", "")
                .Replace(" ", "");
        }

        private static void Load()
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            Commands.Clear();
            CommandStrs.Clear();
            int menu = 0;
            foreach (string fs in Directory.GetFiles(Directory.GetCurrentDirectory()))
            {
                if (fs.EndsWith(".txt"))
                {
                    Console.WriteLine((++menu) + " : " + Path.GetFileName(fs));
                    Commands.Add(fs);
                    CommandStrs.Add(File.ReadAllText(fs));
                }
            }
            Console.WriteLine();
        }
    }
}

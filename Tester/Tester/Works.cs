using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    public class Works
    {
        public static void FileWatcherText()
        {
            FileSystemWatcher watcher = new FileSystemWatcher(@"TestDir");

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            watcher.Filter = "*.txt";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine($"Changed: {e.FullPath}");
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e) =>
            Console.WriteLine($"Deleted: {e.FullPath}");

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
        }

        private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }


        public static void FileSearch()
        {
            //Todo
            //1.File Watcher            







            //File Watcher Test
            //string dir = @"TestDir";            
            //DirectoryInfo d = new DirectoryInfo(dir);
            //GetDrillDownDirectories(d, 0);

            //while (true)
            //{
            //    try
            //    {
            //        string s;
            //        s = Console.ReadLine();
            //        DirectoryInfo dx = new DirectoryInfo(s);
            //        GetDrillDownDirectories(dx, 0);
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //    }
            //}
        }

        public static void GetDrillDownDirectories(DirectoryInfo d, int depth)
        {
            foreach (DirectoryInfo di in d.GetDirectories())
            {
                WriteIntend(depth + 1);
                Console.WriteLine("Dir-" + di.Name);
                GetDrillDownDirectories(di, depth + 1);
            }

            foreach (FileInfo fi in d.GetFiles())
            {
                WriteIntend(depth + 1);
                Console.WriteLine("File-" + fi.Name);
                DoFile(fi);
            }
        }

        public static void DoFile(FileInfo fi)
        {
            string target = "0778754007010";
            string text = File.ReadAllText(fi.FullName);
            if (text.Contains(target))
            {
                Console.WriteLine("res ----->" + fi.FullName);
                Process.Start(fi.FullName);
            }
        }

        public static void WriteIntend(int depth)
        {
            for (int i = 0; i < depth; i++)
            {
                Console.Write("  ");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SSMSHelper2
{
    public delegate int MouseEventCallBack(IntPtr code, int x, int y);
    public delegate int KeyEventCallBack(IntPtr code, int key);

    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        public static extern bool SetKeyboardState(byte[] lpKeyState);

        private static readonly List<MyCommand> Commands = new List<MyCommand>();

        public Form1()
        {
            InitializeComponent();
            AllocConsole();

            HookEvents.EnableHook();
            //HookEvents.Mc = new MouseEventCallBack(MCallback);
            HookEvents.Kc = new KeyEventCallBack(KCallback);

            checkBox1.Checked = true;

            LoadSetting();
        }

        private static int KCallback(IntPtr code, int key)
        {
            try
            {
                //Console.WriteLine(key);
                //New Object Model Test
                foreach (MyCommand myCommand in Commands)
                {
                    int res;
                    //0 -> no action, -1 -> action, 1 -> action and sendkey
                    res = myCommand.Execute(key
                        , GetActiveWindowTitle()
                        , HookEvents.keyPressing[160] == 1 //shift
                        , HookEvents.keyPressing[162] == 1 //ctrl
                        );
                    if (res != 0)
                    {
                        return res;
                    }
                }

                //~New Object Model Test
            }
            catch (Exception e)
            {
                //File.WriteAllText("ErrorLog" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt", e.ToString());
                Console.WriteLine(e.ToString());
            }
            finally
            {

            }
            return 0;
        }

        private static int MCallback(IntPtr code, int x, int y)
        {
            return 0;
        }

        private static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }

            return "";
        }

        private static void LoadSetting()
        {
            Commands.Clear();
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            FileInfo[] files = di.GetFiles();
            foreach (FileInfo fi in files)
            {
                if (fi.Name.StartsWith("!"))
                {
                    //Console.WriteLine(fi.FullName);
                    Commands.Add(new MyCommand(File.ReadAllText(fi.FullName)));
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                TopMost = true;
            }
            else
            {
                TopMost = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadSetting();
        }
    }

    public class MyCommand
    {
        public string fileText = "";

        public readonly List<string> includes = new List<string>();
        public readonly List<string> excludes = new List<string>();

        public readonly List<MyCommandItem> items = new List<MyCommandItem>();

        public MyCommand(string fileText)
        {
            this.fileText = fileText;

            string[] itemstrings = fileText.Split('@');

            int i;
            string[] cludes = itemstrings[0].Replace("\r", "")
                .Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in cludes)
            {
                if (s.StartsWith("+"))
                {
                    includes.Add(s.Substring(1, s.Length - 1));
                }
                else if (s.StartsWith("-"))
                {
                    excludes.Add(s.Substring(1, s.Length - 1));
                }
            }

            for (i = 1; i < itemstrings.Length; i++)
            {
                if (!itemstrings[i].StartsWith("!"))
                {
                    items.Add(new MyCommandItem(itemstrings[i]));
                }
            }
        }

        public int Execute(int key, string window, bool shift = false, bool control = false)
        {
            bool inc = false, exc = false;

            //code
            foreach (string s in includes)
            {
                if (window.Contains(s))
                {
                    inc = true;
                    break;
                }
            }

            foreach (string s in excludes)
            {
                if (window.Contains(s))
                {
                    exc = true;
                    break;
                }
            }

            if (inc && !exc)
            {
                foreach (MyCommandItem item in items)
                {
                    int res;
                    res = item.Execute(key, shift, control);
                    if (res != 0)
                    {
                        return res;
                    }
                }
            }
            //~code

            return 0;
        }
    }

    public class MyCommandItem
    {
        public string itemString = "";
        public string name = "";
        public string key = "";
        public string type = "";
        public string content = "";
        public MyCommandItem(string itemString)
        {
            this.itemString = itemString;
            string[] lines = itemString.Replace("\r", "")
                .Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            int i;
            name = lines[0];
            key = lines[1];
            type = lines[2];

            for (i = 3; i < lines.Length; i++)
            {
                content += lines[i];
                if (i != lines.Length - 1)
                {
                    content += "\r\n";
                }
            }
        }

        public bool Matches(string keyString, bool shift, bool control)
        {
            string keySet;
            if ((key.StartsWith("^+{") || key.StartsWith("+^{")) && shift && control)
            {
                keySet = key.Substring(2, key.Length - 2);
                return keySet == keyString;
            }
            else if ((key.StartsWith("^{") && control && !shift)
                || (key.StartsWith("+{") && !control && shift))
            {
                keySet = key.Substring(1, key.Length - 1);
                return keySet == keyString;
            }
            else if (key.StartsWith("{"))
            {
                keySet = key;
                return keySet == keyString;
            }
            else if (key.Length == 1)
            {
                keySet = "{" + key + "}";
                return keySet == keyString;
            }

            return false;
        }

        public int Execute(int keyCode, bool shift, bool control)
        {
            Keys keyObj = (Keys)keyCode;
            string keyIn = "{" + keyObj.ToString() + "}";            

            if (Matches(keyIn, shift, control))
            {
                Console.Write(keyIn + " matches / ");
                //Run Step
                if (type == "1")//key conversion
                {
                    Console.WriteLine(content + " out");
                    SendKeys.Send(content);
                }
                else if (type[0] == '2')//write text
                {
                    string original = Clipboard.GetText();
                    string toWrite = "";
                    if (type[2] == '1')
                    {
                        toWrite = content;
                    }
                    else if (type[2] == '2')
                    {
                        string[][] items = MyTrimming(original);

                        int i;
                        toWrite = content;
                        for (i = 0; i < items[0].Length; i++)
                        {
                            toWrite = toWrite.Replace("{" + i + "}", items[0][i]);
                        }
                    }
                    else if (type[2] == '3')
                    {
                        string[] cLines = content.Replace("\r", "")
                            .Split(new string[] { "\n" }
                            , StringSplitOptions.RemoveEmptyEntries);

                        if (cLines.Length == 3)
                        {
                            StringBuilder sb = new StringBuilder(cLines[0] + "\r\n");
                            string middle;
                            string[][] items = MyTrimming(original);

                            int i, j;

                            for (i = 0; i < items.Length; i++)
                            {
                                middle = cLines[1];
                                for (j = 0; j < items[i].Length; j++)
                                {
                                    middle = middle.Replace("{" + j + "}", items[i][j]);
                                }
                                sb.Append(middle + "\r\n");
                            }

                            sb.Append(cLines[2]);
                            toWrite = sb.ToString();
                        }
                    }
                    Clipboard.SetText(toWrite ?? "");
                    SendKeys.Send((control ? "" : "^") + "{v}");
                    Clipboard.SetText(original ?? "");
                    Console.WriteLine(toWrite + " Text Out");
                }
                else if (type[0] == '3')//run exe
                {
                    string original = Clipboard.GetText();
                    string[][] items = MyTrimming(original);
                    int i;
                    string toWrite = content;
                    if (items.Length > 0)
                    {
                        for (i = 0; i < items[0].Length; i++)
                        {
                            toWrite = toWrite.Replace("{" + i + "}", items[0][i]);
                        }
                    }

                    Process process = new Process();

                    if (type[2] == '1')//cmd with new prompt
                    {
                        process.StartInfo.UseShellExecute = true;
                        process.StartInfo.RedirectStandardOutput = false;
                        process.StartInfo.FileName = "cmd.exe";
                        process.StartInfo.Arguments = content;
                        process.Start();
                    }
                    else if (type[2] == '2')//cmd with output
                    {
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.FileName = "cmd.exe";
                        process.StartInfo.Arguments = content;
                        process.Start();

                        string res = process.StandardOutput.ReadToEnd();
                        //Console.WriteLine(res);

                        if (type.Length == 5)
                        {
                            if (type[4] == '1') //save file and console out filename
                            {
                                string fileName = "Output for " + name
                                    + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                                File.WriteAllText(fileName, res);
                                Console.WriteLine(fileName);
                            }
                            else if (type[4] == '2') //save file and open file
                            {
                                string fileName = "Output for " + name
                                    + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                                File.WriteAllText(fileName, res);
                                Process.Start("Notepad.exe", fileName);
                            }
                            else if (type[4] == '3') //store clipboard
                            {
                                Clipboard.SetText(res);
                            }
                            else if (type[4] == '4') //paste
                            {
                                Clipboard.SetText(res);
                                SendKeys.Send((control ? "" : "^") + "{v}");
                                Clipboard.SetText(original ?? "");
                            }
                        }
                    }
                    else if (type[2] == '3') //open exe
                    {
                        string[] cLines = toWrite.Replace("\r", "")
                            .Split(new string[] { "\n" }
                            , StringSplitOptions.RemoveEmptyEntries);

                        string param = "";

                        for (i = 1; i < cLines.Length; i++)
                        {
                            param += cLines[i];
                        }

                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardOutput = false;
                        process.StartInfo.FileName = cLines[0];
                        process.StartInfo.Arguments = param;
                        process.Start();
                    }

                    Console.WriteLine("Process Start");
                }
                return name.EndsWith("*") ? 1 : -1;
            }
            return 0;
        }

        private string[][] MyTrimming(string str)
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
                            stacks.Add(MyTrim(ss.Trim()));
                        }
                    }
                    tmp.Add(stacks.ToArray());
                }
            }

            return tmp.ToArray();
        }

        private string MyTrim(string s)
        {
            return s.Replace("\r", "")
                .Replace("\n", "")
                .Replace("\t", "");
        }
    }

    class HookEvents
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private const int WH_MOUSE_LL = 14;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_MBUTTONDOWN = 0x0207;

        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_RBUTTONUP = 0x0205;
        private const int WM_MBUTTONUP = 0x0208;

        private const int WM_MOUSEMOVE = 0x0200;

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        private static LowLevelProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private delegate IntPtr LowLevelProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static MouseEventCallBack mc;
        private static KeyEventCallBack kc;

        public static byte[] keyPressing = new byte[256];

        public static byte mouseLPressing;
        public static byte mouseRPressing;
        public static byte mouseMPressing;

        public static MouseEventCallBack Mc { get => mc; set => mc = value; }
        public static KeyEventCallBack Kc { get => kc; set => kc = value; }

        private static IntPtr SetHookKeyBoard(LowLevelProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr SetHookMouse(LowLevelProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int keyCode = Marshal.ReadInt32(lParam);

                if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    if (keyPressing[keyCode] == 0) //not allow pressing
                    {
                        //Console.WriteLine("DOWN : " + keyCode);
                        keyPressing[keyCode] = 1;
                        int res = Kc(wParam, keyCode);

                        if (res == -1)
                        {
                            return (IntPtr)(-1);
                        }
                    }
                }

                if (wParam == (IntPtr)WM_KEYUP)
                {
                    //Console.WriteLine("UP : " + keyCode);
                    keyPressing[keyCode] = 0;
                }

                //if (wParam == (IntPtr)WM_LBUTTONDOWN)
                //{
                //    mouseLPressing = 1;
                //}
                //if (wParam == (IntPtr)WM_LBUTTONUP)
                //{
                //    mouseLPressing = 0;
                //    Mc((IntPtr)0, 0, 0);
                //}

                //if (wParam == (IntPtr)WM_RBUTTONDOWN)
                //{
                //    mouseRPressing = 1;
                //}
                //if (wParam == (IntPtr)WM_RBUTTONUP)
                //{
                //    mouseRPressing = 0;
                //    Mc((IntPtr)1, 0, 0);
                //}

                //if (wParam == (IntPtr)WM_MBUTTONDOWN)
                //{
                //    mouseMPressing = 1;
                //}
                //if (wParam == (IntPtr)WM_MBUTTONUP)
                //{
                //    mouseMPressing = 0;
                //    Mc((IntPtr)2, 0, 0);
                //}
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        public static void EnableHook()
        {
            _hookID = SetHookKeyBoard(_proc);
            _hookID = SetHookMouse(_proc);
        }

        public static void DisableHook()
        {
            UnhookWindowsHookEx(_hookID);
        }
    }
}
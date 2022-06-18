using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SSMSHelper2
{
    public delegate void MouseEventCallBack(IntPtr code, int x, int y);
    public delegate void KeyEventCallBack(IntPtr code, int key);

    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        static readonly string[] opsStr = new string[10];

        public Form1()
        {
            InitializeComponent();
            AllocConsole();

            HookEvents.EnableHook();
            //HookEvents.Mc = new MouseEventCallBack(MCallback);
            HookEvents.Kc = new KeyEventCallBack(KCallback);
            SetCommand();
        }

        private static void KCallback(IntPtr code, int key)
        {
            if (GetActiveWindowTitle().Contains("Microsoft SQL Server Management Studio")
                && HookEvents.keyPressing[162] == 1 && key >= 48 && key <= 57)
            {
                string keyStr = "" + (char)key;
                if (int.TryParse(keyStr, out int x))
                {
                    string command = opsStr[x];
                    if (command != null)
                    {
                        string[] commands = command.Split('\n');
                        if (commands.Length == 3)
                        {
                            string original = Clipboard.GetText();
                            string res = Trim(commands[0]) == "'" ? "" : commands[0].Trim() + Environment.NewLine;
                            int i, j;

                            string[][] sa = Trimming(original);

                            for (i = 0; i < sa.Length; i++)
                            {
                                string regex = commands[1].Trim();
                                List<string> tl = new List<string>();

                                for (j = 0; j < sa[i].Length; j++)
                                {
                                    if (sa[i][j].Length > 0)
                                    {
                                        tl.Add(sa[i][j].Trim());
                                    }
                                }

                                for (j = 0; j < tl.Count; j++)
                                {
                                    regex = regex.Replace("{" + j + "}", tl[j]);
                                }

                                res += regex + Environment.NewLine;

                            }
                            res += Trim(commands[2]) == "'" ? "" : commands[2].Trim() + Environment.NewLine;

                            Clipboard.SetText(res);
                            SendKeys.Send("{v}");
                            Clipboard.SetText(original);
                        }
                        else if (commands.Length == 1)
                        {
                            string original = Clipboard.GetText();
                            Clipboard.SetText(commands[0]);
                            SendKeys.Send("{v}");
                            Clipboard.SetText(original);

                        }
                        else
                        {
                            Console.WriteLine("Wrong Command");
                        }
                    }
                }
            }
        }

        private static void MCallback(IntPtr code, int x, int y)
        {

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

        private static void SetCommand()
        {
            foreach (string fs in Directory.GetFiles(Directory.GetCurrentDirectory()))
            {
                if (fs.EndsWith(".txt"))
                {
                    string target = Path.GetFileName(fs).Substring(0, 1);
                    if (int.TryParse(target, out int i) && opsStr[i] == null)
                    {
                        Console.WriteLine(i + " : " + Path.GetFileName(fs));
                        opsStr[i] = File.ReadAllText(fs);
                    }
                }
            }
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
                    if (keyPressing[keyCode] == 0)
                    {
                        Console.WriteLine("DOWN : " + keyCode);
                        keyPressing[keyCode] = 1;
                        Kc(wParam, keyCode);
                    }
                }

                if (wParam == (IntPtr)WM_KEYUP)
                {
                    Console.WriteLine("UP : " + keyCode);
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
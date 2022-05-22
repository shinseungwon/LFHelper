using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SSMSHelper2
{
    public delegate void MouseEventCallBack(IntPtr code, int x, int y);
    public delegate void KeyEventCallBack(IntPtr code, int key);

    public partial class Form1 : Form
    {
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        static Process p;

        public Form1()
        {
            InitializeComponent();
            AllocConsole();

            p = Process.GetProcessesByName("NotePad++").FirstOrDefault();
            if (p != null)
            {
                Console.WriteLine("Ready");
            }
            else
            {
                Console.WriteLine("Not Ready");
            }

            HookEvents.EnableHook();
            HookEvents.Mc = new MouseEventCallBack(MCallback);
            HookEvents.Kc = new KeyEventCallBack(KCallback);            
        }

        private static void KCallback(IntPtr code, int key)
        {
            if (GetActiveWindowTitle().IndexOf("Notepad++") > 0
                && HookEvents.keyPressing[162] == 1)
            {
                Console.WriteLine("key : " + key);
            }
        }

        private static void MCallback(IntPtr code, int x, int y)
        {
            if (GetActiveWindowTitle().IndexOf("Notepad++") > 0)
            {
                Console.WriteLine("mouse : " + code + " - " + x + "," + y);
            }
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

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            HookEvents.DisableHook();
        }

        private void btn_about_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Email : ssw900528@gmail.com" + Environment.NewLine + "GitHub Source : shinseungwon/D3HelperX", "About");
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

        [StructLayout(LayoutKind.Sequential)]
        public class Point
        {
            public int x;
            public int y;
        }

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
                    keyPressing[keyCode] = 1;
                }

                if (wParam == (IntPtr)WM_KEYUP)
                {
                    keyPressing[keyCode] = 0;
                    Kc(wParam, keyCode);
                }

                if (wParam == (IntPtr)WM_LBUTTONDOWN)
                {
                    mouseLPressing = 1;
                }
                if (wParam == (IntPtr)WM_LBUTTONUP)
                {
                    mouseLPressing = 0;
                    Mc((IntPtr)0, 0, 0);
                }

                if (wParam == (IntPtr)WM_RBUTTONDOWN)
                {
                    mouseRPressing = 1;
                }
                if (wParam == (IntPtr)WM_RBUTTONUP)
                {
                    mouseRPressing = 0;
                    Mc((IntPtr)1, 0, 0);
                }

                if (wParam == (IntPtr)WM_MBUTTONDOWN)
                {
                    mouseMPressing = 1;
                }
                if (wParam == (IntPtr)WM_MBUTTONUP)
                {
                    mouseMPressing = 0;
                    Mc((IntPtr)2, 0, 0);
                }
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
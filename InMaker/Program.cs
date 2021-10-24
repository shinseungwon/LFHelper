using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InMaker
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string s = Clipboard.GetText();
            string[] ss = s.Split('\n');
            int sl = ss.Length, i;
            int cnt = 0;
            for (i = 0; i < sl; i++)
            {
                if(ss[i] != "")
                {
                    cnt++;
                }
            }

            string res = "('Total " + cnt + " Rows'\n";
            for (i = 0; i < sl; i++)
            {
                if(ss[i] != "")
                {
                    res += ",'";
                    res += ss[i].Trim();
                    res += "'\n";
                }                
            }
            res += ")";            
            Clipboard.SetText(res);
        }
    }
}
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HelperDotNet
{
    /// <summary>
    /// Extra tool functions
    /// </summary>
    public static class Tools
    {
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

    /// <summary>
    /// Class for writing object to json when using ajax
    /// </summary>
    public class SPBag
    {
        public int returnValue;
        public string[] returnValueArray;
        public DataSet dataSet;
    }
}

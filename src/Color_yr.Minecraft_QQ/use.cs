using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Color_yr.Minecraft_QQ
{
    class use
    {
        public static string get_string(string a, string b, string c = null)
        {
            int x = a.IndexOf(b) + 1;
            int y;
            if (c != null)
            {
                y = a.IndexOf(c);
                return a.Substring(x, y - x);
            }
            else
            {
                return a.Substring(x);
            }
        }
        public static string RemoveLeft(string s, int len)
        {
            return s.PadLeft(len).Remove(0, len);
        }

        public static bool IsNatural_Number(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if ((int)str[i] > 127)
                    return false;
                else
                    return true;
            }
            return false;
        }

        public static string remove_pic(string a)
        {
            for (; a.IndexOf("[CQ:image") != -1;)
            {
                string b = get_string(a, "[", "]");
                a = a.Replace(b, "");
                a = a.Replace("[]", "");
            }
            return a;
        }
    }
}

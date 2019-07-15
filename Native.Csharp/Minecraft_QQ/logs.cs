using System;
using System.IO;

namespace Color_yr.Minecraft_QQ
{
    public class logs
    {
        public static string log = "logs.log";

        public static void Log_write(string a)
        {
            try
            {
                DateTime date = DateTime.Now;
                string year = date.ToShortDateString().ToString();
                string time = date.ToLongTimeString().ToString();
                string write = "[" + year + "]" + "[" + time + "]" + a;
                File.AppendAllText(Minecraft_QQ.path + log, write + Environment.NewLine);
            }
            catch 
            {

            }
        }
    }
}

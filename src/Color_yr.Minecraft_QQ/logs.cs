using System;
using System.IO;

namespace Color_yr.Minecraft_QQ
{
    public class logs
    {
        public static string log = "logs.log";

        public static void Log_write(string a)
        {
            DateTime date = DateTime.Now;
            String year = date.ToShortDateString().ToString();
            String time = date.ToLongTimeString().ToString();
            String write = "[" + year + "]" + "[" + time + "]" + a;
            File.AppendAllText(config_read.path+log, write + Environment.NewLine);
        }
    }
}

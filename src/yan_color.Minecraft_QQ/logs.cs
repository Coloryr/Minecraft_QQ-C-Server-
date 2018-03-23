using System;
using System.IO;

namespace yan_color.Minecraft_QQ
{
    public class logs
    {
        public static void Log_write(string a)
        {
            DateTime date = DateTime.Now;
            String year = date.ToShortDateString().ToString();
            String time = date.ToLongTimeString().ToString();
            String write = "[" + year + "]" + "[" + time + "]" + a;
            File.AppendAllText(Minecraft_QQ.path+Minecraft_QQ.log, write + Environment.NewLine);
        }
    }
}

using System;
using System.IO;

namespace yan_color.Minecraft_QQ
{
    public class logs
    {
        public static void Log_write(string a)
        {
            DateTime date = new DateTime();
            String year = String.Format("%tF", date);
            String time = String.Format("%tT", date);
            String write = "[" + year + "]" + "[" + time + "]" + a;
            File.AppendAllText(Minecraft_QQ.path+Minecraft_QQ.log, write + Environment.NewLine);
        }
    }
}

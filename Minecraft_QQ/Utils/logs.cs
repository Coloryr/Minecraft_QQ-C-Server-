using System;
using System.IO;

namespace Color_yr.Minecraft_QQ.Utils
{
    public class logs
    {
        public static string log = "logs.log";
        private static object obj = new object();

        public static void LogWrite(string a)
        {
            try
            {
                lock (obj)
                {
                    DateTime date = DateTime.Now;
                    string year = date.ToShortDateString().ToString();
                    string time = date.ToLongTimeString().ToString();
                    string write = "[" + year + "]" + "[" + time + "]" + a;
                    File.AppendAllText(Minecraft_QQ.Path + log, write + Environment.NewLine);
                }
            }
            catch
            {

            }
        }
    }
}

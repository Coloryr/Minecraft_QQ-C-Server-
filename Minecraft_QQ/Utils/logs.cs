using System;
using System.IO;

namespace Minecraft_QQ.Utils
{
    internal class Logs
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

        public static void LogError(Exception e)
        {
            LogWrite("[ERROR]" + e.Message + "\n" + e.StackTrace);
        }

        public static void LogError(string e)
        {
            LogWrite("[ERROR]" + e);
        }
    }
}

using System;
using System.IO;

namespace Minecraft_QQ.Utils
{
    internal class Logs
    {
        public static string log = "logs.log";
        private static object obj = new object();

        private static void LogWrite(string a)
        {
            try
            {
                lock (obj)
                {
                    DateTime date = DateTime.Now;
                    string year = date.ToShortDateString().ToString();
                    string time = date.ToLongTimeString().ToString();
                    string write = "[" + year + "]" + "[" + time + "]" + a;
                    IMinecraft_QQ.LogCall?.Invoke(write);
                    File.AppendAllText(Minecraft_QQ.Path + log, write + Environment.NewLine);
                }
            }
            catch
            {

            }
        }

        public static void LogError(Exception e)
        {
            LogWrite("[Error]" + e.Message + "\n" + e.StackTrace);
        }

        public static void LogError(string e)
        {
            LogWrite("[Error]" + e);
        }

        internal static void LogOut(string v)
        {
            LogWrite("[Info]" + v);
        }
    }
}

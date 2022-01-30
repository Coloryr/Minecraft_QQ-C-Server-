using Minecraft_QQ_Core.Utils;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Minecraft_QQ_Core.Config
{
    public class ConfigWrite
    {
        public static void Ask()
        {
            Save(ConfigFile.AskConfig.FullName, IMinecraft_QQ.Main.AskConfig);
        }
        public static void Command()
        {
            Save(ConfigFile.CommandSave.FullName, IMinecraft_QQ.Main.CommandConfig);
        }
        public static void Config()
        {
            Save(ConfigFile.MainConfig.FullName, IMinecraft_QQ.Main.MainConfig);
        }
        public static void Group()
        {
            Save(ConfigFile.GroupConfig.FullName, IMinecraft_QQ.Main.GroupConfig);
        }
        public static void Player()
        {
            Save(ConfigFile.PlayerSave.FullName, IMinecraft_QQ.Main.PlayerConfig);
        }
        public static void All()
        {
            Ask();
            Command();
            Config();
            Group();
            Player();
        }

        private static void Save(string FileName, object obj)
        {
            Task.Run(() =>
            {
                try
                {
                    File.WriteAllText(FileName,
                    JsonConvert.SerializeObject(obj, Formatting.Indented));
                }
                catch (Exception e)
                {
                    Logs.LogError(e);
                    IMinecraft_QQ.ShowMessageCall?.Invoke($"配置{FileName}在写入时发发生了错误");
                }
            });
        }
    }
}

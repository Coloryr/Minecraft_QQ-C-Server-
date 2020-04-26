using Minecraft_QQ.Utils;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minecraft_QQ.Config
{
    internal class ConfigWrite
    {
        public void Config()
        {
            Save(ConfigFile.主要配置文件.FullName, Minecraft_QQ.MainConfig);
        }
        public void Group()
        {
            Save(ConfigFile.群设置.FullName, Minecraft_QQ.GroupConfig);
        }
        public void Player()
        {
            Save(ConfigFile.玩家储存.FullName, Minecraft_QQ.PlayerConfig);
        }
        public void All()
        {
            Config();
            Group();
            Player();
        }

        private void Save(string FileName, object obj)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    File.WriteAllText(FileName,
                    JsonConvert.SerializeObject(obj, Formatting.Indented));

                }
                catch (Exception e)
                {
                    Logs.LogError(e);
                    if (MessageBox.Show("配置" + FileName + "在写入时发发生了错误，是否要删除原来的配置文件再新生成一个？",
                         "配置文件错误", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        File.Delete(FileName);
                        Save(FileName, obj);
                    }
                }
            });
        }
    }
}

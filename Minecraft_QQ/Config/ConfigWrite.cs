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
            try
            {
                Task.Factory.StartNew(() =>
                {
                    File.WriteAllText(ConfigFile.主要配置文件.FullName,
                    JsonConvert.SerializeObject(Minecraft_QQ.MainConfig, Formatting.Indented));
                });
            }
            catch (Exception)
            {
                if (MessageBox.Show("配置文件在写入时发发生了错误，是否要删除原来的配置文件再新生成一个？",
                    "配置文件错误", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Delete(ConfigFile.主要配置文件.FullName);
                    Config();
                }
            }
        }
        public void Group()
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    File.WriteAllText(ConfigFile.群设置.FullName,
                    JsonConvert.SerializeObject(Minecraft_QQ.GroupConfig, Formatting.Indented));
                });
            }
            catch (Exception)
            {
                if (MessageBox.Show("配置文件在写入时发发生了错误，是否要删除原来的配置文件再新生成一个？",
                     "配置文件错误", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Delete(ConfigFile.群设置.FullName);
                    Group();
                }
            }
        }
        public void Player()
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    File.WriteAllText(ConfigFile.玩家储存.FullName,
                    JsonConvert.SerializeObject(Minecraft_QQ.PlayerConfig, Formatting.Indented));
                });
            }
            catch (Exception)
            {
                if (MessageBox.Show("配置文件在写入时发发生了错误，是否要删除原来的配置文件再新生成一个？",
                     "配置文件错误", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Delete(ConfigFile.玩家储存.FullName);
                    Player();
                }
            }
        }
        public void All()
        {
            Config();
            Group();
            Player();
        }
    }
}

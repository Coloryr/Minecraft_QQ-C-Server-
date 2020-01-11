using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    class Config_write
    {
        public void Write_config()
        {
            try
            {
                File.WriteAllText(Config_file.config.FullName, JsonConvert.SerializeObject(Minecraft_QQ.Mainconfig, Formatting.Indented));
            }
            catch (Exception)
            {
                if (MessageBox.Show("配置文件在写入时发发生了错误，是否要删除原来的配置文件再新生成一个？",
                    "配置文件错误", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Delete(Config_file.config.FullName);
                    Write_config();
                }
            }
        }
        public void Write_Group()
        {
            try
            {
                File.WriteAllText(Config_file.group.FullName, JsonConvert.SerializeObject(Minecraft_QQ.Groupconfig, Formatting.Indented));
            }
            catch (Exception)
            {
                if (MessageBox.Show("配置文件在写入时发发生了错误，是否要删除原来的配置文件再新生成一个？",
                     "配置文件错误", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Delete(Config_file.group.FullName);
                    Write_Group();
                }
            }
        }
        public void Write_player()
        {
            try
            {
                File.WriteAllText(Config_file.player.FullName, JsonConvert.SerializeObject(Minecraft_QQ.Playerconfig, Formatting.Indented));
            }
            catch (Exception)
            {
                if (MessageBox.Show("配置文件在写入时发发生了错误，是否要删除原来的配置文件再新生成一个？",
                     "配置文件错误", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Delete(Config_file.player.FullName);
                    Write_player();
                }
            }
        }
    }
}

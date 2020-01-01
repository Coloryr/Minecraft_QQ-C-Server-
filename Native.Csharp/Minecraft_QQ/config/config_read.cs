using Newtonsoft.Json;
using System;
using System.IO;

namespace Color_yr.Minecraft_QQ
{
    class Config_read
    {
        /// <summary>
        /// 读取主要配置文件
        /// </summary>
        public Mainconfig_obj Read_config()
        {
            logs.Log_write("[INFO][Config]读取主配置");
            try
            {
                return JsonConvert.DeserializeObject<Mainconfig_obj>
                    (File.ReadAllText(Config_file.config.FullName));
            }
            catch (Exception e)
            {
                logs.Log_write("[ERROR][Config]配置文件读取错误：" + e.Message);
                return new Mainconfig_obj();
            }
        }
        public Player_obj Read_player()
        {
            logs.Log_write("[INFO][Config]读取玩家配置");
            try
            {
                return JsonConvert.DeserializeObject<Player_obj>
                    (File.ReadAllText(Config_file.player.FullName));
            }
            catch (Exception e)
            {
                logs.Log_write("[ERROR][Config]配置文件读取错误：" + e.Message);
                return new Player_obj();
            }
        }
        public Group_obj Read_group()
        {
            logs.Log_write("[INFO][Config]读取群设置");
            try
            {
                var temp = JsonConvert.DeserializeObject<Group_obj>
                    (File.ReadAllText(Config_file.group.FullName));
                foreach (Group_save_obj a in temp.群列表.Values)
                { 
                    if(a.main == true)
                    {
                        Minecraft_QQ.GroupSet_Main = a.group_l;
                        break;
                    }
                }
                return temp;
            }
            catch(Exception e)
            {
                logs.Log_write("[ERROR][Config]配置文件读取错误：" + e.Message);
                return new Group_obj();
            }
        }
        public Ask_obj Read_ask()
        {
            logs.Log_write("[INFO][Config]读取自定义应答");
            try
            {
                return JsonConvert.DeserializeObject<Ask_obj>
                    (File.ReadAllText(Config_file.ask.FullName));
            }
            catch (Exception e)
            {
                logs.Log_write("[ERROR][Config]配置文件读取错误：" + e.Message);
                return new Ask_obj();
            }
        }
        public Command_obj Read_commder()
        {
            logs.Log_write("[INFO][Config]读取自定义指令");
            try
            {
                return JsonConvert.DeserializeObject<Command_obj>
                    (File.ReadAllText(Config_file.command.FullName));
            }
            catch (Exception e)
            {
                logs.Log_write("[ERROR][Config]配置文件读取错误：" + e.Message);
                return new Command_obj();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Xml;

namespace Color_yr.Minecraft_QQ
{
    class config_read
    {
        public static void read_config()
        {
            logs.Log_write("[INFO][Config]读取配置文件中");
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(Minecraft_QQ.path + config_file.config);
                XmlNodeList nodeList = xmldoc.SelectSingleNode("config").ChildNodes;
                foreach (XmlNode xn in nodeList)//遍历所有子节点
                {
                    if (xn == null)
                        break;
                    switch (xn.Name)
                    {
                        case "设置":
                            XmlNode xnLurl;
                            xnLurl = xn.SelectSingleNode("自动应答");
                            if (xnLurl != null)
                                main_config.message_enable = xnLurl.InnerText == "开" ? true : false;
                            xnLurl = xn.SelectSingleNode("彩色代码");
                            if (xnLurl != null)
                                main_config.color_code = xnLurl.InnerText == "开" ? true : false;
                            xnLurl = xn.SelectSingleNode("维护模式");
                            if (xnLurl != null)
                                main_config.fix_mode = xnLurl.InnerText == "开" ? true : false;
                            xnLurl = xn.SelectSingleNode("同步对话");
                            if (xnLurl != null)
                                main_config.allways_send = xnLurl.InnerText == "开" ? true : false;
                            xnLurl = xn.SelectSingleNode("服务器昵称");
                            if (xnLurl != null)
                                main_config.nick_server = xnLurl.InnerText == "开" ? true : false;
                            xnLurl = xn.SelectSingleNode("群昵称");
                            if (xnLurl != null)
                                main_config.nick_group = xnLurl.InnerText == "开" ? true : false;
                            xnLurl = xn.SelectSingleNode("允许玩家绑定ID");
                            if (xnLurl != null)
                                main_config.set_name = xnLurl.InnerText == "开" ? true : false;
                            break;
                        case "文本":
                            XmlNode xnLurl0;
                            xnLurl0 = xn.SelectSingleNode("发送至服务器的文本");
                            if (xnLurl0 != null)
                                message_config.send_text = xnLurl0.InnerText;
                            xnLurl0 = xn.SelectSingleNode("维护时发送的文本");
                            if (xnLurl0 != null)
                                message_config.fix_send = xnLurl0.InnerText;
                            xnLurl0 = xn.SelectSingleNode("未知指令");
                            if (xnLurl0 != null)
                                message_config.unknow = xnLurl0.InnerText;
                            break;
                        case "检测":
                            XmlNode xnLurl1;
                            xnLurl1 = xn.SelectSingleNode("检测头");
                            if (xnLurl1 != null)
                                check_config.head = xnLurl1.InnerText;
                            xnLurl1 = xn.SelectSingleNode("在线人数");
                            if (xnLurl1 != null)
                                check_config.online_players = xnLurl1.InnerText;
                            xnLurl1 = xn.SelectSingleNode("状态检测");
                            if (xnLurl1 != null)
                                check_config.online_servers = xnLurl1.InnerText;
                            xnLurl1 = xn.SelectSingleNode("玩家绑定ID");
                            if (xnLurl1 != null)
                                check_config.player_setid = xnLurl1.InnerText;
                            xnLurl1 = xn.SelectSingleNode("玩家发送消息");
                            if (xnLurl1 != null)
                                check_config.send_message = xnLurl1.InnerText;
                            break;
                        case "管理":
                            XmlNode xnLurl2;
                            xnLurl2 = xn.SelectSingleNode("禁言玩家");
                            if (xnLurl2 != null)
                                admin_config.mute = xnLurl2.InnerText;
                            xnLurl2 = xn.SelectSingleNode("取消禁言");
                            if (xnLurl2 != null)
                                admin_config.unmute = xnLurl2.InnerText;
                            xnLurl2 = xn.SelectSingleNode("查询ID");
                            if (xnLurl2 != null)
                                admin_config.check = xnLurl2.InnerText;
                            xnLurl2 = xn.SelectSingleNode("重命名玩家");
                            if (xnLurl2 != null)
                                admin_config.rename = xnLurl2.InnerText;
                            xnLurl2 = xn.SelectSingleNode("切换服务器维护模式");
                            if (xnLurl2 != null)
                                admin_config.fix = xnLurl2.InnerText;
                            xnLurl2 = xn.SelectSingleNode("配置文件重读");
                            if (xnLurl2 != null)
                                admin_config.reload = xnLurl2.InnerText;
                            xnLurl2 = xn.SelectSingleNode("打开菜单");
                            if (xnLurl2 != null)
                                admin_config.menu = xnLurl2.InnerText;
                            xnLurl2 = xn.SelectSingleNode("昵称");
                            if (xnLurl2 != null)
                                admin_config.nick = xnLurl2.InnerText;
                            xnLurl2 = xn.SelectSingleNode("禁止绑定列表");
                            if (xnLurl2 != null)
                                admin_config.unbind_list = xnLurl2.InnerText;
                            xnLurl2 = xn.SelectSingleNode("禁言列表");
                            if (xnLurl2 != null)
                                admin_config.mute_list = xnLurl2.InnerText;
                            xnLurl2 = xn.SelectSingleNode("发送给的人");
                            if (xnLurl2 != null)
                                long.TryParse(xnLurl2.InnerText, out admin_config.Admin_Send);
                            break;
                        case "Socket":
                            XmlNode xnLurl3;
                            xnLurl3 = xn.SelectSingleNode("地址");
                            if (xnLurl3 != null)
                                socket_config.setip = xnLurl3.InnerText;
                            xnLurl3 = xn.SelectSingleNode("端口");
                            if (xnLurl3 != null)
                                int.TryParse(xnLurl3.InnerText, out socket_config.Port);
                            xnLurl3 = xn.SelectSingleNode("编码类型");
                            if (xnLurl3 != null)
                                socket_config.code = xnLurl3.InnerText;
                            xnLurl3 = xn.SelectSingleNode("数据包头");
                            if (xnLurl3 != null)
                                socket_config.data_Head = xnLurl3.InnerText;
                            xnLurl3 = xn.SelectSingleNode("数据包尾");
                            if (xnLurl3 != null)
                                socket_config.data_End = xnLurl3.InnerText;
                            xnLurl3 = xn.SelectSingleNode("绑定IP");
                            if (xnLurl3 != null)
                                socket_config.useip = xnLurl3.InnerText == "开" ? true : false;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logs.Log_write("[ERROR][Config]" + e.Message);
            }
        }
        public static void read_cant_bind()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Minecraft_QQ.path + config_file.player);
            XmlNodeList nodeList = xmldoc.SelectSingleNode("config").ChildNodes;
            config_file.cant_bind.Clear();
            foreach (XmlNode xn in nodeList)//遍历所有子节点
            {
                if (xn.Name == "禁止绑定")
                {
                    foreach (XmlNode xn1 in xn)//遍历所有子节点
                    {
                        XmlNode ID = xn.SelectSingleNode("ID");
                        if (config_file.cant_bind.Contains(ID.InnerText.ToLower()) == false)
                            config_file.cant_bind.Add(ID.InnerText.ToLower());
                    }
                    return;
                }
            }
        }
        public static void read_mute()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Minecraft_QQ.path + config_file.player);
            XmlNodeList nodeList = xmldoc.SelectSingleNode("config").ChildNodes;
            config_file.cant_bind.Clear();
            foreach (XmlNode xn in nodeList)//遍历所有子节点
            {
                if (xn.Name == "禁言")
                {
                    foreach (XmlNode xn1 in xn)//遍历所有子节点
                    {
                        XmlNode ID = xn.SelectSingleNode("ID");
                        if (config_file.mute_list.Contains(ID.InnerText.ToLower()) == false)
                            config_file.mute_list.Add(ID.InnerText.ToLower());
                    }
                    return;
                }
            }
        }
        public static void read_player()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Minecraft_QQ.path + config_file.player);
            XmlNodeList nodeList = xmldoc.SelectSingleNode("config").ChildNodes;
            config_file.player_list.Clear();
            foreach (XmlNode xn in nodeList)//遍历所有子节点
            {
                XmlNode id = xn.SelectSingleNode("绑定ID");
                XmlNode nick = xn.SelectSingleNode("昵称");
                XmlNode qq = xn.SelectSingleNode("QQ号");
                XmlNode admin = xn.SelectSingleNode("管理员");
                if (id != null && nick != null && qq != null && admin != null
                    && use.IsNumber(qq.FirstChild.InnerText) == true)
                {
                    player_save player = new player_save();
                    long.TryParse(qq.InnerText, out player.qq);
                    if (config_file.player_list.ContainsKey(player.qq) == false)
                    {
                        player.player = id.InnerXml;
                        player.nick = nick.InnerXml;
                        player.admin = admin.InnerText == "开" ? true : false;
                        config_file.player_list.Add(player.qq, player);
                    }
                }
            }
        }
        public static void read_group()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Minecraft_QQ.path + config_file.group);
            XmlNodeList nodeList = xmldoc.SelectSingleNode("config").ChildNodes;
            config_file.group_list.Clear();
            foreach (XmlNode xn in nodeList)//遍历所有子节点
            {
                XmlNode group = xn.SelectSingleNode("群号");
                XmlNode commder = xn.SelectSingleNode("命令");
                XmlNode say = xn.SelectSingleNode("对话");
                XmlNode main = xn.SelectSingleNode("主群");
                if (group != null && commder != null && say != null
                    && main != null && use.IsNumber(group.FirstChild.InnerText) == true)
                {
                    group_save list = new group_save();
                    list.group_s = group.FirstChild.InnerText.ToLower();
                    long.TryParse(list.group_s, out list.group_l);
                    if (config_file.group_list.ContainsKey(list.group_l) == false)
                    {
                        list.commder = commder.InnerText == "开" ? true : false;
                        list.say = say.InnerText == "开" ? true : false;
                        bool temp = main.InnerText == "开" ? true : false;
                        list.main = temp;
                        if (temp == true && Minecraft_QQ.GroupSet_Main == 0)
                            Minecraft_QQ.GroupSet_Main = list.group_l;
                        config_file.group_list.Add(list.group_l, list);
                    }
                }
            }
        }
        public static void read_message()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Minecraft_QQ.path + config_file.message);
            XmlNodeList nodeList = xmldoc.SelectSingleNode("config").ChildNodes;
            config_file.message_list.Clear();
            foreach (XmlNode xn in nodeList)//遍历所有子节点
            {
                XmlNode check = xn.SelectSingleNode("检测");
                XmlNode msg = xn.SelectSingleNode("回复");
                if (check != null && msg != null)
                {
                    message_save message = new message_save();
                    message.check = check.InnerText;
                    if (config_file.message_list.ContainsKey(message.check) == false)
                    {
                        message.message = msg.InnerText;
                        config_file.message_list.Add(message.check, message);
                    }
                }
            }
        }
        public static void read_commder()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Minecraft_QQ.path + config_file.commder);
            XmlNodeList nodeList = xmldoc.SelectSingleNode("config").ChildNodes;
            config_file.commder_list.Clear();
            foreach (XmlNode xn in nodeList)//遍历所有子节点
            {
                XmlNode check = xn.SelectSingleNode("触发");
                XmlNode commder = xn.SelectSingleNode("命令");
                XmlNode player_use = xn.SelectSingleNode("玩家使用");
                XmlNode player_send = xn.SelectSingleNode("玩家发送");
                XmlNode parameter = xn.SelectSingleNode("附带参数");
                if (check != null && commder != null && player_use != null
                    && player_send != null && parameter != null)
                {
                    commder_save commder_save = new commder_save();
                    commder_save.check = check.InnerText;
                    if (config_file.commder_list.ContainsKey(commder_save.check) == false)
                    {
                        commder_save.commder = commder.InnerText;
                        commder_save.player_use = player_use.InnerText == "开" ? true : false;
                        commder_save.player_send = player_send.InnerText == "开" ? true : false;
                        commder_save.parameter = parameter.InnerText == "开" ? true : false;
                        config_file.commder_list.Add(commder_save.check, commder_save);
                    }
                }
            }
        }
    }
}

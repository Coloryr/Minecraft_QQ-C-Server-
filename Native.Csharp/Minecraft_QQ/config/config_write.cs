using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Color_yr.Minecraft_QQ
{
    class config_write
    {
        public static void write_config(string path)
        {
            if (File.Exists(path) == false)
                XML.CreateFile(path, 0);
            else
                XML.CreateFile(path, 1);
            try
            {
                ///导入XML文件
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(path);

                //根节点
                XmlElement root = xmldoc.DocumentElement;
                //子节点
                XmlElement Child;
                //保存的值
                XmlElement Text;

                Child = xmldoc.CreateElement("设置");
                root.AppendChild(Child);
                Text = xmldoc.CreateElement("自动应答");
                Text.InnerText = main_config.message_enable ? "开" : "关";
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("彩色代码");
                Text.InnerText = main_config.color_code ? "开" : "关";
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("维护模式");
                Text.InnerText = main_config.fix_mode ? "开" : "关";
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("同步对话");
                Text.InnerText = main_config.allways_send ? "开" : "关";
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("服务器昵称");
                Text.InnerText = main_config.nick_server ? "开" : "关";
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("群昵称");
                Text.InnerText = main_config.nick_group ? "开" : "关";
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("允许玩家绑定ID");
                Text.InnerText = main_config.set_name ? "开" : "关";
                Child.AppendChild(Text);

                Child = xmldoc.CreateElement("文本");
                root.AppendChild(Child);
                Text = xmldoc.CreateElement("发送至服务器的文本");
                Text.InnerText = message_config.send_text;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("维护时发送的文本");
                Text.InnerText = message_config.fix_send;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("未知指令");
                Text.InnerText = message_config.unknow;
                Child.AppendChild(Text);

                Child = xmldoc.CreateElement("检测");
                root.AppendChild(Child);
                Text = xmldoc.CreateElement("检测头");
                Text.InnerText = check_config.head;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("在线人数");
                Text.InnerText = check_config.online_players;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("状态检测");
                Text.InnerText = check_config.online_servers;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("玩家绑定ID");
                Text.InnerText = check_config.player_setid;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("玩家发送消息");
                Text.InnerText = check_config.send_message;
                Child.AppendChild(Text);

                Child = xmldoc.CreateElement("管理");
                root.AppendChild(Child);
                Text = xmldoc.CreateElement("禁言玩家");
                Text.InnerText = admin_config.mute;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("取消禁言");
                Text.InnerText = admin_config.unmute;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("查询ID");
                Text.InnerText = admin_config.check;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("重命名玩家");
                Text.InnerText = admin_config.rename;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("切换服务器维护模式");
                Text.InnerText = admin_config.fix;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("配置文件重读");
                Text.InnerText = admin_config.reload;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("打开菜单");
                Text.InnerText = admin_config.menu;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("昵称");
                Text.InnerText = admin_config.nick;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("禁止绑定列表");
                Text.InnerText = admin_config.unbind_list;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("禁言列表");
                Text.InnerText = admin_config.mute_list;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("发送给的人");
                Text.InnerText = admin_config.Admin_Send.ToString();
                Child.AppendChild(Text);

                Child = xmldoc.CreateElement("Socket");
                root.AppendChild(Child);
                Text = xmldoc.CreateElement("地址");
                Text.InnerText = socket_config.setip;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("端口");
                Text.InnerText = socket_config.Port.ToString();
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("编码类型");
                Text.InnerText = socket_config.code;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("数据包头");
                Text.InnerText = socket_config.data_Head;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("数据包尾");
                Text.InnerText = socket_config.data_End;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("绑定IP");
                Text.InnerText = socket_config.useip ? "开" : "关";
                Child.AppendChild(Text);

                xmldoc.Save(path);
            }
            catch (Exception)
            {
                if (MessageBox.Show("配置文件在写入时发发生了错误，是否要删除原来的配置文件再新生成一个？",
                    "配置文件错误", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    XML.CreateFile(path, 1);
                    write_config(path);
                }
            }
        }
        public static void write_group(string path, group_save obj)
        {
            if (File.Exists(path) == false)
                XML.CreateFile(path, 0);
            try
            {
                ///导入XML文件
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(path);
                if (XML.read(path, "QQ群", "群号") == obj.group_s)
                {
                    ///导入XML文件
                    XmlNodeList nodeList = xmldoc.SelectSingleNode("config").ChildNodes;
                    foreach (XmlNode xn in nodeList)//遍历所有子节点
                    {
                        //查找玩家
                        if (xn.Name == "QQ群")
                        {
                            XmlNode xnLurl = xn.SelectSingleNode("群号");
                            if (xnLurl.InnerText == obj.group_s)
                            {
                                xnLurl = xn.SelectSingleNode("命令");
                                xnLurl.InnerText = obj.commder ? "开" : "关";
                                xnLurl = xn.SelectSingleNode("对话");
                                xnLurl.InnerText = obj.say ? "开" : "关";
                                xnLurl = xn.SelectSingleNode("主群");
                                xnLurl.InnerText = obj.main ? "开" : "关";
                                xmldoc.Save(path);
                                return;
                            }
                        }
                    }
                }
                //根节点
                XmlElement root = xmldoc.DocumentElement;
                //子节点
                XmlElement Child;
                //保存的值
                XmlElement Text;
                Child = xmldoc.CreateElement("QQ群");
                root.AppendChild(Child);

                Text = xmldoc.CreateElement("群号");
                Text.InnerText = obj.group_s;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("命令");
                Text.InnerText = obj.commder ? "开" : "关";
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("对话");
                Text.InnerText = obj.say ? "开" : "关";
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("主群");
                Text.InnerText = obj.main ? "开" : "关";
                Child.AppendChild(Text);

                xmldoc.Save(path);
            }
            catch (Exception)
            {
                if (MessageBox.Show("配置文件在写入时发发生了错误，是否要删除原来的配置文件再新生成一个？",
                    "配置文件错误", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    XML.CreateFile(path, 1);
                    write_group(path, obj);
                }
            }
        }
        public static void write_player(string path, player_save obj)
        {
            if (File.Exists(path) == false)
                XML.CreateFile(path, 0);
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(path);
                if (XML.read(path, "玩家", "QQ号") == obj.qq.ToString())
                {
                    ///导入XML文件
                    XmlNodeList nodeList = xmldoc.SelectSingleNode("config").ChildNodes;
                    foreach (XmlNode xn in nodeList)//遍历所有子节点
                    {
                        //查找玩家
                        if (xn.Name == "玩家")
                        {
                            XmlNode xnLurl = xn.SelectSingleNode("QQ号");
                            if (xnLurl.InnerText == obj.qq.ToString())
                            {
                                xnLurl = xn.SelectSingleNode("绑定ID");
                                xnLurl.InnerText = obj.player;
                                xnLurl = xn.SelectSingleNode("昵称");
                                xnLurl.InnerText = obj.nick;
                                xnLurl = xn.SelectSingleNode("管理员");
                                xnLurl.InnerText = obj.admin ? "开" : "关";
                                xmldoc.Save(path);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    //根节点
                    XmlElement root = xmldoc.DocumentElement;
                    //子节点
                    XmlElement Child;
                    //保存的值
                    XmlElement Text;
                    Child = xmldoc.CreateElement("玩家");
                    root.AppendChild(Child);

                    Text = xmldoc.CreateElement("绑定ID");
                    Text.InnerText = obj.player;
                    Child.AppendChild(Text);
                    Text = xmldoc.CreateElement("昵称");
                    Text.InnerText = obj.nick;
                    Child.AppendChild(Text);
                    Text = xmldoc.CreateElement("QQ号");
                    Text.InnerText = obj.qq.ToString();
                    Child.AppendChild(Text);
                    Child.AppendChild(Text);
                    Text = xmldoc.CreateElement("管理员");
                    Text.InnerText = obj.admin ? "开" : "关";
                    Child.AppendChild(Text);

                    xmldoc.Save(path);
                }
            }
            catch (Exception)
            {
                if (MessageBox.Show("配置文件在写入时发发生了错误，是否要删除原来的配置文件再新生成一个？",
                    "配置文件错误", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    XML.CreateFile(path, 1);
                    write_player(path, obj);
                }
            }
        }
        public static void write_cant_bind(string path, string id)
        {
            if (File.Exists(path) == false)
                XML.CreateFile(path, 0);
            try
            {
                ///导入XML文件
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(path);

                //根节点
                XmlElement root = xmldoc.DocumentElement;
                //子节点
                XmlElement Child;
                //保存的值
                XmlElement Text;
                Child = xmldoc.CreateElement("禁止绑定");
                root.AppendChild(Child);

                Text = xmldoc.CreateElement("ID");
                Text.InnerText = id;
                Child.AppendChild(Text);

                xmldoc.Save(path);
            }
            catch (Exception)
            {
                if (MessageBox.Show("配置文件在写入时发发生了错误，是否要删除原来的配置文件再新生成一个？",
                    "配置文件错误", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    XML.CreateFile(path, 1);
                    write_cant_bind(path, id);
                }
            }
        }
        public static void write_mute(string path, string id)
        {
            if (File.Exists(path) == false)
                XML.CreateFile(path, 0);
            try
            {
                ///导入XML文件
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(path);

                //根节点
                XmlElement root = xmldoc.DocumentElement;
                //子节点
                XmlElement Child;
                //保存的值
                XmlElement Text;
                Child = xmldoc.CreateElement("禁言");
                root.AppendChild(Child);

                Text = xmldoc.CreateElement("ID");
                Text.InnerText = id;
                Child.AppendChild(Text);

                xmldoc.Save(path);
            }
            catch (Exception)
            {
                if (MessageBox.Show("配置文件在写入时发发生了错误，是否要删除原来的配置文件再新生成一个？",
                    "配置文件错误", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    XML.CreateFile(path, 1);
                    write_mute(path, id);
                }
            }
        }
        public static void write_unmute(string path, string id)
        {
            if (File.Exists(path) == false)
                XML.CreateFile(path, 0);
            try
            {
                if (config_file.cant_bind.Contains(id) == true)
                    return;
                ///导入XML文件
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(path);
                XmlNodeList nodeList = xmldoc.SelectSingleNode("config").ChildNodes;
                foreach (XmlNode xn in nodeList)//遍历所有子节点
                {
                    //查找玩家
                    if (xn.Name == "禁言")
                    {
                        XmlNodeList nodeList1 = xn.ChildNodes;
                        foreach (XmlNode xn1 in nodeList1)//遍历所有子节点
                        {
                            if (xn1.InnerText == id)
                            {
                                xn.RemoveChild(xn1);
                                xmldoc.Save(path);
                                return;
                            }
                        }
                    }
                }  
            }
            catch (Exception)
            {

            }
        }
        public static void write_commder(string path, commder_save obj)
        {
            if (File.Exists(path) == false)
                XML.CreateFile(path, 0);
            try
            {
                ///导入XML文件
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(path);

                //根节点
                XmlElement root = xmldoc.DocumentElement;
                //子节点
                XmlElement Child;
                //保存的值
                XmlElement Text;
                Child = xmldoc.CreateElement("命令");
                root.AppendChild(Child);

                Text = xmldoc.CreateElement("触发");
                Text.InnerText = obj.check;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("命令");
                Text.InnerText = obj.commder;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("玩家使用");
                Text.InnerText = obj.player_use ? "开" : "关";
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("玩家发送");
                Text.InnerText = obj.player_send ? "开" : "关";
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("附带参数");
                Text.InnerText = obj.parameter ? "开" : "关";
                Child.AppendChild(Text);

                xmldoc.Save(path);
            }
            catch (Exception)
            {
                if (MessageBox.Show("配置文件在写入时发发生了错误，是否要删除原来的配置文件再新生成一个？",
                    "配置文件错误", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    XML.CreateFile(path, 1);
                    write_commder(path, obj);
                }
            }
        }
        public static void write_message(string path, message_save obj)
        {
            if (File.Exists(path) == false)
                XML.CreateFile(path, 0);
            try
            {
                ///导入XML文件
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(path);

                //根节点
                XmlElement root = xmldoc.DocumentElement;
                //子节点
                XmlElement Child;
                //保存的值
                XmlElement Text;
                Child = xmldoc.CreateElement("自动回复消息");
                root.AppendChild(Child);

                Text = xmldoc.CreateElement("检测");
                Text.InnerText = obj.check;
                Child.AppendChild(Text);
                Text = xmldoc.CreateElement("回复");
                Text.InnerText = obj.message;
                Child.AppendChild(Text);

                xmldoc.Save(path);
            }
            catch (Exception)
            {
                if (MessageBox.Show("配置文件在写入时发发生了错误，是否要删除原来的配置文件再新生成一个？",
                    "配置文件错误", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    XML.CreateFile(path, 1);
                    write_message(path, obj);
                }
            }
        }
    }
}

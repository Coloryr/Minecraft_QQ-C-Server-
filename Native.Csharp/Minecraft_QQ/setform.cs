using System;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    public partial class setform : Form
    {
        public setform()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 退出按钮事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox5.Text) == true
                || (checkBox6.Checked == true && string.IsNullOrWhiteSpace(textBox4.Text) == true))
            {
                MessageBox.Show("请输入所需的参数");
                return;
            }
            if (use.IsNumber(textBox5.Text) == false)
            {
                MessageBox.Show("请输入正确的参数");
                return;
            }
            int.TryParse(textBox5.Text, out int port);
            socket_config.Port = port;
            socket_config.setip = textBox4.Text;
        }
        private void FormSettings_Load(object sender, EventArgs e)
        {
            label2.Text = "插件版本：" + Minecraft_QQ.vision;
            textBox5.Text = socket_config.Port.ToString();
            textBox4.Text = socket_config.setip;
            if (socket_config.code == "UTF-8")
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            else
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
            }
            checkBox2.Checked = main_config.message_enable;
            checkBox4.Checked = main_config.allways_send;
            if (main_config.fix_mode == false)
            {
                checkBox1.Checked = false;
                checkBox1.Text = "服务器维护模式：关";
            }
            else if (main_config.fix_mode == true)
            {
                checkBox1.Checked = true;
                checkBox1.Text = "服务器维护模式：开";
            }
            if (socket_config.useip == true)
            {
                checkBox6.Checked = true;
                textBox4.ReadOnly = false;
            }
            else
            {
                checkBox6.Checked = false;
                textBox4.ReadOnly = true;
            }
            checkBox8.Checked = main_config.color_code;
            checkBox9.Checked = main_config.nick_server;
            checkBox10.Checked = main_config.nick_group;
            checkBox11.Checked = main_config.set_name;
            checkBox12.Checked = main_config.bq_message;

            mysql_ip.Text = mysql_config.ip;
            mysql_port.Text = mysql_config.Port.ToString();
            mysql_user.Text = mysql_config.user;
            mysql_password.Text = mysql_config.password;
            mysql_use.Checked = mysql_config.use;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (use.IsNumber(textBox1.Text) == false)
            {
                button1.Text = "错误";
                return;
            }
            group_save list = new group_save();
            list.group_s = textBox1.Text;
            list.commder = checkBox3.Checked;
            list.say = checkBox5.Checked;
            list.main = checkBox7.Checked;
            long.TryParse(textBox1.Text, out list.group_l);
            config_write.write_group(Minecraft_QQ.path + config_file.group, list);
            if (config_file.group_list.ContainsKey(list.group_l) == false)
                config_file.group_list.Add(list.group_l, list);
            button1.Text = "已添加";
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (use.IsNumber(textBox5.Text) == false)
            {
                button4.Text = "错误";
                return;
            }
            int.TryParse(textBox5.Text, out int a);
            socket_config.Port = a;
            socket_config.setip = textBox4.Text;
            config_write.write_config(Minecraft_QQ.path + config_file.config);
            button4.Text = "已设置";
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (use.IsNumber(textBox6.Text) == false || textBox6.Text == null)
            {
                button5.Text = "错误";
                return;
            }
            long.TryParse(textBox6.Text, out long qq);
            if (config_file.player_list.ContainsKey(qq) == false)
            {
                player_save player = new player_save();
                player.qq = qq;
                player.admin = true;
                config_file.player_list.Add(qq, player);
                if (config_file.Mysql_ok == true)
                {
                    new Mysql_Add_data().player(player);
                }
                else
                {
                    config_write.write_player(Minecraft_QQ.path + config_file.player, player);
                }
            }
            else
            {
                config_file.player_list[qq].admin = true;
                if (config_file.Mysql_ok == true)
                {
                    new Mysql_Add_data().player(config_file.player_list[qq]);
                }
                else
                {
                    config_write.write_player(Minecraft_QQ.path + config_file.player, config_file.player_list[qq]);
                }
            }
            button5.Text = "添加成功";
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (use.IsNumber(textBox7.Text) == false || textBox7.Text == null)
            {
                button6.Text = "设置失败";
                return;
            }
            long.TryParse(textBox7.Text, out long a);
            admin_config.Admin_Send = a;
            config_write.write_config(Minecraft_QQ.path + config_file.config);
            button6.Text = "已设置";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox8.Text != null)
            {
                if (config_file.Mysql_ok == true)
                {
                    new Mysql_Add_data().notid(textBox8.Text.ToLower());
                }
                else
                {
                    config_write.write_cant_bind(Minecraft_QQ.path + config_file.player, textBox8.Text.ToLower());
                }
                button7.Text = "已添加";
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            socket_config.code = "UTF-8";
            config_write.write_config(Minecraft_QQ.path + config_file.config);
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            socket_config.code = "ANSI";
            config_write.write_config(Minecraft_QQ.path + config_file.config);
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            main_config.fix_mode = checkBox1.Checked;
            checkBox1.Text = checkBox1.Checked ? "服务器维护模式：开" : "服务器维护模式：关";
            config_write.write_config(Minecraft_QQ.path + config_file.config);
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            main_config.message_enable = checkBox2.Checked;
            config_write.write_config(Minecraft_QQ.path + config_file.config);
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            main_config.allways_send = checkBox4.Checked;
            config_write.write_config(Minecraft_QQ.path + config_file.config);
        }
        private void CheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            socket_config.useip = checkBox6.Checked;
            checkBox6.Checked = socket_config.useip;
            textBox4.ReadOnly = !socket_config.useip;
            config_write.write_config(Minecraft_QQ.path + config_file.config);
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            main_config.color_code = checkBox8.Checked;
            config_write.write_config(Minecraft_QQ.path + config_file.config);
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            main_config.nick_server = checkBox9.Checked;
            config_write.write_config(Minecraft_QQ.path + config_file.config);
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            main_config.nick_group = checkBox10.Checked;
            config_write.write_config(Minecraft_QQ.path + config_file.config);
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            main_config.set_name = checkBox11.Checked;
            config_write.write_config(Minecraft_QQ.path + config_file.config);
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            main_config.bq_message = checkBox12.Checked;
            config_write.write_config(Minecraft_QQ.path + config_file.config);
        }

        private void mysql_b_Click(object sender, EventArgs e)
        {
            mysql_config.ip = mysql_ip.Text;
            int.TryParse(mysql_port.Text, out int a);
            mysql_config.Port = a;
            mysql_config.user = mysql_user.Text;
            mysql_config.password = mysql_password.Text;
            config_write.write_config(Minecraft_QQ.path + config_file.config);
            if (Mysql.mysql_start() == false)
            {
                MessageBox.Show("Mysql链接错误，请检查设置");
                mysql_b.Text = "已断开";
                mysql_use.Checked = false;
                config_file.Mysql_ok = false;
            }
            else
            {
                mysql_b.Text = "已连接";
                mysql_use.Checked = true;
                config_file.Mysql_ok = true;
            }
        }

        private void mysql_use_CheckedChanged(object sender, EventArgs e)
        {
            mysql_config.use = mysql_use.Checked;
            if (mysql_use.Checked == false)
            {
                config_file.Mysql_ok = false;
            }
            else
            {
                if (config_file.Mysql_ok == false)
                {
                    mysql_b_Click(null, null);
                }
            }
        }
    }
}

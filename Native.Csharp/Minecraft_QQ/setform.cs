using System;
using System.IO;
using System.Text;
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
            if (textBox5.Text == "" || textBox1.Text == "" || textBox5.Text == null || textBox1.Text == null
                || (checkBox6.Checked == true && textBox4.Text == null))
            {
                MessageBox.Show("请输入所需的参数");
                return;
            }
            if (use.IsNumber(textBox5.Text) == false || use.IsNumber(textBox6.Text) == false || use.IsNumber(textBox7.Text) == false)
            {
                MessageBox.Show("请输入正确的参数");
                return;
            }

            XML.write(config_read.config, "Socket", "端口", textBox5.Text,true);
            socket.Port = int.Parse(textBox5.Text);
            socket.setip = textBox4.Text;
            Close();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            string temp;
            textBox5.Text = XML.read(config_read.config, "Socket", "端口");
            textBox4.Text = XML.read(config_read.config, "Socket", "地址");

            temp = XML.read(config_read.config, "Mysql", "地址");
            if (temp != null)
                textBox9.Text = temp;
            temp = XML.read(config_read.config, "Mysql", "端口");
            if (temp != null)
                textBox10.Text = temp;
            temp = XML.read(config_read.config, "Mysql", "账户");
            if (temp != null)
                textBox11.Text = temp;
            temp = XML.read(config_read.config, "Mysql", "密码");
            if (temp != null)
                textBox12.Text = temp;

            if (config_read.ANSI == "UTF-8")
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            else if (config_read.ANSI == "ANSI（GBK）")
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
            }
            if (config_read.allways_send == true)
                checkBox4.Checked = true;
            else
                checkBox4.Checked = false;
            if (config_read.fix_mode == false)
            {
                checkBox1.Checked = false;
                checkBox1.Text = "服务器维护模式：关";
            }
            else if (config_read.fix_mode == true)
            {
                checkBox1.Checked = true;
                checkBox1.Text = "服务器维护模式：开";
            }
            if (config_read.Mysql_mode == true)
                checkBox5.Checked = true;
            else
                checkBox5.Checked = false;
            if (socket.useip == true)
            {
                checkBox6.Checked = true;
                textBox4.ReadOnly = false;
            }
            else
            {
                checkBox6.Checked = false;
                textBox4.ReadOnly = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (use.IsNumber(textBox1.Text) == false)
            {
                button1.Text = "错误";
                return;
            }
            grouplist list = new grouplist();
            list.group = textBox1.Text;
            list.commder = checkedListBox1.GetItemChecked(0);
            list.say = checkedListBox1.GetItemChecked(1);
            list.main = checkedListBox1.GetItemChecked(2);
            XML.write_object(config_read.group, "QQ群设置", list);
            button1.Text = "已添加";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (use.IsNumber(textBox5.Text) == false)
            {
                button4.Text = "错误";
                return;
            }
            XML.write(config_read.config, "Socket", "端口", textBox5.Text, true);
            XML.write(config_read.config, "Socket", "地址", textBox4.Text, true);
            socket.setip = textBox4.Text;
            int.TryParse(textBox5.Text, out socket.Port);
            button4.Text = "已设置";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (use.IsNumber(textBox6.Text) == false)
            {
                button5.Text = "错误";
                return;
            }
            if (textBox6.Text != null)
            {
                XML.write(config_read.player,"QQ" + textBox6.Text, "管理员", "是", true);
                StreamReader sr = new StreamReader(config_read.path + config_read.player, Encoding.Default);
                config_read.player_m = sr.ReadToEnd().TrimStart();
                sr.Close();
                button5.Text = "添加成功";
            }          
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (use.IsNumber(textBox7.Text) == false)
            {
                button6.Text = "设置失败";
                return;
            }
            if (textBox7.Text != null)
            {
                XML.write(config_read.player, "管理员", "发送给的人", textBox7.Text, false);
                StreamReader sr = new StreamReader(config_read.path + config_read.player, Encoding.Default);
                config_read.player_m = sr.ReadToEnd().TrimStart();
                sr.Close();
                long a;
                long.TryParse(textBox7.Text,out a);
                if (a != 0)
                    config_read.Admin_Send = a;
                button6.Text = "已设置";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox8.Text != null)
            {
                if (config_read.Mysql_mode == true)
                    Mysql_user.mysql_add(Mysql_user.Mysql_notid, textBox8.Text.ToLower(), "notid");
                else
                    XML.write(config_read.player, "ID" + textBox8.Text.ToLower(), "禁止绑定", "是", true);
                button7.Text = "已添加";
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (use.IsNumber(textBox10.Text) == false)
            {
                button8.Text = "错误";
                return;
            }
            XML.write(config_read.config, "Mysql", "地址", textBox9.Text, true);
            Mysql_user.Mysql_IP = textBox9.Text;
            XML.write(config_read.config, "Mysql", "端口", textBox10.Text, true);
            Mysql_user.Mysql_Port = textBox10.Text;
            XML.write(config_read.config, "Mysql", "账户", textBox11.Text, true);
            Mysql_user.Mysql_User = textBox11.Text;
            XML.write(config_read.config, "Mysql", "密码", textBox12.Text, true);
            Mysql_user.Mysql_Password = textBox12.Text;
            if (Mysql_user.mysql_start() == true)
            {
                button8.Text = "Mysql已设置";
                config_read.Mysql_mode = true;
                checkBox5.Checked = true;
                XML.write(config_read.config, "Mysql", "启用", "开", true);
                config_read.Mysql_mode = true;
            }
            else
            {
                button8.Text = "Mysql错误";
                config_read.Mysql_mode = false;
                checkBox5.Checked = false;
                XML.write(config_read.config, "Mysql", "启用", "关", true);
                config_read.Mysql_mode = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            XML.write(config_read.config, "Socket", "编码", "UTF-8", true);
            config_read.ANSI = "UTF-8";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            XML.write(config_read.config, "Socket", "编码", "ANSI（GBK）", true);
            config_read.ANSI = "ANSI（GBK）";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                XML.write(config_read.config, "核心设置", "维护模式", "开", true);
                config_read.fix_mode = true;
                checkBox1.Text = "服务器维护模式：开";            
            }
            else
            {
                XML.write(config_read.config, "核心设置", "维护模式", "关", true);
                config_read.fix_mode = false;
                checkBox1.Text = "服务器维护模式：关";
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                XML.write(config_read.config, "消息模式", "始终发送消息", "当然！", true);
                config_read.allways_send = true;
            }
            else
            {
                XML.write(config_read.config, "消息模式", "始终发送消息", "不！", true);
                config_read.allways_send = false;
            }
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                XML.write(config_read.config, "Mysql", "启用", "开", true);
                config_read.Mysql_mode = true;
                config_read.Mysql_mode = true;
            }
            if (checkBox5.Checked == false)
            {
                XML.write(config_read.config, "Mysql", "启用", "关", true);
                config_read.Mysql_mode = false;
                config_read.Mysql_mode = false;
            }
        }
        private void CheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked == true)
            {
                XML.write(config_read.config, "Socket", "绑定IP", "开", true);
                socket.useip = true;
                checkBox6.Checked = true;
                textBox4.ReadOnly = false;
            }
            if (checkBox6.Checked == false)
            {
                XML.write(config_read.config, "Socket", "绑定IP", "关", true);
                socket.useip = false;
                checkBox6.Checked = false;
                textBox4.ReadOnly = true;
            }
        }
        private void TextBox_KeyPress(object sender, KeyEventArgs e)
        {
            if (use.key_ok(e) == false)
                e.Handled = true;
        }

    }
}

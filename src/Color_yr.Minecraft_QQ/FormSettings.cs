using System;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    public partial class FormSettings : Form
    {
        public FormSettings()
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
            int tmp;
            XML xml = new XML();
            xml.write(config_read.config, "群号1", textBox1.Text);
            Minecraft_QQ.GroupSet1 = long.Parse(textBox1.Text);
            xml.write(config_read.config, "群号2", textBox2.Text);
            Minecraft_QQ.GroupSet2 = long.Parse(textBox2.Text);
            xml.write(config_read.config, "IP", textBox4.Text);
            xml.write(config_read.config, "Port", textBox5.Text);
            xml.write(config_read.config, "群号3", textBox3.Text);
            Minecraft_QQ.GroupSet3 = long.Parse(textBox3.Text);
            if (!int.TryParse(textBox5.Text, out tmp))
            {
                MessageBox.Show("请输入端口数字");
                return;
            }
            Minecraft_QQ.Port = int.Parse(textBox5.Text);
            Minecraft_QQ.ipaddress = textBox4.Text;
            Close();
        }

        /// <summary>
        /// 保存按钮事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            //参数保存处理代码。
            this.btnExit_Click(null, null);
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            textBox1.Text = config_read.group1;
            textBox2.Text = config_read.group2;
            textBox3.Text = config_read.group3;

            textBox4.Text = XML.read(config_read.config, "IP");
            textBox5.Text = XML.read(config_read.config, "Port");

            textBox7.Text = XML.read(config_read.admin, "发送给的人");

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
            if (config_read.group2_mode == true)
                checkBox2.Checked = true;
            else
                checkBox2.Checked = false;
            if (config_read.group3_mode == true)
                checkBox3.Checked = true;
            else
                checkBox3.Checked = false;
            if (Minecraft_QQ.Mysql_mode == true)
                checkBox5.Checked = true;
            else
                checkBox5.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XML xml = new XML();
            xml.write(config_read.config, "群号1", textBox1.Text);
            config_read.group1 = textBox1.Text;
            long.TryParse(textBox1.Text, out Minecraft_QQ.GroupSet1);
            button1.Text = "设置成功";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            XML xml = new XML();
            xml.write(config_read.config, "群号2", textBox2.Text);
            config_read.group2 = textBox2.Text;
            long.TryParse(textBox2.Text, out Minecraft_QQ.GroupSet2);
            button2.Text = "设置成功";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            XML xml = new XML();
            xml.write(config_read.config, "群号3", textBox3.Text);
            config_read.group3 = textBox3.Text;
            long.TryParse(textBox3.Text,out Minecraft_QQ.GroupSet3);
            button3.Text = "设置成功";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            XML xml = new XML();
            xml.write(config_read.config, "IP", textBox4.Text);
            config_read.IP = textBox4.Text;
            xml.write(config_read.config, "Port", textBox5.Text);
            config_read.Port = textBox5.Text;
            int.TryParse(textBox5.Text, out Minecraft_QQ.Port);
            Minecraft_QQ.ipaddress = textBox4.Text;
            button4.Text = "已设置";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox6.Text != null)
            {
                XML xml = new XML();
                xml.write(config_read.admin, textBox6.Text, "admin");
                button5.Text = "添加成功";
            }          
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox7.Text != null)
            {
                XML xml = new XML();
                xml.write(config_read.admin, "发送给的人", textBox7.Text);
                button6.Text = "已设置";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox8.Text != null)
            {
                if (Minecraft_QQ.Mysql_mode == true)
                {
                    Mysql_user sql = new Mysql_user();
                    sql.mysql_add(Mysql_user.Mysql_notid, textBox8.Text.ToLower(), "notid");
                }
                else
                {
                    XML xml = new XML();
                    xml.write(config_read.notid, textBox8.Text.ToLower(), "notid");
                }
                button7.Text = "已添加";
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            XML xml = new XML();
            xml.write(config_read.config, "Mysql地址", textBox9.Text);
            config_read.Mysql_IP = textBox9.Text;
            xml.write(config_read.config, "Mysql端口", textBox10.Text);
            config_read.Mysql_Port = textBox10.Text;
            xml.write(config_read.config, "Mysql账户", textBox11.Text);
            config_read.Mysql_User = textBox11.Text;
            xml.write(config_read.config, "Mysql密码", textBox12.Text);
            config_read.Mysql_Password = textBox12.Text;
            Mysql_user sql = new Mysql_user();
            if (sql.mysql_start() == true)
            {
                button8.Text = "Mysql已设置";
                Minecraft_QQ.Mysql_mode = true;
                checkBox5.Checked = true;
                xml.write(config_read.config, "Mysql启用", "开");
                config_read.Mysql_mode = true;
            }
            else
            {
                button8.Text = "Mysql错误";
                Minecraft_QQ.Mysql_mode = false;
                checkBox5.Checked = false;
                xml.write(config_read.config, "Mysql启用", "关");
                config_read.Mysql_mode = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            XML xml = new XML();
            xml.write(config_read.config, "编码", "UTF-8");
            config_read.ANSI = "UTF-8";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            XML xml = new XML();
            xml.write(config_read.config, "编码", "ANSI（GBK）");
            config_read.ANSI = "ANSI（GBK）";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            XML xml = new XML();
            if (checkBox1.Checked == true)
            {
                xml.write(config_read.config, "维护模式", "开");
                Minecraft_QQ.server = true;
                checkBox1.Text = "服务器维护模式：开";
                config_read.fix_mode = true;
            }
            else
            {
                xml.write(config_read.config, "维护模式", "关");
                Minecraft_QQ.server = false;
                checkBox1.Text = "服务器维护模式：关";
                config_read.fix_mode = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            XML xml = new XML();
            if (checkBox2.Checked == true)
            {
                xml.write(config_read.config, "群2发送消息", "开");
                Minecraft_QQ.Group2_on = true;
                config_read.group2_mode = true;
            }
            else
            {
                xml.write(config_read.config, "群2发送消息", "关");
                Minecraft_QQ.Group2_on = false;
                config_read.group2_mode = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            XML xml = new XML();
            if (checkBox3.Checked == true)
            {
                xml.write(config_read.config, "群3发送消息", "开");
                Minecraft_QQ.Group3_on = true;
                config_read.group3_mode = true;
            }
            else
            {
                xml.write(config_read.config, "群3发送消息", "关");
                Minecraft_QQ.Group3_on = false;
                config_read.group3_mode = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            XML xml = new XML();
            if (checkBox4.Checked == true)
            {            
                xml.write(config_read.config, "发送消息", "当然！");
                config_read.allways_send = true;
            }
            else
            {
                xml.write(config_read.config, "发送消息", "不！");
                config_read.allways_send = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            XML xml = new XML();
            if (checkBox5.Checked == true)
            {
                xml.write(config_read.config, "Mysql启用", "开");
                Minecraft_QQ.Mysql_mode = true;
                config_read.Mysql_mode = true;
            }
            if (checkBox5.Checked == false)
            {
                xml.write(config_read.config, "Mysql启用", "关");
                Minecraft_QQ.Mysql_mode = false;
                config_read.Mysql_mode = false;
            }
        }
        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (use.isok(e) == false)
                e.Handled = true;
        }
        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (use.isok(e) == false)
                e.Handled = true;
        }
        private void TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (use.isok(e) == false)
                e.Handled = true;
        }
        private void TextBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (use.isok(e) == false)
                e.Handled = true;
        }
        private void TextBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (use.isok(e) == false)
                e.Handled = true;
        }
        private void TextBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (use.isok(e) == false)
                e.Handled = true;
        }
        private void TextBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (use.isok(e) == false)
                e.Handled = true;
        }
    }
}

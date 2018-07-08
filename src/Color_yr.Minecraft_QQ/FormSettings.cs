using System;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
            //加载标题。
            Text = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name + "参数设置";
        }

        /// <summary>
        /// 退出按钮事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            int tmp;
            XML.write(config_read.config, "群号1", textBox1.Text);
            Minecraft_QQ.GroupSet1 = long.Parse(textBox1.Text);
            XML.write(config_read.config, "群号2", textBox3.Text);
            Minecraft_QQ.GroupSet2 = long.Parse(textBox3.Text);
            XML.write(config_read.config, "IP", textBox4.Text);
            XML.write(config_read.config, "Port", textBox5.Text);
            XML.write(config_read.config, "群号3", textBox6.Text);
            Minecraft_QQ.GroupSet3 = long.Parse(textBox6.Text);
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

        private void button1_Click(object sender, EventArgs e)
        {
            XML.write(config_read.config, "群号1", textBox1.Text);
            Minecraft_QQ.GroupSet1 = long.Parse(textBox1.Text);
            button1.Text = "设置成功";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            XML.write(config_read.admin, textBox2.Text, "admin");
            button2.Text = "添加成功";
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            textBox1.Text = XML.read(config_read.config, "群号1");
            textBox3.Text = XML.read(config_read.config, "群号2");
            textBox6.Text = XML.read(config_read.config, "群号3");

            textBox4.Text = XML.read(config_read.config, "IP");
            textBox5.Text = XML.read(config_read.config, "Port");

            textBox11.Text = XML.read(config_read.admin, "发送给的人");

            textBox7.Text = XML.read(config_read.config, "Mysql地址");
            textBox8.Text = XML.read(config_read.config, "Mysql端口");
            textBox9.Text = XML.read(config_read.config, "Mysql账户");
            textBox10.Text = XML.read(config_read.config, "Mysql密码");

            if(Minecraft_QQ.Mysql_mode==true)
            {
                checkBox3.Checked = true;
            }

            if (XML.read(config_read.config, "编码") == "UTF-8")
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            else if (XML.read(config_read.config, "编码") == "ANSI（GBK）")
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
            }
            if (XML.read(config_read.config, "发送消息") == "当然！")
            {
                checkBox1.Checked = true;
            }
            else if (XML.read(config_read.config, "发送消息") == "不！")
            {
                checkBox1.Checked = false;
            }
            if (XML.read(config_read.config, "维护模式") == "关")
            {
                checkBox2.Checked = false;
                checkBox2.Text = "服务器维护模式：关";
            }
            else if (XML.read(config_read.config, "维护模式") == "开")
            {
                checkBox2.Checked = true;
                checkBox2.Text = "服务器维护模式：开";
            }
            if (XML.read(config_read.config, "群2发送消息") == "开")
                checkBox4.Checked = true;
            else
                checkBox4.Checked = false;
            if (XML.read(config_read.config, "群3发送消息") == "开")
                checkBox5.Checked = true;
            else
                checkBox5.Checked = false;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button_Click(object sender, EventArgs e)
        {
            int tmp;
            XML.write(config_read.config, "IP", textBox4.Text);
            XML.write(config_read.config, "Port", textBox5.Text);
            if (!int.TryParse(textBox5.Text, out tmp))
            {
                MessageBox.Show("请正确输入数字");
                return;
            }
            Minecraft_QQ.Port = int.Parse(textBox5.Text);
            Minecraft_QQ.ipaddress = textBox4.Text;
            button.Text = "已设置";
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Text = "正在关闭";
            //socket.stop_socket();
            button3.Text = "已关闭";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            XML.write(config_read.config, "群号2", textBox3.Text);
            Minecraft_QQ.GroupSet2 = long.Parse(textBox3.Text);
            button4.Text = "设置成功";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            XML.write(config_read.config, "群号3", textBox6.Text);
            Minecraft_QQ.GroupSet3 = long.Parse(textBox6.Text);
            button5.Text = "设置成功";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            XML.write(config_read.config, "编码", "UTF-8");
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            XML.write(config_read.config, "编码", "ANSI（GBK）");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked==true) XML.write(config_read.config, "发送消息", "当然！");
            else XML.write(config_read.config, "发送消息", "不！");
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                XML.write(config_read.config, "维护模式", "开");
                Minecraft_QQ.server = true;
                checkBox2.Text = "服务器维护模式：开";
            }
            else
            {
                XML.write(config_read.config, "维护模式", "关");
                Minecraft_QQ.server = false;
                checkBox2.Text = "服务器维护模式：关";
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                XML.write(config_read.config, "群2发送消息", "开");
                Minecraft_QQ.Group2_on = true;
            }
            else
            {
                XML.write(config_read.config, "群2发送消息", "关");
                Minecraft_QQ.Group2_on = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                XML.write(config_read.config, "群3发送消息", "开");
                Minecraft_QQ.Group3_on = true;
            }
            else
            {
                XML.write(config_read.config, "群3发送消息", "关");
                Minecraft_QQ.Group3_on = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            XML.write(config_read.config, "Mysql地址", textBox7.Text);
            XML.write(config_read.config, "Mysql端口", textBox8.Text);
            XML.write(config_read.config, "Mysql账户", textBox9.Text);
            XML.write(config_read.config, "Mysql密码", textBox10.Text);
            if (Mysql.mysql_start() == true)
            {
                button6.Text = "Mysql已设置";
                Minecraft_QQ.Mysql_mode = true;
                checkBox3.Checked = true;
                XML.write(config_read.config, "Mysql启用", "开");
            }
            else
            {
                button6.Text = "Mysql错误";
                Minecraft_QQ.Mysql_mode = false;
                checkBox3.Checked = false;
                XML.write(config_read.config, "Mysql启用", "关");
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                XML.write(config_read.config, "Mysql启用", "开");
                Minecraft_QQ.Mysql_mode = true;
            }
            if (checkBox3.Checked == false)
            {
                XML.write(config_read.config, "Mysql启用", "关");
                Minecraft_QQ.Mysql_mode = false;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            XML.write(config_read.admin, "发送给的人", textBox11.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox12.Text != null)
            {
                if (Minecraft_QQ.Mysql_mode == true)
                {
                    Mysql.mysql_add(Mysql.Mysql_notid, textBox12.Text.ToLower(), "notid");
                }
                else
                {
                    XML.write(config_read.notid, textBox12.Text.ToLower(), "notid");
                }
                MessageBox.Show("已添加：" + textBox12.Text);
            }
        }
    }
}

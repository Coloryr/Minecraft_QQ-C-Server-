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
            if (Utils.IsNumber(textBox5.Text) == false)
            {
                MessageBox.Show("请输入正确的参数");
                return;
            }
            int.TryParse(textBox5.Text, out int port);
            Minecraft_QQ.Mainconfig.链接.端口 = port;
            Minecraft_QQ.Mainconfig.链接.地址 = textBox4.Text;
            new Config_write().Write_config();
        }
        private void FormSettings_Load(object sender, EventArgs e)
        {
            label2.Text = "插件版本：" + Minecraft_QQ.vision;
            textBox5.Text = Minecraft_QQ.Mainconfig.链接.端口.ToString();
            textBox4.Text = Minecraft_QQ.Mainconfig.链接.地址;
            if (Minecraft_QQ.Mainconfig.链接.编码 == "UTF-8")
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            else
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
            }
            checkBox2.Checked = Minecraft_QQ.Mainconfig.设置.自动应答开关;
            checkBox4.Checked = Minecraft_QQ.Mainconfig.设置.始终发送消息;
            if (Minecraft_QQ.Mainconfig.设置.维护模式 == false)
            {
                checkBox1.Checked = false;
                checkBox1.Text = "服务器维护模式：关";
            }
            else if (Minecraft_QQ.Mainconfig.设置.维护模式 == true)
            {
                checkBox1.Checked = true;
                checkBox1.Text = "服务器维护模式：开";
            }
            if (Minecraft_QQ.Mainconfig.链接.是否绑定地址 == true)
            {
                checkBox6.Checked = true;
                textBox4.ReadOnly = false;
            }
            else
            {
                checkBox6.Checked = false;
                textBox4.ReadOnly = true;
            }
            checkBox8.Checked = Minecraft_QQ.Mainconfig.设置.颜色代码开关;
            checkBox9.Checked = Minecraft_QQ.Mainconfig.设置.使用昵称发送至服务器;
            checkBox10.Checked = Minecraft_QQ.Mainconfig.设置.使用昵称发送至群;
            checkBox11.Checked = Minecraft_QQ.Mainconfig.设置.可以绑定名字;
            checkBox12.Checked = Minecraft_QQ.Mainconfig.设置.发送日志到群;

            mysql_ip.Text = Minecraft_QQ.Mainconfig.数据库.地址;
            mysql_port.Text = Minecraft_QQ.Mainconfig.数据库.端口.ToString();
            mysql_user.Text = Minecraft_QQ.Mainconfig.数据库.用户名;
            mysql_password.Text = Minecraft_QQ.Mainconfig.数据库.密码;
            mysql_use.Checked = Minecraft_QQ.Mainconfig.数据库.是否启用;
            mysql_database.Text = Minecraft_QQ.Mainconfig.数据库.数据库;
            mysql_now.Text = Minecraft_QQ.Mysql_ok ? "已连接" : "未连接";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (Utils.IsNumber(textBox1.Text) == false)
            {
                button1.Text = "错误";
                return;
            }
            Group_save_obj list = new Group_save_obj();
            list.group_s = textBox1.Text;
            list.commder = checkBox3.Checked;
            list.say = checkBox5.Checked;
            list.main = checkBox7.Checked;
            long.TryParse(textBox1.Text, out list.group_l);
            if (Minecraft_QQ.Groupconfig.群列表.ContainsKey(list.group_l) == false)
                Minecraft_QQ.Groupconfig.群列表.Add(list.group_l, list);
            new Config_write().Write_Group();
            
            button1.Text = "已添加";
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (Utils.IsNumber(textBox5.Text) == false)
            {
                button4.Text = "错误";
                return;
            }
            int.TryParse(textBox5.Text, out int a);
            Minecraft_QQ.Mainconfig.链接.端口 = a;
            Minecraft_QQ.Mainconfig.链接.地址 = textBox4.Text;
            new Config_write().Write_config();
            button4.Text = "已设置";
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (Utils.IsNumber(textBox6.Text) == false || textBox6.Text == null)
            {
                button5.Text = "错误";
                return;
            }
            long.TryParse(textBox6.Text, out long qq);
            if (Minecraft_QQ.Playerconfig.玩家列表.ContainsKey(qq) == false)
            {
                Player_save_obj player = new Player_save_obj();
                player.qq = qq;
                player.admin = true;
                Minecraft_QQ.Playerconfig.玩家列表.Add(qq, player);
                if (Minecraft_QQ.Mysql_ok == true)
                {
                    new Mysql_Add_data().player(player);
                }
                else
                {
                    new Config_write().Write_player();
                }
            }
            else
            {
                Minecraft_QQ.Playerconfig.玩家列表[qq].admin = true;
                if (Minecraft_QQ.Mysql_ok == true)
                {
                    new Mysql_Add_data().player(Minecraft_QQ.Playerconfig.玩家列表[qq]);
                }
                else
                {
                    new Config_write().Write_player();
                }
            }
            button5.Text = "添加成功";
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (Utils.IsNumber(textBox7.Text) == false || textBox7.Text == null)
            {
                button6.Text = "设置失败";
                return;
            }
            long.TryParse(textBox7.Text, out long a);
            Minecraft_QQ.Mainconfig.管理员.发送绑定信息QQ号 = a;
            new Config_write().Write_config();
            button6.Text = "已设置";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox8.Text != null)
            {
                if (!Minecraft_QQ.Playerconfig.禁止绑定列表.Contains(textBox8.Text))
                    Minecraft_QQ.Playerconfig.禁止绑定列表.Add(textBox8.Text.ToLower());
                if (Minecraft_QQ.Mysql_ok == true)
                {
                    new Mysql_Add_data().notid(textBox8.Text.ToLower());
                }
                else
                {
                    new Config_write().Write_player();
                }
                button7.Text = "已添加";
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.Mainconfig.链接.编码 = "UTF-8";
            new Config_write().Write_config();
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.Mainconfig.链接.编码 = "ANSI";
            new Config_write().Write_config();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.Mainconfig.设置.维护模式 = checkBox1.Checked;
            checkBox1.Text = checkBox1.Checked ? "服务器维护模式：开" : "服务器维护模式：关";
            new Config_write().Write_config();
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.Mainconfig.设置.自动应答开关 = checkBox2.Checked;
            new Config_write().Write_config();
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.Mainconfig.设置.始终发送消息 = checkBox4.Checked;
            new Config_write().Write_config();
        }
        private void CheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.Mainconfig.链接.是否绑定地址 = checkBox6.Checked;
            checkBox6.Checked = Minecraft_QQ.Mainconfig.链接.是否绑定地址;
            textBox4.ReadOnly = !Minecraft_QQ.Mainconfig.链接.是否绑定地址;
            new Config_write().Write_config();
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.Mainconfig.设置.颜色代码开关 = checkBox8.Checked;
            new Config_write().Write_config();
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.Mainconfig.设置.使用昵称发送至服务器 = checkBox9.Checked;
            new Config_write().Write_config();
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.Mainconfig.设置.使用昵称发送至群 = checkBox10.Checked;
            new Config_write().Write_config();
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.Mainconfig.设置.可以绑定名字 = checkBox11.Checked;
            new Config_write().Write_config();
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.Mainconfig.设置.发送日志到群 = checkBox12.Checked;
            new Config_write().Write_config();
        }

        private void mysql_b_Click(object sender, EventArgs e)
        {
            Minecraft_QQ.Mainconfig.数据库.地址 = mysql_ip.Text;
            int.TryParse(mysql_port.Text, out int a);
            Minecraft_QQ.Mainconfig.数据库.端口 = a;
            Minecraft_QQ.Mainconfig.数据库.用户名 = mysql_user.Text;
            Minecraft_QQ.Mainconfig.数据库.密码 = mysql_password.Text;
            Minecraft_QQ.Mainconfig.数据库.数据库 = mysql_database.Text;
            new Config_write().Write_config();
            if (Mysql.Mysql_start() == false)
            {
                mysql_now.Text = "Mysql无法连接";
                mysql_use.Checked = false;
                Minecraft_QQ.Mysql_ok = false;
            }
            else
            {
                mysql_now.Text = "Mysql已连接";
                mysql_use.Checked = true;
                Minecraft_QQ.Mysql_ok = true;
            }
        }

        private void mysql_use_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.Mainconfig.数据库.是否启用 = mysql_use.Checked;
        }
    }
}

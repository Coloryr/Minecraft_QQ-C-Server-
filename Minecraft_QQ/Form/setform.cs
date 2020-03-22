using Color_yr.Minecraft_QQ.Config;
using Color_yr.Minecraft_QQ.MyMysql;
using Color_yr.Minecraft_QQ.Utils;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    public partial class setform : Form
    {
        public setform()
        {
            InitializeComponent();
        }

        private bool IsNumber(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;
            foreach (char i in str)
            {
                if (i < '0' || i > '9')
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 退出按钮事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox5.Text) == true)
            {
                MessageBox.Show("请输入所需的参数");
                return;
            }
            if (IsNumber(textBox5.Text) == false)
            {
                MessageBox.Show("请输入正确的参数");
                return;
            }
            int.TryParse(textBox5.Text, out int port);
            Minecraft_QQ.MainConfig.链接.端口 = port;
            Minecraft_QQ.MainConfig.链接.地址 = textBox4.Text;
            new ConfigWrite().Config();
        }
        private void FormSettings_Load(object sender, EventArgs e)
        {
            label2.Text = "插件版本：" + Minecraft_QQ.Version;
            textBox5.Text = Minecraft_QQ.MainConfig.链接.端口.ToString();
            textBox4.Text = Minecraft_QQ.MainConfig.链接.地址;
            if (Minecraft_QQ.MainConfig.链接.编码 == "UTF-8")
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            else
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
            }
            checkBox2.Checked = Minecraft_QQ.MainConfig.设置.自动应答开关;
            checkBox4.Checked = Minecraft_QQ.MainConfig.设置.始终发送消息;
            if (Minecraft_QQ.MainConfig.设置.维护模式 == false)
            {
                checkBox1.Checked = false;
                checkBox1.Text = "服务器维护模式：关";
            }
            else if (Minecraft_QQ.MainConfig.设置.维护模式 == true)
            {
                checkBox1.Checked = true;
                checkBox1.Text = "服务器维护模式：开";
            }
            checkBox8.Checked = Minecraft_QQ.MainConfig.设置.颜色代码开关;
            checkBox9.Checked = Minecraft_QQ.MainConfig.设置.使用昵称发送至服务器;
            checkBox10.Checked = Minecraft_QQ.MainConfig.设置.使用昵称发送至群;
            checkBox11.Checked = Minecraft_QQ.MainConfig.设置.可以绑定名字;
            checkBox12.Checked = Minecraft_QQ.MainConfig.设置.发送日志到群;

            mysql_ip.Text = Minecraft_QQ.MainConfig.数据库.地址;
            mysql_port.Text = Minecraft_QQ.MainConfig.数据库.端口.ToString();
            mysql_user.Text = Minecraft_QQ.MainConfig.数据库.用户名;
            mysql_password.Text = Minecraft_QQ.MainConfig.数据库.密码;
            mysql_use.Checked = Minecraft_QQ.MainConfig.数据库.是否启用;
            mysql_database.Text = Minecraft_QQ.MainConfig.数据库.数据库;
            mysql_now.Text = Minecraft_QQ.MysqlOK ? "已连接" : "未连接";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (IsNumber(textBox1.Text) == false)
            {
                button1.Text = "错误";
                return;
            }
            GroupObj list = new GroupObj()
            {
                群号 = textBox1.Text,
                启用命令 = checkBox3.Checked,
                开启对话 = checkBox5.Checked,
                主群 = checkBox7.Checked
            };

            long.TryParse(textBox1.Text, out long group);
            if (Minecraft_QQ.GroupConfig.群列表.ContainsKey(group) == false)
                Minecraft_QQ.GroupConfig.群列表.Add(group, list);
            new ConfigWrite().Group();

            button1.Text = "已添加";
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (IsNumber(textBox5.Text) == false)
            {
                button4.Text = "错误";
                return;
            }
            int.TryParse(textBox5.Text, out int a);
            Minecraft_QQ.MainConfig.链接.端口 = a;
            Minecraft_QQ.MainConfig.链接.地址 = textBox4.Text;
            new ConfigWrite().Config();
            button4.Text = "已设置";
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (IsNumber(textBox6.Text) == false || textBox6.Text == null)
            {
                button5.Text = "错误";
                return;
            }
            long.TryParse(textBox6.Text, out long qq);
            if (Minecraft_QQ.PlayerConfig == null)
                Minecraft_QQ.PlayerConfig = new PlayerConfig();
            if (!Minecraft_QQ.PlayerConfig.玩家列表.ContainsKey(qq))
            {
                PlayerObj player = new PlayerObj();
                player.QQ号 = qq;
                player.管理员 = true;
                Minecraft_QQ.PlayerConfig.玩家列表.Add(qq, player);
                if (Minecraft_QQ.MysqlOK == true)
                {
                    Task.Factory.StartNew(async () =>
                    {
                        await new MysqlAddData().PlayerAsync(player);
                    });
                }
                else
                {
                    new ConfigWrite().Player();
                }
            }
            else
            {
                Minecraft_QQ.PlayerConfig.玩家列表[qq].管理员 = true;
                if (Minecraft_QQ.MysqlOK == true)
                {
                    Task.Factory.StartNew(async () =>
                    {
                        await new MysqlAddData().PlayerAsync(Minecraft_QQ.PlayerConfig.玩家列表[qq]);
                    });
                }
                else
                {
                    new ConfigWrite().Player();
                }
            }
            button5.Text = "添加成功";
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (IsNumber(textBox7.Text) == false || textBox7.Text == null)
            {
                button6.Text = "设置失败";
                return;
            }
            long.TryParse(textBox7.Text, out long a);
            Minecraft_QQ.MainConfig.管理员.发送绑定信息QQ号 = a;
            new ConfigWrite().Config();
            button6.Text = "已设置";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Minecraft_QQ.reload();
            button7.Text = "已重载";
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.MainConfig.链接.编码 = "UTF-8";
            new ConfigWrite().Config();
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.MainConfig.链接.编码 = "ANSI";
            new ConfigWrite().Config();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.MainConfig.设置.维护模式 = checkBox1.Checked;
            checkBox1.Text = checkBox1.Checked ? "服务器维护模式：开" : "服务器维护模式：关";
            new ConfigWrite().Config();
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.MainConfig.设置.自动应答开关 = checkBox2.Checked;
            new ConfigWrite().Config();
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.MainConfig.设置.始终发送消息 = checkBox4.Checked;
            new ConfigWrite().Config();
        }
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.MainConfig.设置.颜色代码开关 = checkBox8.Checked;
            new ConfigWrite().Config();
        }
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.MainConfig.设置.使用昵称发送至服务器 = checkBox9.Checked;
            new ConfigWrite().Config();
        }
        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.MainConfig.设置.使用昵称发送至群 = checkBox10.Checked;
            new ConfigWrite().Config();
        }
        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.MainConfig.设置.可以绑定名字 = checkBox11.Checked;
            new ConfigWrite().Config();
        }
        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.MainConfig.设置.发送日志到群 = checkBox12.Checked;
            new ConfigWrite().Config();
        }
        private void mysql_b_Click(object sender, EventArgs e)
        {
            Minecraft_QQ.MainConfig.数据库.地址 = mysql_ip.Text;
            int.TryParse(mysql_port.Text, out int a);
            Minecraft_QQ.MainConfig.数据库.端口 = a;
            Minecraft_QQ.MainConfig.数据库.用户名 = mysql_user.Text;
            Minecraft_QQ.MainConfig.数据库.密码 = mysql_password.Text;
            Minecraft_QQ.MainConfig.数据库.数据库 = mysql_database.Text;
            new ConfigWrite().Config();
            if (Mysql.MysqlStart() == false)
            {
                mysql_now.Text = "Mysql无法连接";
                mysql_use.Checked = false;
                Minecraft_QQ.MysqlOK = false;
            }
            else
            {
                mysql_now.Text = "Mysql已连接";
                mysql_use.Checked = true;
                Minecraft_QQ.MysqlOK = true;
            }
        }
        private void mysql_use_CheckedChanged(object sender, EventArgs e)
        {
            Minecraft_QQ.MainConfig.数据库.是否启用 = mysql_use.Checked;
        }
    }
}

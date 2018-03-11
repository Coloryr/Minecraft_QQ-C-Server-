using System;
using System.Windows.Forms;
using Flexlive.CQP.Framework;
using System.Net;

namespace yan_color.Minecraft_QQ
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
            //加载标题。
            this.Text = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name + "参数设置";
        }

        /// <summary>
        /// 退出按钮事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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
            LinqXML.write(MyPlugin.confirm, "群号1", textBox1.Text);
            MyPlugin.GroupSet1 = long.Parse(textBox1.Text);
            button1.Text = "设置成功";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LinqXML.write(MyPlugin.confirm, "admin", textBox2.Text);
            button2.Text = "添加成功";
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            textBox1.Text = LinqXML.read(MyPlugin.confirm, "群号");
            textBox2.Text = LinqXML.read(MyPlugin.confirm, "admin");
            textBox4.Text = LinqXML.read(MyPlugin.confirm, "IP");
            textBox5.Text = LinqXML.read(MyPlugin.confirm, "Port");

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
            LinqXML.write(MyPlugin.confirm, "IP", textBox4.Text);
            LinqXML.write(MyPlugin.confirm, "Port", textBox5.Text);
            if (!int.TryParse(textBox5.Text, out tmp))
            {
                MessageBox.Show("请正确输入数字");
                return;
            }
            MyPlugin.Port = int.Parse(textBox5.Text);
            MyPlugin.ipaddress = textBox4.Text;
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
            LinqXML.write(MyPlugin.confirm, "群号2", textBox3.Text);
            MyPlugin.GroupSet2 = long.Parse(textBox3.Text);
            button4.Text = "设置成功";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LinqXML.write(MyPlugin.confirm, "群号3", textBox6.Text);
            MyPlugin.GroupSet3 = long.Parse(textBox6.Text);
            button4.Text = "设置成功";
        }
    }
}

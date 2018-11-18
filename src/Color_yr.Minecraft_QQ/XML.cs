using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Color_yr.Minecraft_QQ
{
    public partial class XML
    {
        /// <summary>
        /// 使用linq 建立xml
        /// </summary>
        /// <param name="text">文件</param>
        /// <param name="mode">模式</param>>
        public static void CreateFile(string text, int mode)
        {
            FileInfo file = new FileInfo(config_read.path + text);
            if (file.Exists && mode == 1) //文件存在就删除
            {
                file.Delete();
                XElement contacts = new XElement("config");
                contacts.Save(config_read.path + text);
            }
            else
            {
                XElement contacts = new XElement("config");
                contacts.Save(config_read.path + text);
            }
        }
        /// <summary>
        /// //修改XML文件中的元素
        /// </summary>
        /// <param name="text">文件名</param>
        /// <param name="data">属性名</param>
        /// <param name="data1">元素名</param>
        public static void setXml(string text, string data, string data1)
        {
            if (File.Exists(config_read.path + text) == false)
            {
                CreateFile(text, 0);//创建该文件，如果路径文件夹不存在，则报错。
            }
            ///导入XML文件
            XElement xe = XElement.Load(config_read.path + text);
            ///查找被替换的元素
            IEnumerable<XElement> element = from e in xe.Elements("config")
                                            where e.Attribute("int").Value == data
                                            select e;
            ///替换为新元素，并保存
            if (element.Count() > 0)
            {
                XElement first = element.First();
                ///设置新的属性
                //first.SetAttributeValue(data, data1);
                ///替换新的节点
                first.ReplaceNodes(
                new XElement("data", data1)              ///添加元素Name
                 );
            }
            xe.Save(config_read.path + text);
        }
        /// <summary>
        /// //增加元素到XML文件
        /// </summary>
        /// <param name="text">文件名</param>
        /// <param name="data">属性名</param>
        /// <param name="data1">元素名</param>
        public static void write(string text, string data, string data1)
        {
            if (File.Exists(config_read.path + text) == false)
            {
                CreateFile(text, 0);//创建该文件，如果路径文件夹不存在，则报错。
            }
            try
            {
                string a = read(text, data);
                if (a != null)
                {
                    setXml(text, data, data1);
                }
                else
                {
                    ///导入XML文件
                    XElement xe = XElement.Load(config_read.path + text);
                    ///创建一个新的节点
                    XElement student = new XElement("config",
                     new XAttribute("int", data),                    ///添加属性number
             new XElement("data", data1)                     ///添加元素Name
             );
                    ///添加节点到文件中，并保存
                    xe.Add(student);
                    xe.Save(config_read.path + text);
                }
            }
            catch (Exception)
            {
                if (MessageBox.Show("配置文件在读取时发发生了错误，是否要删除原来的配置文件再新生成一个？", "配置文件错误", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CreateFile(text, 0);
                    rewrite(text, data, data1);
                }
            }
        }

        private static void rewrite(string text, string data, string data1)
        {
            try
            {
                string a = read(text, data);
                if (a != null)
                {
                    setXml(text, data, data1);
                }
                else
                {
                    ///导入XML文件
                    XElement xe = XElement.Load(config_read.path + text);
                    ///创建一个新的节点
                    XElement student = new XElement("config",
                     new XAttribute("int", data),                    ///添加属性number
             new XElement("data", data1)                     ///添加元素Name
             );
                    ///添加节点到文件中，并保存
                    xe.Add(student);
                    xe.Save(config_read.path + text);
                }
            }
            catch (Exception e)
            {
                logs.Log_write("[ERROR][Config]写文件错误，请检查" + e.Message + "|写的内容：" + data);
            }
        }
        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="text">文件名</param>
        /// <param name="text">属性名</param>
        /// <param name="text">元素名</param>
        public static void Remove(string text, string data, string data1)//删除XML文件中的元素
        {
            if (File.Exists(config_read.path + text) == false)
            {
                CreateFile(text, 0);//创建该文件，如果路径文件夹不存在，则报错。
            }
            ///导入XML文件
            XElement xe = XElement.Load(config_read.path + text);
            ///查找被删除的元素
            IEnumerable<XElement> element = from e in xe.Elements()
                                            where e.Attribute(data).Value == data1
                                            select e;
            ///删除指定的元素，并保存
            if (element.Count() > 0)
            {
                element.First().Remove();
            }
            xe.Save(config_read.path + text);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="text">文件名</param>
        /// <param name="data">属性名</param>
        public static string read(string text, string data)
        {
            string a = null;
            if (File.Exists(config_read.path + text) == false)
            {
                CreateFile(text, 0);//创建该文件，如果路径文件夹不存在，则报错。
            }
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(config_read.path + text);

                XmlNode xnP = xmlDoc.SelectSingleNode("config/config[@int='" + data + "']/data");
                a = xnP.InnerText;
                if (a == "") a = null;
            }
            catch (Exception e)
            {
                logs.Log_write("[ERROR][Config]读文件错误，请检查" + e.Message + "|读取的内容：" + data);
            }
            return a;
        }
    }
}

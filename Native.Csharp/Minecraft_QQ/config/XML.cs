using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Color_yr.Minecraft_QQ
{
    class XML
    {
        /// <summary>
        /// 使用linq 建立xml
        /// </summary>
        /// <param name="path">文件(包含路径)</param>
        /// <param name="mode">模式</param>>
        public static void CreateFile(string path, int mode)
        {
            if (File.Exists(path) && mode == 1) //文件存在就删除
                File.Delete(path);
            XElement contacts = new XElement("config");
            contacts.Save(path);
        }
        /// <summary>
        /// //修改XML文件中的元素
        /// </summary>
        /// <param name="path">文件(包含路径)</param>
        /// <param name="type">类型名</param>
        /// <param name="attribute">属性名</param>
        /// <param name="data">数据</param>
        public static void setXml(string path, string type, string attribute, string data)
        {
            if (File.Exists(path) == false)
                CreateFile(path, 0);//创建该文件，如果路径文件夹不存在，则报错。
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(path);
            XmlNodeList nodeList = xmldoc.SelectSingleNode("config/" + type).ChildNodes;
            foreach (XmlNode xn in nodeList)//遍历所有子节点
            {
                XmlElement xe2 = (XmlElement)xn;//转换类型
                if (xe2.Name == attribute)//如果找到
                {
                    xe2.InnerText = data;//则修改
                    break;//找到退出来就可以了
                }
            }
            xmldoc.Save(path);//保存。
        }
        /// <summary>
        /// //增加元素到XML文件
        /// </summary>
        /// <param name="path">文件(带路径)</param>
        /// <param name="type">类型名</param>
        /// <param name="attribute">属性名</param>
        /// <param name="data">数据</param>
        public static void write(string path, string type, string attribute, string data)
        {
            if (File.Exists(path) == false)
                CreateFile(path, 0);//创建该文件，如果路径文件夹不存在，则报错。
            try
            {
                string a = read(path, type, attribute);
                if (a != null)
                    setXml(path, type, attribute, data);
                else
                {
                    ///导入XML文件
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(path);
                    XmlElement node = (XmlElement)xmldoc.SelectSingleNode("config/" + type);
                    if (node == null)
                        node = xmldoc.CreateElement(type);
                    XmlElement xesub1 = xmldoc.CreateElement(attribute);
                    xesub1.InnerText = data;
                    node.AppendChild(xesub1);
                    xmldoc.DocumentElement.AppendChild(node);
                    xmldoc.Save(path);
                }
            }
            catch (Exception)
            {
                if (MessageBox.Show("配置文件在写入时发发生了错误，是否要删除原来的配置文件再新生成一个？",
                    "配置文件错误", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CreateFile(path, 0);
                    write(path, type, attribute, data);
                }
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="path">文件(带路径)</param>
        /// <param name="type">类型名</param>
        /// <param name="attribute">属性名</param>
        public static string read(string path, string type, string attribute)
        {
            string temp = null;
            if (File.Exists(path) == false)
                CreateFile(path, 0);//创建该文件，如果路径文件夹不存在，则报错。
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);

                XmlNode xnP = xmlDoc.SelectSingleNode("config/" + type + "/" + attribute);
                temp = xnP.InnerText;
                if (temp == "") temp = null;
            }
            catch (Exception)
            { }
            return temp;
        }

        public static bool read_id_memory(string file, string id)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(file);
            XmlNodeList nodeList = xmldoc.SelectSingleNode("config").ChildNodes;
            foreach (XmlNode xn in nodeList)//遍历所有子节点
            {
                XmlNode xnLurl = xn.SelectSingleNode("绑定");
                if (xnLurl != null && xnLurl.FirstChild.InnerText == id)
                    return true;
            }
            return false;
        }
    }
}

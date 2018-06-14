using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    class Mysql
    {
        private static MySqlConnection conn;

        public static string Mysql_IP = "127.0.0.1";
        public static string Mysql_Port = "3306";
        public static string Mysql_User = "root";
        public static string Mysql_Password = "123456";

        public static string Mysql_player = "minecraft_qq_player";                
        //qq字段qq的值，name字段name的值

        public static bool mysql_start()
        {
            
            Mysql_IP = XML.read(config_read.config, "Mysql地址");
            Mysql_Port = XML.read(config_read.config, "Mysql端口");
            Mysql_User = XML.read(config_read.config, "Mysql账户");
            Mysql_Password = XML.read(config_read.config, "Mysql密码");
            
            string ConnectString = string.Format("SslMode=none;Server={0};Port={1};User ID={2};Password={3};Database=minecraft_qq;", Mysql_IP, Mysql_Port, Mysql_User, Mysql_Password);
            conn = new MySqlConnection(ConnectString);

            try
            {
                conn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter();
                DataTable dt = conn.GetSchema();
                MySqlCommand cmd = new MySqlCommand("select * from " + Mysql_player, conn);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1146:
                        MessageBox.Show(Mysql_player + "表不存在，系统正在为您创建！");
                        string mySelectQuery = "CREATE TABLE " + Mysql_player + "(qq VARCHAR(20),name VARCHAR(20))";
                        MySqlCommand cmd = new MySqlCommand(mySelectQuery, conn);
                        cmd.ExecuteNonQuery();
                        break;
                    default:
                        MessageBox.Show("错误ID：" + ex.Number + "\n" + ex.Message);
                        break;
                        return false;
                }
            }
            conn.Close();
            return true;
        }
        public static void mysql_add(string database, string qq, string name)
        {

            if (mysql_search(database, qq) != null)
            {
                mysql_replace(database, qq, name);
            }
            else
            {
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                string str = "INSERT INTO {0}(qq,name)VALUES('{1}','{2}')";
                command.CommandText = string.Format(str, database, qq, name);
                command.ExecuteNonQuery();
                command = null;
                conn.Close();
            }
        }
        public static string mysql_search(string database, string qq)
        {
            conn.Open();
            string command = string.Format("SELECT * FROM {0} where qq='{1}'", database, qq);
            MySqlCommand mycmd = new MySqlCommand(command, conn);
            MySqlDataReader reader = mycmd.ExecuteReader();
            string name = null;
            while (reader.Read())
            {
                name = reader.GetString(1);
            }
            mycmd = null;
            command = null;
            reader.Close();
            conn.Close();
            return name;
        }

        public static void mysql_remove(string database, string qq)
        {
            conn.Open();
            MySqlCommand command = conn.CreateCommand();
            string str = string.Format("DELETE FROM {0} WHERE qq='{1}'", database, qq);
            command.CommandText = str;
            command.ExecuteNonQuery();
            conn.Close();
        }
        
        public static void mysql_replace(string database, string qq, string name)
        {
            conn.Open();
            MySqlCommand command = conn.CreateCommand();
            string str = "UPDATE {0} SET qq='{1}',name='{2}' WHERE qq='{2}'";
            command.CommandText = string.Format(str, database, qq, name);
            command.ExecuteNonQuery();
            command = null;
            conn.Close();
        }
    }
}

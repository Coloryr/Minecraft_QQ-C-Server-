using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace Color_yr.Minecraft_QQ
{
    class Mysql_user
    {
        private static MySqlConnection conn;

        public static string Mysql_player = "minecraft_qq_player";
        public static string Mysql_notid = "minecraft_qq_notid";
        public static string Mysql_mute = "minecraft_qq_mute";
        //qq字段qq的值，name字段name的值

        private static string ConnectString = null;

        public static string GBK_UTF8(string msg)
        {
            byte[] srcBytes = Encoding.Default.GetBytes(msg);
            byte[] bytes = Encoding.Convert(Encoding.Default, Encoding.UTF8, srcBytes);
            return Encoding.UTF8.GetString(bytes);
        }

        public static bool mysql_start()
        {
            ConnectString = string.Format("SslMode=none;Server={0};Port={1};User ID={2};Password={3};Database=minecraft_qq;Charset=utf8;",
                Mysql_IP, Mysql_Port, Mysql_User, Mysql_Password);
            conn = new MySqlConnection(ConnectString);

            if (mysql_add_table(Mysql_player) == false) return false;
            if (mysql_add_table(Mysql_notid) == false) return false;
            if (mysql_add_table(Mysql_mute) == false) return false;
            return true;
        }
        public static bool mysql_add_table(string table_name)
        {
            try
            {
                conn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter();
                DataTable dt = conn.GetSchema();
                MySqlCommand cmd = new MySqlCommand("select * from " + table_name, conn);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1146:
                        string mySelectQuery = "CREATE TABLE " + table_name + "(qq VARCHAR(20),name VARCHAR(20))";
                        MySqlCommand cmd = new MySqlCommand(mySelectQuery, conn);
                        cmd.ExecuteNonQuery();
                        break;
                    default:
                        logs.Log_write("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
                        break;
                }
                return false;
            }
            conn.Close();
            return true;
        }
        public static void mysql_add(string table_name, string qq, string name)
        {
            if (mysql_search(table_name, qq) != null)
                mysql_replace(table_name, qq, name);
            else
            {
                try
                {
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();
                    string str = "INSERT INTO {0}(qq,name)VALUES('{1}','{2}')";
                    command.CommandText = string.Format(str, table_name, qq, GBK_UTF8(name));
                    command.ExecuteNonQuery();
                    command = null;
                }
                catch (MySqlException ex)
                {
                    logs.Log_write("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
                }
                conn.Close();
            }
        }
        public static string mysql_search(string table_name, string qq)
        {
            string name = null;
            try
            {
                conn.Open();
                string command = string.Format("SELECT * FROM {0} where qq='{1}'", table_name, qq);
                MySqlCommand mycmd = new MySqlCommand(command, conn);
                MySqlDataReader reader = mycmd.ExecuteReader();
                while (reader.Read())
                    name = reader.GetString(1);
                mycmd = null;
                command = null;
                reader.Close();
            }
            catch (MySqlException ex)
            {
                logs.Log_write("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
            }
            conn.Close();
            return name;
        }

        public static string mysql_search_id(string table_name, string id)
        {
            string name = null;
            try
            {
                conn.Open();
                string command = string.Format("SELECT * FROM {0} WHERE name='{1}'", table_name, id);
                MySqlCommand mycmd = new MySqlCommand(command, conn);
                MySqlDataReader reader = mycmd.ExecuteReader();
                while (reader.Read())
                    name = reader.GetString(0);
                mycmd = null;
                command = null;
                reader.Close();
            }
            catch (MySqlException ex)
            {
                logs.Log_write("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
            }
            conn.Close();
            return name;
        }

        public static void mysql_remove(string table_name, string qq)
        {
            try
            {
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                string str = string.Format("DELETE FROM {0} WHERE qq='{1}'", table_name, qq);
                command.CommandText = str;
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                logs.Log_write("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
            }
            conn.Close();
        }

        public static void mysql_replace(string table_name, string qq, string name)
        {
            try
            {
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                string str = "UPDATE {0} SET name='{1}' WHERE qq='{2}'";
                command.CommandText = string.Format(str, table_name, GBK_UTF8(name), qq);
                command.ExecuteNonQuery();
                command = null;
            }
            catch (MySqlException ex)
            {
                logs.Log_write("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
            }
            conn.Close();
        }
    }
}

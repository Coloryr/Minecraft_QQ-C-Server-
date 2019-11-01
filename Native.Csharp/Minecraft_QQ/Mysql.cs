using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace Color_yr.Minecraft_QQ
{
    class Mysql
    {
        private static MySqlConnection conn;

        public static string Mysql_player = "minecraft_qq_player";
        public static string Mysql_notid = "minecraft_qq_notid";
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
                mysql_config.ip, mysql_config.Port, mysql_config.user, mysql_config.password);
            conn = new MySqlConnection(ConnectString);

            if (mysql_add_table_player(Mysql_player) == false) return false;
            if (mysql_add_table_notid(Mysql_notid) == false) return false;
            return true;
        }
        public static bool mysql_add_table_player(string table_name)
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
                        string mySelectQuery = "CREATE TABLE " + table_name + "(id VARCHAR(20),nick VARCHAR(20),qq VARCHAR(20),mute VARCHAR(20),admin VARCHAR(20))";
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
        public static bool mysql_add_table_notid(string table_name)
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
                        string mySelectQuery = "CREATE TABLE " + table_name + "(id VARCHAR(20))";
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
        public static void mysql_add(string table_name, player_save player)
        {
            if (mysql_search(table_name, player) != null)
                mysql_replace(table_name, player);
            else
            {
                try
                {
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();
                    string str = "INSERT INTO {0}(id,nick,qq,mute,admin)VALUES('{1}','{2}','{3}','{4}','{5}')";
                    command.CommandText = string.Format(str, table_name, GBK_UTF8(player.player), GBK_UTF8(player.nick), player.qq, player.mute, player.admin);
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
        public static string mysql_search(string table_name, player_save player)
        {
            string name = null;
            try
            {
                conn.Open();
                string command = string.Format("SELECT * FROM {0} where qq='{1}'", table_name, player.qq);
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

        public static void mysql_remove(string table_name, player_save player)
        {
            try
            {
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                string str = string.Format("DELETE FROM {0} WHERE qq='{1}'", table_name, player.qq);
                command.CommandText = str;
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                logs.Log_write("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
            }
            conn.Close();
        }

        public static void mysql_replace(string table_name, player_save player)
        {
            try
            {
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                string str = "UPDATE {0} SET id='{1}' nick='{2}' mute='{3}' admin='{4}' WHERE qq='{5}'";
                command.CommandText = string.Format(str, table_name, GBK_UTF8(player.player), GBK_UTF8(player.nick), player.mute, player.admin, player.qq);
                command.ExecuteNonQuery();
                command = null;
            }
            catch (MySqlException ex)
            {
                logs.Log_write("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
            }
            conn.Close();
        }

        public static void mysql_load()
        { 
            
        }
    }
}

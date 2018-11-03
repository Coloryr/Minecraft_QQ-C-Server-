using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    class Mysql
    {
        private static MySqlConnection conn;

        public static string Mysql_player = "minecraft_qq_player";
        public static string Mysql_notid = "minecraft_qq_notid";
        public static string Mysql_mute = "minecraft_qq_mute";
        //qq字段qq的值，name字段name的值

        static string ConnectString = null;

        public static bool mysql_start()
        {
            ConnectString = string.Format("SslMode=none;Server={0};Port={1};User ID={2};Password={3};Database=minecraft_qq;", use.Mysql_IP, use.Mysql_Port, use.Mysql_User, use.Mysql_Password);
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
                        MessageBox.Show("错误ID：" + ex.Number + "\n" + ex.Message);
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
            {
                mysql_replace(table_name, qq, name);
            }
            else
            {
                try
                {
                    byte[] srcBytes = Encoding.Default.GetBytes(name);
                    byte[] bytes = Encoding.Convert(Encoding.Default, Encoding.UTF8, srcBytes);
                    name = Encoding.UTF8.GetString(bytes);
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();
                    string str = "INSERT INTO {0}(qq,name)VALUES('{1}','{2}')";
                    command.CommandText = string.Format(str, table_name, qq, name);
                    command.ExecuteNonQuery();
                    command = null;
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("错误ID：" + ex.Number + "\n" + ex.Message);
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
                {
                    name = reader.GetString(1);
                }
                mycmd = null;
                command = null;
                reader.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("错误ID：" + ex.Number + "\n" + ex.Message);
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
                MessageBox.Show("错误ID：" + ex.Number + "\n" + ex.Message);
            }
            conn.Close();
        }
        
        public static void mysql_replace(string table_name, string qq, string name)
        {
            try
            {
                byte[] srcBytes = Encoding.Default.GetBytes(name);
                byte[] bytes = Encoding.Convert(Encoding.Default, Encoding.UTF8, srcBytes);
                name = Encoding.UTF8.GetString(bytes);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                string str = "UPDATE {0} SET name='{1}' WHERE qq='{2}'";
                command.CommandText = string.Format(str, table_name, name, qq);
                command.ExecuteNonQuery();
                command = null;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("错误ID：" + ex.Number + "\n" + ex.Message);
            }
            conn.Close();
        }
    }
}

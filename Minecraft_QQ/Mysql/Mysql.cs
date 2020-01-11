using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    class Mysql
    {
        public static MySqlConnection conn;

        public static string Mysql_player = "minecraft_qq_player";
        public static string Mysql_notid = "minecraft_qq_notid";
        public static string Mysql_mute = "minecraft_qq_mute";

        public static bool Mysql_start()
        {
            string ConnectString = string.Format("SslMode=none;Server={0};Port={1};User ID={2};Password={3};Database={4};Charset=utf8;",
                Minecraft_QQ.Mainconfig.数据库.地址, Minecraft_QQ.Mainconfig.数据库.端口, Minecraft_QQ.Mainconfig.数据库.用户名,
                Minecraft_QQ.Mainconfig.数据库.密码, Minecraft_QQ.Mainconfig.数据库.数据库);
            conn = new MySqlConnection(ConnectString);
            if (conn == null)
            {
                MessageBox.Show("Mysql错误\n" + ConnectString);
            }
            Mysql_Add_table table = new Mysql_Add_table();

            if (table.player(Mysql_player) == false) return false;
            if (table.only(Mysql_notid) == false) return false;
            if (table.only(Mysql_mute) == false) return false;
            return true;
        }

        public void Load()
        {
            Load_player();
            load_mute();
            Load_notid();
        }

        private void Load_player()
        {
            Minecraft_QQ.Playerconfig.玩家列表.Clear();
            try
            {
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = "SELECT * FROM " + Mysql_player;
                command.ExecuteNonQuery();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Player_save_obj player = new Player_save_obj
                    {
                        player = reader.GetString(0),
                        nick = reader.GetString(1),
                        admin = reader.GetString(3).ToLower() == "true" ? true : false,
                    };
                    long.TryParse(reader.GetString(2), out player.qq);
                    Minecraft_QQ.Playerconfig.玩家列表.Add(player.qq, player);
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                logs.Log_write("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
            }
            conn.Close();
        }
        private void Load_notid()
        {
            Minecraft_QQ.Playerconfig.禁止绑定列表.Clear();
            try
            {
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = "SELECT * FROM " + Mysql_notid;
                command.ExecuteNonQuery();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (!string.IsNullOrWhiteSpace(reader.GetString(0)))
                        if (Minecraft_QQ.Playerconfig.禁止绑定列表.Contains(reader.GetString(0).ToLower()) == false)
                            Minecraft_QQ.Playerconfig.禁止绑定列表.Add(reader.GetString(0).ToLower());
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                logs.Log_write("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
            }
            conn.Close();
        }
        private void load_mute()
        {
            Minecraft_QQ.Playerconfig.禁言列表.Clear();
            try
            {
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = "SELECT * FROM " + Mysql_notid;
                command.ExecuteNonQuery();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (!string.IsNullOrWhiteSpace(reader.GetString(0)))
                        if (Minecraft_QQ.Playerconfig.禁言列表.Contains(reader.GetString(0).ToLower()) == false)
                            Minecraft_QQ.Playerconfig.禁言列表.Add(reader.GetString(0).ToLower());
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                logs.Log_write("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
            }
            conn.Close();
        }
    }
}

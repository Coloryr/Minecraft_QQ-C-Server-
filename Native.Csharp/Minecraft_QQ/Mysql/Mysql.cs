using MySql.Data.MySqlClient;
using System.Text;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    class Mysql
    {
        public static MySqlConnection conn;

        public static string Mysql_player = "minecraft_qq_player";
        public static string Mysql_notid = "minecraft_qq_notid";
        public static string Mysql_mute = "minecraft_qq_mute";

        public static bool mysql_start()
        {
            string ConnectString = string.Format("SslMode=none;Server={0};Port={1};User ID={2};Password={3};Database={4};Charset=utf8;",
                mysql_config.ip, mysql_config.Port, mysql_config.user, mysql_config.password, mysql_config.database);
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

        public void load()
        {
            load_player();
            load_mute();
            load_notid();
        }

        private void load_player()
        {
            config_file.player_list.Clear();
            try
            {
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = "SELECT * FROM " + Mysql_player;
                command.ExecuteNonQuery();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    player_save player = new player_save
                    {
                        player = reader.GetString(0),
                        nick = reader.GetString(1),
                        admin = reader.GetString(3).ToLower() == "true" ? true : false,
                    };
                    long.TryParse(reader.GetString(2), out player.qq);
                    config_file.player_list.Add(player.qq, player);
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                logs.Log_write("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
            }
            conn.Close();
        }
        private void load_notid()
        {
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
                        if (config_file.cant_bind.Contains(reader.GetString(0).ToLower()) == false)
                            config_file.cant_bind.Add(reader.GetString(0).ToLower());
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
                        if (config_file.cant_bind.Contains(reader.GetString(0).ToLower()) == false)
                            config_file.cant_bind.Add(reader.GetString(0).ToLower());
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

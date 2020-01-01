using MySql.Data.MySqlClient;

namespace Color_yr.Minecraft_QQ
{
    class Mysql_search_data
    {
        public Player_save_obj player(long qq)
        {
            Player_save_obj player = null;
            try
            {
                Mysql.conn.Open();
                string command = string.Format("SELECT * FROM {0} where qq='{1}'", Mysql.Mysql_player, qq);
                MySqlCommand mycmd = new MySqlCommand(command, Mysql.conn);
                MySqlDataReader reader = mycmd.ExecuteReader();
                while (reader.Read())
                {
                    player = new Player_save_obj();
                    player.player = reader.GetString(0);
                    player.nick = reader.GetString(1);
                    long.TryParse(reader.GetString(2), out player.qq);
                    player.admin = reader.GetString(3).ToLower() == "true" ? true : false;
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                logs.Log_write("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
            }
            Mysql.conn.Close();
            return player;
        }
    }
}

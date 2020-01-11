using MySql.Data.MySqlClient;

namespace Color_yr.Minecraft_QQ
{
    class Mysql_remove_data
    {
        public void mute(string name)
        {
            try
            {
                Mysql.conn.Open();
                MySqlCommand command = Mysql.conn.CreateCommand();
                string str = string.Format("DELETE FROM {0} WHERE name='{1}'", Mysql.Mysql_mute, name);
                command.CommandText = str;
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                logs.Log_write("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
            }
            Mysql.conn.Close();
        }
    }
}

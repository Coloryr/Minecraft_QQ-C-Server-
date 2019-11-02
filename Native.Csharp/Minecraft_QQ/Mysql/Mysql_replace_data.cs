using MySql.Data.MySqlClient;
using System.Text;

namespace Color_yr.Minecraft_QQ
{
    class Mysql_replace_data
    {
        private string GBK_UTF8(string msg)
        {
            try
            {
                byte[] srcBytes = Encoding.Default.GetBytes(msg);
                byte[] bytes = Encoding.Convert(Encoding.Default, Encoding.UTF8, srcBytes);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return msg;
            }
        }

        public void player(player_save player)
        {
            try
            {
                Mysql.conn.Open();
                MySqlCommand command = Mysql.conn.CreateCommand();
                string str = "UPDATE {0} SET id='{1}',nick='{2}',admin='{3}' WHERE qq='{4}'";
                command.CommandText = string.Format(str, Mysql.Mysql_player, GBK_UTF8(player.player), GBK_UTF8(player.nick), player.admin, player.qq);
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

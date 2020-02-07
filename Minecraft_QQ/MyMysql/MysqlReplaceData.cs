using Color_yr.Minecraft_QQ.Utils;
using MySql.Data.MySqlClient;

namespace Color_yr.Minecraft_QQ.MyMysql
{
    class MysqlReplaceData
    {
        public void player(PlayerObj player)
        {
            try
            {
                Mysql.conn.Open();
                MySqlCommand command = Mysql.conn.CreateCommand();
                string str = "UPDATE {0} SET Name='{1}',Nick='{2}',Admin='{3}' WHERE QQ='{4}'";
                command.CommandText = string.Format(str, Mysql.MysqlPlayerTable, Funtion.GBKtoUTF8(player.名字), Funtion.GBKtoUTF8(player.昵称), player.管理员, player.QQ号);
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                logs.LogWrite("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
            }
            Mysql.conn.Close();
        }
    }
}

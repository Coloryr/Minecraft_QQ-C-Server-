using Color_yr.Minecraft_QQ.Utils;
using MySql.Data.MySqlClient;

namespace Color_yr.Minecraft_QQ.MyMysql
{
    class MysqlRemoveData
    {
        public void Mute(string name)
        {
            try
            {
                Mysql.conn.Open();
                MySqlCommand command = Mysql.conn.CreateCommand();
                string str = string.Format("DELETE FROM {0} WHERE Name='{1}'", Mysql.MysqlMuteTable, name);
                command.CommandText = str;
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

using Color_yr.Minecraft_QQ.Utils;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace Color_yr.Minecraft_QQ.MyMysql
{
    internal class MysqlReplaceData
    {
        public async Task playerAsync(PlayerObj player)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(string.Format("UPDATE {0} SET Name=@name,Nick=@nick,Admin=@admin WHERE QQ=@qq", Mysql.MysqlPlayerTable));
                cmd.Parameters.AddRange(new MySqlParameter[]
                {
                    new MySqlParameter("@name", Funtion.GBKtoUTF8(player.名字)),
                    new MySqlParameter("@nick", Funtion.GBKtoUTF8(player.昵称)),
                    new MySqlParameter("@admin", player.管理员),
                    new MySqlParameter("@qq", player.QQ号)
                });
                await Mysql.MysqlSql(cmd);
            }
            catch (MySqlException ex)
            {
                logs.LogWrite("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
            }
            Mysql.conn.Close();
        }
    }
}

using Minecraft_QQ.Config;
using Minecraft_QQ.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace Minecraft_QQ.MyMysql
{
    internal class MysqlReplaceData
    {
        public async Task PlayerAsync(PlayerObj player)
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
            catch (Exception e)
            {
                Logs.LogError(e);
            }
            Mysql.conn.Close();
        }
    }
}

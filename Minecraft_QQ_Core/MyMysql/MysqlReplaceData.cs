using Minecraft_QQ_Core.Config;
using Minecraft_QQ_Core.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace Minecraft_QQ_Core.MyMysql
{
    internal class MysqlReplaceData
    {
        public static async Task PlayerAsync(PlayerObj player)
        {
            try
            {
                MySqlCommand cmd = new($"UPDATE {Mysql.MysqlPlayerTable} SET Name=@name,Nick=@nick,Admin=@admin WHERE QQ=@qq");
                cmd.Parameters.AddRange(new MySqlParameter[]
                {
                    new("@name", Funtion.GBKtoUTF8(player.名字)),
                    new("@nick", Funtion.GBKtoUTF8(player.昵称)),
                    new("@admin", player.管理员),
                    new("@qq", player.QQ号)
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

using Minecraft_QQ_Core.Config;
using Minecraft_QQ_Core.Utils;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace Minecraft_QQ_Core.MyMysql
{
    internal class MysqlAddData
    {
        /// <summary>
        /// 添加玩家
        /// </summary>
        /// <param name="player">玩家名字</param>
        public static async Task PlayerAsync(PlayerObj player)
        {
            if (await MysqlSearchData.PlayerAsync(player.QQ号) != null)
                await MysqlReplaceData.PlayerAsync(player);
            else
            {
                MySqlCommand cmd;
                if (string.IsNullOrWhiteSpace(player.昵称))
                {
                    cmd = new($"INSERT INTO {Mysql.MysqlPlayerTable}(Name,QQ,Admin)VALUES(@name,@qq,@admin)");
                    cmd.Parameters.AddRange(new MySqlParameter[]
                    {
                        new("@name", Funtion.GBKtoUTF8(player.名字)),
                        new("@admin", player.管理员),
                        new("@qq", player.QQ号)
                    });
                }
                else
                {
                    cmd = new($"INSERT INTO {Mysql.MysqlPlayerTable}(Name,Nick,QQ,Admin)VALUES(@name,@nick,@qq,@admin)");
                    cmd.Parameters.AddRange(new MySqlParameter[]
                    {
                        new("@name", Funtion.GBKtoUTF8(player.名字)),
                        new("@nick", Funtion.GBKtoUTF8(player.昵称)),
                        new("@admin", player.管理员),
                        new("@qq", player.QQ号)
                    });
                }
                await Mysql.MysqlSql(cmd);
            }
            Mysql.conn.Close();
        }

        public static async Task MuteAsync(string name)
        {
            MySqlCommand cmd = new($"INSERT INTO {Mysql.MysqlMuteTable}(Name)VALUES(@name)");
            cmd.Parameters.AddRange(new MySqlParameter[]
            {
                new("@name", Funtion.GBKtoUTF8(name.ToLower())),
            });
            await Mysql.MysqlSql(cmd);
            Mysql.conn.Close();
        }
    }
}

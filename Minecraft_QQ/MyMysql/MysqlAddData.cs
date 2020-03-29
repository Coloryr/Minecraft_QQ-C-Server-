using Color_yr.Minecraft_QQ.Utils;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace Color_yr.Minecraft_QQ.MyMysql
{
    internal class MysqlAddData
    {
        /// <summary>
        /// 添加玩家
        /// </summary>
        /// <param name="player">玩家名字</param>
        public async Task PlayerAsync(PlayerObj player)
        {
            MysqlSearchData search = new MysqlSearchData();
            if (await search.PlayerAsync(player.QQ号) != null)
                await new MysqlReplaceData().playerAsync(player);
            else
            {
                MySqlCommand cmd;
                if (string.IsNullOrWhiteSpace(player.昵称))
                {
                    cmd = new MySqlCommand(string.Format("INSERT INTO {0}(Name,QQ,Admin)VALUES(@name,@qq,@admin)", Mysql.MysqlPlayerTable));
                    cmd.Parameters.AddRange(new MySqlParameter[]
                    {
                    new MySqlParameter("@name", Funtion.GBKtoUTF8(player.名字)),
                    new MySqlParameter("@admin", player.管理员),
                    new MySqlParameter("@qq", player.QQ号)
                    });
                }
                else
                {
                    cmd = new MySqlCommand(string.Format("INSERT INTO {0}(Name,Nick,QQ,Admin)VALUES(@name,@nick,@qq,@admin)", Mysql.MysqlPlayerTable));
                    cmd.Parameters.AddRange(new MySqlParameter[]
                    {
                    new MySqlParameter("@name", Funtion.GBKtoUTF8(player.名字)),
                    new MySqlParameter("@nick", Funtion.GBKtoUTF8(player.昵称)),
                    new MySqlParameter("@admin", player.管理员),
                    new MySqlParameter("@qq", player.QQ号)
                    });
                }
                await Mysql.MysqlSql(cmd);
            }
        }
        /// <summary>
        /// 添加禁止绑定ID
        /// </summary>
        /// <param name="name">禁止的ID</param>
        public async Task NotIDAsync(string name)
        {
            MySqlCommand cmd = new MySqlCommand(string.Format("INSERT INTO {0}(Name)VALUES(@name)", Mysql.MysqlNotIDTable));
            cmd.Parameters.AddRange(new MySqlParameter[]
            {
                new MySqlParameter("@name", Funtion.GBKtoUTF8(name.ToLower())),
            });
            await Mysql.MysqlSql(cmd);
        }
        public async Task MuteAsync(string name)
        {
            MySqlCommand cmd = new MySqlCommand(string.Format("INSERT INTO {0}(Name)VALUES(@name)", Mysql.MysqlMuteTable));
            cmd.Parameters.AddRange(new MySqlParameter[]
            {
                new MySqlParameter("@name", Funtion.GBKtoUTF8(name.ToLower())),
            });
            await Mysql.MysqlSql(cmd);
        }
    }
}

using Minecraft_QQ_Core.Config;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace Minecraft_QQ_Core.MyMysql
{
    internal class MysqlSearchData
    {
        public static async Task<PlayerObj> PlayerAsync(long qq)
        {
            PlayerObj player = null;
            MySqlCommand cmd = new($"SELECT `Name`,`Nick`,`Admin` FROM {Mysql.MysqlPlayerTable} WHERE QQ=@qq");
            cmd.Parameters.AddRange(new MySqlParameter[]
            {
                new("@qq", qq)
            });
            var item = await Mysql.MysqlSql(cmd, true);
            if (item != null && item.HasRows)
            {
                player.名字 = item.GetString(0);
                player.昵称 = item.GetString(1);
                player.QQ号 = qq;
                player.管理员 = item.GetBoolean(2);
                item.Close();
            }
            await item.CloseAsync();
            Mysql.conn.Close();
            return player;
        }
    }
}

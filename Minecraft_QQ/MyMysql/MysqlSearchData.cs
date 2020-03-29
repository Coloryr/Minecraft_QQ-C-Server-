using Color_yr.Minecraft_QQ.Utils;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace Color_yr.Minecraft_QQ.MyMysql
{
    internal class MysqlSearchData
    {
        public async Task<PlayerObj> PlayerAsync(long qq)
        {
            PlayerObj player = null;
            MySqlCommand cmd = new MySqlCommand(string.Format("SELECT `Name`,`Nick`,`Admin` FROM {0} WHERE QQ=@qq", Mysql.MysqlPlayerTable));
            cmd.Parameters.AddRange(new MySqlParameter[] 
            { 
                new MySqlParameter("@qq", qq)
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
            item.Close();
            await Mysql.conn.CloseAsync();
            return player;
        }
    }
}

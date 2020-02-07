using Color_yr.Minecraft_QQ.Utils;
using System.Threading.Tasks;

namespace Color_yr.Minecraft_QQ.MyMysql
{
    class MysqlSearchData
    {
        public async Task<PlayerObj> PlayerAsync(long qq)
        {
            PlayerObj player = null;
            var item = await Mysql.MysqlSql(string.Format("SELECT `Name`,`Nick`,`Admin` FROM {0} where QQ='{1}'", Mysql.MysqlPlayerTable, qq));
            if (item != null && item.HasRows)
            {
                player.名字 = item.GetString(0);
                player.昵称 = item.GetString(1);
                player.QQ号 = qq;
                player.管理员 = item.GetBoolean(2);
                item.Close();
            }
            return player;
        }
    }
}

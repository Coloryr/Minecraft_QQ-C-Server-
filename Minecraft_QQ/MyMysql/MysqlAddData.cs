using Color_yr.Minecraft_QQ.Utils;
using System.Threading.Tasks;

namespace Color_yr.Minecraft_QQ.MyMysql
{
    class MysqlAddData
    {
        /// <summary>
        /// 添加玩家
        /// </summary>
        /// <param name="player">玩家名字</param>
        public async Task PlayerAsync(PlayerObj player)
        {
            MysqlSearchData search = new MysqlSearchData();
            if (await search.PlayerAsync(player.QQ号) != null)
                new MysqlReplaceData().player(player);
            else
            {
                string str = "INSERT INTO {0}(Name,Nick,QQ,Admiin)VALUES('{1}','{2}','{3}','{4}')";
                await Mysql.MysqlSql(string.Format(str, Mysql.MysqlPlayerTable, Funtion.GBKtoUTF8(player.名字), Funtion.GBKtoUTF8(player.昵称), player.QQ号, player.管理员));
            }
        }
        /// <summary>
        /// 添加禁止绑定ID
        /// </summary>
        /// <param name="name">禁止的ID</param>
        public async Task NotIDAsync(string name)
        {
            string str = "INSERT INTO {0}(Name)VALUES('{1}')";
            await Mysql.MysqlSql(string.Format(str, Mysql.MysqlNotIDTable, Funtion.GBKtoUTF8(name.ToLower())));
        }
        public async Task MuteAsync(string name)
        {
            string str = "INSERT INTO {0}(Name)VALUES('{1}')";
            await Mysql.MysqlSql(string.Format(str, Mysql.MysqlMuteTable, Funtion.GBKtoUTF8(name)));
        }
    }
}

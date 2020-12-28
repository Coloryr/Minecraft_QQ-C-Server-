using Minecraft_QQ_Core.Utils;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace Minecraft_QQ_Core.MyMysql
{
    internal class MysqlRemoveData
    {
        public static async Task MuteAsync(string name)
        {
            try
            {
                MySqlCommand cmd = new($"DELETE FROM {Mysql.MysqlMuteTable} WHERE Name=@name");
                cmd.Parameters.AddRange(new MySqlParameter[]
                {
                    new("@name", name)
                });
                await Mysql.MysqlSql(cmd);
            }
            catch (MySqlException ex)
            {
                Logs.LogError("[Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
            }
            Mysql.conn.Close();
        }
    }
}

using Color_yr.Minecraft_QQ.Utils;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace Color_yr.Minecraft_QQ.MyMysql
{
    internal class MysqlRemoveData
    {
        public async Task MuteAsync(string name)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(string.Format("DELETE FROM {0} WHERE Name=@name", Mysql.MysqlMuteTable));
                cmd.Parameters.AddRange(new MySqlParameter[]
                {
                    new MySqlParameter("@name", name)
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

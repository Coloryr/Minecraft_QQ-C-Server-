using Minecraft_QQ_Core.Config;
using Minecraft_QQ_Core.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Minecraft_QQ_Core.MyMysql
{
    public class Mysql
    {
        public static MySqlConnection conn;

        public const string MysqlPlayerTable = "minecraft_qq_player";
        public const string MysqlMuteTable = "minecraft_qq_mute";

        public static void MysqlStart()
        {
            string ConnectString = $"SslMode=none;Server={Minecraft_QQ.MainConfig.数据库.地址};" +
                $"Port={Minecraft_QQ.MainConfig.数据库.端口};User ID={Minecraft_QQ.MainConfig.数据库.用户名};" +
                $"Password={Minecraft_QQ.MainConfig.数据库.密码};Database={Minecraft_QQ.MainConfig.数据库.数据库};Charset=utf8;";
            conn = new MySqlConnection(ConnectString);
            if (conn == null)
            {
                IMinecraft_QQ.ShowMessageCall.Invoke("Mysql错误\n" + ConnectString);
            }
            MysqlAddTable table = new MysqlAddTable();

            if (MysqlAddTable.AddPlayerTable(MysqlPlayerTable) == false) return;
            if (MysqlAddTable.AddOneTable(MysqlMuteTable) == false) return;

            Minecraft_QQ.MysqlOK = true;
        }

        public static void MysqlStop()
        {
            if (conn == null)
                return;
            if (conn.State != ConnectionState.Broken)
                conn.Close();
            Minecraft_QQ.MysqlOK = false;
        }

        public static void Load()
        {
            Task.Run(async () =>
            {
                await LoadPlayerAsync();
                await LoadMuteAsync();
            });
        }

        private static async Task LoadPlayerAsync()
        {
            Minecraft_QQ.PlayerConfig.玩家列表.Clear();
            MySqlCommand cmd = new("SELECT `Name`,`Nick`,`Admin`,`QQ` FROM " + MysqlPlayerTable);
            DbDataReader reader = await MysqlSql(cmd, true);
            while (await reader.ReadAsync())
            {
                PlayerObj player = new PlayerObj
                {
                    名字 = reader.GetString(0),
                    管理员 = reader.GetBoolean(2),
                };
                if (!reader.IsDBNull(1))
                {
                    player.昵称 = reader.GetString(1);
                }
                long.TryParse(reader.GetString(3), out long temp);
                player.QQ号 = temp;
                if (Minecraft_QQ.PlayerConfig.玩家列表.ContainsKey(player.QQ号) == false)
                    Minecraft_QQ.PlayerConfig.玩家列表.Add(player.QQ号, player);
            }
            reader.Close();
            await conn.CloseAsync();
        }
        private static async Task LoadMuteAsync()
        {
            Minecraft_QQ.PlayerConfig.禁言列表.Clear();
            MySqlCommand cmd = new("SELECT `Name` FROM " + MysqlMuteTable);
            DbDataReader reader = await MysqlSql(cmd, true);
            if (reader != null)
                while (await reader.ReadAsync())
                {
                    if (!string.IsNullOrWhiteSpace(reader.GetString(0)))
                        if (Minecraft_QQ.PlayerConfig.禁言列表.Contains(reader.GetString(0).ToLower()) == false)
                            Minecraft_QQ.PlayerConfig.禁言列表.Add(reader.GetString(0).ToLower());
                }
            reader.Close();
            await conn.CloseAsync();
        }
        public static async Task<DbDataReader> MysqlSql(MySqlCommand SQL, bool needRead = false)
        {
            DbDataReader temp = null;
            try
            {
                await conn.OpenAsync();
                SQL.Connection = conn;
                temp = await SQL.ExecuteReaderAsync();
                if (!needRead)
                {
                    await conn.CloseAsync();
                }
            }
            catch (Exception e)
            {
                Logs.LogError(e);
            }
            return temp;
        }
    }
}

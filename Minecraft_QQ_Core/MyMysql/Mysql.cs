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

        public static string MysqlPlayerTable = "minecraft_qq_player";
        public static string MysqlNotIDTable = "minecraft_qq_notid";
        public static string MysqlMuteTable = "minecraft_qq_mute";

        public static void MysqlStart()
        {
            string ConnectString = string.Format("SslMode=none;Server={0};Port={1};User ID={2};Password={3};Database={4};Charset=utf8;",
            Minecraft_QQ.MainConfig.数据库.地址, Minecraft_QQ.MainConfig.数据库.端口, Minecraft_QQ.MainConfig.数据库.用户名,
            Minecraft_QQ.MainConfig.数据库.密码, Minecraft_QQ.MainConfig.数据库.数据库);
            conn = new MySqlConnection(ConnectString);
            if (conn == null)
            {
                IMinecraft_QQ.ShowMessageCall.Invoke("Mysql错误\n" + ConnectString);
            }
            MysqlAddTable table = new MysqlAddTable();

            if (table.AddPlayerTable(MysqlPlayerTable) == false) return;
            if (table.AddOneTable(MysqlNotIDTable) == false) return;
            if (table.AddOneTable(MysqlMuteTable) == false) return;
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

        public void Load()
        {
            Task.Factory.StartNew(async () =>
            {
                await LoadPlayerAsync();
                await LoadMuteAsync();
                await LoadNotIDAsync();
            });
        }

        private async Task LoadPlayerAsync()
        {
            Minecraft_QQ.PlayerConfig.玩家列表.Clear();
            MySqlCommand cmd = new MySqlCommand("SELECT `Name`,`Nick`,`Admin`,`QQ` FROM " + MysqlPlayerTable);
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
        private async Task LoadNotIDAsync()
        {
            Minecraft_QQ.PlayerConfig.禁止绑定列表.Clear();
            MySqlCommand cmd = new MySqlCommand("SELECT `Name` FROM " + MysqlNotIDTable);
            DbDataReader reader = await MysqlSql(cmd, true);
            if (reader != null)
                while (await reader.ReadAsync())
                {
                    if (!string.IsNullOrWhiteSpace(reader.GetString(0)))
                        if (Minecraft_QQ.PlayerConfig.禁止绑定列表.Contains(reader.GetString(0).ToLower()) == false)
                            Minecraft_QQ.PlayerConfig.禁止绑定列表.Add(reader.GetString(0).ToLower());
                }
            reader.Close();
            await conn.CloseAsync();
        }
        private async Task LoadMuteAsync()
        {
            Minecraft_QQ.PlayerConfig.禁言列表.Clear();
            MySqlCommand cmd = new MySqlCommand("SELECT `Name` FROM " + MysqlNotIDTable);
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

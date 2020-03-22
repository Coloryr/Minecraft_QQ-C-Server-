using Color_yr.Minecraft_QQ.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Data.Common;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ.MyMysql
{
    class Mysql
    {
        public static MySqlConnection conn;

        public static string MysqlPlayerTable = "minecraft_qq_player";
        public static string MysqlNotIDTable = "minecraft_qq_notid";
        public static string MysqlMuteTable = "minecraft_qq_mute";

        public static bool MysqlStart()
        {
            string ConnectString = string.Format("SslMode=none;Server={0};Port={1};User ID={2};Password={3};Database={4};Charset=utf8;",
                Minecraft_QQ.MainConfig.数据库.地址, Minecraft_QQ.MainConfig.数据库.端口, Minecraft_QQ.MainConfig.数据库.用户名,
                Minecraft_QQ.MainConfig.数据库.密码, Minecraft_QQ.MainConfig.数据库.数据库);
            conn = new MySqlConnection(ConnectString);
            if (conn == null)
            {
                MessageBox.Show("Mysql错误\n" + ConnectString);
            }
            MysqlAddTable table = new MysqlAddTable();

            if (table.AddPlayerTable(MysqlPlayerTable) == false) return false;
            if (table.AddOneTable(MysqlNotIDTable) == false) return false;
            if (table.AddOneTable(MysqlMuteTable) == false) return false;
            return true;
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
            DbDataReader reader = await MysqlSql(cmd);
            while (await reader.ReadAsync())
            {
                PlayerObj player = new PlayerObj
                {
                    名字 = reader.GetString(0),
                    昵称 = reader.GetString(1),
                    管理员 = reader.GetBoolean(2),
                };
                long.TryParse(reader.GetString(3), out player.QQ号);
                Minecraft_QQ.PlayerConfig.玩家列表.Add(player.QQ号, player);
            }
            reader.Close();
        }
        private async Task LoadNotIDAsync()
        {
            Minecraft_QQ.PlayerConfig.禁止绑定列表.Clear();
            MySqlCommand cmd = new MySqlCommand("SELECT `Name` FROM " + MysqlNotIDTable);
            DbDataReader reader = await MysqlSql(cmd);
            if (reader != null)
                while (await reader.ReadAsync())
                {
                    if (!string.IsNullOrWhiteSpace(reader.GetString(0)))
                        if (Minecraft_QQ.PlayerConfig.禁止绑定列表.Contains(reader.GetString(0).ToLower()) == false)
                            Minecraft_QQ.PlayerConfig.禁止绑定列表.Add(reader.GetString(0).ToLower());
                }
            reader.Close();
        }
        private async Task LoadMuteAsync()
        {
            Minecraft_QQ.PlayerConfig.禁言列表.Clear();
            MySqlCommand cmd = new MySqlCommand("SELECT `Name` FROM " + MysqlNotIDTable);
            DbDataReader reader = await MysqlSql(cmd);
            if (reader != null)
                while (await reader.ReadAsync())
                {
                    if (!string.IsNullOrWhiteSpace(reader.GetString(0)))
                        if (Minecraft_QQ.PlayerConfig.禁言列表.Contains(reader.GetString(0).ToLower()) == false)
                            Minecraft_QQ.PlayerConfig.禁言列表.Add(reader.GetString(0).ToLower());
                }
        }

        public static async Task<DbDataReader> MysqlSql(MySqlCommand SQL)
        {
            DbDataReader temp = null;
            try
            {
                await conn.OpenAsync();
                SQL.Connection = conn;
                await SQL.ExecuteNonQueryAsync();
                temp = await SQL.ExecuteReaderAsync();
            }
            catch (MySqlException ex)
            {
                logs.LogWrite("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
            }
            await conn.CloseAsync();
            return temp;
        }
    }
}

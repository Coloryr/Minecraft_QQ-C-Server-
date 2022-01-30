using Minecraft_QQ_Core.Config;
using Minecraft_QQ_Core.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Minecraft_QQ_Core
{
    public class MyMysql
    {
        public const string MysqlPlayerTable = "minecraft_qq_player";
        public const string MysqlMuteTable = "minecraft_qq_mute";

        private MySqlConnection Conn;
        private readonly Minecraft_QQ Main;
        public MyMysql(Minecraft_QQ Minecraft_QQ)
        {
            Main = Minecraft_QQ;
        }

        public void MysqlStart()
        {
            string ConnectString = $"SslMode=none;Server={Main.MainConfig.Database.IP};" +
                $"Port={Main.MainConfig.Database.Port};User ID={Main.MainConfig.Database.User};" +
                $"Password={Main.MainConfig.Database.Password};Database={Main.MainConfig.Database.Database};Charset=utf8;";
            Conn = new MySqlConnection(ConnectString);
            if (Conn == null)
            {
                IMinecraft_QQ.ShowMessageCall.Invoke("Mysql错误\n" + ConnectString);
                return;
            }
            if (AddPlayerTable(Conn, MysqlPlayerTable) == false) return;
            if (AddOneTable(Conn, MysqlMuteTable) == false) return;

            Main.MysqlOK = true;
        }

        public void MysqlStop()
        {
            if (Conn == null)
                return;
            if (Conn.State != ConnectionState.Broken)
                Conn.Close();
            Main.MysqlOK = false;
        }

        public void Load()
        {
            Task.Run(async () =>
            {
                await LoadPlayerAsync();
                await LoadMuteAsync();
            });
        }

        private async Task LoadPlayerAsync()
        {
            Main.PlayerConfig.PlayerList.Clear();
            MySqlCommand cmd = new("SELECT `Name`,`Nick`,`Admin`,`QQ` FROM " + MysqlPlayerTable);
            DbDataReader reader = await MysqlSql(cmd, true);
            while (await reader.ReadAsync())
            {
                PlayerObj player = new PlayerObj
                {
                    Name = reader.GetString(0),
                    IsAdmin = reader.GetBoolean(2),
                };
                if (!reader.IsDBNull(1))
                {
                    player.Nick = reader.GetString(1);
                }
                long.TryParse(reader.GetString(3), out long temp);
                player.QQ = temp;
                if (Main.PlayerConfig.PlayerList.ContainsKey(player.QQ) == false)
                    Main.PlayerConfig.PlayerList.Add(player.QQ, player);
            }
            reader.Close();
            await Conn.CloseAsync();
        }
        private async Task LoadMuteAsync()
        {
            Main.PlayerConfig.MuteList.Clear();
            MySqlCommand cmd = new("SELECT `Name` FROM " + MysqlMuteTable);
            DbDataReader reader = await MysqlSql(cmd, true);
            if (reader != null)
                while (await reader.ReadAsync())
                {
                    if (!string.IsNullOrWhiteSpace(reader.GetString(0)))
                        if (Main.PlayerConfig.MuteList.Contains(reader.GetString(0).ToLower()) == false)
                            Main.PlayerConfig.MuteList.Add(reader.GetString(0).ToLower());
                }
            reader.Close();
            await Conn.CloseAsync();
        }
        private async Task<DbDataReader> MysqlSql(MySqlCommand SQL, bool needRead = false)
        {
            DbDataReader temp = null;
            try
            {
                await Conn.OpenAsync();
                SQL.Connection = Conn;
                temp = await SQL.ExecuteReaderAsync();
                if (!needRead)
                {
                    await Conn.CloseAsync();
                }
            }
            catch (Exception e)
            {
                Logs.LogError(e);
            }
            return temp;
        }
        /// <summary>
        /// 添加玩家
        /// </summary>
        /// <param name="player">玩家名字</param>
        public async Task AddPlayerAsync(PlayerObj player)
        {
            if (await GetPlayerAsync(player.QQ) != null)
                await UpdatePlayerAsync(player);
            else
            {
                MySqlCommand cmd;
                if (string.IsNullOrWhiteSpace(player.Nick))
                {
                    cmd = new($"INSERT INTO {MysqlPlayerTable}(Name,QQ,Admin)VALUES(@name,@qq,@admin)");
                    cmd.Parameters.AddRange(new MySqlParameter[]
                    {
                        new("@name", Funtion.GBKtoUTF8(player.Name)),
                        new("@admin", player.IsAdmin),
                        new("@qq", player.QQ)
                    });
                }
                else
                {
                    cmd = new($"INSERT INTO {MysqlPlayerTable}(Name,Nick,QQ,Admin)VALUES(@name,@nick,@qq,@admin)");
                    cmd.Parameters.AddRange(new MySqlParameter[]
                    {
                        new("@name", Funtion.GBKtoUTF8(player.Name)),
                        new("@nick", Funtion.GBKtoUTF8(player.Nick)),
                        new("@admin", player.IsAdmin),
                        new("@qq", player.QQ)
                    });
                }
                await MysqlSql(cmd);
            }
            Conn.Close();
        }

        public async Task AddMuteAsync(string name)
        {
            MySqlCommand cmd = new($"INSERT INTO {MysqlMuteTable}(Name)VALUES(@name)");
            cmd.Parameters.AddRange(new MySqlParameter[]
            {
                new("@name", Funtion.GBKtoUTF8(name.ToLower())),
            });
            await MysqlSql(cmd);
            Conn.Close();
        }
        /// <summary>
        /// 添加玩家列表
        /// </summary>
        /// <param name="TableName">表名字</param>
        /// <returns>是否成</returns>
        private static bool AddPlayerTable(MySqlConnection conn, string TableName)
        {
            try
            {
                conn.Open();
                MySqlDataAdapter adp = new();
                DataTable dt = conn.GetSchema();
                MySqlCommand cmd = new("select * from " + TableName, conn);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1146:
                        string mySelectQuery = "CREATE TABLE " + TableName + "( `ID` INT(255) NOT NULL AUTO_INCREMENT COMMENT '自增ID' , `Name` VARCHAR(255) NOT NULL COMMENT '名字' , `Nick` VARCHAR(255) NULL DEFAULT NULL COMMENT '昵称' , `QQ` VARCHAR(255) NOT NULL COMMENT 'QQ号' , `Admin` BOOLEAN NOT NULL COMMENT '管理员' , PRIMARY KEY (`ID`))";
                        MySqlCommand cmd = new(mySelectQuery, conn);
                        cmd.ExecuteNonQuery();
                        break;
                    default:
                        Logs.LogError("[Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
                        conn.Close();
                        return false;
                }
            }
            conn.Close();
            return true;
        }
        /// <summary>
        /// 添加单表格
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <returns>是否成功</returns>
        private static bool AddOneTable(MySqlConnection conn, string TableName)
        {
            try
            {
                conn.Open();
                MySqlDataAdapter adp = new();
                DataTable dt = conn.GetSchema();
                MySqlCommand cmd = new("select * from " + TableName, conn);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1146:
                        string mySelectQuery = "CREATE TABLE " + TableName + "( `ID` INT(255) NOT NULL AUTO_INCREMENT COMMENT '自增ID' , `Name` VARCHAR(255) NOT NULL COMMENT '名字' , PRIMARY KEY (`ID`))";
                        MySqlCommand cmd = new(mySelectQuery, conn);
                        cmd.ExecuteNonQuery();
                        break;
                    default:
                        Logs.LogOut("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
                        conn.Close();
                        return false;
                }
            }
            conn.Close();
            return true;
        }
        public async Task DeleteMuteAsync(string name)
        {
            try
            {
                MySqlCommand cmd = new($"DELETE FROM {MysqlMuteTable} WHERE Name=@name");
                cmd.Parameters.AddRange(new MySqlParameter[]
                {
                    new("@name", name)
                });
                await MysqlSql(cmd);
            }
            catch (MySqlException ex)
            {
                Logs.LogError("[Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
            }
            Conn.Close();
        }
        private async Task UpdatePlayerAsync(PlayerObj player)
        {
            try
            {
                MySqlCommand cmd = new($"UPDATE {MysqlPlayerTable} SET Name=@name,Nick=@nick,Admin=@admin WHERE QQ=@qq");
                cmd.Parameters.AddRange(new MySqlParameter[]
                {
                    new("@name", Funtion.GBKtoUTF8(player.Name)),
                    new("@nick", Funtion.GBKtoUTF8(player.Nick)),
                    new("@admin", player.IsAdmin),
                    new("@qq", player.QQ)
                });
                await MysqlSql(cmd);
            }
            catch (Exception e)
            {
                Logs.LogError(e);
            }
            Conn.Close();
        }
        private async Task<PlayerObj> GetPlayerAsync(long qq)
        {
            PlayerObj player = null;
            MySqlCommand cmd = new($"SELECT `Name`,`Nick`,`Admin` FROM {MysqlPlayerTable} WHERE QQ=@qq");
            cmd.Parameters.AddRange(new MySqlParameter[]
            {
                new("@qq", qq)
            });
            var item = await MysqlSql(cmd, true);
            if (item != null && item.HasRows)
            {
                player.Name = item.GetString(0);
                player.Nick = item.GetString(1);
                player.QQ = qq;
                player.IsAdmin = item.GetBoolean(2);
                item.Close();
            }
            await item.CloseAsync();
            Conn.Close();
            return player;
        }
    }
}

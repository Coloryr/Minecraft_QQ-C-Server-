using Minecraft_QQ_Core.Config;
using Minecraft_QQ_Core.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;
using System.Linq;
using Dapper;

namespace Minecraft_QQ_Core
{
    public class MyMysql
    {
        public const string MysqlPlayerTable = "minecraft_qq_player";
        public const string MysqlMuteTable = "minecraft_qq_mute";
        public const string MysqlNotIDTable = "minecraft_qq_notid";

        private string ConnectString;
        private readonly Minecraft_QQ Main;
        public MyMysql(Minecraft_QQ Minecraft_QQ)
        {
            Main = Minecraft_QQ;
        }
        /// <summary>
        /// 数据库启动
        /// </summary>
        public void MysqlStart()
        {
            ConnectString = $"SslMode=none;Server={Main.MainConfig.Database.IP};" +
                $"Port={Main.MainConfig.Database.Port};User ID={Main.MainConfig.Database.User};" +
                $"Password={Main.MainConfig.Database.Password};Database={Main.MainConfig.Database.Database};Charset=utf8;";
            InitPlayerTable();
            InitMuteTable();
            InitNotIDTable();

            Main.MysqlOK = true;
        }
        /// <summary>
        /// 数据库关闭
        /// </summary>
        public void MysqlStop()
        {
            MySqlConnection.ClearAllPools();
            Main.MysqlOK = false;
        }

        /// <summary>
        /// 添加玩家列表
        /// </summary>
        /// <param name="TableName">表名字</param>
        /// <returns>是否成</returns>
        private void InitPlayerTable()
        {
            try
            {
                var Conn = new MySqlConnection(ConnectString);
                var read = Conn.Query($"show tables like '{MysqlPlayerTable}'");
                if (!read.Any())
                {
                    Logs.LogOut($"[Mysql]不存在数据表{MysqlPlayerTable}，正在创建");
                    Conn.Execute($"CREATE TABLE {MysqlPlayerTable} ( `ID` INT(20) NOT NULL AUTO_INCREMENT , `Name` VARCHAR(255) NOT NULL COMMENT 'ID' , `Nick` VARCHAR(255) NULL COMMENT '昵称' , `QQ` BIGINT NOT NULL COMMENT 'QQ号' , `IsAdmin` TINYINT(1) NOT NULL COMMENT '是否为管理员' , `createtime` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP , `updatetime` TIMESTAMP on update CURRENT_TIMESTAMP NULL , PRIMARY KEY (`ID`)) ENGINE = MyISAM COMMENT = '玩家储存';");
                }
            }
            catch (Exception e)
            {
                Logs.LogError("[Mysql]数据库操作错误", e);
            }
        }
        /// <summary>
        /// 添加禁言表格
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <returns>是否成功</returns>
        private void InitMuteTable()
        {
            try
            {
                var Conn = new MySqlConnection(ConnectString);
                var read = Conn.Query($"show tables like '{MysqlMuteTable}'");
                if (!read.Any())
                {
                    Logs.LogOut($"[Mysql]不存在数据表{MysqlMuteTable}，正在创建");
                    Conn.Execute($"CREATE TABLE {MysqlMuteTable} ( `ID` INT(20) NOT NULL AUTO_INCREMENT , `Name` VARCHAR(255) NOT NULL COMMENT '名字' , `createtime` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP , PRIMARY KEY (`ID`)) ENGINE = MyISAM COMMENT = '禁言ID';");
                }
            }
            catch (Exception e)
            {
                Logs.LogError("[Mysql]数据库操作错误", e);
            }
        }
        /// <summary>
        /// 添加禁言表格
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <returns>是否成功</returns>
        private void InitNotIDTable()
        {
            try
            {
                var Conn = new MySqlConnection(ConnectString);
                var read = Conn.Query($"show tables like '{MysqlNotIDTable}'");
                if (!read.Any())
                {
                    Logs.LogOut($"[Mysql]不存在数据表{MysqlNotIDTable}，正在创建");
                    Conn.Execute($"CREATE TABLE {MysqlNotIDTable} ( `ID` INT(20) NOT NULL AUTO_INCREMENT , `Name` VARCHAR(255) NOT NULL COMMENT '名字' , `createtime` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP , PRIMARY KEY (`ID`)) ENGINE = MyISAM COMMENT = '禁言ID';");
                }
            }
            catch (Exception e)
            {
                Logs.LogError("[Mysql]数据库操作错误", e);
            }
        }

        /// <summary>
        /// 读取所有数据
        /// </summary>
        /// <returns></returns>
        public void Load()
        {
            LoadPlayerAsync();
            LoadMuteAsync();
            LoadNotIDAsync();
        }

        /// <summary>
        /// 读取玩家数据
        /// </summary>
        /// <returns></returns>
        private void LoadPlayerAsync()
        {
            Main.PlayerConfig.PlayerList.Clear();
            var Conn = new MySqlConnection(ConnectString);
            var list = Conn.Query<PlayerObj>($"SELECT `Name`,`Nick`,`IsAdmin`,`QQ` FROM {MysqlPlayerTable}");

            foreach (var item in list)
            {
                Main.PlayerConfig.PlayerList.Add(item.QQ, item);
            }
        }

        public record Obj2
        {
            public string Name { get; set; }
        }
        /// <summary>
        /// 读取禁言数据
        /// </summary>
        /// <returns></returns>
        private void LoadMuteAsync()
        {
            Main.PlayerConfig.MuteList.Clear();
            var Conn = new MySqlConnection(ConnectString);
            var list = Conn.Query<Obj2>($"SELECT `Name` FROM {MysqlMuteTable}");

            foreach (var item in list)
            {
                if (!string.IsNullOrWhiteSpace(item.Name))
                    if (Main.PlayerConfig.MuteList.Contains(item.Name.ToLower()) == false)
                        Main.PlayerConfig.MuteList.Add(item.Name.ToLower());
            }
        }
        /// <summary>
        /// 读取禁止绑定列表
        /// </summary>
        /// <returns></returns>
        private void LoadNotIDAsync()
        {
            Main.PlayerConfig.NotBindList.Clear();
            var Conn = new MySqlConnection(ConnectString);
            var list = Conn.Query<Obj2>($"SELECT `Name` FROM {MysqlNotIDTable}");

            foreach (var item in list)
            {
                if (!string.IsNullOrWhiteSpace(item.Name))
                    if (Main.PlayerConfig.NotBindList.Contains(item.Name.ToLower()) == false)
                        Main.PlayerConfig.NotBindList.Add(item.Name.ToLower());
            }
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
                var Conn = new MySqlConnection(ConnectString);
                await Conn.ExecuteAsync($"INSERT INTO {MysqlPlayerTable}(Name,QQ,IsAdmin,Nick)VALUES(@Name,@QQ,@IsAdmin,@Nick)", player);
            }
        }

        public async Task AddMuteAsync(string name)
        {
            var Conn = new MySqlConnection(ConnectString);
            await Conn.ExecuteAsync($"INSERT INTO {MysqlMuteTable}(Name)VALUES(@name)", new { name});
        }
        
        public async Task DeleteMuteAsync(string name)
        {
            try
            {
                var Conn = new MySqlConnection(ConnectString);
                await Conn.ExecuteAsync($"DELETE FROM {MysqlMuteTable} WHERE Name=@name", new { name });
            }
            catch (MySqlException ex)
            {
                Logs.LogError("[Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
            }
        }
        private async Task UpdatePlayerAsync(PlayerObj player)
        {
            try
            {
                var Conn = new MySqlConnection(ConnectString);
                await Conn.ExecuteAsync($"UPDATE {MysqlPlayerTable} SET Name=@Name,Nick=@Nick,IsAdmin=@IsAdmin WHERE QQ=@QQ", player);
            }
            catch (Exception e)
            {
                Logs.LogError(e);
            }
        }
        private async Task<PlayerObj> GetPlayerAsync(long qq)
        {
            var Conn = new MySqlConnection(ConnectString);
            var list = await Conn.QueryAsync<PlayerObj>($"SELECT `Name`,`Nick`,`IsAdmin`,`QQ` FROM {MysqlPlayerTable} WHERE QQ=@qq", new { qq });

            if (list.Any())
                return list.First();
            return null;
        }
    }
}

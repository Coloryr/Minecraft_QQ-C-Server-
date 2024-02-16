using Minecraft_QQ_Core.Config;
using Minecraft_QQ_Core.Utils;
using System;
using System.Threading.Tasks;
using System.Linq;
using Dapper;
using MySqlConnector;

namespace Minecraft_QQ_Core;

public static class MyMysql
{
    public const string MysqlPlayerTable = "minecraft_qq_player";
    public const string MysqlMuteTable = "minecraft_qq_mute";
    public const string MysqlNotIDTable = "minecraft_qq_notid";

    private static string ConnectString;

    /// <summary>
    /// 数据库启动
    /// </summary>
    public static void MysqlStart()
    {
        ConnectString = Minecraft_QQ.MainConfig.Database.Url;

        Minecraft_QQ.MysqlOK = InitPlayerTable() && InitMuteTable() && InitNotIDTable();
    }
    /// <summary>
    /// 数据库关闭
    /// </summary>
    public static void MysqlStop()
    {
        MySqlConnection.ClearAllPools();
        Minecraft_QQ.MysqlOK = false;
    }

    /// <summary>
    /// 添加玩家列表
    /// </summary>
    /// <param name="TableName">表名字</param>
    /// <returns>是否成</returns>
    private static bool InitPlayerTable()
    {
        try
        {
            using var conn = new MySqlConnection(ConnectString);
            var read = conn.Query($"show tables like '{MysqlPlayerTable}'");
            if (!read.Any())
            {
                Logs.LogOut($"[Mysql]不存在数据表{MysqlPlayerTable}，正在创建");
                conn.Execute($"CREATE TABLE {MysqlPlayerTable} ( " +
                    $"`ID` INT(20) NOT NULL AUTO_INCREMENT , " +
                    $"`Name` VARCHAR(255) NOT NULL COMMENT 'ID' , " +
                    $"`Nick` VARCHAR(255) NULL COMMENT '昵称' , " +
                    $"`QQ` BIGINT NOT NULL COMMENT 'QQ号' , " +
                    $"`IsAdmin` TINYINT(1) NOT NULL COMMENT '是否为管理员' , " +
                    $"`createtime` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP , " +
                    $"`updatetime` TIMESTAMP on update CURRENT_TIMESTAMP NULL , " +
                    $"PRIMARY KEY (`ID`)) ENGINE = MyISAM COMMENT = '玩家储存';");
            }

            return true;
        }
        catch (Exception e)
        {
            Logs.LogError("[Mysql]数据库操作错误", e);
        }
        return false;
    }
    /// <summary>
    /// 添加禁言表格
    /// </summary>
    /// <param name="TableName">表名</param>
    /// <returns>是否成功</returns>
    private static bool InitMuteTable()
    {
        try
        {
            using var conn = new MySqlConnection(ConnectString);
            var read = conn.Query($"show tables like '{MysqlMuteTable}'");
            if (!read.Any())
            {
                Logs.LogOut($"[Mysql]不存在数据表{MysqlMuteTable}，正在创建");
                conn.Execute($"CREATE TABLE {MysqlMuteTable} ( " +
                    $"`ID` INT(20) NOT NULL AUTO_INCREMENT , " +
                    $"`Name` VARCHAR(255) NOT NULL COMMENT '名字' , " +
                    $"`createtime` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP , " +
                    $"PRIMARY KEY (`ID`)) ENGINE = MyISAM COMMENT = '禁言ID';");
            }
            return true;
        }
        catch (Exception e)
        {
            Logs.LogError("[Mysql]数据库操作错误", e);
        }
        return false;
    }
    /// <summary>
    /// 添加禁言表格
    /// </summary>
    /// <param name="TableName">表名</param>
    /// <returns>是否成功</returns>
    private static bool InitNotIDTable()
    {
        try
        {
            using var conn = new MySqlConnection(ConnectString);
            var read = conn.Query($"show tables like '{MysqlNotIDTable}'");
            if (!read.Any())
            {
                Logs.LogOut($"[Mysql]不存在数据表{MysqlNotIDTable}，正在创建");
                conn.Execute($"CREATE TABLE {MysqlNotIDTable} ( " +
                    $"`ID` INT(20) NOT NULL AUTO_INCREMENT , " +
                    $"`Name` VARCHAR(255) NOT NULL COMMENT '名字' , " +
                    $"`createtime` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP , " +
                    $"PRIMARY KEY (`ID`)) ENGINE = MyISAM COMMENT = '禁言ID';");
            }
            return true;
        }
        catch (Exception e)
        {
            Logs.LogError("[Mysql]数据库操作错误", e);
        }
        return false;
    }

    /// <summary>
    /// 读取所有数据
    /// </summary>
    /// <returns></returns>
    public static void Load()
    {
        LoadPlayerAsync();
        LoadMuteAsync();
        LoadNotIDAsync();
    }

    /// <summary>
    /// 读取玩家数据
    /// </summary>
    /// <returns></returns>
    private static void LoadPlayerAsync()
    {
        Minecraft_QQ.PlayerConfig.PlayerList.Clear();
        using var conn = new MySqlConnection(ConnectString);
        var list = conn.Query<PlayerObj>($"SELECT `Name`,`Nick`,`IsAdmin`,`QQ` FROM {MysqlPlayerTable}");

        foreach (var item in list)
        {
            Minecraft_QQ.PlayerConfig.PlayerList.Add(item.QQ, item);
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
    private static void LoadMuteAsync()
    {
        Minecraft_QQ.PlayerConfig.MuteList.Clear();
        using var conn = new MySqlConnection(ConnectString);
        var list = conn.Query<Obj2>($"SELECT `Name` FROM {MysqlMuteTable}");

        foreach (var item in list)
        {
            if (!string.IsNullOrWhiteSpace(item.Name))
                if (Minecraft_QQ.PlayerConfig.MuteList.Contains(item.Name.ToLower()) == false)
                    Minecraft_QQ.PlayerConfig.MuteList.Add(item.Name.ToLower());
        }
    }
    /// <summary>
    /// 读取禁止绑定列表
    /// </summary>
    /// <returns></returns>
    private static void LoadNotIDAsync()
    {
        Minecraft_QQ.PlayerConfig.NotBindList.Clear();
        using var conn = new MySqlConnection(ConnectString);
        var list = conn.Query<Obj2>($"SELECT `Name` FROM {MysqlNotIDTable}");

        foreach (var item in list)
        {
            if (!string.IsNullOrWhiteSpace(item.Name))
                if (Minecraft_QQ.PlayerConfig.NotBindList.Contains(item.Name.ToLower()) == false)
                    Minecraft_QQ.PlayerConfig.NotBindList.Add(item.Name.ToLower());
        }
    }

    /// <summary>
    /// 添加玩家
    /// </summary>
    /// <param name="player">玩家名字</param>
    public static async Task AddPlayerAsync(PlayerObj player)
    {
        if (await GetPlayerAsync(player.QQ) != null)
            await UpdatePlayerAsync(player);
        else
        {
            using var conn = new MySqlConnection(ConnectString);
            await conn.ExecuteAsync($"INSERT INTO {MysqlPlayerTable}(Name,QQ,IsAdmin,Nick)VALUES(@Name,@QQ,@IsAdmin,@Nick)", player);
        }
    }

    public static async Task AddMuteAsync(string name)
    {
        var Conn = new MySqlConnection(ConnectString);
        await Conn.ExecuteAsync($"INSERT INTO {MysqlMuteTable}(Name)VALUES(@name)", new { name});
    }
    
    public static async Task DeleteMuteAsync(string name)
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
    private static async Task UpdatePlayerAsync(PlayerObj player)
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
    private static async Task<PlayerObj?> GetPlayerAsync(long qq)
    {
        var Conn = new MySqlConnection(ConnectString);
        var list = await Conn.QueryAsync<PlayerObj>($"SELECT `Name`,`Nick`,`IsAdmin`,`QQ` FROM {MysqlPlayerTable} WHERE QQ=@qq", new { qq });

        if (list.Any())
            return list.First();
        return null;
    }
}

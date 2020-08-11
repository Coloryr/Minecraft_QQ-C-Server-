using Minecraft_QQ_Core.Utils;
using MySql.Data.MySqlClient;
using System.Data;

namespace Minecraft_QQ_Core.MyMysql
{
    internal class MysqlAddTable
    {
        /// <summary>
        /// 添加玩家列表
        /// </summary>
        /// <param name="TableName">表名字</param>
        /// <returns>是否成</returns>
        public bool AddPlayerTable(string TableName)
        {
            try
            {
                Mysql.conn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter();
                DataTable dt = Mysql.conn.GetSchema();
                MySqlCommand cmd = new MySqlCommand("select * from " + TableName, Mysql.conn);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1146:
                        string mySelectQuery = "CREATE TABLE " + TableName + "( `ID` INT(255) NOT NULL AUTO_INCREMENT COMMENT '自增ID' , `Name` VARCHAR(255) NOT NULL COMMENT '名字' , `Nick` VARCHAR(255) NULL DEFAULT NULL COMMENT '昵称' , `QQ` VARCHAR(255) NOT NULL COMMENT 'QQ号' , `Admin` BOOLEAN NOT NULL COMMENT '管理员' , PRIMARY KEY (`ID`))";
                        MySqlCommand cmd = new MySqlCommand(mySelectQuery, Mysql.conn);
                        cmd.ExecuteNonQuery();
                        break;
                    default:
                        Logs.LogError("[Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
                        Mysql.conn.Close();
                        return false;
                }
            }
            Mysql.conn.Close();
            return true;
        }
        /// <summary>
        /// 添加单表格
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <returns>是否成功</returns>
        public bool AddOneTable(string TableName)
        {
            try
            {
                Mysql.conn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter();
                DataTable dt = Mysql.conn.GetSchema();
                MySqlCommand cmd = new MySqlCommand("select * from " + TableName, Mysql.conn);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1146:
                        string mySelectQuery = "CREATE TABLE " + TableName + "( `ID` INT(255) NOT NULL AUTO_INCREMENT COMMENT '自增ID' , `Name` VARCHAR(255) NOT NULL COMMENT '名字' , PRIMARY KEY (`ID`))";
                        MySqlCommand cmd = new MySqlCommand(mySelectQuery, Mysql.conn);
                        cmd.ExecuteNonQuery();
                        break;
                    default:
                        Logs.LogOut("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
                        Mysql.conn.Close();
                        return false;
                }
            }
            Mysql.conn.Close();
            return true;
        }
    }
}

using MySql.Data.MySqlClient;
using System.Data;

namespace Color_yr.Minecraft_QQ
{
    class Mysql_Add_table
    {

        /// <summary>
        /// 添加玩家列表
        /// </summary>
        /// <param name="table_name">表名字</param>
        /// <returns>是否成</returns>
        public bool player(string table_name)
        {
            try
            {
                Mysql.conn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter();
                DataTable dt = Mysql.conn.GetSchema();
                MySqlCommand cmd = new MySqlCommand("select * from " + table_name, Mysql.conn);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1146:
                        string mySelectQuery = "CREATE TABLE " + table_name + "(id VARCHAR(20),nick VARCHAR(20),qq VARCHAR(20),admin VARCHAR(20))";
                        MySqlCommand cmd = new MySqlCommand(mySelectQuery, Mysql.conn);
                        cmd.ExecuteNonQuery();
                        break;
                    default:
                        logs.Log_write("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
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
        /// <param name="table_name">表名</param>
        /// <returns>是否成功</returns>
        public bool only(string table_name)
        {
            try
            {
                Mysql.conn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter();
                DataTable dt = Mysql.conn.GetSchema();
                MySqlCommand cmd = new MySqlCommand("select * from " + table_name, Mysql.conn);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1146:
                        string mySelectQuery = "CREATE TABLE " + table_name + "(name VARCHAR(20))";
                        MySqlCommand cmd = new MySqlCommand(mySelectQuery, Mysql.conn);
                        cmd.ExecuteNonQuery();
                        break;
                    default:
                        logs.Log_write("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
                        Mysql.conn.Close();
                        return false;
                }
            }
            Mysql.conn.Close();
            return true;
        }
    }
}

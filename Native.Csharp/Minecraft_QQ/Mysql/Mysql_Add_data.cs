using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Color_yr.Minecraft_QQ
{
    class Mysql_Add_data
    {
        private string GBK_UTF8(string msg)
        {
            try
            {
                byte[] srcBytes = Encoding.Default.GetBytes(msg);
                byte[] bytes = Encoding.Convert(Encoding.Default, Encoding.UTF8, srcBytes);
                return Encoding.UTF8.GetString(bytes);
            }
            catch 
            {
                return msg;
            }
        }

        /// <summary>
        /// 添加玩家
        /// </summary>
        /// <param name="player">玩家名字</param>
        public void player(player_save player)
        {
            Mysql_search_data search = new Mysql_search_data();
            if (search.player(player.qq) != null)
            {
                new Mysql_replace_data().player(player);
            }
            else
            {
                try
                {
                    Mysql.conn.Open();
                    MySqlCommand command = Mysql.conn.CreateCommand();
                    string str = "INSERT INTO {0}(id,nick,qq,admin)VALUES('{1}','{2}','{3}','{4}')";
                    command.CommandText = string.Format(str, Mysql.Mysql_player, GBK_UTF8(player.player), GBK_UTF8(player.nick), player.qq, player.admin);
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    logs.Log_write("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
                }
                Mysql.conn.Close();
            }
        }
        /// <summary>
        /// 添加禁止绑定ID
        /// </summary>
        /// <param name="name">禁止的ID</param>
        public void notid(string name)
        {
            try
            {
                Mysql.conn.Open();
                MySqlCommand command = Mysql.conn.CreateCommand();
                string str = "INSERT INTO {0}(name)VALUES('{1}')";
                command.CommandText = string.Format(str, Mysql.Mysql_notid, GBK_UTF8(name.ToLower()));
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                logs.Log_write("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
            }
            Mysql.conn.Close();
        }
        public void mute(string name)
        {
            try
            {
                Mysql.conn.Open();
                MySqlCommand command = Mysql.conn.CreateCommand();
                string str = "INSERT INTO {0}(name)VALUES('{1}')";
                command.CommandText = string.Format(str, Mysql.Mysql_mute, GBK_UTF8(name));
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                logs.Log_write("[ERROR][Mysql]错误ID：" + ex.Number + "\n" + ex.Message);
            }
            Mysql.conn.Close();
        }
    }
}

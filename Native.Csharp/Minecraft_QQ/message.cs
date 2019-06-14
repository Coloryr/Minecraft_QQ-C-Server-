using Native.Csharp.App;
using Newtonsoft.Json.Linq;

namespace Color_yr.Minecraft_QQ
{
    class message
    {

        public static messagelist Message_re(string read)
        {
            messagelist messagelist = new messagelist();
            try
            {
                JObject jsonData = JObject.Parse(read);
                if (jsonData["data"].ToString() == "data")
                {
                    messagelist.group = jsonData["group"].ToString();
                    messagelist.message = jsonData["message"].ToString();
                    messagelist.is_commder = false;
                }
            }
            catch { }
            return messagelist;
        }

        public static void Message(string read)
        {
            while (read.IndexOf(config_read.data_Head) == 0 && read.IndexOf(config_read.data_End) != -1)
            {
                use use = new use();
                string buff = use.get_string(read, config_read.data_Head, config_read.data_End);
                buff = use.RemoveColorCodes(buff);
                messagelist messagelist = Message_re(buff);
                if (messagelist.is_commder == false && messagelist.group == "group")
                {
                    Common.CqApi.SendGroupMessage(config_read.GroupSet1, messagelist.message);
                    if (config_read.GroupSet2 != 0 && config_read.group2_mode == true)
                        Common.CqApi.SendGroupMessage(config_read.GroupSet2, messagelist.message);
                    if (config_read.GroupSet3 != 0 && config_read.group2_mode == true)
                        Common.CqApi.SendGroupMessage(config_read.GroupSet3, messagelist.message);
                }
                else if (messagelist.group == config_read.GroupSet1.ToString())
                    Common.CqApi.SendGroupMessage(config_read.GroupSet1, messagelist.message);
                else if (messagelist.group == config_read.GroupSet2.ToString())
                    Common.CqApi.SendGroupMessage(config_read.GroupSet2, messagelist.message);
                else if (messagelist.group == config_read.GroupSet3.ToString())
                    Common.CqApi.SendGroupMessage(config_read.GroupSet3, messagelist.message);
                int i = read.IndexOf(config_read.data_End);
                read = read.Substring(i + config_read.data_End.Length);
            }
        }
    }
}

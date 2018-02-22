using System;
using System.Text;

namespace yan_color.Minecraft_QQ
{
	public static class Extensions
	{
		public static string RemoveColorCodes(this string text)
		{
			if(!text.Contains("§"))

            {
				return text;
			}
			
			var sb = new StringBuilder(text);
			sb.Replace("§0", string.Empty);
			sb.Replace("§1", string.Empty);
			sb.Replace("§2", string.Empty);
			sb.Replace("§3", string.Empty);
			sb.Replace("§4", string.Empty);
			sb.Replace("§5", string.Empty);
			sb.Replace("§6", string.Empty);
			sb.Replace("§7", string.Empty);
			sb.Replace("§8", string.Empty);
			sb.Replace("§9", string.Empty);
			sb.Replace("§a", string.Empty);
			sb.Replace("§b", string.Empty);
			sb.Replace("§c", string.Empty);
			sb.Replace("§d", string.Empty);
			sb.Replace("§e", string.Empty);
			sb.Replace("§f", string.Empty);
            sb.Replace("§r", string.Empty);

            return sb.ToString();
		}
	}
}

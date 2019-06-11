using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Native.Csharp.App.Model
{
	public class PrivateMessageEventArgs : EventArgsBase
	{
		/// <summary>
		/// 消息ID
		/// </summary>
		public int MsgId { get; set; }
		/// <summary>
		/// 消息内容
		/// </summary>
		public string Msg { get; set; }
	}
}

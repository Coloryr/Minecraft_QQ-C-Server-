﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Native.Csharp.App.Model;

namespace Native.Csharp.App.Interface
{
	/// <summary>
	/// 酷Q 讨论组消息接口
	/// </summary>
	public interface IEvent_DiscussMessage
	{
		/// <summary>
		/// Type=4 讨论组消息 <para/>
		/// 当在派生类中重写时, 处理收到的讨论组消息
		/// </summary>
		/// <param name="sender">事件的触发对象</param>
		/// <param name="e">事件的附加参数</param>
		void ReceiveDiscussMessage (object sender, DiscussMessageEventArgs e);

		/// <summary>
		/// Type=21 讨论组私聊消息 <para/>
		/// 当在派生类中重写时, 处理收到的讨论组私聊消息
		/// </summary>
		/// <param name="sender">事件的触发对象</param>
		/// <param name="e">事件的附加参数</param>
		void ReceiveDiscussPrivateMessage (object sender, PrivateMessageEventArgs e);
	}
}

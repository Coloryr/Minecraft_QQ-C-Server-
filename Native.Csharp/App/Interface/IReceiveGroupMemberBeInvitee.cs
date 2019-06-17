﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Native.Csharp.App.EventArgs;

namespace Native.Csharp.App.Interface
{
	/// <summary>
	/// Type=103 群事件 - 群成员增加 - 被邀入群, 事件接口
	/// </summary>
	public interface IReceiveGroupMemberBeInvitee
	{
		/// <summary>
		/// 当在派生类中重写时, 处理收到的群成员增加 (被邀入群) 事件
		/// </summary>
		/// <param name="sender">事件的触发对象</param>
		/// <param name="e">事件的附加参数</param>
		void ReceiveGroupMemberBeInvitee (object sender, CqGroupMemberIncreaseEventArgs e);
	}
}

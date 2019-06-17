﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.Csharp.App.EventArgs
{
	/// <summary>
	/// 表示群成员增加事件参数的类
	/// </summary>
	public class CqGroupMemberIncreaseEventArgs : CqEventArgsBase
	{
		/// <summary>
		/// 获取一个值, 该值表示当前事件的类型
		/// </summary>
		public override int Type { get { return 103; } }

		/// <summary>
		/// 获取一个值, 该值表示当前事件的名称
		/// </summary>
		public override string Name { get { return "群成员增加"; } }

		/// <summary>
		/// 获取当前事件触发时间
		/// </summary>
		public DateTime SendTime { get; private set; }

		/// <summary>
		/// 获取当前消息的来源群组号
		/// </summary>
		public long FromGroup { get; private set; }

		/// <summary>
		/// 获取当前消息的来源QQ号
		/// </summary>
		public long FromQQ { get; private set; }

		/// <summary>
		/// 获取当前事件触发时的目标QQ
		/// </summary>
		public long BeingOperateQQ { get; private set; }

        /// <summary>
        /// 获取或设置一个值, 指示当前是否处理过此事件. 若此值为 True 将停止处理后续事件
        /// </summary>
        public bool Handler { get; set; }

        /// <summary>
        /// 初始化 <see cref="CqGroupMemberIncreaseEventArgs"/> 类的一个新实例
        /// </summary>
        /// <param name="id">事件ID</param>
        /// <param name="sendTime">发送时间</param>
        /// <param name="fromGroup">来源群</param>
        /// <param name="fromQQ">操作者QQ</param>
        /// <param name="operateQQ">被操作QQ</param>
        public CqGroupMemberIncreaseEventArgs (int id, DateTime sendTime, long fromGroup, long fromQQ, long operateQQ)
		{
			this.Id = id;
			this.SendTime = sendTime;
			this.FromGroup = fromGroup;
			this.FromQQ = fromQQ;
			this.BeingOperateQQ = operateQQ;
		}
	}
}

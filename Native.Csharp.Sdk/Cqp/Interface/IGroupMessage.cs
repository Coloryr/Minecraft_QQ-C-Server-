﻿using Native.Csharp.Sdk.Cqp.EventArgs;

namespace Native.Csharp.Sdk.Cqp.Interface
{
    /// <summary>
    /// 酷Q群聊消息接口
    /// <para/>
    /// Type: 2
    /// </summary>
    public interface IGroupMessage
    {
        /// <summary>
        /// 当在派生类中重写时, 处理 酷Q群聊消息 回调
        /// </summary>
        /// <param name="sender">事件来源对象</param>
        /// <param name="e">附加的事件参数</param>
        void GroupMessage(object sender, CQGroupMessageEventArgs e);
    }
}

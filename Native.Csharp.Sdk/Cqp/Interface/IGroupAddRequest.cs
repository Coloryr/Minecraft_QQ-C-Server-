using Native.Csharp.Sdk.Cqp.EventArgs;

namespace Native.Csharp.Sdk.Cqp.Interface
{
    /// <summary>
    /// 酷Q群添加请求事件接口
    /// <para/>
    /// Type: 302
    /// </summary>
    public interface IGroupAddRequest
    {
        /// <summary>
        /// 当在派生类中重写时, 处理 酷Q群添加请求事件 回调
        /// </summary>
        /// <param name="sender">事件来源对象</param>
        /// <param name="e">附加的事件参数</param>
        void GroupAddRequest(object sender, CQGroupAddRequestEventArgs e);
    }
}

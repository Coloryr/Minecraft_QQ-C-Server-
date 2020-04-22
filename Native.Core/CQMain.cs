using Minecraft_QQ;
using Native.Core.Domain;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Unity;

namespace Native.Core
{
    /// <summary>
    /// 酷Q应用主入口类
    /// </summary>
    public class CQMain
	{
		/// <summary>
		/// 在应用被加载时将调用此方法进行事件注册, 请在此方法里向 <see cref="IUnityContainer"/> 容器中注册需要使用的事件
		/// </summary>
		/// <param name="container">用于注册的 IOC 容器 </param>
		public static void Register(IUnityContainer unityContainer)
		{
			unityContainer.RegisterType<IGroupMessage, Event_GroupMessage>("群消息处理");
			unityContainer.RegisterType<IPrivateMessage, Event_PrivateMessage>("私聊消息处理");
			unityContainer.RegisterType<IAppDisable, Event_AppDisable>("应用将被停用");
			unityContainer.RegisterType<IAppEnable, Event_AppEnable>("应用已被启用");
			unityContainer.RegisterType<IMenuCall, Event_MenuCall>("打开设置");
		}
	}
	public class Event_MenuCall : IMenuCall
	{
		public void MenuCall(object sender, CQMenuCallEventArgs e)
		{
			IMinecraft_QQ.Menu();
		}
	}
	public class Event_GroupMessage : IGroupMessage
	{
		public void GroupMessage(object sender, CQGroupMessageEventArgs e)
		{
			IMinecraft_QQ.RGroupMessage(e.FromGroup.Id, e.FromQQ.Id, e.Message.ToSendString());
		}
	}
	public class Event_PrivateMessage : IPrivateMessage
	{
		public void PrivateMessage(object sender, CQPrivateMessageEventArgs e)
		{
			IMinecraft_QQ.RPrivateMessage(e.FromQQ.Id, e.Message.Text);
		}
	}
	public class Event_AppDisable : IAppDisable
	{
		public void AppDisable(object sender, CQAppDisableEventArgs e)
		{
			IMinecraft_QQ.Stop();
		}
	}
	public class Event_AppEnable : IAppEnable
	{
		public void AppEnable(object sender, CQAppEnableEventArgs e)
		{
			IMinecraft_QQ.Api = AppData.CQApi;
			IMinecraft_QQ.Start();
		}
	}
}

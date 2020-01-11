using Color_yr.Minecraft_QQ.Event;
using Color_yr.Minecraft_QQ.UI;
using Native.Csharp.Sdk.Cqp.Interface;
using Unity;

namespace Native.Csharp.App
{
    /// <summary>
    /// 酷Q应用主入口类
    /// </summary>
    public static class CQMain
    {
        /// <summary>
        /// 在应用被加载时将调用此方法进行事件注册, 请在此方法里向 <see cref="IUnityContainer"/> 容器中注册需要使用的事件
        /// </summary>
        /// <param name="container">用于注册的 IOC 容器 </param>
        public static void Register(IUnityContainer container)
        {
            container.RegisterType<IGroupMessage, Event_GroupMessage>("群消息处理");
            container.RegisterType<IPrivateMessage, Event_PrivateMessage>("私聊消息处理");

            container.RegisterType<IAppEnable, Event_AppEnable>("应用已被启用");
            container.RegisterType<IAppDisable, Event_AppDisable>("应用将被停用");

            container.RegisterType<IMenuCall, Event_MenuCall>("打开设置");
        }
    }
}

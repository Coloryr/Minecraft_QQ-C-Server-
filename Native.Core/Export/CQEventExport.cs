/*
 * 此文件由T4引擎自动生成, 请勿修改此文件中的代码!
 */
using System;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using Native.Core;
using Native.Core.Domain;
using Native.Sdk.Cqp;
using Native.Sdk.Cqp.Enum;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Native.Sdk.Cqp.Expand;
using Native.Sdk.Cqp.Model;
using Unity;
using Unity.Injection;

namespace Native.App.Export
{
	/// <summary>	
	/// 表示酷Q事件导出的类	
	/// </summary>	
	public class CQEventExport	
	{	
		#region --构造函数--	
		/// <summary>	
		/// 由托管环境初始化的 <see cref="CQEventExport"/> 的新实例	
		/// </summary>	
		static CQEventExport ()	
		{	
			// 初始化 Costura.Fody	
			CosturaUtility.Initialize ();	
			
			Type appDataType = typeof (AppData);	
			appDataType.GetRuntimeProperty ("UnityContainer").GetSetMethod (true).Invoke (null, new object[] { new UnityContainer () });	
			// 调用方法进行注册	
			CQMain.Register (AppData.UnityContainer);	
			
			// 调用方法进行实例化	
			ResolveBackcall ();	
		}	
		#endregion	
		
		#region --核心方法--	
		/// <summary>	
		/// 返回酷Q用于识别本应用的 AppID 和 ApiVer	
		/// </summary>	
		/// <returns>酷Q用于识别本应用的 AppID 和 ApiVer</returns>	
		[DllExport (ExportName = "AppInfo", CallingConvention = CallingConvention.StdCall)]	
		private static string AppInfo ()	
		{	
			return "9,com.color_yr.minecraft_qq";	
		}	
		
		/// <summary>	
		/// 接收应用 Authcode, 用于注册接口	
		/// </summary>	
		/// <param name="authCode">酷Q应用验证码</param>	
		/// <returns>返回注册结果给酷Q</returns>	
		[DllExport (ExportName = "Initialize", CallingConvention = CallingConvention.StdCall)]	
		private static int Initialize (int authCode)	
		{	
			// 反射获取 AppData 实例	
			Type appDataType = typeof (AppData);	
			// 注册一个 CQApi 实例	
			AppInfo appInfo = new AppInfo ("com.color_yr.minecraft_qq", 1, 9, "Minecraft_QQ", "2.9.0.2", 1, "Color_yr", "MC服务器与QQ群互通", authCode);	
			appDataType.GetRuntimeProperty ("CQApi").GetSetMethod (true).Invoke (null, new object[] { new CQApi (appInfo) });	
			AppData.UnityContainer.RegisterInstance<CQApi> ("com.color_yr.minecraft_qq", AppData.CQApi);	
			// 向容器注册一个 CQLog 实例	
			appDataType.GetRuntimeProperty ("CQLog").GetSetMethod (true).Invoke (null, new object[] { new CQLog (authCode) });	
			AppData.UnityContainer.RegisterInstance<CQLog> ("com.color_yr.minecraft_qq", AppData.CQLog);	
			// 注册插件全局异常捕获回调, 用于捕获未处理的异常, 回弹给 酷Q 做处理	
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;	
			// 本函数【禁止】处理其他任何代码，以免发生异常情况。如需执行初始化代码请在Startup事件中执行（Type=1001）。	
			return 0;	
		}	
		#endregion	
		
		#region --私有方法--	
		/// <summary>	
		/// 全局异常捕获, 用于捕获开发者未处理的异常, 此异常将回弹至酷Q进行处理	
		/// </summary>	
		/// <param name="sender">事件来源对象</param>	
		/// <param name="e">附加的事件参数</param>	
		private static void CurrentDomain_UnhandledException (object sender, UnhandledExceptionEventArgs e)	
		{	
			Exception ex = e.ExceptionObject as Exception;	
			if (ex != null)	
			{	
				StringBuilder innerLog = new StringBuilder ();	
				innerLog.AppendLine ("发现未处理的异常!");	
				innerLog.AppendLine (ex.ToString ());	
				AppData.CQLog.SetFatalMessage (innerLog.ToString ());	
			}	
		}	
		
		/// <summary>	
		/// 读取容器中的注册项, 进行事件分发	
		/// </summary>	
		private static void ResolveBackcall ()	
		{	
			/*	
			 * Id: 1	
			 * Type: 21	
			 * Name: 私聊消息处理	
			 * Function: _eventPrivateMsg	
			 * Priority: 30000	
			 */	
			if (AppData.UnityContainer.IsRegistered<IPrivateMessage> ("私聊消息处理"))	
			{	
				Event_eventPrivateMsgHandler += AppData.UnityContainer.Resolve<IPrivateMessage> ("私聊消息处理").PrivateMessage;	
			}	
			
			/*	
			 * Id: 2	
			 * Type: 2	
			 * Name: 群消息处理	
			 * Function: _eventGroupMsg	
			 * Priority: 30000	
			 */	
			if (AppData.UnityContainer.IsRegistered<IGroupMessage> ("群消息处理"))	
			{	
				Event_eventGroupMsgHandler += AppData.UnityContainer.Resolve<IGroupMessage> ("群消息处理").GroupMessage;	
			}	
			
			/*	
			 * Id: 1003	
			 * Type: 1003	
			 * Name: 应用已被启用	
			 * Function: _eventEnable	
			 * Priority: 30000	
			 */	
			if (AppData.UnityContainer.IsRegistered<IAppEnable> ("应用已被启用"))	
			{	
				Event_eventEnableHandler += AppData.UnityContainer.Resolve<IAppEnable> ("应用已被启用").AppEnable;	
			}	
			
			/*	
			 * Id: 1004	
			 * Type: 1004	
			 * Name: 应用将被停用	
			 * Function: _eventDisable	
			 * Priority: 30000	
			 */	
			if (AppData.UnityContainer.IsRegistered<IAppDisable> ("应用将被停用"))	
			{	
				Event_eventDisableHandler += AppData.UnityContainer.Resolve<IAppDisable> ("应用将被停用").AppDisable;	
			}	
			
		}	
		#endregion	
		
		#region --导出方法--	
		/// <summary>	
		/// 事件回调, 以下是对应 Json 文件的信息	
		/// <para>Id: 1</para>	
		/// <para>Type: 21</para>	
		/// <para>Name: 私聊消息处理</para>	
		/// <para>Function: _eventPrivateMsg</para>	
		/// <para>Priority: 30000</para>	
		/// <para>IsRegex: False</para>	
		/// </summary>	
		public static event EventHandler<CQPrivateMessageEventArgs> Event_eventPrivateMsgHandler;	
		[DllExport (ExportName = "_eventPrivateMsg", CallingConvention = CallingConvention.StdCall)]	
		public static int Event_eventPrivateMsg (int subType, int msgId, long fromQQ, IntPtr msg, int font)	
		{	
			if (Event_eventPrivateMsgHandler != null)	
			{	
				CQPrivateMessageEventArgs args = new CQPrivateMessageEventArgs (AppData.CQApi, AppData.CQLog, 1, 21, "私聊消息处理", "_eventPrivateMsg", 30000, subType, msgId, fromQQ, msg.ToString(CQApi.DefaultEncoding), false);	
				Event_eventPrivateMsgHandler (typeof (CQEventExport), args);	
				return (int)(args.Handler ? CQMessageHandler.Intercept : CQMessageHandler.Ignore);	
			}	
			return 0;	
		}	
		
		/// <summary>	
		/// 事件回调, 以下是对应 Json 文件的信息	
		/// <para>Id: 2</para>	
		/// <para>Type: 2</para>	
		/// <para>Name: 群消息处理</para>	
		/// <para>Function: _eventGroupMsg</para>	
		/// <para>Priority: 30000</para>	
		/// <para>IsRegex: False</para>	
		/// </summary>	
		public static event EventHandler<CQGroupMessageEventArgs> Event_eventGroupMsgHandler;	
		[DllExport (ExportName = "_eventGroupMsg", CallingConvention = CallingConvention.StdCall)]	
		public static int Event_eventGroupMsg (int subType, int msgId, long fromGroup, long fromQQ, string fromAnonymous, IntPtr msg, int font)	
		{	
			if (Event_eventGroupMsgHandler != null)	
			{	
				CQGroupMessageEventArgs args = new CQGroupMessageEventArgs (AppData.CQApi, AppData.CQLog, 2, 2, "群消息处理", "_eventGroupMsg", 30000, subType, msgId, fromGroup, fromQQ, fromAnonymous, msg.ToString(CQApi.DefaultEncoding), false);	
				Event_eventGroupMsgHandler (typeof (CQEventExport), args);	
				return (int)(args.Handler ? CQMessageHandler.Intercept : CQMessageHandler.Ignore);	
			}	
			return 0;	
		}	
		
		/// <summary>	
		/// 事件回调, 以下是对应 Json 文件的信息	
		/// <para>Id: 1003</para>	
		/// <para>Type: 1003</para>	
		/// <para>Name: 应用已被启用</para>	
		/// <para>Function: _eventEnable</para>	
		/// <para>Priority: 30000</para>	
		/// <para>IsRegex: False</para>	
		/// </summary>	
		public static event EventHandler<CQAppEnableEventArgs> Event_eventEnableHandler;	
		[DllExport (ExportName = "_eventEnable", CallingConvention = CallingConvention.StdCall)]	
		public static int Event_eventEnable ()	
		{	
			if (Event_eventEnableHandler != null)	
			{	
				CQAppEnableEventArgs args = new CQAppEnableEventArgs (AppData.CQApi, AppData.CQLog, 1003, 1003, "应用已被启用", "_eventEnable", 30000);	
				Event_eventEnableHandler (typeof (CQEventExport), args);	
			}	
			return 0;	
		}	
		
		/// <summary>	
		/// 事件回调, 以下是对应 Json 文件的信息	
		/// <para>Id: 1004</para>	
		/// <para>Type: 1004</para>	
		/// <para>Name: 应用将被停用</para>	
		/// <para>Function: _eventDisable</para>	
		/// <para>Priority: 30000</para>	
		/// <para>IsRegex: False</para>	
		/// </summary>	
		public static event EventHandler<CQAppDisableEventArgs> Event_eventDisableHandler;	
		[DllExport (ExportName = "_eventDisable", CallingConvention = CallingConvention.StdCall)]	
		public static int Event_eventDisable ()	
		{	
			if (Event_eventDisableHandler != null)	
			{	
				CQAppDisableEventArgs args = new CQAppDisableEventArgs (AppData.CQApi, AppData.CQLog, 1004, 1004, "应用将被停用", "_eventDisable", 30000);	
				Event_eventDisableHandler (typeof (CQEventExport), args);	
			}	
			return 0;	
		}	
		
		#endregion	
	}	
}

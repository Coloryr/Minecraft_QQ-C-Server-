/*
 *	此代码由 T4 引擎根据 LibExport.tt 模板生成, 若您不了解以下代码的用处, 请勿修改!
 *	
 *	此文件包含项目 Json 文件的事件导出函数.
 */
using System;
using System.Runtime.InteropServices;
using System.Text;
using Native.Csharp.App.Event;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Interface;
using Native.Csharp.Sdk.Cqp;
using Native.Csharp.Sdk.Cqp.Model;
using Native.Csharp.Sdk.Cqp.Other;
using Unity;

namespace Native.Csharp.App.Core
{
    public class LibExport
    {
		#region --字段--
		private static Encoding _defaultEncoding = null;
		#endregion

		#region --构造函数--
		/// <summary>
		/// 静态构造函数, 注册依赖注入回调
		/// </summary>
		static LibExport ()
		{
			_defaultEncoding = Encoding.GetEncoding ("GB18030");
			
			// 初始化 Costura.Fody
			CosturaUtility.Initialize ();
			
			// 初始化依赖注入容器
			Common.UnityContainer = new UnityContainer ();

			// 程序开始调用方法进行注册
			Event_AppMain.Registbackcall (Common.UnityContainer);

			// 注册完毕调用方法进行分发
			Event_AppMain.Resolvebackcall (Common.UnityContainer);

			// 分发应用内事件
			ResolveAppbackcall ();
		}
		#endregion
		
		#region --核心方法--
		/// <summary>
		/// 返回 AppID 与 ApiVer, 本方法在模板运行后会根据项目名称自动填写 AppID 与 ApiVer
		/// </summary>
		/// <returns></returns>
		[DllExport (ExportName = "AppInfo", CallingConvention = CallingConvention.StdCall)]
		private static string AppInfo ()
		{
			// 请勿随意修改
			// 
			Common.AppName = "Minecraft_QQ";
			Common.AppVersion = Version.Parse ("2.0.0");		

			//
			// 当前项目名称: com.color_yr.minecraft_qq
			// Api版本: 9

			return string.Format ("{0},{1}", 9, "com.color_yr.minecraft_qq");
		}

		/// <summary>
		/// 接收插件 AutoCode, 注册异常
		/// </summary>
		/// <param name="authCode"></param>
		/// <returns></returns>
		[DllExport (ExportName = "Initialize", CallingConvention = CallingConvention.StdCall)]
		private static int Initialize (int authCode)
		{
			// 酷Q获取应用信息后，如果接受该应用，将会调用这个函数并传递AuthCode。
			Common.CqApi = new CqApi (authCode);

			// AuthCode 传递完毕后将对象加入容器托管, 以便在其它项目中调用
			Common.UnityContainer.RegisterInstance<CqApi> (Common.CqApi);

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
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void CurrentDomain_UnhandledException (object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = e.ExceptionObject as Exception;
			if (ex != null)
			{
				StringBuilder innerLog = new StringBuilder ();
				innerLog.AppendLine ("发现未处理的异常!");
				innerLog.AppendLine ("异常堆栈：");
				innerLog.AppendLine (ex.ToString ());
				Common.CqApi.AddFatalError (innerLog.ToString ());      //将未经处理的异常弹回酷Q做处理
			}
		}
		
		/// <summary>
		/// 获取所有的注入项, 分发到对应的事件
		/// </summary>
		private static void ResolveAppbackcall ()
		{
			/*
			 * Id: 1
			 * Name: 私聊消息处理
			 */
			if (Common.UnityContainer.IsRegistered<IReceiveFriendMessage> ("私聊消息处理") == true)
			{
				ReceiveFriendMessage_1 = Common.UnityContainer.Resolve<IReceiveFriendMessage> ("私聊消息处理").ReceiveFriendMessage;
			}
			if (Common.UnityContainer.IsRegistered<IReceiveOnlineStatusMessage> ("私聊消息处理") == true)
			{
				ReceiveOnlineStatusMessage_1 = Common.UnityContainer.Resolve<IReceiveOnlineStatusMessage> ("私聊消息处理").ReceiveOnlineStatusMessage;
			}
			if (Common.UnityContainer.IsRegistered<IReceiveGroupPrivateMessage> ("私聊消息处理") == true)
			{
				ReceiveGroupPrivateMessage_1 = Common.UnityContainer.Resolve<IReceiveGroupPrivateMessage> ("私聊消息处理").ReceiveGroupPrivateMessage;
			}
			if (Common.UnityContainer.IsRegistered<IReceiveDiscussPrivateMessage> ("私聊消息处理") == true)
			{
				ReceiveDiscussPrivateMessage_1 = Common.UnityContainer.Resolve<IReceiveDiscussPrivateMessage> ("私聊消息处理").ReceiveDiscussPrivateMessage;
			}
			
			/*
			 * Id: 2
			 * Name: 群消息处理
			 */
			if (Common.UnityContainer.IsRegistered<IReceiveGroupMessage> ("群消息处理") == true)
			{
				ReceiveGroupMessage_2 = Common.UnityContainer.Resolve<IReceiveGroupMessage> ("群消息处理").ReceiveGroupMessage;
			}
			
			/*
			 * Id: 1002
			 * Name: 酷Q关闭事件
			 */
			if (Common.UnityContainer.IsRegistered<ICqExit> ("酷Q关闭事件") == true)
			{
				CqExit_1002 = Common.UnityContainer.Resolve<ICqExit> ("酷Q关闭事件").CqExit;
			}
			
			/*
			 * Id: 1003
			 * Name: 应用已被启用
			 */
			if (Common.UnityContainer.IsRegistered<ICqAppEnable> ("应用已被启用") == true)
			{
				AppEnable_1003 = Common.UnityContainer.Resolve<ICqAppEnable> ("应用已被启用").CqAppEnable;
			}
			

		}
		#endregion
		
		#region --导出方法--
		/*
		 * Id: 1
		 * Type: 21
		 * Name: 私聊消息处理
		 * Function: _eventPrivateMsg
		 */
		public static event EventHandler<CqPrivateMessageEventArgs> ReceiveFriendMessage_1;
		public static event EventHandler<CqPrivateMessageEventArgs> ReceiveOnlineStatusMessage_1;
		public static event EventHandler<CqPrivateMessageEventArgs> ReceiveGroupPrivateMessage_1;
		public static event EventHandler<CqPrivateMessageEventArgs> ReceiveDiscussPrivateMessage_1;
		[DllExport (ExportName = "_eventPrivateMsg", CallingConvention = CallingConvention.StdCall)]
		private static int Evnet__eventPrivateMsg (int subType, int msgId, long fromQQ, IntPtr msg, int font)
		{
			if (ReceiveFriendMessage_1 != null)
			{
				CqPrivateMessageEventArgs args = new CqPrivateMessageEventArgs (1, msgId, fromQQ, msg.ToString (_defaultEncoding));
				if (subType == 11)
				{
					if (ReceiveFriendMessage_1 != null)
					{
						ReceiveFriendMessage_1 (null, args);
					}
				}
				else if (subType == 1)
				{
					if (ReceiveOnlineStatusMessage_1 != null)
					{
						ReceiveOnlineStatusMessage_1 (null, args);
					}
				}
				else if (subType == 2)
				{
					if (ReceiveGroupPrivateMessage_1 != null)
					{
						ReceiveGroupPrivateMessage_1 (null, args);
					}
				}
				else if (subType == 3)
				{
					if (ReceiveDiscussPrivateMessage_1 != null)
					{
						ReceiveDiscussPrivateMessage_1 (null, args);
					}
				}
				return Convert.ToInt32 (args.Handler);
			}
			return -1;
		}

		/*
		 * Id: 2
		 * Type: 2
		 * Name: 群消息处理
		 * Function: _eventGroupMsg
		 */
		public static event EventHandler<CqGroupMessageEventArgs> ReceiveGroupMessage_2;
		[DllExport (ExportName = "_eventGroupMsg", CallingConvention = CallingConvention.StdCall)]
		private static int Evnet__eventGroupMsg (int subType, int msgId, long fromGroup, long fromQQ, string fromAnonymous, IntPtr msg, int font)
		{
			GroupAnonymous anonymous = null;
			if (fromQQ == 80000000 && !string.IsNullOrEmpty (fromAnonymous))
			{
				anonymous = Common.CqApi.GetAnonymous (fromAnonymous);
			}
			CqGroupMessageEventArgs args = new CqGroupMessageEventArgs (2, msgId, fromGroup, fromQQ, anonymous, msg.ToString (_defaultEncoding));
			if (subType == 1)
			{
				if (ReceiveGroupMessage_2 != null)
				{
					ReceiveGroupMessage_2 (null, args);
				}
			}
			return Convert.ToInt32 (args.Handler);
		}

		/*
		 * Id: 1002
		 * Type: 1002
		 * Name: 酷Q关闭事件
		 * Function: _eventExit
		 */
		public static event EventHandler<CqExitEventArgs> CqExit_1002;
		[DllExport (ExportName = "_eventExit", CallingConvention = CallingConvention.StdCall)]
		private static int Evnet__eventExit ()
		{
			if (CqExit_1002 != null)
			{
				CqExit_1002 (null, new CqExitEventArgs (1002));
			}
			return 0;
		}

		/*
		 * Id: 1003
		 * Type: 1003
		 * Name: 应用已被启用
		 * Function: _eventEnable
		 */
		public static event EventHandler<CqAppEnableEventArgs> AppEnable_1003;
		[DllExport (ExportName = "_eventEnable", CallingConvention = CallingConvention.StdCall)]
		private static int Evnet__eventEnable ()
		{
			if (AppEnable_1003 != null)
			{
				AppEnable_1003 (null, new CqAppEnableEventArgs (1003));
			}
			return 0;
		}


		#endregion
    }
}


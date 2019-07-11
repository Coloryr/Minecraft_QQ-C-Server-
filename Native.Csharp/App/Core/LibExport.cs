/*
 *	�˴����� T4 ������� LibExport.tt ģ������, �������˽����´�����ô�, �����޸�!
 *	
 *	���ļ�������Ŀ Json �ļ����¼���������.
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
		#region --�ֶ�--
		private static Encoding _defaultEncoding = null;
		#endregion

		#region --���캯��--
		/// <summary>
		/// ��̬���캯��, ע������ע��ص�
		/// </summary>
		static LibExport ()
		{
			_defaultEncoding = Encoding.GetEncoding ("GB18030");
			
			// ��ʼ�� Costura.Fody
			CosturaUtility.Initialize ();
			
			// ��ʼ������ע������
			Common.UnityContainer = new UnityContainer ();

			// ����ʼ���÷�������ע��
			Event_AppMain.Registbackcall (Common.UnityContainer);

			// ע����ϵ��÷������зַ�
			Event_AppMain.Resolvebackcall (Common.UnityContainer);

			// �ַ�Ӧ�����¼�
			ResolveAppbackcall ();
		}
		#endregion
		
		#region --���ķ���--
		/// <summary>
		/// ���� AppID �� ApiVer, ��������ģ�����к�������Ŀ�����Զ���д AppID �� ApiVer
		/// </summary>
		/// <returns></returns>
		[DllExport (ExportName = "AppInfo", CallingConvention = CallingConvention.StdCall)]
		private static string AppInfo ()
		{
			// ���������޸�
			// 
			Common.AppName = "��Q����Ӧ�� for C#";
			Common.AppVersion = Version.Parse ("1.0.0");		

			//
			// ��ǰ��Ŀ����: Native.Csharp
			// Api�汾: 9

			return string.Format ("{0},{1}", 9, "Native.Csharp");
		}

		/// <summary>
		/// ���ղ�� AutoCode, ע���쳣
		/// </summary>
		/// <param name="authCode"></param>
		/// <returns></returns>
		[DllExport (ExportName = "Initialize", CallingConvention = CallingConvention.StdCall)]
		private static int Initialize (int authCode)
		{
			// ��Q��ȡӦ����Ϣ��������ܸ�Ӧ�ã���������������������AuthCode��
			Common.CqApi = new CqApi (authCode);

			// AuthCode ������Ϻ󽫶�����������й�, �Ա���������Ŀ�е���
			Common.UnityContainer.RegisterInstance<CqApi> (Common.CqApi);

			// ע����ȫ���쳣����ص�, ���ڲ���δ������쳣, �ص��� ��Q ������
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			// ����������ֹ�����������κδ��룬���ⷢ���쳣���������ִ�г�ʼ����������Startup�¼���ִ�У�Type=1001����
			return 0;
		}
		#endregion
		
		#region --˽�з���--
		/// <summary>
		/// ȫ���쳣����, ���ڲ��񿪷���δ������쳣, ���쳣���ص�����Q���д���
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void CurrentDomain_UnhandledException (object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = e.ExceptionObject as Exception;
			if (ex != null)
			{
				StringBuilder innerLog = new StringBuilder ();
				innerLog.AppendLine ("����δ������쳣!");
				innerLog.AppendLine ("�쳣��ջ��");
				innerLog.AppendLine (ex.ToString ());
				Common.CqApi.AddFatalError (innerLog.ToString ());      //��δ��������쳣���ؿ�Q������
			}
		}
		
		/// <summary>
		/// ��ȡ���е�ע����, �ַ�����Ӧ���¼�
		/// </summary>
		private static void ResolveAppbackcall ()
		{
			/*
			 * Id: 1
			 * Name: ˽����Ϣ����
			 */
			if (Common.UnityContainer.IsRegistered<IReceiveFriendMessage> ("˽����Ϣ����") == true)
			{
				ReceiveFriendMessage_1 = Common.UnityContainer.Resolve<IReceiveFriendMessage> ("˽����Ϣ����").ReceiveFriendMessage;
			}
			if (Common.UnityContainer.IsRegistered<IReceiveOnlineStatusMessage> ("˽����Ϣ����") == true)
			{
				ReceiveOnlineStatusMessage_1 = Common.UnityContainer.Resolve<IReceiveOnlineStatusMessage> ("˽����Ϣ����").ReceiveOnlineStatusMessage;
			}
			if (Common.UnityContainer.IsRegistered<IReceiveGroupPrivateMessage> ("˽����Ϣ����") == true)
			{
				ReceiveGroupPrivateMessage_1 = Common.UnityContainer.Resolve<IReceiveGroupPrivateMessage> ("˽����Ϣ����").ReceiveGroupPrivateMessage;
			}
			if (Common.UnityContainer.IsRegistered<IReceiveDiscussPrivateMessage> ("˽����Ϣ����") == true)
			{
				ReceiveDiscussPrivateMessage_1 = Common.UnityContainer.Resolve<IReceiveDiscussPrivateMessage> ("˽����Ϣ����").ReceiveDiscussPrivateMessage;
			}
			
			/*
			 * Id: 2
			 * Name: Ⱥ��Ϣ����
			 */
			if (Common.UnityContainer.IsRegistered<IReceiveGroupMessage> ("Ⱥ��Ϣ����") == true)
			{
				ReceiveGroupMessage_2 = Common.UnityContainer.Resolve<IReceiveGroupMessage> ("Ⱥ��Ϣ����").ReceiveGroupMessage;
			}
			
			/*
			 * Id: 3
			 * Name: ��������Ϣ����
			 */
			if (Common.UnityContainer.IsRegistered<IReceiveDiscussMessage> ("��������Ϣ����") == true)
			{
				ReceiveDiscussMessage_3 = Common.UnityContainer.Resolve<IReceiveDiscussMessage> ("��������Ϣ����").ReceiveDiscussMessage;
			}
			
			/*
			 * Id: 4
			 * Name: Ⱥ�ļ��ϴ��¼�����
			 */
			if (Common.UnityContainer.IsRegistered<IReceiveGroupFileUpload> ("Ⱥ�ļ��ϴ��¼�����") == true)
			{
				ReceiveFileUploadMessage_4 = Common.UnityContainer.Resolve<IReceiveGroupFileUpload> ("Ⱥ�ļ��ϴ��¼�����").ReceiveGroupFileUpload;
			}
			
			/*
			 * Id: 5
			 * Name: Ⱥ����䶯�¼�����
			 */
			if (Common.UnityContainer.IsRegistered<IReceiveGroupManageIncrease> ("Ⱥ����䶯�¼�����") == true)
			{
				ReceiveManageIncrease_5 = Common.UnityContainer.Resolve<IReceiveGroupManageIncrease> ("Ⱥ����䶯�¼�����").ReceiveGroupManageIncrease;
			}
			if (Common.UnityContainer.IsRegistered<IReceiveGroupManageDecrease> ("Ⱥ����䶯�¼�����") == true)
			{
				ReceiveManageDecrease_5 = Common.UnityContainer.Resolve<IReceiveGroupManageDecrease> ("Ⱥ����䶯�¼�����").ReceiveGroupManageDecrease;
			}
			
			/*
			 * Id: 6
			 * Name: Ⱥ��Ա�����¼�����
			 */
			if (Common.UnityContainer.IsRegistered<IReceiveGroupMemberLeave> ("Ⱥ��Ա�����¼�����") == true)
			{
				ReceiveMemberLeave_6 = Common.UnityContainer.Resolve<IReceiveGroupMemberLeave> ("Ⱥ��Ա�����¼�����").ReceiveGroupMemberLeave;
			}
			if (Common.UnityContainer.IsRegistered<IReceiveGroupMemberRemove> ("Ⱥ��Ա�����¼�����") == true)
			{
				ReceiveMemberRemove_6 = Common.UnityContainer.Resolve<IReceiveGroupMemberRemove> ("Ⱥ��Ա�����¼�����").ReceiveGroupMemberRemove;
			}
			
			/*
			 * Id: 7
			 * Name: Ⱥ��Ա�����¼�����
			 */
			if (Common.UnityContainer.IsRegistered<IReceiveGroupMemberPass> ("Ⱥ��Ա�����¼�����") == true)
			{
				ReceiveMemberPass_7 = Common.UnityContainer.Resolve<IReceiveGroupMemberPass> ("Ⱥ��Ա�����¼�����").ReceiveGroupMemberPass;
			}
			if (Common.UnityContainer.IsRegistered<IReceiveGroupMemberBeInvitee> ("Ⱥ��Ա�����¼�����") == true)
			{
				ReceiveMemberBeInvitee_7 = Common.UnityContainer.Resolve<IReceiveGroupMemberBeInvitee> ("Ⱥ��Ա�����¼�����").ReceiveGroupMemberBeInvitee;
			}
			
			/*
			 * Id: 10
			 * Name: ����������¼�����
			 */
			if (Common.UnityContainer.IsRegistered<IReceiveFriendIncrease> ("����������¼�����") == true)
			{
				ReceiveFriendIncrease_10 = Common.UnityContainer.Resolve<IReceiveFriendIncrease> ("����������¼�����").ReceiveFriendIncrease;
			}
			
			/*
			 * Id: 8
			 * Name: �������������
			 */
			if (Common.UnityContainer.IsRegistered<IReceiveFriendAddRequest> ("�������������") == true)
			{
				ReceiveFriendAdd_8 = Common.UnityContainer.Resolve<IReceiveFriendAddRequest> ("�������������").ReceiveFriendAddRequest;
			}
			
			/*
			 * Id: 9
			 * Name: Ⱥ���������
			 */
			if (Common.UnityContainer.IsRegistered<IReceiveAddGroupRequest> ("Ⱥ���������") == true)
			{
				ReceiveAddGroupRequest_9 = Common.UnityContainer.Resolve<IReceiveAddGroupRequest> ("Ⱥ���������").ReceiveAddGroupRequest;
			}
			if (Common.UnityContainer.IsRegistered<IReceiveAddGroupBeInvitee> ("Ⱥ���������") == true)
			{
				ReceiveAddGroupBeInvitee_9 = Common.UnityContainer.Resolve<IReceiveAddGroupBeInvitee> ("Ⱥ���������").ReceiveAddGroupBeInvitee;
			}
			
			/*
			 * Id: 1001
			 * Name: ��Q�����¼�
			 */
			if (Common.UnityContainer.IsRegistered<ICqStartup> ("��Q�����¼�") == true)
			{
				CqStartup_1001 = Common.UnityContainer.Resolve<ICqStartup> ("��Q�����¼�").CqStartup;
			}
			
			/*
			 * Id: 1002
			 * Name: ��Q�ر��¼�
			 */
			if (Common.UnityContainer.IsRegistered<ICqExit> ("��Q�ر��¼�") == true)
			{
				CqExit_1002 = Common.UnityContainer.Resolve<ICqExit> ("��Q�ر��¼�").CqExit;
			}
			
			/*
			 * Id: 1003
			 * Name: Ӧ���ѱ�����
			 */
			if (Common.UnityContainer.IsRegistered<ICqAppEnable> ("Ӧ���ѱ�����") == true)
			{
				AppEnable_1003 = Common.UnityContainer.Resolve<ICqAppEnable> ("Ӧ���ѱ�����").CqAppEnable;
			}
			
			/*
			 * Id: 1004
			 * Name: Ӧ�ý���ͣ��
			 */
			if (Common.UnityContainer.IsRegistered<ICqAppDisable> ("Ӧ�ý���ͣ��") == true)
			{
				AppDisable_1004 = Common.UnityContainer.Resolve<ICqAppDisable> ("Ӧ�ý���ͣ��").CqAppDisable;
			}
			

		}
		#endregion
		
		#region --��������--
		/*
		 * Id: 1
		 * Type: 21
		 * Name: ˽����Ϣ����
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
		 * Name: Ⱥ��Ϣ����
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
		 * Id: 3
		 * Type: 4
		 * Name: ��������Ϣ����
		 * Function: _eventDiscussMsg
		 */
		public static event EventHandler<CqDiscussMessageEventArgs> ReceiveDiscussMessage_3;
		[DllExport (ExportName = "_eventDiscussMsg", CallingConvention = CallingConvention.StdCall)]
		private static int Evnet__eventDiscussMsg (int subType, int msgId, long fromDiscuss, long fromQQ, IntPtr msg, int font)
		{
			CqDiscussMessageEventArgs args = new CqDiscussMessageEventArgs (3, msgId, fromDiscuss, fromQQ, msg.ToString (_defaultEncoding));
			if (subType == 1)
			{
				if (ReceiveDiscussMessage_3 != null)
				{
					ReceiveDiscussMessage_3 (null, args);
				}
			}
			return Convert.ToInt32 (args.Handler);
		}

		/*
		 * Id: 4
		 * Type: 11
		 * Name: Ⱥ�ļ��ϴ��¼�����
		 * Function: _eventGroupUpload
		 */
		public static event EventHandler<CqGroupFileUploadEventArgs> ReceiveFileUploadMessage_4;
		[DllExport (ExportName = "_eventGroupUpload", CallingConvention = CallingConvention.StdCall)]
		private static int Evnet__eventGroupUpload (int subType, int sendTime, long fromGroup, long fromQQ, string file)
		{
			CqGroupFileUploadEventArgs args = new CqGroupFileUploadEventArgs (4, sendTime.ToDateTime (), fromGroup, fromQQ, Common.CqApi.GetFile (file));
			if (subType == 1)
			{
				if (ReceiveFileUploadMessage_4 != null)
				{
					ReceiveFileUploadMessage_4 (null, args);
				}
			}
			return Convert.ToInt32 (args.Handler);
		}

		/*
		 * Id: 5
		 * Type: 101
		 * Name: Ⱥ����䶯�¼�����
		 * Function: _eventSystem_GroupAdmin
		 */
		public static event EventHandler<CqGroupManageChangeEventArgs> ReceiveManageIncrease_5;
		public static event EventHandler<CqGroupManageChangeEventArgs> ReceiveManageDecrease_5;
		[DllExport (ExportName = "_eventSystem_GroupAdmin", CallingConvention = CallingConvention.StdCall)]
		private static int Evnet__eventSystem_GroupAdmin (int subType, int sendTime, long fromGroup, long beingOperateQQ)
		{
			CqGroupManageChangeEventArgs args = new CqGroupManageChangeEventArgs (5, sendTime.ToDateTime (), fromGroup, beingOperateQQ);
			if (subType == 1)
			{
				if (ReceiveManageDecrease_5 != null)
				{
					ReceiveManageDecrease_5 (null, args);
				}
			}
			else if (subType == 2)
			{
				if (ReceiveManageIncrease_5 != null)
				{
					ReceiveManageIncrease_5 (null, args);
				}
			}
			return Convert.ToInt32 (args.Handler);
		}

		/*
		 * Id: 6
		 * Type: 102
		 * Name: Ⱥ��Ա�����¼�����
		 * Function: _eventSystem_GroupMemberDecrease
		 */
		public static event EventHandler<CqGroupMemberDecreaseEventArgs> ReceiveMemberLeave_6;
		public static event EventHandler<CqGroupMemberDecreaseEventArgs> ReceiveMemberRemove_6;
		[DllExport (ExportName = "_eventSystem_GroupMemberDecrease", CallingConvention = CallingConvention.StdCall)]
		private static int Evnet__eventSystem_GroupMemberDecrease (int subType, int sendTime, long fromGroup, long fromQQ, long beingOperateQQ)
		{
			CqGroupMemberDecreaseEventArgs args = new CqGroupMemberDecreaseEventArgs (6, sendTime.ToDateTime (), fromGroup, fromQQ, beingOperateQQ);
			if (subType == 1)
			{
				if (ReceiveMemberLeave_6 != null)
				{
					ReceiveMemberLeave_6 (null, args);
				}
			}
			else if (subType == 2)
			{
				if (ReceiveMemberRemove_6 != null)
				{
					ReceiveMemberRemove_6 (null, args);
				}
			}
			return Convert.ToInt32 (args.Handler);
		}

		/*
		 * Id: 7
		 * Type: 103
		 * Name: Ⱥ��Ա�����¼�����
		 * Function: _eventSystem_GroupMemberIncrease
		 */
		public static event EventHandler<CqGroupMemberIncreaseEventArgs> ReceiveMemberPass_7;
		public static event EventHandler<CqGroupMemberIncreaseEventArgs> ReceiveMemberBeInvitee_7;
		[DllExport (ExportName = "_eventSystem_GroupMemberIncrease", CallingConvention = CallingConvention.StdCall)]
		private static int Evnet__eventSystem_GroupMemberIncrease (int subType, int sendTime, long fromGroup, long fromQQ, long beingOperateQQ)
		{
			CqGroupMemberIncreaseEventArgs args = new CqGroupMemberIncreaseEventArgs (7, sendTime.ToDateTime (), fromGroup, fromQQ, beingOperateQQ);
			if (subType == 1)
			{
				if (ReceiveMemberPass_7 != null)
				{
					ReceiveMemberPass_7 (null, args);
				}
			}
			else if (subType == 2)
			{
				if (ReceiveMemberBeInvitee_7 != null)
				{
					ReceiveMemberBeInvitee_7 (null, args);
				}
			}
			return Convert.ToInt32 (args.Handler);
		}

		/*
		 * Id: 10
		 * Type: 201
		 * Name: ����������¼�����
		 * Function: _eventFriend_Add
		 */
		public static event EventHandler<CqFriendIncreaseEventArgs> ReceiveFriendIncrease_10;
		[DllExport (ExportName = "_eventFriend_Add", CallingConvention = CallingConvention.StdCall)]
		private static int Evnet__eventFriend_Add (int subType, int sendTime, long fromQQ)
		{
			CqFriendIncreaseEventArgs args = new CqFriendIncreaseEventArgs (10, sendTime.ToDateTime (), fromQQ);
			if (subType == 1)
			{
				if (ReceiveFriendIncrease_10 != null)
				{
					ReceiveFriendIncrease_10 (null, args);
				}
			}
			return Convert.ToInt32 (args.Handler);
		}

		/*
		 * Id: 8
		 * Type: 301
		 * Name: �������������
		 * Function: _eventRequest_AddFriend
		 */
		public static event EventHandler<CqAddFriendRequestEventArgs> ReceiveFriendAdd_8;
		[DllExport (ExportName = "_eventRequest_AddFriend", CallingConvention = CallingConvention.StdCall)]
		private static int Evnet__eventRequest_AddFriend (int subType, int sendTime, long fromQQ, IntPtr msg, string responseFlag)
		{
			CqAddFriendRequestEventArgs args = new CqAddFriendRequestEventArgs (8, sendTime.ToDateTime (), fromQQ, msg.ToString (_defaultEncoding), responseFlag);
			if (subType == 1)
			{
				if (ReceiveFriendAdd_8 != null)
				{
					ReceiveFriendAdd_8 (null, args);
				}
			}
			return Convert.ToInt32 (args.Handler);
		}

		/*
		 * Id: 9
		 * Type: 302
		 * Name: Ⱥ���������
		 * Function: _eventRequest_AddGroup
		 */
		public static event EventHandler<CqAddGroupRequestEventArgs> ReceiveAddGroupRequest_9;
		public static event EventHandler<CqAddGroupRequestEventArgs> ReceiveAddGroupBeInvitee_9;
		[DllExport (ExportName = "_eventRequest_AddGroup", CallingConvention = CallingConvention.StdCall)]
		private static int Evnet__eventRequest_AddGroup (int subType, int sendTime, long fromGroup, long fromQQ, IntPtr msg, string responseFlag)
		{
			CqAddGroupRequestEventArgs args = new CqAddGroupRequestEventArgs (9, sendTime.ToDateTime (), fromGroup, fromQQ, msg.ToString (_defaultEncoding), responseFlag);
			if (subType == 1)
			{
				if (ReceiveAddGroupRequest_9 != null)
				{
					ReceiveAddGroupRequest_9 (null, args);
				}
			}
			else if (subType == 2)
			{
				if (ReceiveAddGroupBeInvitee_9 != null)
				{
					ReceiveAddGroupBeInvitee_9 (null, args);
				}
			}
			return Convert.ToInt32 (args.Handler);
		}

		/*
		 * Id: 1001
		 * Type: 1001
		 * Name: ��Q�����¼�
		 * Function: _eventStartup
		 */
		public static event EventHandler<CqStartupEventArgs> CqStartup_1001;
		[DllExport (ExportName = "_eventStartup", CallingConvention = CallingConvention.StdCall)]
		private static int Evnet__eventStartup ()
		{
			if (CqStartup_1001 != null)
			{
				CqStartup_1001 (null, new CqStartupEventArgs (1001));
			}
			return 0;
		}

		/*
		 * Id: 1002
		 * Type: 1002
		 * Name: ��Q�ر��¼�
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
		 * Name: Ӧ���ѱ�����
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

		/*
		 * Id: 1004
		 * Type: 1004
		 * Name: Ӧ�ý���ͣ��
		 * Function: _eventDisable
		 */
		public static event EventHandler<CqAppDisableEventArgs> AppDisable_1004;
		[DllExport (ExportName = "_eventDisable", CallingConvention = CallingConvention.StdCall)]
		private static int Evnet__eventDisable ()
		{
			if (AppDisable_1004 != null)
			{
				AppDisable_1004 (null, new CqAppDisableEventArgs (1004));
			}
			return 0;
		}


		#endregion
    }
}


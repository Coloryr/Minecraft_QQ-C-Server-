/*
 *	�˴����� T4 ������� MenuExport.tt ģ������, �������˽����´�����ô�, �����޸�!
 *	
 *	���ļ�������Ŀ Json �ļ��Ĳ˵���������.
 */
using System;
using System.Runtime.InteropServices;
using System.Text;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Interface;
using Unity;

namespace Native.Csharp.App.Core
{
    public class MenuExport
    {
		#region --���캯��--
		/// <summary>
		/// ��̬���캯��, ע������ע��ص�
		/// </summary>
        static MenuExport ()
        {
			// �ַ�Ӧ�����¼�
			ResolveAppbackcall ();
        }
        #endregion

		#region --˽�з���--
		/// <summary>
		/// ��ȡ���е�ע����, �ַ�����Ӧ���¼�
		/// </summary>
		private static void ResolveAppbackcall ()
		{
			/*
			 * Name: �򿪿���̨
			 * Function: _eventOpenConsole
			 */
			if (Common.UnityContainer.IsRegistered<ICallMenu> ("�򿪿���̨") == true)
			{
				Menu__eventOpenConsole = Common.UnityContainer.Resolve<ICallMenu> ("�򿪿���̨").CallMenu;
			}


		}
        #endregion

		#region --��������--
		/*
		 * Name: �򿪿���̨
		 * Function: _eventOpenConsole
		 */
		public static event EventHandler<CqCallMenuEventArgs> Menu__eventOpenConsole;
		[DllExport (ExportName = "_eventOpenConsole", CallingConvention = CallingConvention.StdCall)]
		private static int Evnet__eventOpenConsole ()
		{
			if (Menu__eventOpenConsole != null)
			{
				Menu__eventOpenConsole (null, new CqCallMenuEventArgs ("�򿪿���̨"));
			}
			return 0;
		}


		#endregion
    }
}


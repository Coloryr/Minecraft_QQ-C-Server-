/*
 * 此文件由T4引擎自动生成, 请勿修改此文件中的代码!
 */
using Native.Csharp.App.Common;
using Native.Csharp.Sdk.Cqp;
using Native.Csharp.Sdk.Cqp.EventArgs;
using Native.Csharp.Sdk.Cqp.Interface;
using System;
using System.Runtime.InteropServices;
using Unity;

namespace Native.Csharp.App.Export
{
    /// <summary>	
    /// 表示酷Q菜单导出的类	
    /// </summary>	
    public class CQMenuExport
    {
        #region --字段--	
        private static CQApi api = null;
        private static CQLog log = null;
        #endregion

        #region --构造函数--	
        /// <summary>	
        /// 由托管环境初始化的 <see cref="CQMenuExport"/> 的新实例	
        /// </summary>	
        static CQMenuExport()
        {
            api = AppInfo.UnityContainer.Resolve<CQApi>(AppInfo.Id);
            log = AppInfo.UnityContainer.Resolve<CQLog>(AppInfo.Id);

            // 调用方法进行实例化	
            ResolveBackcall();
        }
        #endregion

        #region --私有方法--	
        /// <summary>	
        /// 读取容器中的注册项, 进行事件分发	
        /// </summary>	
        private static void ResolveBackcall()
        {
            /*	
			 * Name: 打开设置	
			 * Function: _menu	
			 */
            if (Common.AppInfo.UnityContainer.IsRegistered<IMenuCall>("打开设置"))
            {
                Menu_menuHandler += Common.AppInfo.UnityContainer.Resolve<IMenuCall>("打开设置").MenuCall;
            }

        }
        #endregion

        #region --导出方法--	
        /*	
		 * Name: 打开设置	
		 * Function: _menu	
		 */
        public static event EventHandler<CQMenuCallEventArgs> Menu_menuHandler;
        [DllExport(ExportName = "_menu", CallingConvention = CallingConvention.StdCall)]
        public static int Menu_menu()
        {
            if (Menu_menuHandler != null)
            {
                CQMenuCallEventArgs args = new CQMenuCallEventArgs(api, log, "打开设置", "_menu");
                Menu_menuHandler(typeof(CQMenuExport), args);
            }
            return 0;
        }

        #endregion
    }
}

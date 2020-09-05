using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using WinApi.User32;

namespace HexagonTile.Helper
{
    class StyleHelper
    {
        /// <summary>
        /// 使目标句柄位于系统最下层
        /// </summary>
        /// <param name="window">WPF window object</param>
        public static void SetButtom(Window window)
        {
            var ptr = new WindowInteropHelper(window).Handle;
             
            User32Methods.SetWindowPos(ptr, new IntPtr(1), 0, 0, 0, 0,
                WindowPositionFlags.SWP_NOMOVE
                | WindowPositionFlags.SWP_NOSIZE
                | WindowPositionFlags.SWP_NOACTIVATE);
        }

        /// <summary>
        /// 使目标句柄位于系统最下层
        /// </summary>
        /// <param name="window">WPF window object</param>
        public static void HideAppInTaskbar(Window window)
        {
            window.WindowStyle = WindowStyle.None;
            window.WindowState = WindowState.Maximized;
            window.ShowInTaskbar = false;
            var ptr = new WindowInteropHelper(window).Handle;

            var longvalue = User32Methods.GetWindowLongPtr(ptr, -20).ToInt64();

            longvalue = longvalue | 0x00000080;

            User32Methods.SetWindowLongPtr(ptr, -20, new IntPtr(longvalue));
        }
    }
}

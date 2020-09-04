using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using WinApi.User32;

namespace HexagonTile
{
    class StyleHelper
    {
        public static void SetButtom(Window window)
        {
            var ptr = new WindowInteropHelper(window).Handle;
             
            User32Methods.SetWindowPos(ptr, new IntPtr(1), 0, 0, 0, 0,
                WindowPositionFlags.SWP_NOMOVE
                | WindowPositionFlags.SWP_NOSIZE
                | WindowPositionFlags.SWP_NOACTIVATE);
        }
    }
}

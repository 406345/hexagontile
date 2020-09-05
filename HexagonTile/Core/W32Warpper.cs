using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WinApi.User32;

namespace HexagonTile.Core
{
    class W32Warpper
    {
        [DllImport("shell32.DLL", EntryPoint = "ExtractAssociatedIcon")]
        private static extern int ExtractAssociatedIconA(int hInst, string lpIconPath, ref int lpiIcon);

        [DllImport("User32.dll")]
        public static extern int PrivateExtractIcons(
             //文件名可以是exe,dll,ico,cur,ani,bmp
             string lpszFile,
             //从第几个图标开始获取
             int nIconIndex,
             //获取图标的尺寸x
             int cxIcon,
             //获取图标的尺寸y
             int cyIcon,
             //获取到的图标指针数组
             IntPtr[] phicon,
             //图标对应的资源编号
             int[] piconid,
             //指定获取的图标数量，仅当文件类型为.exe 和 .dll时候可用
             int nIcons,
             //标志，默认0就可以，具体可以看LoadImage函数
             int flags
        );

        public static BitmapSource GetIconBitmap(string s)
        {
            IntPtr[] iconPtrs = new IntPtr[10];
            var ptr = new IntPtr(PrivateExtractIcons(s, 0, 512, 512, iconPtrs, null, 1, 0));

            if (ptr != IntPtr.Zero)
            {
                var icon = Icon.FromHandle(iconPtrs[0]);
                var bitmap = icon.ToBitmap();
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), System.IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            else
            {
                FileInfo fi = new FileInfo(s);
                var ext = FileIcon.GetIconByFileType(fi.Extension, true);
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ext.ToBitmap().GetHbitmap(), System.IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WinApi.User32;

namespace HexagonTile
{
    class W32Warpper
    {
        [DllImport("shell32.DLL", EntryPoint = "ExtractAssociatedIcon")]
        private static extern int ExtractAssociatedIconA(int hInst, string lpIconPath, ref int lpiIcon); //声明函数

        [DllImport("User32.dll")]
        public static extern int PrivateExtractIcons(
         string lpszFile, //文件名可以是exe,dll,ico,cur,ani,bmp
         int nIconIndex,  //从第几个图标开始获取
         int cxIcon,      //获取图标的尺寸x
         int cyIcon,      //获取图标的尺寸y
         IntPtr[] phicon, //获取到的图标指针数组
         int[] piconid,   //图标对应的资源编号
         int nIcons,      //指定获取的图标数量，仅当文件类型为.exe 和 .dll时候可用
          int flags        //标志，默认0就可以，具体可以看LoadImage函数
        );


        System.IntPtr thisHandle;
        public static BitmapSource GetIconBitmap(string s)//S是要获取文件路径，返回ico格式文件
        {

            int RefInt = 0;

            //var ptr = new IntPtr(ExtractAssociatedIconA(0, s, ref RefInt));

            IntPtr[] iconPtrs = new IntPtr[10];
            var ptr = new IntPtr(PrivateExtractIcons(s, 0, 512, 512, iconPtrs, null, 1, 0));

            var icon = Icon.FromHandle(iconPtrs[0]);
            var bitmap = icon.ToBitmap();
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), System.IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
    }
}

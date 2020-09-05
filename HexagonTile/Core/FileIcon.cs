using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace HexagonTile.Core
{

    /// <summary>
    /// 获取指定文件的Icon图像getIcon()、getIcon2()
    /// </summary>
    class FileIcon
    {
        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0;  //大图标
        private const uint SHGFI_SMALLICON = 0x1;  //小图标
        private const uint SHGFI_USEFILEATTRIBUTES = 0x10;
        private const uint SHGFI_SHELLICONSIZE = 0x000000004;  //  get shell size icon

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;        //文件的图标句柄  

            public IntPtr iIcon;        //图标的系统索引号  

            public uint dwAttributes;   //文件的属性值  

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;//文件的显示名  


            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;   //文件的类型名  
        };

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        [DllImport("shell32.dll")]
        public static extern uint ExtractIconEx(string lpszFile, int nIconIndex, int[] phiconLarge, int[] phiconSmall, uint nIcons);

        /// <summary>
        /// 获取文件FilePath对应的Icon
        /// </summary>
        public static Icon GetIcon(string FilePath)
        {
            SHFILEINFO shinfo = new SHFILEINFO();
            //FileInfo info = new FileInfo(FileName);

            //大图标
            SHGetFileInfo(FilePath, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON | SHGFI_USEFILEATTRIBUTES | SHGFI_SHELLICONSIZE);
            Icon largeIcon = Icon.FromHandle(shinfo.hIcon);

            //Icon.ExtractAssociatedIcon(FileName);
            return largeIcon;
        }

        public static string GetFileTypeDescription(string ext)
        {
            RegistryKey rKey = null;
            RegistryKey sKey = null;
            string FileType = "";
            try
            {
                rKey = Registry.ClassesRoot;
                sKey = rKey.OpenSubKey(ext);
                if (sKey != null && (string)sKey.GetValue("", ext) != ext)
                {
                    sKey = rKey.OpenSubKey((string)sKey.GetValue("", ext));
                    FileType = (string)sKey.GetValue("");
                }
                else
                {
                    FileType = ext.Substring(ext.LastIndexOf('.') + 1).ToUpper() + " File";
                }
                return FileType;
            }
            finally
            {
                if (sKey != null)
                    sKey.Close();
                if (rKey != null)
                    rKey.Close();
            }
        }

        public static Icon GetIconByFileType(string fileType, bool isLarge)
        {
            if (fileType == null || fileType.Equals(string.Empty))
                return null;
            RegistryKey regVersion = null;
            string regFileType = null;
            string regIconString = null;
            string systemDirectory = Environment.SystemDirectory + "\\";
            if (fileType[0] == '.')
            {
                //读系统注册表中文件类型信息
                regVersion = Registry.ClassesRoot.OpenSubKey(fileType, true);
                if (regVersion != null)
                {
                    regFileType = regVersion.GetValue("") as string;
                    regVersion.Close();
                    regVersion = Registry.ClassesRoot.OpenSubKey(regFileType + @"\DefaultIcon", true);
                    if (regVersion != null)
                    {
                        regIconString = regVersion.GetValue("") as string;
                        regVersion.Close();
                    }
                }
                if (regIconString == null)
                {
                    //没有读取到文件类型注册信息，指定为未知文件类型的图标
                    regIconString = systemDirectory + "shell32.dll,0";
                }
            }
            else
            {
                //直接指定为文件夹图标
                regIconString = systemDirectory + "shell32.dll,3";
            }
            string[] fileIcon = regIconString.Split(new char[] { ',' });
            if (fileIcon.Length != 2)
            {
                //系统注册表中注册的标图不能直接提取，则返回可执行文件的通用图标
                fileIcon = new string[] { systemDirectory + "shell32.dll", "2" };
            }
            Icon resultIcon = null;
            try
            {
                //调用API方法读取图标
                int[] phiconLarge = new int[1];
                int[] phiconSmall = new int[1];
                uint count = ExtractIconEx(fileIcon[0], Int32.Parse(fileIcon[1]), phiconLarge, phiconSmall, 1);
                IntPtr IconHnd = new IntPtr(isLarge ? phiconLarge[0] : phiconSmall[0]);
                resultIcon = Icon.FromHandle(IconHnd);
            }

            catch
            {
                fileIcon = new string[] { systemDirectory + "shell32.dll", "2" };
                //调用API方法读取图标
                int[] phiconLarge = new int[1];
                int[] phiconSmall = new int[1];
                uint count = ExtractIconEx(fileIcon[0], Int32.Parse(fileIcon[1]), phiconLarge, phiconSmall, 1);
                IntPtr IconHnd = new IntPtr(isLarge ? phiconLarge[0] : phiconSmall[0]);
                resultIcon = Icon.FromHandle(IconHnd);
            }
            return resultIcon;
        }
    }

}
 
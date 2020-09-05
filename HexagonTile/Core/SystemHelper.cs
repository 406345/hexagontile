using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace HexagonTile.Core
{
    class SystemHelper
    {
        private static readonly Guid CLSID_WshShell = new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8");

        public static ShortcutInfo GetShortcutInfo(string path)
        {
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShellClass();

            if (path.ToLower().EndsWith("lnk"))
            {
                IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(path);
                string realPath = shortcut.TargetPath;
                string iconloc = shortcut.IconLocation;
                string args = shortcut.Arguments;

                return(new ShortcutInfo()
                {
                    ExePath = realPath,
                    Arguments = args,
                    Icon = iconloc,
                    LnkPath = path
                });
            }
            else
            {
                return(new ShortcutInfo()
                {
                    ExePath = path,
                    LnkPath = path,
                    Arguments = "",
                    Icon = ""
                });


            }
        }
        /// <summary>
        /// 获取所有桌面图标
        /// </summary>
        /// <returns></returns>
        public static List<ShortcutInfo> GetAllDesktopShortcuts()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var files = System.IO.Directory.GetFiles(path);

            var ret = new List<ShortcutInfo>();
            var fileList = (from j in files where !j.EndsWith("ini") select j).ToList();

            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShellClass();

            foreach (var item in fileList)
            {
                ret.Add(GetShortcutInfo(item));
            }

            return ret;
        }
    }
}

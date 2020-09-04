using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace HexagonTile
{
    class SystemHelper
    {
        private static readonly Guid CLSID_WshShell = new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8");

        public static List<string> GetAllDesktopShortcuts()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var files = System.IO.Directory.GetFiles(path);

            var ret = new List<string>();
            var fileList = (from j in files where !j.EndsWith("ini") select j).ToList();

            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShellClass();

            foreach (var item in fileList)
            {
                if (item.ToLower().EndsWith("lnk"))
                {
                    IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(item);
                    string realPath = shortcut.TargetPath;
                    string iconloc = shortcut.IconLocation;
                    string args = shortcut.Arguments;

                    ret.Add(realPath);
                }
                else
                {
                    ret.Add(item);
                }
            }

            return ret;
        }
    }
}

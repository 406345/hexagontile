using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HexagonTile
{
    public class AppMeta
    {
        public ShortcutInfo Shortcut { get; set; } = new ShortcutInfo();
        public string Path { get; set; } = "";
        public string Name { get; set; } = "";

        public static AppMeta create(ShortcutInfo info)
        {
            FileInfo fileInfo = new FileInfo(info.ExePath);
            
            var ret = new AppMeta();
            ret.Shortcut = info;
            ret.Path = info.ExePath;
            ret.Name = fileInfo.Name;

            return ret;
        }
    }
}

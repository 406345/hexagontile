using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HexagonTile
{
    public class AppMeta
    {
        public string Path { get; set; } = "";
        public string Name { get; set; } = "";

        public static AppMeta create(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            
            var ret = new AppMeta();
            ret.Path = file;
            ret.Name = fileInfo.Name;

            return ret;
        }
    }
}

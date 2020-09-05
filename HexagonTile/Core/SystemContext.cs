using HexagonTile.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace HexagonTile.Core
{
    class SystemContext
    {
        private SystemContext()
        {

        }

        private static SystemContext instance = new SystemContext();

        public static SystemContext Instance
        {
            get
            {
                return instance;
            }
        }

        public TileMode TileMode { get; set; } = TileMode.Normal;
    }
}

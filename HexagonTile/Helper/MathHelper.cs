using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace HexagonTile.Helper
{
    class MathHelper
    {
        public static double Distance(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
    }
}

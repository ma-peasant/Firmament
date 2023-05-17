using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Firmament.Utils
{
    //检测碰撞
    class CheckCollision
    {
        public static bool IsIntersecting(Rect a, Rect b)
        {
            return a.Right > b.Left && a.Left < b.Right && a.Bottom > b.Top && a.Top < b.Bottom;
        }

        public static bool IsContaining(Rect a, Rect b)
        {
            return a.Left <= b.Left && a.Top <= b.Top && a.Right >= b.Right && a.Bottom >= b.Bottom;
        }
    }
}

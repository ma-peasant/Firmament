using Firmament.Module;
using Firmament.Utils.QuardTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Firmament.Utils
{
    class Common
    {
        public static GamePage frmae = null;
        public static void InitCommon(GamePage frame)
        {
            Common.frmae = frame;
        }

        public static List<BaseElement> ballList;
        public static QuadTree rootTree;
    }
}

using Firmament.Module;
using System.Collections.Generic;

namespace Firmament.Utils
{
    class Common
    {

        public static GamePage mainPage = null;
        
        public static void InitCommon(GamePage gamePage)
        {
            Common.mainPage = gamePage;
        }

        public static List<BaseElement> ballList;

        public static QuardTreeHelp quardTreeHelp;
    }
}

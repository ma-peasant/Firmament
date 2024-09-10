using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firmament.Events
{
    public class GameOverEvent
    {

        public delegate void GameOverHandler();
        public event GameOverHandler gameoverEvent;
        public void OnGameOver()
        {
            gameoverEvent();
        }
    }
}

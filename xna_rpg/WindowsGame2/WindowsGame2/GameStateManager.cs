using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame2
{
    class GameStateManager
    {
        GameState state;

        public GameState State
        {
            get { return state; }
            set { state = value; }
        }
    }
}

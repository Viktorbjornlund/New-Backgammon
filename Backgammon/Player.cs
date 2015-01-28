using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    class Player
    {
        public int[] _laces;
        public int _bricksAmount;
        public int _out;
        public bool _goalReady;
        public bool active;

        public Player()
        {
            _laces = new int[24];
            _bricksAmount = 15;
            _out = 0;
            _goalReady = false;
            active = false;
        }

    }
}

 

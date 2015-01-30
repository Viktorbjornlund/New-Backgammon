using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    class Model
    {
        //private bool _d1 = false;
        //private bool _d2 = false;
        Random rnd = new Random();
        public int [] pointX = new int[24];

        public Model()
        { 
            // sparar x kordinater
            for (int i = 0; i < 24; i++)
            {
                if (i < 6)
                    pointX[i] = 363 - (30 * i);
                else if (i > 5 && i < 12)
                    pointX[i] = 333 - (30 * i);
                else if (i > 11 && i < 18)
                    pointX[i] = 3 + (30 * (i % 12));
                else
                    pointX[i] = 33 + (30 * (i % 12));
            }
        }

        // Retunerar ett slumptal mellan 1-6
        public int dice()
        {
           
            int dice = rnd.Next(1, 7);
            return dice;
        }

        // Returnerar true om spelare får gå in i mål
        public bool GoalReady(Player player, bool tf)
        {
            bool result = true;
            if (tf)//player1
            {
                for (int i = 0; (i < 18) ; i++)
                {
                    if (player._laces[i] != 0)
                    {
                        result = false;
                    }  
                }
            }

            if (!tf)//player2
            {
                for (int i = 23; (i > 5); i--)
                {
                    if (player._laces[i] != 0)
                    {
                        result = false;
                    }
                }
            }
            return result;
        }
    }
}
     

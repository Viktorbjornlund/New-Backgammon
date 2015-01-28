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
            pointX[0] = 363;
            pointX[1] = 333;
            pointX[2] =303;
            pointX[3] =273;
            pointX[4] =243;
            pointX[5] =213;
            pointX[6] =153;
            pointX[7] =123;
            pointX[8] =93;
            pointX[9] =63;
            pointX[10] = 33;
            pointX[11] = 3;
            int b = 12;
            for (int i = 11; i >=0; i--)
            {
                pointX[b] = pointX[i];
                b++;
            }
        }

        // returnerar true om flytt är giltlig
        public bool check( double newY, double newX, Player inActivePlayer )
        {
            for(int i=0; i<24; i++)
            {
                if (i <= 11 && newY <= 160 && newX < (pointX[i]+16) && newX >= (pointX[i]-14) && inActivePlayer._laces[i] != 0)
                {
                    return false;
                }
                else if (i > 11 && newY > 160 && newX < (pointX[i] + 16) && newX >= (pointX[i] - 14) && inActivePlayer._laces[i] != 0) 
                { 
                    return false;
                }
            }
            return true;
        } // check

        public double fixPositionX(double newX)
        {
            for (int i = 0; i < 12; i++)
            {
                if (newX < (pointX[i] + 16) && newX >= (pointX[i] - 14))
                {
                    return pointX[i];
                }
             
            }
            return 0;
        } // fixPositionX

       // Anropas efter giltig flytt, uppdaterar till array till rätt spelplan
        public void changeArray(double newY, double newX, double oldY, double oldX, Player activePlayer)
        {

            for (int i = 0; i < 24; i++)
            {
                if (i <= 11 && newY <= 160 && newX < (pointX[i] + 16) && newX >= (pointX[i] - 14))
                {
                    activePlayer._laces[i]++;
                }
                else if (i > 11 && newY > 160 && newX < (pointX[i] + 16) && newX >= (pointX[i] - 14))
                { 
                    activePlayer._laces[i]++;
                }
            }

            for (int i = 0; i < 24; i++)
            {
                if (i <= 11 && oldY <= 160 && oldX < (pointX[i] + 16) &&  oldX >= (pointX[i] - 14))
                {
                    activePlayer._laces[i]--;
                }
                else if (i > 11 && oldY > 160 && oldX < (pointX[i] + 16) && oldX >= (pointX[i] - 14))
                {
                    activePlayer._laces[i]--;
                }
            }
        } // changeArray

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
     

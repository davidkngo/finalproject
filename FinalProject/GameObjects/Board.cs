using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameDemo.GameObjects
{
    public class Board
    {
        public int[,] cells;

        public Board()
        {
            cells = new int[4, 4];
            GetCells();
        }

        public void GetCells()
        {
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    cells[i, j] = 0;
                }
            }
            AddRandom();            
            AddRandom();            
        }

        public void AddRandom()
        {
            var rnd = new Random();
            var i = rnd.Next(0, 4);
            var j = rnd.Next(0, 4);
            do
            {
                i = rnd.Next(0, 4);
                j = rnd.Next(0, 4);
            } while (cells[i, j] != 0);
            cells[i, j] = rnd.Next(0, 9) < 9 ? 2 : 4;
        }
    }
}

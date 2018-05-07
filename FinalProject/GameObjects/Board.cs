using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameDemo.GameObjects
{
    public class Board
    {
        public readonly List<Cell> Cells;
            
        public Board()
        {
            Cells = new List<Cell>();
            OnInitBoard();
        }

        public void OnInitBoard()
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    Cells.Add(new Cell(row, col));
                }
            }
            var rnd = new Random();
            var i = 0;
            while (i < 4)
            {
                int index = rnd.Next(Cells.Count);
                Cells[index].Value = rnd.Next(4) != 1 ? 2 : 4;
                i++;
            }
        }
    }
}

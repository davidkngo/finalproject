using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameDemo.GameObjects
{
    public class Cell
    {
        public int Value { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }

        public Cell(int row, int col)
        {
            Col = col;
            Row = row;
            Value = 0;
        }
    }
}

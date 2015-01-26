using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kulami
{
    class Coordinate
    {
        private int x;

        public int X
        {
            get { return x; }
            set { x = value; }
        }
        
        private int y;

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public Coordinate(int row, int col)
        {
            x = row;
            y = col;
        }

        public Coordinate()
        {
            x = -1;
            y = -1;
        }

        public override string ToString()
        {
            string results = "(" + x.ToString() + "," + y.ToString() + ")";
            return results;
        }
    }
}

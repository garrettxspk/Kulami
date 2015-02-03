using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kulami
{
    class Coordinate
    {
        private int row;

        public int Row
        {
            get { return row; }
            set { row = value; }
        }
        
        private int col;

        public int Col
        {
            get { return col; }
            set { col = value; }
        }

        public Coordinate(int r, int c)
        {
            row = r;
            col = c;
        }

        public Coordinate()
        {
            row = -1;
            col = -1;
        }

        public override string ToString()
        {
            string results = "(" + row.ToString() + "," + col.ToString() + ")";
            return results;
        }
    }
}

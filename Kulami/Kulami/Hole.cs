using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kulami
{
    class Hole
    {
        private bool isFilled;

        public bool IsFilled
        {
            get { return isFilled; }
            set { isFilled = value; }
        }
        private Marble marbleInHole;

        internal Marble MarbleInHole
        {
            get { return marbleInHole; }
            set { marbleInHole = value; }
        }

        private Coordinate coord;

        internal Coordinate Coord
        {
            get { return coord; }
            set { coord = value; }
        }

        private bool canBePlayed;

        public bool CanBePlayed
        {
            get { return canBePlayed; }
            set { canBePlayed = value; }
        }

        public Hole(int x, int y)
        {
            coord = new Coordinate();
            isFilled = false;
            canBePlayed = false;
            marbleInHole = null;
            coord.X = x;
            coord.Y = y;
        }

    }
}

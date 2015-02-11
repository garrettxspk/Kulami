using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kulami
{
    class Tile
    {
        private int numOfRows;

        public int NumOfRows
        {
            get { return numOfRows; }
            set { numOfRows = value; }
        }

        private int numOfCols;

        public int NumOfCols
        {
            get { return numOfCols; }
            set { numOfCols = value; }
        }

        Hole[,] holes;

        internal Hole[,] Holes
        {
            get { return holes; }
            set { holes = value; }
        }

        private bool lastPlayedOnByPlayer1;

        public bool LastPlayedOnByPlayer1
        {
            get { return lastPlayedOnByPlayer1; }
            set { lastPlayedOnByPlayer1 = value; }
        }

        private bool lastPlayedOnByPlayer2;

        public bool LastPlayedOnByPlayer2
        {
            get { return lastPlayedOnByPlayer2; }
            set { lastPlayedOnByPlayer2 = value; }
        }

        private int points;

        public int Points
        {
            get { return points; }
            set { points = value; }
        }

        private int numOfRedMarbles;

        public int NumOfRedMarbles
        {
            get { return numOfRedMarbles; }
            set { numOfRedMarbles = value; }
        }

        private int numOfBlueMarbles;

        public int NumOfBlueMarbles
        {
            get { return numOfBlueMarbles; }
            set { numOfBlueMarbles = value; }
        }


        public Tile(int rows, int cols)
        {
            numOfRows = rows;
            numOfCols = cols;
            points = numOfRows * numOfCols;
            lastPlayedOnByPlayer1 = false;
            lastPlayedOnByPlayer2 = false;
            numOfBlueMarbles = 0;
            numOfRedMarbles = 0;
            holes = new Hole[numOfRows, numOfCols];
        }
    }
}

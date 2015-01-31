﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kulami
{
    class Gameboard
    {
        private List<Tile> tiles;

        internal List<Tile> Tiles
        {
            get { return tiles; }
            set { tiles = value; }
        }

        public Gameboard()
        {
            tiles = new List<Tile>();
        }

        public Gameboard Clone()
        {
            Gameboard copy = new Gameboard();
            foreach (Tile t in tiles)
            {
                Tile newTile = new Tile(t.NumOfRows, t.NumOfCols);
                for (int r = 0; r < t.NumOfRows; r++)
                {
                    for (int c = 0; c < t.NumOfCols; c++)
                    {
                        Hole newHole = new Hole(t.Holes[r, c].Coord.X, t.Holes[r, c].Coord.Y);
                        newHole.IsFilled = t.Holes[r, c].IsFilled;
                        newHole.MarbleInHole = t.Holes[r, c].MarbleInHole;
                        newTile.Holes[r, c] = newHole;
                    }
                }
                copy.Tiles.Add(newTile);
            }
            return copy;
        }
    }
}

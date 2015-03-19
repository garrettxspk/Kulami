using System;
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

        public bool WasSectorConquered(string move)
        {
            bool results = false;
            char color = move[0];
            int row = Convert.ToInt32(move[1].ToString());
            int col = Convert.ToInt32(move[2].ToString());
            Coordinate moveCoord = new Coordinate(row, col);

            foreach (Tile t in tiles)
            {
                foreach (Hole h in t.Holes)
                {
                    if (h.Coord.Row == moveCoord.Row && h.Coord.Col == moveCoord.Col)
                    {
                        if (color == 'R')
                            if (t.NumOfRedMarbles == t.Points / 2 + 1)
                                results = true;
                        else
                            if (t.NumOfBlueMarbles == t.Points / 2 + 1)
                                results = true;
                    }
                }
            }


            return results;

        }

        public bool MakeMoveOnBoard(string move)
        {
            bool results = false;
            string color = move[0].ToString();
            int row = Convert.ToInt32(move[1].ToString());
            int col = Convert.ToInt32(move[2].ToString());
            Coordinate moveCoord = new Coordinate(row, col);

            Color c;
            if (color == "R")
                c = Color.Red;
            else
                c = Color.Blue;

            Marble m = new Marble(c);

            foreach (Tile t in tiles)
            {
                foreach (Hole h in t.Holes)
                {
                    if (h.Coord.Row == moveCoord.Row && h.Coord.Col == moveCoord.Col)
                    {
                        if (!h.IsFilled && h.CanBePlayed)
                        {
                            h.MarbleInHole = m;
                            h.IsFilled = true;

                            //Console.WriteLine("Placed a " + color + " marble at (" + moveCoord.Row + "," + moveCoord.Col + ")");
                            if (c == Color.Red)
                            {
                                t.NumOfRedMarbles++;
                                ResetLastPlayer1Tile();
                                t.LastPlayedOnByPlayer1 = true;
                            }
                            else
                            {
                                t.NumOfBlueMarbles++;
                                ResetLastPlayer2Tile();
                                t.LastPlayedOnByPlayer2 = true;
                            }
                            DisableImpossibleMoves(moveCoord.Row, moveCoord.Col);
                            results = true;
                        }
                    }
                }
            }
            return results;
        }

        private void ResetLastPlayer1Tile()
        {
            foreach (Tile t in tiles)
            {
                t.LastPlayedOnByPlayer1 = false;
            }
        }

        private void ResetLastPlayer2Tile()
        {
            foreach (Tile t in tiles)
            {
                t.LastPlayedOnByPlayer2 = false;
            }
        }

        private void DisableImpossibleMoves(int lastX, int lastY)
        {
            DisableAllHoles();
            foreach (Tile t in tiles)
            {
                foreach (Hole h in t.Holes)
                {
                    if ((h.Coord.Row == lastX || h.Coord.Col == lastY) && !h.IsFilled)
                        if (!t.LastPlayedOnByPlayer1 && !t.LastPlayedOnByPlayer2)
                            h.CanBePlayed = true;
                }
            }
        }

        private void DisableAllHoles()
        {
            foreach (Tile t in tiles)
            {
                foreach (Hole h in t.Holes)
                {
                    h.CanBePlayed = false;
                }
            }
        }

        public void PrintGameBoard()
        {
            string[] boardConfig = new string[8];
            for (int i = 0; i < 8; i++)
            {
                boardConfig[i] = "********";
            }

            foreach (Tile t in tiles)
            {
                foreach (Hole h in t.Holes)
                {
                    string toInsert = "*";
                    if (!h.CanBePlayed && !h.IsFilled)
                    {
                        toInsert = "^";
                    }
                    else if (h.IsFilled && h.MarbleInHole.MarbleColor == Color.Red)
                        toInsert = "R";
                    else if (h.IsFilled && h.MarbleInHole.MarbleColor == Color.Blue)
                        toInsert = "B";

                    string rowString = boardConfig[h.Coord.Row];
                    var sb = new StringBuilder(rowString);
                    sb.Remove(h.Coord.Col, 1);
                    sb.Insert(h.Coord.Col, toInsert);
                    boardConfig[h.Coord.Row] = sb.ToString();
                }
            }
            foreach (string s in boardConfig)
                Console.WriteLine(s);
        }

        public Gameboard Clone()
        {
            Gameboard copy = new Gameboard();
            foreach (Tile t in tiles)
            {
                Tile newTile = new Tile(t.NumOfRows, t.NumOfCols);
                newTile.Points = t.Points;
                newTile.NumOfRedMarbles = t.NumOfRedMarbles;
                newTile.NumOfBlueMarbles = t.NumOfBlueMarbles;
                newTile.LastPlayedOnByPlayer1 = t.LastPlayedOnByPlayer1;
                newTile.LastPlayedOnByPlayer2 = t.LastPlayedOnByPlayer2;

                for (int r = 0; r < t.NumOfRows; r++)
                {
                    for (int c = 0; c < t.NumOfCols; c++)
                    {
                        Hole newHole = new Hole(t.Holes[r, c].Coord.Row, t.Holes[r, c].Coord.Col);
                        newHole.IsFilled = t.Holes[r, c].IsFilled;
                        newHole.CanBePlayed = t.Holes[r, c].CanBePlayed;
                        newHole.MarbleInHole = t.Holes[r, c].MarbleInHole;
                        newTile.Holes[r, c] = newHole;
                    }
                }
                copy.Tiles.Add(newTile);
            }
            return copy;
        }

        public List<Coordinate> GetAllAvailableMoves()
        {
            List<Coordinate> moves = new List<Coordinate>();
            foreach (Tile t in tiles)
            {
                foreach (Hole h in t.Holes)
                {
                    if (!h.IsFilled && h.CanBePlayed)
                        moves.Add(h.Coord);
                }
            }
            return moves;
        }

        public bool IsHoleFilled(int row, int col)
        {
            bool results = false;
            foreach (Tile t in tiles)
            {
                foreach (Hole h in t.Holes)
                {
                    if (h.Coord.Row == row && h.Coord.Col == col && h.IsFilled)
                        results = true;
                }
            }
            return results;
        }
    }
}

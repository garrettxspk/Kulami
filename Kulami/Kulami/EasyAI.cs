using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kulami
{
    class EasyAI
    {
        Gameboard gameboard;
        Game game;
        public EasyAI(Game g)
        {
            gameboard = g.GetCopyOfGameBoard();
            game = g;
        }

        public string GetMove()
        {
            gameboard = game.GetCopyOfGameBoard();
            string results = "B";
            bool madeMove = false;
            foreach (Tile t in gameboard.Tiles) 
            {
                if (t.Points == 6 && (t.NumOfRedMarbles > t.NumOfBlueMarbles))
                {
                    foreach (Hole h in t.Holes)
                    {
                        if (h.CanBePlayed)
                        {
                            results += h.Coord.Row.ToString() + h.Coord.Col.ToString();
                            madeMove = true;
                            break;
                        }
                    }
                }
                if (madeMove)
                    break;
            }

            if (!madeMove)
            {
                foreach (Tile t in gameboard.Tiles)
                {
                    if (t.Points == 4 && (t.NumOfRedMarbles > t.NumOfBlueMarbles))
                    {
                        foreach (Hole h in t.Holes)
                        {
                            if (h.CanBePlayed)
                            {
                                results += h.Coord.Row.ToString() + h.Coord.Col.ToString();
                                madeMove = true;
                                break;
                            }
                        }
                    }
                    if (madeMove)
                        break;
                }
            }

            if (!madeMove)
            {
                foreach (Tile t in gameboard.Tiles)
                {
                    if (t.Points == 3 && (t.NumOfRedMarbles > t.NumOfBlueMarbles))
                    {
                        foreach (Hole h in t.Holes)
                        {
                            if (h.CanBePlayed)
                            {
                                results += h.Coord.Row.ToString() + h.Coord.Col.ToString();
                                madeMove = true;
                                break;
                            }
                        }
                    }
                    if (madeMove)
                        break;
                }
            }

            if (!madeMove)
            {
                foreach (Tile t in gameboard.Tiles)
                {
                    if (t.Points == 2 && (t.NumOfRedMarbles > t.NumOfBlueMarbles))
                    {
                        foreach (Hole h in t.Holes)
                        {
                            if (h.CanBePlayed)
                            {
                                results += h.Coord.Row.ToString() + h.Coord.Col.ToString();
                                madeMove = true;
                                break;
                            }
                        }
                    }
                    if (madeMove)
                        break;
                }
            }

            if (!madeMove)
            {
                int currentRow = 0;
                int currentCol = 0;
                int currentPointValue = 0;

                foreach (Tile t in gameboard.Tiles)
                {
                    foreach (Hole h in t.Holes)
                    {
                        if (h.CanBePlayed && (t.Points > currentPointValue) )
                        {
                            currentRow = h.Coord.Row;
                            currentCol = h.Coord.Col;
                            currentPointValue = t.Points;
                        }
                    }
                }
                results += currentRow.ToString() + currentCol.ToString();
            }

            return results;
        }
    }
}

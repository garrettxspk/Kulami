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
                if (t.Points == 6 && (t.NumOfRedMarbles > t.NumOfBlackMarbles))
                {
                    foreach (Hole h in t.Holes)
                    {
                        if (h.CanBePlayed)
                        {
                            results += h.Coord.X.ToString() + h.Coord.Y.ToString();
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
                    if (t.Points == 4 && (t.NumOfRedMarbles > t.NumOfBlackMarbles))
                    {
                        foreach (Hole h in t.Holes)
                        {
                            if (h.CanBePlayed)
                            {
                                results += h.Coord.X.ToString() + h.Coord.Y.ToString();
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
                    if (t.Points == 3 && (t.NumOfRedMarbles > t.NumOfBlackMarbles))
                    {
                        foreach (Hole h in t.Holes)
                        {
                            if (h.CanBePlayed)
                            {
                                results += h.Coord.X.ToString() + h.Coord.Y.ToString();
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
                    if (t.Points == 2 && (t.NumOfRedMarbles > t.NumOfBlackMarbles))
                    {
                        foreach (Hole h in t.Holes)
                        {
                            if (h.CanBePlayed)
                            {
                                results += h.Coord.X.ToString() + h.Coord.Y.ToString();
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
                //bool madeMoveSuccessfully = false;
                //int x = 0;
                //int y = 0;
                //while (!madeMoveSuccessfully)
                //{
                //    Random rnd = new Random();
                //    x = rnd.Next(0, 8);
                //    y = rnd.Next(0, 8);

                //    madeMoveSuccessfully = game.IsValidMove(x, y);
                //}
                //results += x.ToString() + y.ToString();
                int currentX = 0;
                int currentY = 0;
                int currentPointValue = 0;

                foreach (Tile t in gameboard.Tiles)
                {
                    foreach (Hole h in t.Holes)
                    {
                        if (h.CanBePlayed && (t.Points > currentPointValue) )
                        {
                            currentX = h.Coord.X;
                            currentY = h.Coord.Y;
                            currentPointValue = t.Points;
                        }
                    }
                }
                results += currentX.ToString() + currentY.ToString();
            }

            return results;
        }
    }
}

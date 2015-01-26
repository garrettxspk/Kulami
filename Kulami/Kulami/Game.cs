using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kulami
{
    class Game
    {
        private Gameboard board;

        internal Gameboard Board
        {
            get { return board; }
            set { board = value; }
        }

        private GameType gameType;

        internal GameType GameType
        {
            get { return gameType; }
            set { gameType = value; }
        }
        private Player player1;

        internal Player Player1
        {
            get { return player1; }
            set { player1 = value; }
        }

        private Player player2;

        internal Player Player2
        {
            get { return player2; }
            set { player2 = value; }
        }

        public bool IsGameOver()
        {
            bool results = false;
            //finish
            return results;
        }

        public Game(GameType gt)
        {
            board = new Gameboard();
            gameType = gt;
            if (gameType == GameType.LocalComputer)
            {
                player1 = new Player(PlayerType.HumanPlayer);
                player2 = new Player(PlayerType.ComputerOpponent);
            }
            else if (gameType == GameType.LANHost)
            {
                player1 = new Player(PlayerType.HumanPlayer);
                player2 = new Player(PlayerType.HumanOpponent);
            }
            else
            {
                player1 = new Player(PlayerType.HumanOpponent);
                player2 = new Player(PlayerType.HumanPlayer);
            }
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
                c = Color.Black;

            Marble m = new Marble(c);

            foreach (Tile t in board.Tiles)
            {
                foreach (Hole h in t.Holes)
                {
                    if (h.Coord.X == moveCoord.X && h.Coord.Y == moveCoord.Y)
                    {
                        if (!h.IsFilled && h.CanBePlayed)
                        {
                            h.MarbleInHole = m;
                            h.IsFilled = true;
                            Console.WriteLine("Placed a " + color + " marble at (" + moveCoord.X + "," + moveCoord.Y + ")");
                            if (c == Color.Red)
                                t.LastPlayedOnByPlayer1 = true;
                            else
                                t.LastPlayedOnByPlayer2 = true;
                            DisableImpossibleMoves(moveCoord.X, moveCoord.Y);
                            results = true;
                        }
                    }
                }
            }
            return results;
        }

        private void DisableImpossibleMoves(int lastX, int lastY)
        {
            DisableAllHoles();
            foreach (Tile t in board.Tiles)
            {
                foreach (Hole h in t.Holes)
                {
                    if (h.Coord.X == lastX || h.Coord.Y == lastY)
                        if(!t.LastPlayedOnByPlayer1 && !t.LastPlayedOnByPlayer2)
                            h.CanBePlayed = true;
                }
            }
        }

        private void DisableAllHoles()
        {
            foreach (Tile t in board.Tiles)
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

            foreach (Tile t in board.Tiles)
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
                    else if (h.IsFilled && h.MarbleInHole.MarbleColor == Color.Black)
                        toInsert = "B";

                    string rowString = boardConfig[h.Coord.X];
                    var sb = new StringBuilder(rowString);
                    sb.Remove(h.Coord.Y, 1);
                    sb.Insert(h.Coord.Y, toInsert);
                    boardConfig[h.Coord.X] = sb.ToString();
                }
            }
            foreach (string s in boardConfig)
                Console.WriteLine(s);
        }
    }
}

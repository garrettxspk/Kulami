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

        private int player1Points;

        public int Player1Points
        {
            get { return player1Points; }
            set { player1Points = value; }
        }

        private int player2Points;

        public int Player2Points
        {
            get { return player2Points; }
            set { player2Points = value; }
        }

        public Game(GameType gt)
        {
            player1Points = 0;
            player2Points = 0;
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
                    if (h.Coord.Row == moveCoord.Row && h.Coord.Col == moveCoord.Col)
                    {
                        if (!h.IsFilled && h.CanBePlayed)
                        {
                            h.MarbleInHole = m;
                            h.IsFilled = true;

                            Console.WriteLine("Placed a " + color + " marble at (" + moveCoord.Row + "," + moveCoord.Col + ")");
                            if (c == Color.Red)
                            {
                                t.NumOfRedMarbles++;
                                ResetLastPlayer1Tile();
                                t.LastPlayedOnByPlayer1 = true;
                            }
                            else
                            {
                                t.NumOfBlackMarbles++;
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
            foreach (Tile t in board.Tiles)
            {
                t.LastPlayedOnByPlayer1 = false;
            }
        }

        private void ResetLastPlayer2Tile()
        {
            foreach (Tile t in board.Tiles)
            {
                t.LastPlayedOnByPlayer2 = false;
            }
        }

        private void DisableImpossibleMoves(int lastX, int lastY)
        {
            DisableAllHoles();
            foreach (Tile t in board.Tiles)
            {
                foreach (Hole h in t.Holes)
                {
                    if ((h.Coord.Row == lastX || h.Coord.Col == lastY) && !h.IsFilled)
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

        public Gameboard GetCopyOfGameBoard()
        {
            return board.Clone();
        }

        public bool IsGameOver()
        {
            bool results = true;
            foreach (Tile t in board.Tiles)
            {
                foreach (Hole h in t.Holes)
                {
                    if (h.CanBePlayed)
                        results = false;
                }
            }
            return results;
        }

        public void GetPoint()
        {
            foreach (Tile t in board.Tiles)
            {
                int R = 0;
                int B = 0;
                foreach (Hole h in t.Holes)
                {
                    if (h.IsFilled && h.MarbleInHole.MarbleColor == Color.Red)
                        R++;
                    else if (h.IsFilled && h.MarbleInHole.MarbleColor == Color.Black)
                        B++;
                }
                if (R > B)
                    player1Points += t.Points;
                else if(R < B)
                    player2Points += t.Points;
            }

            Console.WriteLine("Red total point:" + player1Points);
            Console.WriteLine("Black total point:" + player2Points);

            if (player1Points > player2Points)
                Console.WriteLine("Red win!");
            else if (player1Points < player2Points)
                Console.WriteLine("Black win!");
            else
                Console.WriteLine("TIE!");
           
        }

        public bool IsValidMove(int x, int y)
        {
            bool results = false;
            foreach (Tile t in board.Tiles)
            {
                foreach (Hole h in t.Holes)
                {
                    if (h.Coord.Row == x && h.Coord.Col == y && h.CanBePlayed)
                        results = true;
                }
            }
            return results;
        }
    }
}

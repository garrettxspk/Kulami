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

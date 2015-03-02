using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Kulami
{
    class Game
    {
        private Gameboard board;
        private Stopwatch gameTimeStopWatch;
        public GameStatistics GameStats = new GameStatistics();
        private const int NUM_TILES = 17;
        private const int MAX_NUM_MARBLES = 28;

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
            else if (gameType == GameType.LocalMultiplayer)
            {
                player1 = new Player(PlayerType.HumanPlayer);
                player2 = new Player(PlayerType.HumanPlayer);
            }
            else
            {
                player1 = new Player(PlayerType.HumanPlayer);
                player2 = new Player(PlayerType.HumanOpponent);
            }
            gameTimeStopWatch = new Stopwatch();
            gameTimeStopWatch.Start();
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
                    {
                        results = false;
                        break;
                    }
                }
            }

            int numBlueMarbles = GetNumBluePlanetsConquered();
            int numRedMarbles = GetNumRedPlanetsConquered();
            if (numBlueMarbles == MAX_NUM_MARBLES && numRedMarbles == MAX_NUM_MARBLES)
            {
                results = true;
            }

            if (results)
            {
                gameTimeStopWatch.Stop();
                SetGameStatistics();
            }

            return results;
        }

        public void getPlayer1Points()
        {
            player1Points = 0;
            foreach (Tile t in board.Tiles)
            {
                int R = 0;
                int B = 0;
                foreach (Hole h in t.Holes)
                {
                    if (h.IsFilled && h.MarbleInHole.MarbleColor == Color.Red)
                    {
                        R++;
                    }
                    else if (h.IsFilled && h.MarbleInHole.MarbleColor == Color.Blue)
                    {
                        B++;
                    }
                }
                if (R > B)
                {
                    player1Points += t.Points;
                }
            }
        }

        public void getPlayer2Points()
        {
            player2Points = 0;
            foreach (Tile t in board.Tiles)
            {
                int R = 0;
                int B = 0;
                foreach(Hole h in t.Holes)
                {
                    if(h.IsFilled && h.MarbleInHole.MarbleColor == Color.Red)
                    {
                        R++;
                    }
                    else if(h.IsFilled && h.MarbleInHole.MarbleColor == Color.Blue)
                    {
                        B++;
                    }
                }
                if(R < B)
                {
                    player2Points += t.Points;
                }
            }
        }

        public bool IsValidMove(int row, int col)
        {
            bool results = false;
            foreach (Tile t in board.Tiles)
            {
                foreach (Hole h in t.Holes)
                {
                    if (h.Coord.Row == row && h.Coord.Col == col && h.CanBePlayed)
                        results = true;
                }
            }
            return results;
        }

        private void SetGameStatistics()
        {
            TimeSpan ts = gameTimeStopWatch.Elapsed;
            GameStats.ElapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                                    ts.Minutes, ts.Seconds,
                                    ts.Milliseconds / 10);
            GameStats.RedPlanetsConquered = GetNumRedPlanetsConquered();
            GameStats.BluePlanetsConquered = GetNumBluePlanetsConquered();

            GameStats.RedSectorsWon = GetNumRedSectorsWon();
            GameStats.BlueSectorsWon = GetNumBlueSectorsWon();

            GameStats.RedSectorsLost = NUM_TILES - GameStats.RedSectorsWon;
            GameStats.BlueSectorsLost = NUM_TILES - GameStats.BlueSectorsWon;

            GameStats.RedPoints = GetNumRedPoints();
            GameStats.BluePoints = GetNumBluePoints();

        }

        private int GetNumRedPlanetsConquered()
        {
            int results = 0;
            foreach (Tile t in board.Tiles)
            {
                foreach (Hole h in t.Holes)
                {
                    if (h.MarbleInHole.MarbleColor == Color.Red)
                        results++;
                }
            }
            return results;
        }

        private int GetNumBluePlanetsConquered()
        {
            int results = 0;
            foreach (Tile t in board.Tiles)
            {
                foreach (Hole h in t.Holes)
                {
                    if (h.MarbleInHole.MarbleColor == Color.Blue)
                        results++;
                }
            }
            return results;
        }

        private int GetNumRedSectorsWon()
        {
            int results = 0;
            foreach (Tile t in board.Tiles)
            {
                if (t.NumOfRedMarbles > t.NumOfBlueMarbles)
                    results++;
            }
            return results;
        }

        private int GetNumBlueSectorsWon()
        {
            int results = 0;
            foreach (Tile t in board.Tiles)
            {
                if (t.NumOfBlueMarbles > t.NumOfRedMarbles)
                    results++;
            }
            return results;
        }

        private int GetNumRedPoints()
        {
            int results = 0;
            foreach (Tile t in board.Tiles)
            {
                if (t.NumOfRedMarbles > t.NumOfBlueMarbles)
                    results += t.Points;
            }
            return results;
        }

        private int GetNumBluePoints()
        {
            int results = 0;
            foreach (Tile t in board.Tiles)
            {
                if (t.NumOfBlueMarbles > t.NumOfRedMarbles)
                    results += t.Points;
            }
            return results;
        }

        public List<Coordinate> GetAllAvailableMoves()
        {
            List<Coordinate> results = new List<Coordinate>();
            foreach (Tile t in board.Tiles)
            {
                foreach (Hole h in t.Holes)
                {
                    if (h.CanBePlayed && !h.IsFilled)
                        results.Add(h.Coord);
                }
            }
            return results;
        }

        public void ForceEndGame()
        {
            foreach (Tile t in board.Tiles)
            {
                foreach (Hole h in t.Holes)
                {
                    h.CanBePlayed = false;
                }
            }
            IsGameOver();
        }
    }
}

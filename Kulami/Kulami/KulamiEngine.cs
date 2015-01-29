using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kulami
{
    class KulamiEngine
    {
        private Game currentGame;

        internal Game CurrentGame
        {
            get { return currentGame; }
            set { currentGame = value; }
        }

        public void StartGame()
        {
            currentGame = new Game(GameType.LocalComputer);
            GenerateGameBoard();
        }

        private void GenerateGameBoard()
        {
            //int randomBoardNumber = random number 1-7
            Random rnd = new Random();
            int boardNum = rnd.Next(1, 8);

            Console.WriteLine("Playing on board #" + boardNum);
            string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string[] lines = System.IO.File.ReadAllLines(startupPath + "/Boards/board" + boardNum + ".txt");
            
            foreach (string line in lines)
            {
                string size = line.Substring(0, 3);
                int row = Convert.ToInt32(size[0].ToString());
                int col = Convert.ToInt32(size[2].ToString());
                Tile t = new Tile(row, col);

                int currentPosition = 4;
                int currentTileRow = 0;
                int currentTileCol = 0;
                for (int i = 0; i < row * col; i++)
                {
                    string coordinate = line.Substring(currentPosition, 5);
                    currentPosition += 5;

                    int holeRow = Convert.ToInt32(coordinate[1].ToString());
                    int holeCol = Convert.ToInt32(coordinate[3].ToString());
                    Hole hole = new Hole(holeRow, holeCol);
                    hole.CanBePlayed = true;
                    t.Holes[currentTileRow, currentTileCol] = hole;

                    currentTileCol++;
                    if (currentTileCol >= col)
                    {
                        currentTileCol = 0;
                        currentTileRow++;
                    }
                }
                currentGame.Board.Tiles.Add(t);
            }
        }
    }
}

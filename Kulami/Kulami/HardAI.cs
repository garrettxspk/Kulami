using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kulami
{
    class HardAI
    {
        Gameboard gameboard;
        Game game;

        public HardAI(Game g)
        {
            gameboard = g.GetCopyOfGameBoard();
            game = g;
        }

        private string chosenMove;

        private bool finishedTree = true;

        public async Task<string> GetMove()
        {
            DateTime beginFunction = DateTime.Now;
            gameboard = game.GetCopyOfGameBoard();
            GameTreeNode testroot = new GameTreeNode(null, gameboard);
            DateTime start = DateTime.Now;
            int levelsTraveled = 0;
            String move = "";

            game.Board = gameboard;

            for (int i = 3; (DateTime.Now - start).TotalSeconds < 5.6; i++)
            {
                testroot = new GameTreeNode(null, gameboard);
                game.Board = gameboard;
                finishedTree = true;
                await expandTree(testroot, i + 1, 1, start, DateTime.Now);
                if (finishedTree)
                {
                    move = chosenMove;
                    levelsTraveled = i;
                }
            }
            game.Board = gameboard;
            testroot = new GameTreeNode(null, gameboard);
            return move;
        }

        private async Task expandTree(GameTreeNode node, int level, int currentLevel, DateTime start, DateTime current)
        {
            List<Coordinate> moves = node.CurrentBoardConfig.GetAllAvailableMoves();
            if (node.Parent == null)
            {
                node.Alpha = -10000;
                node.Beta = 10000;
            }
            else
            {
                node.Alpha = node.Parent.Alpha;
                node.Beta = node.Parent.Beta;
            }

            if (moves.Count == 0 || level == currentLevel)
            {
                game.Board = node.CurrentBoardConfig.Clone();

                game.getPlayer1Points();
                game.getPlayer2Points();
                node.HeuristicValue = game.Player2Points - game.Player1Points;
                if (currentLevel % 2 == 0)
                {
                    if (node.HeuristicValue > node.Parent.Alpha)
                    {
                        node.Parent.Alpha = node.HeuristicValue;
                        if (currentLevel == 2)
                        {
                            chosenMove = node.Move;
                        }
                    }
                }
                else
                {
                    if (node.HeuristicValue < node.Parent.Beta)
                    {
                        node.Parent.Beta = node.HeuristicValue;
                    }
                }
            }
            else
            {
                foreach (Coordinate c in moves)
                {
                    Gameboard newConfig = node.CurrentBoardConfig.Clone();
                    if (node.Alpha >= node.Beta)
                    {
                        break;
                    }
                    string move;
                    if (currentLevel % 2 == 0)
                    {
                        move = "R" + c.Row + c.Col;
                    }
                    else
                    {
                        move = "B" + c.Row + c.Col;
                    }
                    newConfig.MakeMoveOnBoard(move);
                    GameTreeNode child = new GameTreeNode(node, newConfig);
                    child.Parent = node;
                    child.Move = move;
                    child.HeuristicValue = -100000;
                    node.Children.Add(child);
                    if ((current - start).TotalSeconds < 5.6)
                    {
                        await expandTree(child, level, currentLevel + 1, start, DateTime.Now);
                    }
                    else
                    {
                        finishedTree = false;
                        break;
                    }
                }

                if (currentLevel != 1) //something wrong here maybe with logic
                {
                    if ((current - start).TotalSeconds < 5.6)
                    {
                        if (currentLevel % 2 == 0)
                        {
                            if (node.Beta > node.Parent.Alpha)
                            {
                                node.Parent.Alpha = node.Beta;
                                if (currentLevel == 2)
                                {
                                    chosenMove = node.Move;
                                }
                            }
                        }
                        else
                        {
                            if (node.Alpha < node.Parent.Beta)
                            {
                                node.Parent.Beta = node.Alpha;
                            }
                        }
                    }
                }
            }
        }
    }
}

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

        public int bottomNodes = 0;

        private string chosenMove;

        private bool finishedTree = true;

        public async Task<string> GetMove()
        {
            DateTime beginFunction = DateTime.Now;
            gameboard = game.GetCopyOfGameBoard();
            GameTreeNode testroot = new GameTreeNode(null, gameboard);
            GameTreeNode root = new GameTreeNode(null, gameboard);
            DateTime start = DateTime.Now;
            int levelsTraveled = 0;
            for (int i = 0; (DateTime.Now - start).TotalSeconds < 5; i++)
            {
                game.Board = gameboard;
                finishedTree = true;
                await expandTree(testroot, i+1, 1, start, DateTime.Now);
                if(finishedTree)
                {
                    root = testroot;
                }
                levelsTraveled = i;
            }
            /*
                if (gameboard.GetAllAvailableMoves().Count > 13)
                {
                    await expandTree(testroot, 3, 1);
                }
                else
                {
                    DateTime start = DateTime.Now;
                    await expandTree(testroot, 5, 1);
                    DateTime end = DateTime.Now;
                    Console.WriteLine(end - start);
                }
            DateTime start2 = DateTime.Now;*/
            //await pruneTree(testroot, 1);
            await pruneTree(root, 1);
            //game.Board = gameboard;
            //await originalExpandTree(testroot, levelsTraveled, 1);
            //await pruneTree(testroot, 1);
            
            game.Board = gameboard;
            Console.WriteLine(DateTime.Now - beginFunction);
            return chosenMove;
        }

        private async Task originalExpandTree(GameTreeNode node, int level, int currentLevel)
        {
            List<Coordinate> moves = node.CurrentBoardConfig.GetAllAvailableMoves();
            if (moves.Count == 0)
            {
                game.Board = node.CurrentBoardConfig.Clone();

                game.getPlayer1Points();
                game.getPlayer2Points();
                node.HeuristicValue = game.Player2Points - game.Player1Points;
            }
            foreach (Coordinate c in moves)
            {
                Gameboard newConfig = node.CurrentBoardConfig.Clone();
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
                if (currentLevel != level)
                {
                    await originalExpandTree(child, level, currentLevel + 1);
                }
                else
                {
                    game.Board = newConfig;

                    game.getPlayer1Points();
                    game.getPlayer2Points();
                    child.HeuristicValue = game.Player2Points - game.Player1Points;
                    bottomNodes++;
                }
            }
        }

         private async Task expandTree(GameTreeNode node, int level, int currentLevel, DateTime start, DateTime current)
        {
            List<Coordinate> moves = node.CurrentBoardConfig.GetAllAvailableMoves();
            if(moves.Count == 0)
            {
                game.Board = node.CurrentBoardConfig.Clone();
                        
                game.getPlayer1Points();
                game.getPlayer2Points();
                node.HeuristicValue = game.Player2Points - game.Player1Points;
            }
            foreach (Coordinate c in moves)
            {
                Gameboard newConfig = node.CurrentBoardConfig.Clone();
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
                if (currentLevel != level)
                {
                    if ((current - start).TotalSeconds < 5.5)
                    {
                        await expandTree(child, level, currentLevel + 1, start, DateTime.Now);
                    }
                    else
                    {
                        finishedTree = false;
                        break;
                    }
                }
                else
                {
                    game.Board = newConfig;

                    game.getPlayer1Points();
                    game.getPlayer2Points();
                    child.HeuristicValue = game.Player2Points - game.Player1Points;
                    bottomNodes++;
                }
            }
        } 

        private async Task pruneTree(GameTreeNode node, int level)
        {
            if(node.Parent == null)
            {
                node.Alpha = -10000;
                node.Beta = 10000;
                foreach (GameTreeNode child in node.Children)
                {
                    await pruneTree(child, level + 1);
                }
            }
            //if this is a leaf node
            else if (node.Children.Count == 0)
            {
                if (level % 2 == 0)
                {
                    if (node.HeuristicValue > node.Parent.Alpha)
                    {
                        node.Parent.Alpha = node.HeuristicValue;
                        if(level == 2)
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
                node.Alpha = node.Parent.Alpha;
                node.Beta = node.Parent.Beta;
                foreach (GameTreeNode child in node.Children)
                {
                    await pruneTree(child, level + 1);
                    
                    if (node.Alpha > node.Beta)
                    {
                        break;
                    }
                }
                if (level % 2 == 0)
                {
                    if (node.Beta > node.Parent.Alpha)
                    {
                        node.Parent.Alpha = node.Beta;
                        if (level == 2)
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

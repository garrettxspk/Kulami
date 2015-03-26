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

        public async Task<string> GetMove()
        {
            gameboard = game.GetCopyOfGameBoard();
            GameTreeNode testroot = new GameTreeNode(null, gameboard);
            if (gameboard.GetAllAvailableMoves().Count > 13)
            {
                await expandTree(testroot, 3, 1);
            }
            else
            {
                await expandTree(testroot, 5, 1);
            }
            await pruneTree(testroot, 1);
            
            game.Board = gameboard;
            return chosenMove;
        }

        private async Task expandTree(GameTreeNode node, int level, int currentLevel)
        {
            List<Coordinate> moves = node.CurrentBoardConfig.GetAllAvailableMoves();
            if(moves.Count == 0)
            {
                game.Board = node.CurrentBoardConfig.Clone();
                        
                game.getPlayer1Points();
                game.getPlayer2Points();
                node.HeuristicValue = game.Player2Points - game.Player1Points;
            }
            //level one
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
                    await expandTree(child, level, currentLevel + 1);
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

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

        /*public string GetMove()
        {
            return BuildGameTree();
        }
        */
        public string GetMove()
        {
            gameboard = game.GetCopyOfGameBoard();

            GameTreeNode testroot = new GameTreeNode(null, gameboard);
            expandTree(testroot, 4, 1);
            pruneTree(testroot, 1);
            game.Board = gameboard;
            return chosenMove;
            /*
            game.Board = gameboard;
            GameTreeNode root = new GameTreeNode(null, gameboard);
            List<Coordinate> moves = game.Board.GetAllAvailableMoves();
            bottomNodes = 0;
            //level one
            foreach (Coordinate c in moves)
            {
                Gameboard newConfig = game.GetCopyOfGameBoard();
                string move = "B" + c.Row + c.Col;
                newConfig.MakeMoveOnBoard(move);
                GameTreeNode child = new GameTreeNode(root, newConfig);
                child.Move = move;
                child.HeuristicValue = -100000;
                root.Children.Add(child);
            }
            //level two
            foreach (GameTreeNode node in root.Children)
            {
                List<Coordinate> childMoves = node.CurrentBoardConfig.GetAllAvailableMoves();
                foreach (Coordinate c in childMoves)
                {
                    Gameboard newConfig = node.CurrentBoardConfig.Clone();
                    string move = "R" + c.Row + c.Col;
                    newConfig.MakeMoveOnBoard(move);
                    GameTreeNode child = new GameTreeNode(node, newConfig);
                    child.Move = move;
                    child.HeuristicValue = 100000;
                    node.Children.Add(child);
                }
            }
            //level three 
            foreach (GameTreeNode node in root.Children)
            {
                foreach (GameTreeNode childNode in node.Children)
                {
                    List<Coordinate> childMoves = childNode.CurrentBoardConfig.GetAllAvailableMoves();
                    if(childMoves.Count == 0)
                    {
                        game.getPlayer1Points();
                        game.getPlayer2Points();
                        childNode.HeuristicValue = game.Player2Points - game.Player1Points;
                    }
                    foreach (Coordinate c in childMoves)
                    {
                        Gameboard newConfig = childNode.CurrentBoardConfig.Clone();
                        string move = "B" + c.Row + c.Col;
                        newConfig.MakeMoveOnBoard(move);
                        GameTreeNode child = new GameTreeNode(childNode, newConfig);
                        child.Move = move;
                        game.Board = newConfig;
                        
                        game.getPlayer1Points();
                        game.getPlayer2Points();

                        child.HeuristicValue = game.Player2Points - game.Player1Points;
                        childNode.Children.Add(child);
                        bottomNodes++;
                    }
                }
            }

            game.Board = gameboard;

            GameTreeNode chosenMove = new GameTreeNode(null, gameboard);

            //Game tree has been built, now perform min-max/AB Pruning
            foreach(GameTreeNode node in root.Children)
            {
                node.Alpha = root.Alpha;
                node.Beta = root.Beta;
                foreach(GameTreeNode childNode in node.Children)
                {
                    if (childNode.Children.Count == 0)
                    {
                        childNode.Alpha = childNode.HeuristicValue;
                    }
                    else
                    {
                        childNode.Alpha = node.Alpha;
                        childNode.Beta = node.Beta;

                        foreach (GameTreeNode grandChildNode in childNode.Children)
                        {
                            grandChildNode.Alpha = childNode.Alpha;
                            grandChildNode.Beta = childNode.Beta;

                            if (grandChildNode.HeuristicValue > childNode.Alpha)
                            {
                                childNode.Alpha = grandChildNode.HeuristicValue;
                            }
                            if (childNode.Alpha > childNode.Beta)
                            {
                                break;
                            }
                        }
                    }
                    if(childNode.Alpha < node.Beta)
                    {
                        node.Beta = childNode.Alpha;
                    }
                    if(node.Alpha > node.Beta)
                    {
                        break;
                    }
                }
                if(node.Beta > root.Alpha)
                {
                    root.Alpha = node.Beta;
                    chosenMove = node;
                }
            }

            return chosenMove.Move;
             */
        }

        /*private string AlphaBeta(int levels, int currentLevel, GameTreeNode gameNode = null)
        {
            if(currentLevel == 1)
            {
                gameboard = game.GetCopyOfGameBoard();
                GameTreeNode root = new GameTreeNode(null, gameboard);

                List<Coordinate> moves = game.Board.GetAllAvailableMoves();
                //level one
                foreach (Coordinate c in moves)
                {
                    Gameboard newConfig = game.GetCopyOfGameBoard();
                    string move = "B" + c.Row + c.Col;
                    newConfig.MakeMoveOnBoard(move);
                    GameTreeNode child = new GameTreeNode(root, newConfig);
                    child.Move = move;
                    child.HeuristicValue = -100000;
                    root.Children.Add(child);
                    AlphaBeta(levels, currentLevel + 1, child);
                }
                foreach(GameTreeNode node in root.Children)
                {
                    AlphaBeta(levels, currentLevel + 1);
                }
            }
            else
            {

            }
        }*/

        private void expandTree(GameTreeNode node, int level, int currentLevel)
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
                    expandTree(child, level, currentLevel + 1);
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

        private void pruneTree(GameTreeNode node, int level)
        {
            if(node.Parent == null)
            {
                node.Alpha = -10000;
                node.Beta = 10000;
                foreach (GameTreeNode child in node.Children)
                {
                    pruneTree(child, level + 1);
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
                    pruneTree(child, level + 1);
                    
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

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

        /*public string GetMove()
        {
            return BuildGameTree();
        }
        */
        public string GetMove()
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
                        game.getPlayer2Points();
                        childNode.HeuristicValue = game.Player2Points;
                    }
                    foreach (Coordinate c in childMoves)
                    {
                        Gameboard newConfig = childNode.CurrentBoardConfig.Clone();
                        string move = "B" + c.Row + c.Col;
                        newConfig.MakeMoveOnBoard(move);
                        GameTreeNode child = new GameTreeNode(childNode, newConfig);
                        child.Move = move;
                        game.Board = newConfig;
                        
                        game.getPlayer2Points();

                        child.HeuristicValue = game.Player2Points;
                        childNode.Children.Add(child);
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
        }
    }
}

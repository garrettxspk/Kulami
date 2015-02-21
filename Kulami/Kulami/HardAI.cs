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

        public string GetMove()
        {
            BuildGameTree();
            return "";
        }

        private void BuildGameTree()
        {
            GameTreeNode root = new GameTreeNode(null, gameboard);
            List<Coordinate> moves = game.Board.GetAllAvailableMoves();
            //level one
            foreach (Coordinate c in moves)
            {
                Gameboard newConfig = game.GetCopyOfGameBoard();
                string move = "B" + c.Row + c.Col;
                newConfig.MakeMoveOnBoard(move);
                GameTreeNode child = new GameTreeNode(root, newConfig);
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
                    node.Children.Add(child);
                }
            }
            //level three 
            foreach (GameTreeNode node in root.Children)
            {
                foreach (GameTreeNode childNode in node.Children)
                {
                    List<Coordinate> childMoves = childNode.CurrentBoardConfig.GetAllAvailableMoves();
                    foreach (Coordinate c in childMoves)
                    {
                        Gameboard newConfig = childNode.CurrentBoardConfig.Clone();
                        string move = "B" + c.Row + c.Col;
                        newConfig.MakeMoveOnBoard(move);
                        GameTreeNode child = new GameTreeNode(childNode, newConfig);
                        childNode.Children.Add(child);
                    }
                }
            }
        }
    }
}

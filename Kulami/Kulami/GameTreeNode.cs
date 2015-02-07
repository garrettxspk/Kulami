using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kulami
{
    class GameTreeNode
    {
        private GameTreeNode parent;

        internal GameTreeNode Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        private Gameboard currentBoardConfig;

        internal Gameboard CurrentBoardConfig
        {
            get { return currentBoardConfig; }
            set { currentBoardConfig = value; }
        }

        private int heuristicValue;

        public int HeuristicValue
        {
            get { return heuristicValue; }
            set { heuristicValue = value; }
        }

        private List<GameTreeNode> children;

        internal List<GameTreeNode> Children
        {
            get { return children; }
            set { children = value; }
        }

        public GameTreeNode(GameTreeNode p, Gameboard board)
        {
            parent = p;
            currentBoardConfig = board;
            children = new List<GameTreeNode>();
            heuristicValue = 0;
        }
    }
}

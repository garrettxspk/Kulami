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

        private int alpha;

        public int Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        private int beta;

        public int Beta
        {
            get { return beta; }
            set { beta = value; }
        }

        private string move;

        public string Move
        {
            get { return move; }
            set { move = value; }
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
            Alpha = -100000;
            Beta = 100000;
        }
    }
}

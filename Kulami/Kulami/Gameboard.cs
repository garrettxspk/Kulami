using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kulami
{
    class Gameboard
    {
        private List<Tile> tiles;

        internal List<Tile> Tiles
        {
            get { return tiles; }
            set { tiles = value; }
        }

        public Gameboard()
        {
            tiles = new List<Tile>();
        }

        
    }
}

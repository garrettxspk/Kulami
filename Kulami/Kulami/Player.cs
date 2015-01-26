using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kulami
{
    class Player
    {
        private PlayerType type;

        internal PlayerType Type
        {
            get { return type; }
            set { type = value; }
        }

        public Player(PlayerType pt)
        {
            type = pt;
        }
    }
}

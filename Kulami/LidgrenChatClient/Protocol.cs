using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LidgrenKulamiPeer
{
    class Protocol
    {
        public enum messageType
        {
            move = 1,
            statusReport = 2,
            chat = 3,
        }
    }
}
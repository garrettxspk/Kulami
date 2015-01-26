using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kulami
{
    class Marble
    {
        Color marbleColor;

        internal Color MarbleColor
        {
            get { return marbleColor; }
            set { marbleColor = value; }
        }

        public Marble(Color c)
        {
            marbleColor = c;
        }
    }
}

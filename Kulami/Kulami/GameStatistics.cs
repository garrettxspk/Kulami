using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kulami
{
    public class GameStatistics
    {
        private string elapsedTime;

        public string ElapsedTime
        {
            get { return elapsedTime; }
            set { elapsedTime = value; }
        }

        private int redPlanetsConquered;

        public int RedPlanetsConquered
        {
            get { return redPlanetsConquered; }
            set { redPlanetsConquered = value; }
        }

        private int bluePlanetsConquered;

        public int BluePlanetsConquered
        {
            get { return bluePlanetsConquered; }
            set { bluePlanetsConquered = value; }
        }

        private int redSectorsWon;

        public int RedSectorsWon
        {
            get { return redSectorsWon; }
            set { redSectorsWon = value; }
        }

        private int redSectorsLost;

        public int RedSectorsLost
        {
            get { return redSectorsLost; }
            set { redSectorsLost = value; }
        }

        private int blueSectorsWon;

        public int BlueSectorsWon
        {
            get { return blueSectorsWon; }
            set { blueSectorsWon = value; }
        }

        private int blueSectorsLost;

        public int BlueSectorsLost
        {
            get { return blueSectorsLost; }
            set { blueSectorsLost = value; }
        }

        private int bluePoints;

        public int BluePoints
        {
            get { return bluePoints; }
            set { bluePoints = value; }
        }

        private int redPoints;

        public int RedPoints
        {
            get { return redPoints; }
            set { redPoints = value; }
        }
    }
}

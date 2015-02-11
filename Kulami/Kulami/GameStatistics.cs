using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kulami
{
    class GameStatistics
    {
        private string elapsedTime;

        public string ElapsedTime
        {
            get { return elapsedTime; }
            set { elapsedTime = value; }
        }

        private int planetsConquered;

        public int PlanetsConquered
        {
            get { return planetsConquered; }
            set { planetsConquered = value; }
        }

        private int sectorsConquered;

        public int SectorsConquered
        {
            get { return sectorsConquered; }
            set { sectorsConquered = value; }
        }

        private int sectorsLost;

        public int SectorsLost
        {
            get { return sectorsLost; }
            set { sectorsLost = value; }
        }

        private int points;

        public int Points
        {
            get { return points; }
            set { points = value; }
        }
    }
}

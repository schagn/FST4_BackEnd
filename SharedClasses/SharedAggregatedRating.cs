using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class SharedAggregatedRating
    {

        private int accumulatedRating;

        public int AccumulatedRating
        {
            get { return accumulatedRating; }
            set { accumulatedRating = value; }
        }


        private int numberOfRatins;

        public int NumberofRatings
        {
            get { return numberOfRatins; }
            set { numberOfRatins = value; }
        }


    }
}

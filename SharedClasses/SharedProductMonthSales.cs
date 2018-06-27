using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class SharedProductMonthSales
    {

        public Guid Id;

        public string name;

        public int TimesSold;

        public override string ToString()
        {
            return name + " (" + TimesSold + ")";
        }

    }
}


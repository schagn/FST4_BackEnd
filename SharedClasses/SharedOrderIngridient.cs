﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class SharedOrderIngridient
    {

        public string name;

        public double amount;

        public string unit;



        override
            public string ToString()
        {
            return this.name + " " + this.amount + " " + this.unit; 
        }
    }
}

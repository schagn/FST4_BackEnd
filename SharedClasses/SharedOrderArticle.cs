using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class SharedOrderArticle
    {

        public Guid id;
        public string name;
        public int quantity;


        override
            public string ToString()
        {
            return this.name;
        }
    }
}

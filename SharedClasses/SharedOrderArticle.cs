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


        override
            public string ToString()
        {
            return this.name;
        }
    }
}

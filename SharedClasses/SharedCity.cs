using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class SharedCity
    {

        public SharedCity()
        {

        }

        public SharedCity(int zip, string name)
        {
            ZipCode = zip;
            Name = name;
        }

        public int ZipCode { get; set; }

        public string Name { get; set; }

    }
}

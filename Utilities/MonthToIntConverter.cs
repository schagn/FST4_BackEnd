using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class MonthToIntConverter
    {

        public static int GetIntForMonth(string month)
        {

            switch (month)
            {
                default:
                    return 0;

                case "Jänner":
                    return 1;

                case "Februar":
                    return 2;

                case "März":
                    return 3;

                case "April":
                    return 4;

                case "Mai":
                    return 5;

                case "Juni":
                    return 6;

                case "Juli":
                    return 7;

                case "August":
                    return 8;

                case "September":
                    return 9;

                case "Oktober":
                    return 10;

                case "November":
                    return 11;

                case "Dezember":
                    return 12;
            }


        }



    }
}

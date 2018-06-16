using DataRepository;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAggregator
{
    public static class DataAggregator
    {
        private static DataHandler dh =  new DataHandler();

        public static void GetSalesPerMonth()
        {

            List <SharedBestellung> orders = dh.GetOrdersByStatus("abgeschlossen");
            Dictionary<int, double> sales = new Dictionary<int, double>();
            int month = 0;
            double orderVal = 0;

            foreach (var order in orders)
            {
                month = order.LieferDatum.Value.Month;
                orderVal = order.GesamtSumme.Value;

                if (!sales.ContainsKey(month))
                {
                    sales.Add(month, orderVal);
                }
                else
                {
                    sales[month] = sales[month] + orderVal;

                }
                
            }




        }


    }
}

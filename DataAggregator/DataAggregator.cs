using DataRepository;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Data;
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

        public static List<SharedProductMonthSales> GetTop5ProductsPerMonth(int month)
        {

            List<SharedOrderArticle> soldArticelsPerMonth = dh.GetSoldProductsPerMonth(month);
            Dictionary<Guid, SharedProductMonthSales> MonthlySalesPerProduct = new Dictionary<Guid, SharedProductMonthSales>();

            foreach (var item in soldArticelsPerMonth)
            {
                var id = item.id;

                if (!MonthlySalesPerProduct.ContainsKey(id))
                {

                    MonthlySalesPerProduct.Add(id, new SharedProductMonthSales
                    {
                        Id = id,
                        name = item.name,
                        TimesSold = item.quantity

                    });

                }
                else
                {
                    MonthlySalesPerProduct[id].TimesSold += item.quantity;


                }

            }

            List<SharedProductMonthSales> monthlySalesPerProductList = new List<SharedProductMonthSales>();

            foreach (var entry in MonthlySalesPerProduct)
            {

                monthlySalesPerProductList.Add(entry.Value);

            }

            return monthlySalesPerProductList;
            


        }

        public static DataTable getRatingPerProduct()
        {

            List<SharedBewertung> allRatings = dh.GetRatingAll();
            Dictionary<string, SharedAggregatedRating> ratingPerProduct = new Dictionary<string, SharedAggregatedRating>();
            string name;
            foreach (var item in allRatings)
            {
                name = item.ArtikelName;

                if (!ratingPerProduct.ContainsKey(name))
                {
                    ratingPerProduct.Add(name, new SharedAggregatedRating
                    {
                        AccumulatedRating = item.Sterne.GetValueOrDefault(),
                        NumberofRatings = 1

                    });
                }
                else
                {
                    ratingPerProduct[name].AccumulatedRating += item.Sterne.GetValueOrDefault();
                    ratingPerProduct[name].NumberofRatings += 1;

                }
            }

            return DataTableConverter.RatingsPerProductToDataTable(ratingPerProduct);


        }


        public static DataTable getRawMaterialConsumptionOfMonth(int month)
        {

            List<SharedShortOrder> orders = dh.GetDoneOrdersByMonth(month);
            List<SharedOrderArticle> products  = new List<SharedOrderArticle>();
            List<SharedOrderIngridient> usedIngridients = new List<SharedOrderIngridient>();

            foreach (var item in orders)
            {

                products.AddRange(dh.GetArticelsforOrder(item.OrderID));

            }

            foreach (var product in products)
            {


                usedIngridients.AddRange(dh.getRawMaterialForOrderProduct(product.id, product.quantity));

            }

            Dictionary<string, SharedAggregatedValueWithUnit> usedRawMaterial = new Dictionary<string, SharedAggregatedValueWithUnit>();
            string name;

            foreach (var ingr in usedIngridients)
            {
                name = ingr.name;

                if (!usedRawMaterial.ContainsKey(name))
                {

                    usedRawMaterial.Add(name, new SharedAggregatedValueWithUnit
                    {
                        amount = ingr.amount,
                        unit = ingr.unit

                    });

                }
                else
                {

                    usedRawMaterial[name].amount += ingr.amount;

                }

            }

            return DataTableConverter.RawMaterialToDataTable(usedRawMaterial);




        }

    }


   




 
}

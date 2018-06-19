using SharedClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAggregator
{
    public static class DataTableConverter
    {

        public static DataTable RawMaterialToDataTable(Dictionary<string, SharedAggregatedValueWithUnit> usedRawMaterial)
        {

            DataTable rawMaterialUsageTable = new DataTable();

            rawMaterialUsageTable.Columns.Add("Zutat", typeof(string));
            rawMaterialUsageTable.Columns.Add("Verbrauch",typeof(double));
            rawMaterialUsageTable.Columns.Add("Einheit", typeof(string));

            foreach(KeyValuePair<string, SharedAggregatedValueWithUnit> entry in usedRawMaterial)
            {

                rawMaterialUsageTable.Rows.Add(entry.Key, entry.Value.amount, entry.Value.unit);

            }

            return rawMaterialUsageTable;

        }


        public static DataTable RatingsPerProductToDataTable(Dictionary<string, SharedAggregatedRating> ratings)
        {

            DataTable ratingPerProductTable = new DataTable();

            ratingPerProductTable.Columns.Add("Artikel");
            ratingPerProductTable.Columns.Add("Bewertung");
            ratingPerProductTable.Columns.Add("Anzahl der Bewertungen");

            foreach (KeyValuePair<string, SharedAggregatedRating> entry in ratings)
            {

                ratingPerProductTable.Rows.Add(entry.Key, entry.Value.AccumulatedRating, entry.Value.NumberofRatings);

            }


        }


    }
}

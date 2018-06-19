using OfficeOpenXml;
using OfficeOpenXml.Table;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST
{
    class Program
    {
        static void Main(string[] args)
        {


            DataTable table = DataAggregator.DataAggregator.getRawMaterialConsumptionOfMonth(2);

            

            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Accounts");
                ws.Cells["A1"].LoadFromDataTable(table, true, TableStyles.Light17);
                ws.Cells.AutoFitColumns();
                pck.Save();

                byte[] data = pck.GetAsByteArray();
                string path =  @"C:\Users\stefa\Desktop\test.xlsx";
                File.WriteAllBytes(path,data);
            }


            Console.ReadLine();

        }
    }
}

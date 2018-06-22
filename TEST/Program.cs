using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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


            DataTable table = DataAggregator.DataAggregator.getRawMaterialConsumptionOfMonth(4);


            

            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Accounts");
                ws.Cells["A7"].LoadFromDataTable(table, true, TableStyles.Light17);
                Image img = Image.FromFile(@"C:\Users\stefa\Desktop\GetYourCake_logo.PNG");
                int RowIndex = 0;
                int ColIndex = 0;
                ExcelPicture pic = ws.Drawings.AddPicture("Logo", img);
                pic.SetPosition(RowIndex, 0, ColIndex, 0);
                ws.Cells.AutoFitColumns();
                ws.Column(1).Width = 18.93;
                ws.Column(2).Width = 22.27;
                ws.Column(3).Width = 18.47;
                ws.Cells["A7:C7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                pck.Save();

                byte[] data = pck.GetAsByteArray();
                string path =  @"C:\Users\stefa\Desktop\test.xlsx";
                File.WriteAllBytes(path,data);
            }


            Console.ReadLine();

        }
    }
}

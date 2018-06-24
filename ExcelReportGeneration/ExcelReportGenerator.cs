using DataAggregator;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReportGeneration
{
    public class ExcelReportGenerator
    {
        public  void GenerateRawMaterialExcelReport(int month, string savePath)
        {

            DataTable table = DataAggregator.DataAggregator.getRawMaterialConsumptionOfMonth(month);

            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Rohmaterialverbrauch_Monat" + month + "_" + DateTime.Now.Year);
                ws.Cells["A7"].LoadFromDataTable(table, true, TableStyles.Light17);
                Image img = ExcelReportGeneration.Properties.Resources.GetYourCake_logo;
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
                string fileName = savePath + "\\Rohmaterialverbrauch_Monat" + month + "_" + DateTime.Now.Year + ".xlsx";
                File.WriteAllBytes(fileName, data);
            }
        }

        public void  GenerateRatingExcelReport(string savePath)
        {
            DataTable table = DataAggregator.DataAggregator.getRatingPerProduct();

            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Bewertung/Produkt_" + DateTime.Now.Year);
                ws.Cells["A7"].LoadFromDataTable(table, true, TableStyles.Light17);
                Image img = ExcelReportGeneration.Properties.Resources.GetYourCake_logo;
                int RowIndex = 0;
                int ColIndex = 0;
                ExcelPicture pic = ws.Drawings.AddPicture("Logo", img);
                pic.SetPosition(RowIndex, 0, ColIndex, 0);
                ws.Cells.AutoFitColumns();
                ws.Column(1).Width = 18.93;
                ws.Column(2).Width = 22.27;
                ws.Column(3).Width = 24.87;
                ws.Cells["A7:C7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                pck.Save();

                byte[] data = pck.GetAsByteArray();
                string fileName = savePath + "\\BewertungProProdukt" + "_" + DateTime.Now.Year + ".xlsx";
                File.WriteAllBytes(fileName, data);
            }

        }

    }
}

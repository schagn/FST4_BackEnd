using DataRepository;
using ExcelReportGeneration;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using OfficeOpenXml;
using OxyPlot;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BackEndView.ViewModel
{
    public class DashboardVm : ViewModelBase
    {
        private ObservableCollection<DataPoint> sales;

        public ObservableCollection<DataPoint> Sales
        {
            get { return sales; }
            set { sales = value; }
        }

        private int offeneBestellungen;

        public int OffeneBestellungen
        {
            get { return offeneBestellungen; }
            set { offeneBestellungen = value; RaisePropertyChanged(); }
        }

        private int gecancelteBestellungen;

        public int GecancelteBestellungen
        {
            get { return gecancelteBestellungen; }
            set { gecancelteBestellungen = value; RaisePropertyChanged(); }
        }

        private int gesamtBestellungen;

        public int GesamtBestellungen
        {
            get { return gesamtBestellungen; }
            set { gesamtBestellungen = value; }
        }


        private ObservableCollection<SharedProductMonthSales> topProdukte;

        public ObservableCollection<SharedProductMonthSales> TopProdukte
        {
            get { return topProdukte; }
            set { topProdukte = value; }
        }



        private List<string> perioden;

        public List<string> Perioden
        {
            get { return perioden; }
            set { perioden = value; }
        }

        private string selectedPeriode;

        public string SelectedPeriode
        {
            get { return selectedPeriode; }
            set { selectedPeriode = value; RaisePropertyChanged(); }
        }

        public RelayCommand RawMaterialReportBtnClicked { get; set; }

        public RelayCommand RatingReportBtnClicked { get; set; }

        public ObservableCollection<int> NumerischePerioden { get; set; }


        private DataHandler dh;
        private ExcelReportGenerator reportGenerator;

        public DashboardVm()
        {
            Perioden = new List<string> {"Alle", "Jänner", "Februar", "März", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "Novermber", "Dezember" };
            dh = new DataHandler();
            TopProdukte = new ObservableCollection<SharedProductMonthSales>();
            reportGenerator = new ExcelReportGenerator();
            RawMaterialReportBtnClicked = new RelayCommand(CreateRawMaterialExcelReport);
            RatingReportBtnClicked = new RelayCommand(CreateRatingExcelReport);
            InitialzeDashboard();
            NumerischePerioden = new ObservableCollection<int>()
            {
                0,1,2,3,4,5,6,7,8,9,10,11,12
            };

            GenerateDemoData();

        }

        private void CreateRatingExcelReport()
        {

            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            string savePath = dialog.SelectedPath;

            reportGenerator.GenerateRatingExcelReport(savePath);

        }

        private void CreateRawMaterialExcelReport()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            string savePath = dialog.SelectedPath;

            reportGenerator.GenerateRawMaterialExcelReport(MonthToIntConverter.GetIntForMonth(selectedPeriode), savePath);

        }

        private void InitialzeDashboard()
        {
            OffeneBestellungen = dh.GetOfOrdersInCurrentMonthByType("offen").Count();
            GecancelteBestellungen = dh.GetOfOrdersInCurrentMonthByType("abgebrochen").Count();
            GesamtBestellungen = dh.GetOfOrdersInCurrentMonthByType("alle").Count();
            TopProdukte = GenerateTop5Products();
        }

        private ObservableCollection<SharedProductMonthSales> GenerateTop5Products()
        {
            var tmpList = DataAggregator.DataAggregator.GetTop5ProductsPerMonth(DateTime.Now.Month);
            tmpList.Sort((p1, p2) => -1*p1.TimesSold.CompareTo(p2.TimesSold));

            return new ObservableCollection<SharedProductMonthSales>(tmpList.Take(5));
        }

        private void GenerateDemoData()
        {
            Sales = new ObservableCollection<DataPoint>
            {
                new DataPoint(0,0),
                new DataPoint(1,12000),
                new DataPoint(2,15000),
                new DataPoint(3,16000),
                new DataPoint(4, 22000),
                new DataPoint(5,27000),
                new DataPoint(6,32000),
                

                
              


            };

           


        }
    }
}

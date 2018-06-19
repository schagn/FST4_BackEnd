using DataRepository;
using GalaSoft.MvvmLight;
using OfficeOpenXml;
using OxyPlot;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        private ObservableCollection<SharedArticle> topProdukte;

        public ObservableCollection<SharedArticle> TopProdukte
        {
            get { return topProdukte; }
            set { topProdukte = value; }
        }

        private DataHandler dh;

        public DashboardVm()
        {

            dh = new DataHandler();
            InitialzeDashboard();
            GenerateDemoData();

        }

        private void InitialzeDashboard()
        {
            OffeneBestellungen = dh.GetOfOrdersInCurrentMonthByType("offen").Count();
            GecancelteBestellungen = dh.GetOfOrdersInCurrentMonthByType("abgebrochen").Count();
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
                new DataPoint(6,20000),
                new DataPoint(7,19000),
                new DataPoint(8,26000),
                new DataPoint(9,29000),
                new DataPoint(10,32000),
                new DataPoint(11,45000),
                new DataPoint(12,57000)


              


            };

            ExcelPackage exP = new ExcelPackage();


        }
    }
}

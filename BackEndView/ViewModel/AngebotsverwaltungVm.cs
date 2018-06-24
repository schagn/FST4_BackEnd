using DataRepository;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEndView.ViewModel { 
    public class AngebotsverwaltungVm : ViewModelBase
    {

        public RelayCommand CancelDataBtnClick { get; set; }

        public RelayCommand EditAngebotBtnClick { get; set; }

        public RelayCommand DeleteAngebotBtnClick { get; set; }

        public RelayCommand SaveAngebotBtnClick { get; set; }

        public ObservableCollection<SharedAngebot> Angebote { get; set; }

        private string code;

        public string Code
        {
            get { return code; }
            set { code = value; RaisePropertyChanged(); }
        }

        private double? prozent;

        public double? Prozent
        {
            get { return prozent; }
            set { prozent = value; RaisePropertyChanged(); }
        }

        private DateTime? startDatum;

        public DateTime? StartDatum
        {
            get { return startDatum; }
            set { startDatum = value; RaisePropertyChanged(); }
        }

        private DateTime? endDatum;

        public DateTime? EndDatum
        {
            get { return endDatum; }
            set { endDatum = value; RaisePropertyChanged(); }
        }

        /*private ObservableCollection<SharedArticle> produkte;
   
        public ObservableCollection<SharedArticle> Produkte
        {
            get { return produkte; }
            set { produkte = value; RaisePropertyChanged(); }
        }*/


        /*private SharedArticle selectedProdukt;

        public SharedArticle SelectedProdukt
        {
            get { return selectedProdukt; }
            set { selectedProdukt = value; RaisePropertyChanged(); }
        }
        */

        private SharedAngebot selectedAngebot;

        public SharedAngebot SelectedAngebot
        {
            get { return selectedAngebot; }
            set { selectedAngebot = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<string> FilterMethoden { get; set; }

        private string selectedFilterMethode;

        public string SelectedFilterMethode
        {
            get { return selectedFilterMethode; }
            set { selectedFilterMethode = value; RaisePropertyChanged(); RefreshList(SelectedFilterMethode); }
        }

        bool IsEditingProcess;

        private DataHandler dh;

        public AngebotsverwaltungVm()
        {
            dh = new DataHandler();

            Angebote = new ObservableCollection<SharedAngebot>(dh.GetAllSpecialOffers());

            EditAngebotBtnClick = new RelayCommand(EditAngebot);

            SaveAngebotBtnClick = new RelayCommand(SaveAngebot);

            DeleteAngebotBtnClick = new RelayCommand(
                () =>
                {
                    DeleteSelectedAngebot(SelectedAngebot);
                });

            CancelDataBtnClick = new RelayCommand(CancelData);  

            StartDatum = DateTime.Today;
            EndDatum = DateTime.Today;

            IsEditingProcess = false;

            FilterMethoden = new ObservableCollection<string>();
            FilterMethoden.Add("Alle");
            FilterMethoden.Add("Aktuell");
            FilterMethoden.Add("Vergangenheit");
            FilterMethoden.Add("Zukunft");
        }

        
        private void EditAngebot()
        {
            Code = SelectedAngebot.Code;
            Prozent = SelectedAngebot.Prozent;
            StartDatum = SelectedAngebot.StartDatum;
            EndDatum = SelectedAngebot.EndDatum;
           
            //Produkt darf hier nicht änderbar sein !!!!! 
            //Combobox List leer??

            IsEditingProcess = true;
        }

        private void DeleteSelectedAngebot(SharedAngebot a)
        {
            dh.DeleteSpecialOffer(a);
            Angebote.Remove(a);
            RaisePropertyChanged("Angebote");
        }

        private void SaveAngebot()
        {
            if (IsEditingProcess == true)
            {
                SelectedAngebot.Code = Code;
                SelectedAngebot.StartDatum = StartDatum;
                SelectedAngebot.EndDatum = EndDatum;
                SelectedAngebot.Prozent = Prozent;
                dh.UpdateSpecialOffer(SelectedAngebot);
            }
            else
            {

                dh.AddNewSpecialOffer(new SharedAngebot()
                {
                    Code = this.Code,
                    StartDatum = this.StartDatum,
                    EndDatum = this.EndDatum,
                    Prozent = this.Prozent

                });
                
            }

            Code = "";
            Prozent = 0;
            StartDatum = DateTime.Today;
            EndDatum = DateTime.Today;
            //SelectedProdukt = null;
            
            ReloadListFromDatabase();

            IsEditingProcess = false;
        }

        private void RefreshList(string selectedFilterMethode)
        {
            if (SelectedFilterMethode.Equals("Alle"))
            {
                ReloadListFromDatabase();

            }
            else
            {
                ReloadListFromDatabase();
               

                foreach (var item in FilterListByTime())
                {

                    Angebote.Add(item);
                }
                
                


            }

        }

        private void ReloadListFromDatabase()
        {


            Angebote.Clear();

            foreach (var item in dh.GetAllSpecialOffers())
            {
                Angebote.Add(item);
            }

        }

        private void CancelData()
        {
            Code = "";
            Prozent = 0;
            StartDatum = DateTime.Today;
            EndDatum = DateTime.Today;
            SelectedAngebot = null;
            //SelectedProdukt = null;

            IsEditingProcess = false;

        }

        private List<SharedAngebot> FilterListByTime()
        {
            var newList = new List<SharedAngebot>();
            DateTime today = DateTime.Today;
            

            if (SelectedFilterMethode.Equals("Aktuell")) newList = Angebote.Where(x => (DateTime.Compare(today,x.StartDatum.GetValueOrDefault()) > 0) && (DateTime.Compare(today,x.EndDatum.GetValueOrDefault()) < 0)).ToList();

            if (SelectedFilterMethode.Equals("Vergangenheit")) newList = Angebote.Where(y => (DateTime.Compare(today,y.EndDatum.GetValueOrDefault()) > 0)).ToList();

            if (SelectedFilterMethode.Equals("Zukunft")) newList = Angebote.Where(z => (DateTime.Compare(today,z.StartDatum.GetValueOrDefault()) < 0)).ToList();

            Angebote.Clear();

            return newList;
                    

        }

    }
}

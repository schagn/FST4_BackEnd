using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BackEndView.ViewModel
{
    public class BestellverwaltungVm : ViewModelBase
    {

        //TODO Kunde kontaktieren per Mail

        public RelayCommand CancelDataBtnClick { get; set; }

        public RelayCommand EditBestellungBtnClick { get; set; }

        public RelayCommand DeleteBestellungBtnClick { get; set; }

        public RelayCommand SaveBestellungBtnClick { get; set; }

        public RelayCommand ProduktLöschenBtnClick { get; set; }

        public RelayCommand KundeKontaktierenBtnClick { get; set; }

        public ObservableCollection<SharedBestellung> Bestellungen { get; set; }

        private SharedBestellung selectedBestellung;

        public SharedBestellung SelectedBestellung
        {
            get { return selectedBestellung; }
            set { selectedBestellung = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<string> Bestellstatusse { get; set; }

        private string selectedStatus;

        public string SelectedStatus
        {
            get { return selectedStatus; }
            set { selectedStatus = value; RaisePropertyChanged(); }
        }

        private string bestellNummer;

        public string BestellNummer
        {
            get { return bestellNummer; }
            set { bestellNummer = value; RaisePropertyChanged(); }
        }

        private string bestellDatum;

        public string BestellDatum 
        {
            get { return bestellDatum; }
            set { bestellDatum = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<string> FilterMethoden { get; set; }

        private string selectedFilterMethode;

        public string SelectedFilterMethode
        {
            get { return selectedFilterMethode; }
            set { selectedFilterMethode = value; RaisePropertyChanged(); RefreshList(SelectedFilterMethode); }
        }

        private ObservableCollection<string> selectedBestellungProdukte;

        public ObservableCollection<string> SelectedBestellungProdukte
        {
            get { return selectedBestellungProdukte; }
            set { selectedBestellungProdukte = value; RaisePropertyChanged(); }
        }

        private string selectedProdukt;

        public string SelectedProdukt
        {
            get { return selectedProdukt; }
            set { selectedProdukt = value; RaisePropertyChanged(); }
        }


        public BestellverwaltungVm()
        {
            Bestellungen = new ObservableCollection<SharedBestellung>();
            Bestellstatusse = new ObservableCollection<string>();
            Bestellstatusse.Add("erledigt");
            Bestellstatusse.Add("gecancelt");
            Bestellstatusse.Add("in Bearbeitung");


            EditBestellungBtnClick = new RelayCommand(EditBestellung);

            SaveBestellungBtnClick = new RelayCommand(SaveBestellung);


            DeleteBestellungBtnClick = new RelayCommand(
                () =>
                {
                    DeleteSelectedBestellung(SelectedBestellung);
                });

            ProduktLöschenBtnClick = new RelayCommand(ProduktLöschen);

            CancelDataBtnClick = new RelayCommand(CancelData);

            //KundeKontaktierenBtnClick = new RelayCommand();

            //Create Demo Data
            Bestellungen.Add(new SharedBestellung()
            {
                BestellId = Guid.NewGuid(),
                Bestellstatus = "erledigt",
                Artikel = new List<string>() { "Regenbogentorte", "Sachertorte", "Linzerschnitte" },
                BestellDatum = DateTime.Now.ToLocalTime(),
                GesamtSumme = 130,
                GutscheinUsed = false,
                GutscheinWert = 0,
                KundenName = "jklfödas"

            });

            Bestellungen.Add(new SharedBestellung()
            {
                BestellId = Guid.NewGuid(),
                Bestellstatus = "in Bearbeitung",
                Artikel = new List<string>() { "Kardinalschnitte", "Tiramisu", "Roulade" },
                BestellDatum = DateTime.Now.ToLocalTime(),
                GesamtSumme = 90,
                GutscheinUsed = true,
                GutscheinWert = 50,
                KundenName = "jklhgfhdrsdftzu"

            });

            FilterMethoden = new ObservableCollection<string>();
            FilterMethoden = Bestellstatusse;

        }

        private void ProduktLöschen()
        {

            foreach (var item in Bestellungen)
            {
                if(item.BestellId == SelectedBestellung.BestellId)
                {
                    //foreach (var Produkt in item.Artikel)
                    //{
                      //  if(Produkt.Equals(SelectedProdukt))
                       // {
                            //SelectedBestellung.Artikel.Remove(Produkt);

                            item.Artikel.Remove(SelectedProdukt);
                        //}
                    //}
                }
            }

            //SelectedBestellung.Artikel.Remove(SelectedProdukt);
            RaisePropertyChanged("Bestellungen");
            SelectedStatus = null;
            BestellDatum = "";
            BestellNummer = "";
            SelectedProdukt = null;

        }

        private void EditBestellung()
        {
            SelectedStatus = SelectedBestellung.Bestellstatus;
            BestellDatum = SelectedBestellung.BestellDatum.ToString();
            BestellNummer = SelectedBestellung.BestellId.ToString();
            //SelectedBestellungProdukte = ObservableListConverter.ConvertToObservableCollection(SelectedBestellung.Artikel);


        }

        private void DeleteSelectedBestellung(SharedBestellung b)
        {
            //client.deleteZutat
            //client Zutaten neu abfragen
            Bestellungen.Remove(b);
            RaisePropertyChanged("Bestellungen");
        }

        private void SaveBestellung()
        {
        
             foreach (var item in Bestellungen)
             {
                  if (item.BestellId == SelectedBestellung.BestellId)
                  {
                     item.Bestellstatus = SelectedStatus;
                  }
             }


            SelectedStatus = null;
            BestellNummer = "";
            BestellDatum = "";

            RaisePropertyChanged("Bestellungen");
        }


        private void RefreshList(string selectedFilterMethode)
        {
            // je nachdem welche Filtermethode ausgewählt ist --> neu von DB laden  
        }

        private void CancelData()
        {
            SelectedStatus = null;
            BestellNummer = "";
            BestellDatum = "";
            SelectedBestellung = null;
            SelectedProdukt = null;

        }

    }
}

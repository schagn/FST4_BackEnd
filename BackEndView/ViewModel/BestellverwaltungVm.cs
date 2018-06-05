﻿using DataRepository;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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
            set { selectedFilterMethode = value; RaisePropertyChanged(); FilterList(SelectedFilterMethode); }
        }

        

        private ObservableCollection<SharedOrderArticle> selectedBestellungProdukte;

        public ObservableCollection<SharedOrderArticle> SelectedBestellungProdukte
        {
            get { return selectedBestellungProdukte; }
            set { selectedBestellungProdukte = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<string> selectedBestellungProduktnamen;

        public ObservableCollection<string> SelectedBestellungProduktnamen
        {
            get { return selectedBestellungProduktnamen; }
            set { selectedBestellungProduktnamen = value; RaisePropertyChanged(); }
        }


        private string selectedProdukt;

        public string SelectedProdukt
        {
            get { return selectedProdukt; }
            set { selectedProdukt = value; RaisePropertyChanged(); }
        }

        private DataHandler dh;

        public BestellverwaltungVm()
        {
            dh = new DataHandler();
            Bestellungen = GetAllOrders();
            Bestellstatusse = new ObservableCollection<string>();
            Bestellstatusse.Add("offen");
            Bestellstatusse.Add("abgebrochen");
            Bestellstatusse.Add("abgeschlossen");


            EditBestellungBtnClick = new RelayCommand(EditBestellung);

            SaveBestellungBtnClick = new RelayCommand(SaveBestellung);

            KundeKontaktierenBtnClick = new RelayCommand(ContactCustomer);


            DeleteBestellungBtnClick = new RelayCommand(
                () =>
                {
                    DeleteSelectedBestellung(SelectedBestellung);
                });

            ProduktLöschenBtnClick = new RelayCommand(ProduktLöschen);

            CancelDataBtnClick = new RelayCommand(CancelData);

            //KundeKontaktierenBtnClick = new RelayCommand();


            FilterMethoden = new ObservableCollection<string>(Bestellstatusse);
            FilterMethoden.Add("Alle");

        }

        private void ContactCustomer()
        {
           if(SelectedBestellung != null && SelectedProdukt != null)
            {

                CustomerMailService.InformCustomerAboutDelay(SelectedBestellung, dh.GetEmailCustomerForOrder(SelectedBestellung), SelectedProdukt);
            }


        }


        // TODO: mit Gollner klären ob notwendig
        private void ProduktLöschen()
        {



        }

        private void EditBestellung()
        {
            SelectedStatus = SelectedBestellung.Bestellstatus;
            BestellDatum = SelectedBestellung.BestellDatum.ToString();
            BestellNummer = SelectedBestellung.BestellId.ToString();
            GetSelectedBestellungProduktnamen();


        }

        private void GetSelectedBestellungProduktnamen()
        {
            if(selectedBestellungProduktnamen == null)
            {
                SelectedBestellungProduktnamen = new ObservableCollection<string>();
            }

            foreach (var item in selectedBestellung.Artikel)
            {

                SelectedBestellungProduktnamen.Add(item.name);
            }
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
        
             if(SelectedStatus != null && SelectedBestellung != null)
            {
                dh.UpdateOrderStatus(SelectedBestellung.BestellId, SelectedStatus);
            }


            SelectedStatus = null;
            BestellNummer = "";
            BestellDatum = "";

            RefreshOrders();
        }


        private void FilterList(string selectedFilterMethode)
        {
            Bestellungen.Clear();

            if (selectedFilterMethode.Equals("Alle"))
            {
                RefreshOrders();
            }
            else
            {
                foreach (var item in dh.GetOrdersByStatus(selectedFilterMethode))
                {

                    Bestellungen.Add(item);
                }

            }

            

        }

        private ObservableCollection<SharedBestellung> GetAllOrders()
        {
            
            return new ObservableCollection<SharedBestellung>(dh.GetAllOrders());

        }

        private void RefreshOrders()
        {

            Bestellungen.Clear();

            foreach (var item in dh.GetAllOrders())
            {
                Bestellungen.Add(item);
            }
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

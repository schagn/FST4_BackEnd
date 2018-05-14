using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SharedClasses;
using System;
using System.Collections.ObjectModel;

namespace BackEndView.ViewModel
{
    public class BewertungsverwaltungVm : ViewModelBase
    {

        public RelayCommand EditBewertungBtnClick { get; set; }
        public RelayCommand DeleteBewertungBtnClick { get; set; }
        public RelayCommand SaveBewertungBtnClick { get; set; }
        public ObservableCollection<SharedBewertung> Bewertungen { get; set; }

        private bool visibility;
        public bool Visibility
        {
            get { return visibility; }
            set { visibility = value; RaisePropertyChanged(); }
        }

        private string kundenName;
        public string KundenName
        {
            get { return kundenName; }
            set { kundenName = value; RaisePropertyChanged(); }
        }

        private string artikelName;
        public string ArtikelName
        {
            get { return artikelName; }
            set { artikelName = value; RaisePropertyChanged(); }
        }

        private SharedBewertung selectedBewertung;
        public SharedBewertung SelectedBewertung
        {
            get { return selectedBewertung; }
            set { selectedBewertung = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<string> FilterMethoden { get; set; }

        private string selectedFilterMethode;

        public string SelectedFilterMethode
        {
            get { return selectedFilterMethode; }
            set { selectedFilterMethode = value; RaisePropertyChanged(); LoadNewData(); }
        }

        public BewertungsverwaltungVm()
        {
            Bewertungen = new ObservableCollection<SharedBewertung>();

            Bewertungen.Add(new SharedBewertung()
            {
                BewertungId = Guid.NewGuid(),
                ArtikelName = "RegenbogenTorte",
                KundenName = "Helle",
                Kommentar = "Sehr lecker",
                Sterne = 3,
                Visible = true
            });

            Bewertungen.Add(new SharedBewertung()
            {
                BewertungId = Guid.NewGuid(),
                ArtikelName = "Sachertorte",
                KundenName = "Christl Sch",
                Kommentar = "Super Gut",
                Sterne = 5,
                Visible = true
            });

            EditBewertungBtnClick = new RelayCommand(EditZutat);

            SaveBewertungBtnClick = new RelayCommand(SaveZutat);

            DeleteBewertungBtnClick = new RelayCommand(
                () =>
                {
                    DeleteSelectedProduct(SelectedBewertung);
                });

            FilterMethoden = new ObservableCollection<string>();
            FilterMethoden.Add("Visible");
            FilterMethoden.Add("Non-Visible");
            FilterMethoden.Add("Alle");
        }

        private void EditZutat()
        {
            Visibility = SelectedBewertung.Visible;
            ArtikelName = SelectedBewertung.ArtikelName;
            KundenName = SelectedBewertung.KundenName;
        }

        private void DeleteSelectedProduct(SharedBewertung p)
        {
            //client.deleteZutat
            //client Zutaten neu abfragen
            Bewertungen.Remove(p);
            RaisePropertyChanged("Bewertungen");
        }

        private void SaveZutat()
        {
                foreach (var item in Bewertungen)
                {
                    if (item.BewertungId == SelectedBewertung.BewertungId)
                    {
                    item.Visible = Visibility;

                    }
                }

            RaisePropertyChanged("Bewertungen");

            KundenName = "";
            ArtikelName = "";
            Visibility = false;

            // client. SaveList 

        }

        private void LoadNewData()
        {
            // db lade neue Daten, abfrage direkt über query
            ObservableCollection<SharedBewertung> list = new ObservableCollection<SharedBewertung>();

            //ALL: To be deleted after implemented with DB 

            foreach (var item in Bewertungen)
            {
                list.Add(item);
            }
            Bewertungen.Clear();

            if(SelectedFilterMethode.Equals("Alle"))
            {
                foreach (var item in list)
                {
                    Bewertungen.Add(item);
                }
            } else if (SelectedFilterMethode.Equals("Visible"))
            {
                foreach (var item in list)
                {
                    if(item.Visible == true)
                    {
                        Bewertungen.Add(item);
                    }    
                }
            } else if (SelectedFilterMethode.Equals("Non-Visible"))
            {
                foreach (var item in list)
                {
                    if (item.Visible == false)
                    {
                        Bewertungen.Add(item);
                    }
                }
            }

            RaisePropertyChanged("Bewertungen");
           
        }

    }
}

using DataRepository;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SharedClasses;
using System;
using System.Collections.ObjectModel;

namespace BackEndView.ViewModel
{
    public class BewertungsverwaltungVm : ViewModelBase
    {

        public RelayCommand CancelDataBtnClick { get; set; }
        public RelayCommand EditBewertungBtnClick { get; set; }
        public RelayCommand DeleteBewertungBtnClick { get; set; }
        public RelayCommand SaveBewertungBtnClick { get; set; }
        public ObservableCollection<SharedBewertung> Bewertungen { get; set; }

        private bool? visibility;
        public bool? Visibility
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
            set { selectedFilterMethode = value; RaisePropertyChanged(); RefreshList(SelectedFilterMethode); }
        }

        private DataHandler dataHandler;

        public BewertungsverwaltungVm()
        {
            dataHandler = new DataHandler();

            EditBewertungBtnClick = new RelayCommand(Edit);
            SaveBewertungBtnClick = new RelayCommand(Save);
            DeleteBewertungBtnClick = new RelayCommand(
                () =>
                {
                    Delete(SelectedBewertung);
                });

            CancelDataBtnClick = new RelayCommand(CancelData);

            FilterMethoden = new ObservableCollection<string>();
            FilterMethoden.Add("Visible");
            FilterMethoden.Add("Non-Visible");
            FilterMethoden.Add("Alle");

            RefreshList(null);

        }

        private void RefreshList(string selected)
        {

            if(selected == null)
            {
                Bewertungen = new ObservableCollection<SharedBewertung>(dataHandler.GetRatingAll());
            }
            else if (selected.Equals("Visible")) 
            {
                Bewertungen = new ObservableCollection<SharedBewertung>(dataHandler.GetRatingVisible());
            }
            else if (selected.Equals("Non-Visible"))
            {
                Bewertungen = new ObservableCollection<SharedBewertung>(dataHandler.GetRatingNonVisible());
            }
            else
            {
                Bewertungen = new ObservableCollection<SharedBewertung>(dataHandler.GetRatingAll());
            }
            RaisePropertyChanged("Bewertungen");
        }

        private void Edit()
        {
            Visibility= SelectedBewertung.Visible;
            ArtikelName = SelectedBewertung.ArtikelName;
            KundenName = SelectedBewertung.KundenName;
        }

        private void Delete(SharedBewertung p)
        {
            dataHandler.DeleteRating(p);
            RefreshList(selectedFilterMethode);
        }

        private void Save()
        {
            var tempRating = selectedBewertung;
            tempRating.Visible = visibility;
            dataHandler.UpdateRatingVisibility(tempRating);

            KundenName = "";
            ArtikelName = "";
            Visibility = false;

            RefreshList(selectedFilterMethode);
        }

        private void CancelData()
        {
            SelectedBewertung = null;

            KundenName = "";
            ArtikelName = "";
            Visibility = false;

        }
    }
}

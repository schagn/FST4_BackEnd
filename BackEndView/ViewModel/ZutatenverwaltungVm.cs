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

namespace BackEndView.ViewModel
{
    public class ZutatenverwaltungVm : ViewModelBase
    {

        public RelayCommand CancelDataBtnClick { get; set; }
        public RelayCommand EditZutatBtnClick { get; set; }

        public RelayCommand DeleteZutatBtnClick { get; set; }

        public RelayCommand SaveZutatBtnClick { get; set; }

        public RelayCommand SaveZutatKategorieBtnClick { get; set; }

        public RelayCommand ZutatKategorieLöschenBtnClick { get; set; }

        public ObservableCollection<SharedZutat> Zutaten { get; set; }

        private string zutatenName;

        public string ZutatenName
        {
            get { return zutatenName; }
            set { zutatenName = value; RaisePropertyChanged(); }
        }

        private double? zutatenPreis;

        public double? ZutatenPreis
        {
            get { return zutatenPreis; }
            set { zutatenPreis = value; RaisePropertyChanged(); }
        }

        private bool? visibility;

        public bool? Visibility
        {
            get { return visibility; }
            set { visibility = value; RaisePropertyChanged(); }
        }


        private SharedZutat selectedZutat;

        public SharedZutat SelectedZutat
        {
            get { return selectedZutat; }
            set { selectedZutat = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<string> Categories { get; set; }

        private string selectedCategorie;

        public string SelectedCategorie
        {
            get { return selectedCategorie; }
            set { selectedCategorie = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<string> zutatKategorien;

        public ObservableCollection<string> ZutatKategorien
        {
            get { return zutatKategorien; }
            set { zutatKategorien = value; }
        }

        private string selectedLöschenKategorie;

        public string SelectedLöschenKategorie
        {
            get { return selectedLöschenKategorie; }
            set { selectedLöschenKategorie = value; }
        }



        public ObservableCollection<string> FilterMethoden { get; set; }

        private string selectedFilterMethode;

        public string SelectedFilterMethode
        {
            get { return selectedFilterMethode; }
            set { selectedFilterMethode = value; RaisePropertyChanged(); RefreshList(SelectedFilterMethode); }
        }


        bool IsEditingProcess;

        private DataHandler dataHandler;

        public ZutatenverwaltungVm()
        {
            dataHandler = new DataHandler();
            ZutatKategorien = new ObservableCollection<string>();

            EditZutatBtnClick = new RelayCommand(EditZutat);

            SaveZutatBtnClick = new RelayCommand(
                () =>
                {
                    if (IsEditingProcess)
                    {
                        SelectedZutat.Beschreibung = ZutatenName;
                        SelectedZutat.Preis = ZutatenPreis;
                        SelectedZutat.IsAvailable = Visibility;
                        dataHandler.UpdateZutat(selectedZutat);
                    }
                    else
                    {
                        dataHandler.CreateZutat(new SharedZutat()
                        { 
                            ZutatenId = Guid.NewGuid(),
                            Beschreibung = ZutatenName,
                            Preis = ZutatenPreis,
                            IsAvailable = Visibility
                        });
                    }
                    CancelData();
                    RefreshList(selectedFilterMethode);
                });


            DeleteZutatBtnClick = new RelayCommand(
                () =>
                {
                    DeleteSelectedProduct(SelectedZutat);
                });

            SaveZutatKategorieBtnClick = new RelayCommand(SaveZutatKategorie);

            ZutatKategorieLöschenBtnClick = new RelayCommand(LöschenZutatKategorie);

            CancelDataBtnClick = new RelayCommand(CancelData);

            FilterMethoden = new ObservableCollection<string>();
            FilterMethoden.Add("Available");
            FilterMethoden.Add("Not available");
            FilterMethoden.Add("Alle");

            SelectedFilterMethode = "Alle";

            Categories = new ObservableCollection<string>(dataHandler.GetCategories());
            RefreshList(selectedFilterMethode);

            IsEditingProcess = false;
        }

        private void RefreshList(string selectedFilterMethode)
        {
            Zutaten = new ObservableCollection<SharedZutat>(dataHandler.GetZutat(SelectedFilterMethode));
            RaisePropertyChanged("Zutaten");
        }

        private void EditZutat()
        {
            ZutatenName = SelectedZutat.Beschreibung;
            ZutatenPreis = SelectedZutat.Preis;
            if (ZutatKategorien != null)
            {
                ZutatKategorien.Clear();
            }
            foreach(var item in SelectedZutat.Kategorie)
            {
                ZutatKategorien.Add(item);
            }
            Visibility = SelectedZutat.IsAvailable;

            IsEditingProcess = true;
        }

        private void SaveZutatKategorie()
        {
            dataHandler.CreateZutatKategorie(ZutatenName, SelectedCategorie);
            //ZutatKategorien.Add(SelectedCategorie);
            RefreshList(SelectedFilterMethode);
            RaisePropertyChanged("Zutat");
        }

        private void LöschenZutatKategorie()
        {
            dataHandler.DeleteZutatKategorie(ZutatenName, SelectedLöschenKategorie);
            //ZutatKategorien.Remove(SelectedCategorie);
            RefreshList(SelectedFilterMethode);
            RaisePropertyChanged("Zutat");
        }


        private void DeleteSelectedProduct(SharedZutat p)
        {
            dataHandler.DeleteZutat(p);
            RefreshList(selectedFilterMethode);
        }

        private void CancelData()
        {
            ZutatenName = "";
            ZutatenPreis = 0;
            Visibility = false;
            SelectedCategorie = null;
            SelectedZutat = null;
            ZutatKategorien = null;

            IsEditingProcess = false;

        }
    }
}

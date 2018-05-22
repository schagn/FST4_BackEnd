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
        
        public RelayCommand EditZutatBtnClick { get; set; }

        public RelayCommand DeleteZutatBtnClick { get; set; }

        public RelayCommand SaveZutatBtnClick { get; set; }

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

        bool IsEditingProcess;

        private DataHandler dataHandler;

        public ZutatenverwaltungVm()
        {
            dataHandler = new DataHandler();

            EditZutatBtnClick = new RelayCommand(EditZutat);

            SaveZutatBtnClick = new RelayCommand(
                () =>
                {
                    if (IsEditingProcess)
                    {
                        SelectedZutat.Beschreibung = ZutatenName;
                        SelectedZutat.Preis = ZutatenPreis;
                        SelectedZutat.IsAvailable = Visibility;
                        //SelectedZutat.Kategorie = SelectedCategorie;
                        dataHandler.UpdateZutat(selectedZutat);
                    }
                    else
                    {
                        dataHandler.CreateZutat(new SharedZutat()
                        { 
                            ZutatenId = Guid.NewGuid(),
                            Beschreibung = ZutatenName,
                            Preis = ZutatenPreis,
                            IsAvailable = Visibility,
                            //Kategorie = SelectedCategorie
                        });
                    }
                    ZutatenName = "";
                    ZutatenPreis = 0;
                    SelectedCategorie = null;

                    IsEditingProcess = false;
                    RefreshList();
                });


            DeleteZutatBtnClick = new RelayCommand(
                () =>
                {
                    DeleteSelectedProduct(SelectedZutat);
                });


            RefreshList();

            IsEditingProcess = false;
        }

        private void RefreshList()
        {
            Zutaten = new ObservableCollection<SharedZutat>(dataHandler.GetZutat());
            Categories = new ObservableCollection<string>(dataHandler.GetIngredientCategories());
            RaisePropertyChanged("Zutaten");
        }


        private void EditZutat()
        {
            ZutatenName = SelectedZutat.Beschreibung;
            ZutatenPreis = SelectedZutat.Preis;
            SelectedCategorie = SelectedZutat.Kategorie;
            Visibility = SelectedZutat.IsAvailable;

            IsEditingProcess = true;
        }

        private void DeleteSelectedProduct(SharedZutat p)
        {
            dataHandler.DeleteZutat(p);
            RefreshList();
        }
    }
}

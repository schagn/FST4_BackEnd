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

        private double zutatenPreis;

        public double ZutatenPreis
        {
            get { return zutatenPreis; }
            set { zutatenPreis = value; RaisePropertyChanged(); }
        }

        private bool visibility;

        public bool Visibility
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

        public ZutatenverwaltungVm()
        {
            Zutaten = new ObservableCollection<SharedZutat>();
            Categories = new ObservableCollection<string>();
            Categories.Add("hallo");
            Categories.Add("thüss");

            EditZutatBtnClick = new RelayCommand(EditZutat);

            SaveZutatBtnClick = new RelayCommand(SaveZutat);


            DeleteZutatBtnClick = new RelayCommand(
                () =>
                {
                    DeleteSelectedProduct(SelectedZutat);
                });


            IsEditingProcess = false;
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
            //client.deleteZutat
            //client Zutaten neu abfragen
            Zutaten.Remove(p);
            RaisePropertyChanged("Zutaten");
        }

        private void SaveZutat()
        {
            if(IsEditingProcess == true)
            {
                foreach (var item in Zutaten)
                {
                    if(item.ZutatenId == SelectedZutat.ZutatenId)
                    {
                        item.Kategorie = SelectedCategorie;
                        item.Preis = ZutatenPreis;
                        item.Beschreibung = ZutatenName;
                        item.IsAvailable = Visibility;

                    }
                }
            } else
            {
                Zutaten.Add(new SharedZutat()
                {
                    Beschreibung = ZutatenName,
                    Preis = ZutatenPreis,
                    Kategorie = SelectedCategorie,
                    ZutatenId = Guid.NewGuid(),
                    IsAvailable = Visibility
                });
            }

            ZutatenName = "";
            ZutatenPreis = 0;
            SelectedCategorie = null;

            RaisePropertyChanged("Zutaten");
            
            
            // client. SaveList 

            IsEditingProcess = false;
        }
    }
}

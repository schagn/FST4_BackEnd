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
    public class VerpackungsverwaltungVm : ViewModelBase
    {


        public RelayCommand EditVerpackungBtnClick { get; set; }

        public RelayCommand DeleteVerpackungBtnClick { get; set; }

        public RelayCommand SaveVerpackungBtnClick { get; set; }

        public RelayCommand SaveVerpackungsItemBtnClick { get; set; }

        public ObservableCollection<SharedVerpackung> Verpackungen { get; set; }

        private string beschreibung;
        public string Beschreibung
        {
            get { return beschreibung; }
            set { beschreibung = value; RaisePropertyChanged(); }
        }

        private bool visibility;
        public bool Visibility
        {
            get { return visibility; }
            set { visibility = value; RaisePropertyChanged(); }
        }

        private double preis;
        public double Preis
        {
            get { return preis; }
            set { preis = value; RaisePropertyChanged(); }
        }

        private SharedVerpackung selectedVerpackung;

        public SharedVerpackung SelectedVerpackung
        {
            get { return selectedVerpackung; }
            set { selectedVerpackung = value; RaisePropertyChanged(); }
        }

        bool IsEditingProcess;

        public VerpackungsverwaltungVm()
        {
            Verpackungen = new ObservableCollection<SharedVerpackung>();

            EditVerpackungBtnClick = new RelayCommand(EditVerpackung);

            SaveVerpackungBtnClick = new RelayCommand(SaveVerpackung);


            DeleteVerpackungBtnClick = new RelayCommand(
                () =>
                {
                    DeleteSelectedVerpackung(SelectedVerpackung);
                });

            SaveVerpackungsItemBtnClick = new RelayCommand(SaveVerpackungsItem);

            IsEditingProcess = false;
        }

        private void EditVerpackung()
        {
            Beschreibung = SelectedVerpackung.Description;
            Visibility = SelectedVerpackung.Visible;
            Preis = SelectedVerpackung.Price;

            IsEditingProcess = true;
        }

        private void DeleteSelectedVerpackung(SharedVerpackung v)
        {
            //client.deleteZutat
            //client Zutaten neu abfragen
            Verpackungen.Remove(v);
            RaisePropertyChanged("Verpackungen");
        }

        private void SaveVerpackung()
        {
            if (IsEditingProcess == true)
            {
                foreach (var item in Verpackungen)
                {
                    if (item.VerpackungsId == SelectedVerpackung.VerpackungsId)
                    {
                        item.Description = Beschreibung;
                        item.Visible = Visibility;
                        item.Price = Preis;

                    }
                }
            }
            else
            {
                Verpackungen.Add(new SharedVerpackung()
                {
                    Description = Beschreibung,
                    VerpackungsId = Guid.NewGuid(),
                    Visible = Visibility,
                    Price = Preis,
                    Creation = false,
                    Komponenten = new List<SharedZutat>()
                });
            }

            Beschreibung = "";
            Visibility = false;
            Preis = 0;

            RaisePropertyChanged("Verpackungen");

            // client. SaveList 

            IsEditingProcess = false;
        }

        private void SaveVerpackungsItem()
        {
            foreach (var item in Verpackungen)
            {
                if (item.VerpackungsId == SelectedVerpackung.VerpackungsId)
                {
                    item.Komponenten.Add(new SharedZutat()
                    {
                        
                    });

                }
            }
        }


    }
}

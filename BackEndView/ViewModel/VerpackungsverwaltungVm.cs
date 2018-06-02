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

        public RelayCommand CancelDataBtnClick { get; set; }
        public RelayCommand EditVerpackungBtnClick { get; set; }

        public RelayCommand DeleteVerpackungBtnClick { get; set; }

        public RelayCommand SaveVerpackungBtnClick { get; set; }

        public RelayCommand SaveVerpackungsItemBtnClick { get; set; }

        public RelayCommand KomponenteLöschenBtnClick { get; set; }

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

        private bool creation;
        public bool Creation
        {
            get { return creation; }
            set { creation = value; RaisePropertyChanged(); }
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

        private ObservableCollection<string> verpackungsArten;

        public ObservableCollection<string> VerpackungsArten
        {
            get { return verpackungsArten; }
            set { verpackungsArten = value; RaisePropertyChanged();  }
        }

        private ObservableCollection<string> verpackungsteile;

        public ObservableCollection<string> Verpackungsteile
        {
            get { return verpackungsteile; }
            set { verpackungsteile = value; RaisePropertyChanged(); }
        }

        private string selectedVerpackungsArt;

        public string SelectedVerpackungsArt
        {
            get { return selectedVerpackungsArt; }
            set { selectedVerpackungsArt = value; RaisePropertyChanged(); ChangeVerpackungsteile(); }
        }

        private string selectedVerpackungsteil;

        public string SelectedVerpackungsteil
        {
            get { return selectedVerpackungsteil; }
            set { selectedVerpackungsteil = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<string> verpackungsKomponenten;

        public ObservableCollection<string> VerpackungsKomponenten
        {
            get { return verpackungsKomponenten; }
            set { verpackungsKomponenten = value; RaisePropertyChanged(); }
        }

        private string selectedLöschenVerpackungsteil;

        public string SelectedLöschenVerpackungsteil
        {
            get { return selectedLöschenVerpackungsteil; }
            set { selectedLöschenVerpackungsteil = value; RaisePropertyChanged(); }
        }

        public List<string> VisibilityFilter { get; set; }
        public List<string> CreationFilter { get; set; }
        private string selectedVisibilityFilter;

        public string SelectedVisibilityFilter
        {
            get { return selectedVisibilityFilter; }
            set { selectedVisibilityFilter = value; RaisePropertyChanged(); RefreshList(); }
        }
        private string selectedCreationFilter;

        public string SelectedCreationFilter
        {
            get { return selectedCreationFilter; }
            set { selectedCreationFilter = value; RaisePropertyChanged(); RefreshList(); }
        }

        bool IsEditingProcess;

        public VerpackungsverwaltungVm()
        {
            Verpackungen = new ObservableCollection<SharedVerpackung>();
            Verpackungsteile = new ObservableCollection<string>();
            Verpackungsteile.Add("Blumenmasche");
            Verpackungsteile.Add("Sternekarton");
            VerpackungsArten = new ObservableCollection<string>();
            VerpackungsArten.Add("Karton");
            VerpackungsArten.Add("Masche");
            VerpackungsKomponenten = new ObservableCollection<string>();

            EditVerpackungBtnClick = new RelayCommand(EditVerpackung);

            SaveVerpackungBtnClick = new RelayCommand(SaveVerpackung);


            DeleteVerpackungBtnClick = new RelayCommand(
                () =>
                {
                    DeleteSelectedVerpackung(SelectedVerpackung);
                });

            SaveVerpackungsItemBtnClick = new RelayCommand(SaveVerpackungsItem);

            KomponenteLöschenBtnClick = new RelayCommand(KomponenteLöschen);

            CancelDataBtnClick = new RelayCommand(CancelData);

            VisibilityFilter = new List<string>() { "Sichtbar & Nicht Sichtbar", "Sichtbar", "Nicht Sichtbar" };
            CreationFilter = new List<string>() { "Kreation & Nicht Kreation", "Kreation", "Nicht Kreation" };
            selectedVisibilityFilter = "Sichtbar & Nicht Sichtbar";
            selectedCreationFilter = "Kreation & Nicht Kreation";


            IsEditingProcess = false;
        }

        private void EditVerpackung()
        {
            Beschreibung = SelectedVerpackung.Description;
            Visibility = SelectedVerpackung.Visible;
            Preis = SelectedVerpackung.Price;
            foreach (var item in SelectedVerpackung.Komponenten)
            {
                VerpackungsKomponenten.Add(item.Beschreibung);   
            }

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
                        item.Creation = Creation;

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
                    Creation = Creation,
                    Komponenten = new ObservableCollection<SharedZutat>()
                });
            }

            Beschreibung = "";
            Visibility = false;
            Creation = false;
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
                    SelectedVerpackung.Komponenten.Add(new SharedZutat()
                    {
                        Beschreibung = SelectedVerpackungsteil,
                        IsAvailable = true,
                        Preis = 5,
                        ZutatenId = Guid.NewGuid()
                        
                    });
                    RaisePropertyChanged("item");
                    break;
                }
            }
            RaisePropertyChanged("SelectedVerpackung");
            RaisePropertyChanged("SelectedVerpackung.Komponenten");
            RaisePropertyChanged("Verpackungen");

            VerpackungsKomponenten.Clear();
            foreach (var item in SelectedVerpackung.Komponenten)
            {
                VerpackungsKomponenten.Add(item.Beschreibung);
            }


            SelectedVerpackungsteil = "";
            SelectedVerpackungsArt = "";
        }


        private void ChangeVerpackungsteile()
        {
            //wenn vorher Karton ausgewählt und gespeichert, dann jetzt nicht mehr anzeigen
            // jede Kategorie, die bereits in DB ist soll nicht mehr angezeigt werden 

            Verpackungsteile.Clear();

            if(SelectedVerpackungsArt.Contains("Karton"))
            {
                Verpackungsteile.Add("Blumenkarton");
                Verpackungsteile.Add("Sternekarton");
                Verpackungsteile.Add("weißer Karton");
            } else
            {
                Verpackungsteile.Add("Sternemasche");
                Verpackungsteile.Add("Goldmasche");
                Verpackungsteile.Add("Schleifchen");
            }

            

        }

        private void KomponenteLöschen()
        {

            foreach (var item in Verpackungen)
            {
                if (item.VerpackungsId == SelectedVerpackung.VerpackungsId)
                {
                    foreach (var teil in item.Komponenten)
                    {
                        if(teil.Beschreibung.Contains(SelectedLöschenVerpackungsteil))
                        {
                            item.Komponenten.Remove(teil);
                            break;
                        }
                    }

                }
            }

            //SelectedBestellung.Artikel.Remove(SelectedProdukt);
            RaisePropertyChanged("Verpackungen");
            SelectedLöschenVerpackungsteil = null;
            SelectedVerpackung = null;
            Beschreibung = "";
            Visibility = false;
            Creation = false;
            Preis = 0;
        }

        private void RefreshList()
        {
            // je nachdem welche Filtermethode ausgewählt ist --> neu von DB laden  
        }

        private void CancelData()
        {
            SelectedLöschenVerpackungsteil = null;
            SelectedVerpackung = null;
            Beschreibung = "";
            Visibility = false;
            Creation = false;
            Preis = 0;
            SelectedVerpackungsteil = "";
            SelectedVerpackungsArt = "";

            IsEditingProcess = false;

        }

    }
}

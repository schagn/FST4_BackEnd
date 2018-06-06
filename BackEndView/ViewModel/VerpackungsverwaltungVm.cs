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

        private bool? visibility;
        public bool? Visibility
        {
            get { return visibility; }
            set { visibility = value; RaisePropertyChanged(); }
        }

        private bool? creation;
        public bool? Creation
        {
            get { return creation; }
            set { creation = value; RaisePropertyChanged(); }
        }

        private double? preis;
        public double? Preis
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

        private DataHandler dataHanlder;

        public VerpackungsverwaltungVm()
        {
            dataHanlder = new DataHandler();

            Verpackungsteile = new ObservableCollection<string>();
            Verpackungsteile.Add("Blumenmasche");
            Verpackungsteile.Add("Sternekarton");
            VerpackungsArten = new ObservableCollection<string>();
            VerpackungsArten.Add("Karton");
            VerpackungsArten.Add("Masche");
            VerpackungsKomponenten = new ObservableCollection<string>();

            EditVerpackungBtnClick = new RelayCommand(EditVerpackung);

            SaveVerpackungBtnClick = new RelayCommand(
                () => 
                {
                    if (IsEditingProcess)
                    {
                        SelectedVerpackung.Description = Beschreibung;
                        SelectedVerpackung.Creation = Creation;
                        SelectedVerpackung.Price = Preis;
                        SelectedVerpackung.Visible = Visibility;
                        SelectedVerpackung.Komponenten.Clear();
                        foreach(var item in VerpackungsKomponenten)
                        {
                            SelectedVerpackung.Komponenten.Add(item);
                        }
                        dataHanlder.UpdateVerpackung(SelectedVerpackung);
                    }
                    else
                    {
                        dataHanlder.CreateVerpackung(new SharedVerpackung()
                        {
                            VerpackungsId = Guid.NewGuid(),
                            Description = Beschreibung,
                            Price = Preis,
                            Creation = Creation,
                            Visible = Visibility
                        });
                    }
                    CancelData();
                    RefreshList();
                });


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

            RefreshList();
        }

        private void RefreshList()
        {
            Verpackungen = new ObservableCollection<SharedVerpackung>(dataHanlder.GetPackaging(SelectedVisibilityFilter,SelectedCreationFilter));
            RaisePropertyChanged("Verpackungen");
        }

        private void EditVerpackung()
        {
            Beschreibung = SelectedVerpackung.Description;
            Visibility = SelectedVerpackung.Visible;
            Creation = SelectedVerpackung.Creation;
            Preis = SelectedVerpackung.Price;
            foreach (var item in SelectedVerpackung.Komponenten)
            {
                VerpackungsKomponenten.Add(item);   
            }

            IsEditingProcess = true;
        }

        private void DeleteSelectedVerpackung(SharedVerpackung v)
        {
            dataHanlder.DeleteVerpackung(v);
            RefreshList();
        }

        private void SaveVerpackungsItem()
        {
            dataHanlder.CreateVerpackungskomponenten(SelectedVerpackung.VerpackungsId, SelectedVerpackungsteil);
            RaisePropertyChanged("SelectedVerpackung");
            RaisePropertyChanged("SelectedVerpackung.Komponenten");
            RaisePropertyChanged("Verpackungen");

            SelectedVerpackungsteil = "";
            SelectedVerpackungsArt = "";

            RefreshList();
        }


        private void ChangeVerpackungsteile()
        {
            //wenn vorher Karton ausgewählt und gespeichert, dann jetzt nicht mehr anzeigen
            // jede Kategorie, die bereits in DB ist soll nicht mehr angezeigt werden 

            Verpackungsteile.Clear();

            if (SelectedVerpackungsArt.Contains("Karton"))
            {
                Verpackungsteile.Add("Blumenkarton");
                Verpackungsteile.Add("Sternekarton");
                Verpackungsteile.Add("weißer Karton");
            }
            else
            {
                Verpackungsteile.Add("Sternemasche");
                Verpackungsteile.Add("Goldmasche");
                Verpackungsteile.Add("Schleifchen");
            }
        }

        private void KomponenteLöschen()
        {

            dataHanlder.DeleteVerpackungskomponenten(SelectedVerpackung.VerpackungsId, SelectedLöschenVerpackungsteil);

            RaisePropertyChanged("Verpackungen");
            SelectedLöschenVerpackungsteil = null;
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
            VerpackungsKomponenten = null;

            IsEditingProcess = false;

        }

    }
}

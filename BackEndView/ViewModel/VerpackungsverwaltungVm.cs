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

        private ObservableCollection<string> maschen;

        public ObservableCollection<string> Maschen
        {
            get { return maschen; }
            set { maschen = value; RaisePropertyChanged();  }
        }

        private ObservableCollection<string> kartons;

        public ObservableCollection<string> Kartons
        {
            get { return kartons; }
            set { kartons = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<string> sticker;

        public ObservableCollection<string> Sticker
        {
            get { return sticker; }
            set { sticker = value; RaisePropertyChanged(); }
        }

        private string selectedMasche;

        public string SelectedMasche
        {
            get { return selectedMasche; }
            set { selectedMasche = value; RaisePropertyChanged(); }
        }
        
        private string selectedKarton;

        public string SelectedKarton
        {
            get { return selectedKarton; }
            set { selectedKarton = value; RaisePropertyChanged(); }
        }

        private string selectedSticker;

        public string SelectedSticker
        {
            get { return selectedSticker; }
            set { selectedSticker = value; RaisePropertyChanged(); }
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

            Maschen = new ObservableCollection<string>(dataHanlder.GetMaschen());
            Kartons = new ObservableCollection<string>(dataHanlder.GetKarton());
            Sticker = new ObservableCollection<string>(dataHanlder.GetSticker());
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
                        if(VerpackungsKomponenten != null)
                        {
                            VerpackungsKomponenten.Clear();
                        }
                        VerpackungsKomponenten.Add(SelectedKarton);
                        VerpackungsKomponenten.Add(SelectedMasche);
                        VerpackungsKomponenten.Add(SelectedSticker);
                        foreach (var item in VerpackungsKomponenten)
                        {
                            SelectedVerpackung.Komponenten.Add(item);
                            dataHanlder.CreateVerpackungskomponenten(SelectedVerpackung.VerpackungsId, SelectedKarton);
                        }
                        dataHanlder.UpdateVerpackung(SelectedVerpackung);
                    }
                    else
                    {
                        Guid tempGuid = Guid.NewGuid();
                        dataHanlder.CreateVerpackung(new SharedVerpackung()
                        {
                            VerpackungsId = tempGuid,
                            Description = Beschreibung,
                            Price = Preis,
                            Creation = Creation,
                            Visible = Visibility
                        });
                        dataHanlder.CreateVerpackungskomponenten(tempGuid, SelectedKarton);
                        dataHanlder.CreateVerpackungskomponenten(tempGuid, SelectedMasche);
                        dataHanlder.CreateVerpackungskomponenten(tempGuid, SelectedSticker);
                    }
                    CancelData();
                    RefreshList();
                });


            DeleteVerpackungBtnClick = new RelayCommand(
                () =>
                {
                    DeleteSelectedVerpackung(SelectedVerpackung);
                });

            //SaveVerpackungsItemBtnClick = new RelayCommand(SaveVerpackungsItem);

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

        //private void SaveVerpackungsItem()
        //{
        //    dataHanlder.CreateVerpackungskomponenten(SelectedVerpackung.VerpackungsId, SelectedVerpackungsteil);
        //    RaisePropertyChanged("SelectedVerpackung");
        //    RaisePropertyChanged("SelectedVerpackung.Komponenten");
        //    RaisePropertyChanged("Verpackungen");

        //    SelectedVerpackungsteil = "";
        //    SelectedVerpackungsArt = "";

        //    RefreshList();
        //}


       

        private void KomponenteLöschen()
        {

            dataHanlder.DeleteVerpackungskomponenten(Beschreibung, SelectedLöschenVerpackungsteil);

            RaisePropertyChanged("Verpackungen");
            SelectedLöschenVerpackungsteil = null;
            RefreshList();
        }

        private void CancelData()
        {
            SelectedLöschenVerpackungsteil = null;
            SelectedVerpackung = null;
            Beschreibung = "";
            Visibility = false;
            Creation = false;
            Preis = 0;
            SelectedKarton = "";
            SelectedMasche = "";
            SelectedSticker = "";
            VerpackungsKomponenten = null;

            IsEditingProcess = false;

        }

    }
}

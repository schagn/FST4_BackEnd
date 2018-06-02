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
    public class PackageverwaltungVm : ViewModelBase
    {

        public RelayCommand CancelDataBtnClick { get; set; }
        public RelayCommand EditPackageBtnClick { get; set; }

        public RelayCommand DeletePackageBtnClick { get; set; }

        public RelayCommand SavePackageBtnClick { get; set; }

        public RelayCommand SavePackageItemBtnClick { get; set; }

        public RelayCommand KomponenteLöschenBtnClick { get; set; }

        public ObservableCollection<SharedPackage> Packages { get; set; }

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

        private SharedPackage selectedPackage;

        public SharedPackage SelectedPackage
        {
            get { return selectedPackage; }
            set { selectedPackage = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<string> kuchenArten;

        public ObservableCollection<string> KuchenArten
        {
            get { return kuchenArten; }
            set { kuchenArten = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<string> kuchenAuswahl;

        public ObservableCollection<string> KuchenAuswahl
        {
            get { return kuchenAuswahl; }
            set { kuchenAuswahl = value; RaisePropertyChanged(); }
        }

        private string selectedKuchenArt;

        public string SelectedKuchenArt
        {
            get { return selectedKuchenArt; }
            set { selectedKuchenArt = value; RaisePropertyChanged(); ChangeKuchenAuswahl(); }
        }

        private string selectedKuchen;

        public string SelectedKuchen
        {
            get { return selectedKuchen; }
            set { selectedKuchen = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<string> packageKomponenten;

        public ObservableCollection<string> PackageKomponenten
        {
            get { return packageKomponenten; }
            set { packageKomponenten = value; RaisePropertyChanged(); }
        }

        private string selectedLöschenKuchen;

        public string SelectedLöschenKuchen
        {
            get { return selectedLöschenKuchen; }
            set { selectedLöschenKuchen = value; RaisePropertyChanged(); }
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

        public PackageverwaltungVm()
        {
            Packages = new ObservableCollection<SharedPackage>();
            KuchenArten = new ObservableCollection<string>();
            KuchenArten.Add("Kuchenkreationen");
            KuchenArten.Add("Standardkuchen");
            KuchenAuswahl = new ObservableCollection<string>();
            KuchenAuswahl.Add("Regenbogentorte");
            KuchenAuswahl.Add("Himbeertiramisu");
            KuchenAuswahl.Add("Pyramide");

            PackageKomponenten = new ObservableCollection<string>();

            EditPackageBtnClick = new RelayCommand(EditPackage);

            SavePackageBtnClick = new RelayCommand(SavePackage);


            DeletePackageBtnClick = new RelayCommand(
                () =>
                {
                    DeleteSelectedPackage(SelectedPackage);
                });

            SavePackageItemBtnClick = new RelayCommand(SavePackageItem);

            KomponenteLöschenBtnClick = new RelayCommand(KomponenteLöschen);

            CancelDataBtnClick = new RelayCommand(CancelData);

            VisibilityFilter = new List<string>() { "Sichtbar & Nicht Sichtbar", "Sichtbar", "Nicht Sichtbar" };
            CreationFilter = new List<string>() { "Kreation & Nicht Kreation", "Kreation", "Nicht Kreation" };
            selectedVisibilityFilter = "Sichtbar & Nicht Sichtbar";
            selectedCreationFilter = "Kreation & Nicht Kreation";


            IsEditingProcess = false;
        }

        private void EditPackage()
        {
            Beschreibung = SelectedPackage.Beschreibung;
            Visibility = SelectedPackage.Visible;
            Preis = SelectedPackage.Preis;
            foreach (var item in SelectedPackage.Kuchen)
            {
                PackageKomponenten.Add(item.Description);
            }

            IsEditingProcess = true;
        }

        private void DeleteSelectedPackage(SharedPackage p)
        {
            //client.deleteZutat
            //client Zutaten neu abfragen
            Packages.Remove(p);
            RaisePropertyChanged("Packages");
        }

        private void SavePackage()
        {
            if (IsEditingProcess == true)
            {
                foreach (var item in Packages)
                {
                    if (item.PackageId == SelectedPackage.PackageId)
                    {
                        item.Beschreibung = Beschreibung;
                        item.Visible = Visibility;
                        item.Preis = Preis;
                        item.Creation = Creation;
                    }
                }
            }
            else
            {
                Packages.Add(new SharedPackage()
                {
                    Beschreibung = Beschreibung,
                    PackageId = Guid.NewGuid(),
                    Visible = Visibility,
                    Preis = Preis,
                    Creation = Creation,
                    Kuchen = new ObservableCollection<SharedArticle>()
                });
            }

            Beschreibung = "";
            Visibility = false;
            Creation = false;
            Preis = 0;

            RaisePropertyChanged("Packages");

            IsEditingProcess = false;
        }

        private void SavePackageItem()
        {
            foreach (var item in Packages)
            {
                if (item.PackageId == SelectedPackage.PackageId)
                {
                    SelectedPackage.Kuchen.Add(new SharedArticle()
                    {
                        Description = SelectedKuchen,
                        Visible = true,
                        Price = 5,
                        ArticleId = Guid.NewGuid()

                    });
                    RaisePropertyChanged("item");
                    break;
                }
            }
            RaisePropertyChanged("SelectedPackage");
            RaisePropertyChanged("Packages");

            PackageKomponenten.Clear();
            foreach (var item in SelectedPackage.Kuchen)
            {
                PackageKomponenten.Add(item.Description);
            }

            
            SelectedKuchenArt = "";
            SelectedKuchen = "";
        }


        private void ChangeKuchenAuswahl()
        {
            //wenn vorher Karton ausgewählt und gespeichert, dann jetzt nicht mehr anzeigen
            // jede Kategorie, die bereits in DB ist soll nicht mehr angezeigt werden 

            KuchenAuswahl.Clear();

            if (SelectedKuchenArt.Contains("Kuchenkreation"))
            {
                KuchenAuswahl.Add("Regenbogentorte");
                KuchenAuswahl.Add("Himbeertiramisu");
                KuchenAuswahl.Add("Pyramide");
            }
            else
            {
                KuchenAuswahl.Add("Sachertorte");
                KuchenAuswahl.Add("Linzer Schnitte");
                KuchenAuswahl.Add("Esterhazy Schnitte");
            }



        }

        private void KomponenteLöschen()
        {

            foreach (var item in Packages)
            {
                if (item.PackageId == SelectedPackage.PackageId)
                {
                    foreach (var teil in item.Kuchen)
                    {
                        if (teil.Description.Contains(SelectedLöschenKuchen))
                        {
                            item.Kuchen.Remove(teil);
                            break;
                        }
                    }

                }
            }

            //SelectedBestellung.Artikel.Remove(SelectedProdukt);
            RaisePropertyChanged("Packages");
            SelectedLöschenKuchen = null;
            SelectedPackage = null;
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
            SelectedPackage = null;
            SelectedKuchen = null;
            Beschreibung = "";
            Visibility = false;
            Creation = false;
            Preis = 0;
            SelectedLöschenKuchen = "";
            SelectedKuchenArt = "";

            IsEditingProcess = false;
        }

    }
}

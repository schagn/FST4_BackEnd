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
            set { selectedKuchenArt = value; RaisePropertyChanged(); RefreshKuchenList(); }
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
            set { selectedVisibilityFilter = value; RaisePropertyChanged(); RefreshList(SelectedVisibilityFilter, SelectedCreationFilter); }
        }
        private string selectedCreationFilter;

        public string SelectedCreationFilter
        {
            get { return selectedCreationFilter; }
            set { selectedCreationFilter = value; RaisePropertyChanged(); RefreshList(SelectedVisibilityFilter, SelectedCreationFilter); }
        }

        bool IsEditingProcess;

        private DataHandler dataHandler;

        public PackageverwaltungVm()
        {
            dataHandler = new DataHandler();

            Packages = new ObservableCollection<SharedPackage>();

            KuchenArten = new ObservableCollection<string>();
            KuchenArten.Add("Standardkuchen");
            KuchenArten.Add("Kuchenkreationen");
            SelectedKuchenArt = "Standardkuchen";

            //db
            

            PackageKomponenten = new ObservableCollection<string>();

            EditPackageBtnClick = new RelayCommand(EditPackage);

            SavePackageBtnClick = new RelayCommand(
                () =>
                {
                    if (IsEditingProcess)
                    {
                        SelectedPackage.Beschreibung = Beschreibung;
                        SelectedPackage.Preis = Preis;
                        SelectedPackage.Visible = Visibility;
                        dataHandler.UpdatePackage(SelectedPackage);
                    }
                    else
                    {
                        dataHandler.CreatePackage(new SharedPackage()
                        {
                            PackageId = Guid.NewGuid(),
                            Beschreibung = Beschreibung,
                            Preis = Preis,
                            Visible = Visibility
                        });
                    }
                    CancelData();
                    RefreshList(SelectedVisibilityFilter, SelectedCreationFilter);
                });


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

            RefreshList(SelectedVisibilityFilter, SelectedCreationFilter);
        }

        public void RefreshKuchenList()
        {
            KuchenAuswahl = new ObservableCollection<string>(dataHandler.GetCakeTypes(SelectedKuchenArt));
        }

        private void RefreshList(string filterVisible, string filterCreation)
        {
            Packages = new ObservableCollection<SharedPackage>(dataHandler.GetPackages(filterVisible, filterCreation));
            RaisePropertyChanged("Packages");
        }

        private void EditPackage()
        {
            PackageKomponenten.Clear();
            Beschreibung = SelectedPackage.Beschreibung;
            Visibility = SelectedPackage.Visible;
            Preis = SelectedPackage.Preis;
            if(SelectedPackage.Kuchen == null)
            {
                foreach (var item in SelectedPackage.Kuchen)
                {
                    PackageKomponenten.Add(item);
                }

            }

            IsEditingProcess = true;
        }

        private void DeleteSelectedPackage(SharedPackage p)
        {
            dataHandler.DeletePackage(p);
            RefreshList(SelectedVisibilityFilter, SelectedCreationFilter);
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
                        item.Kuchen.Add(SelectedKuchen);

                        dataHandler.UpdatePackage(item);
                    }
                }
            }
            else
            {
                dataHandler.CreatePackage(new SharedPackage()
                {
                    Beschreibung = Beschreibung,
                    PackageId = Guid.NewGuid(),
                    Visible = Visibility,
                    Preis = Preis,
                    Creation = Creation,
                    //kuchen = new ObservableCollection<SharedArticle>()
                });
            }

            Beschreibung = "";
            Visibility = false;
            Creation = false;
            Preis = 0;

            RefreshList(SelectedVisibilityFilter, SelectedCreationFilter);

            IsEditingProcess = false;
        }

        private void SavePackageItem()
        {
            //dataHandler
        }

        private void KomponenteLöschen()
        {

            foreach (var item in Packages)
            {
                if (item.PackageId == SelectedPackage.PackageId)
                {
                    foreach (var teil in item.Kuchen)
                    {
                        if (teil.Contains(SelectedLöschenKuchen))
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

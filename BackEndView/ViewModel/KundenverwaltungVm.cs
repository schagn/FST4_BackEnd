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
using System.Windows;

namespace BackEndView.ViewModel
{
    public class KundenverwaltungVm : ViewModelBase
    {
        //TODO PASSWORT !!!!! 

        public RelayCommand CancelDataBtnClick { get; set; }
        public RelayCommand EditKundeBtnClick { get; set; }

        public RelayCommand DeleteKundeBtnClick { get; set; }

        public RelayCommand SaveKundeBtnClick { get; set; }

        public ObservableCollection<SharedKunde> Kunden { get; set; }


        private string vorName;
        public string VorName
        {
            get { return vorName; }
            set { vorName = value; RaisePropertyChanged(); }
        }

        private string nachName;
        public string NachName
        {
            get { return nachName; }
            set { nachName = value; RaisePropertyChanged(); }
        }

        private string eMail;
        public string EMail
        {
            get { return eMail; }
            set { eMail = value; RaisePropertyChanged(); }
        }

        private string passwort;
        public string Passwort
        {
            get { return passwort; }
            set { passwort = value; RaisePropertyChanged(); }
        }

        private string geburtsdatum;
        public string Geburtsdatum
        {
            get { return geburtsdatum; }
            set { geburtsdatum = value; RaisePropertyChanged(); }
        }

        private string strasse;
        public string Strasse
        {
            get { return strasse; }
            set { strasse = value; RaisePropertyChanged(); }
        }

        private int plz;
        public int PLZ
        {
            get { return plz; }
            set { plz = value; RaisePropertyChanged(); }
        }

        private string ort;
        public string Ort
        {
            get { return ort; }
            set { ort = value; RaisePropertyChanged(); }
        }

        private string land;
        public string Land
        {
            get { return land; }
            set { land = value; RaisePropertyChanged(); }
        }

        private bool isBusinessCustomer;

        public bool IsBusinessCustomer
        {
            get { return isBusinessCustomer; }
            set { isBusinessCustomer = value;RaisePropertyChanged(); }
        }

        private float uid;

        public float UID
        {
            get { return uid; }
            set { uid = value; RaisePropertyChanged(); }
        }



        private SharedKunde selectedKunde;

        public SharedKunde SelectedKunde
        {
            get { return selectedKunde; }
            set { selectedKunde = value; RaisePropertyChanged(); }
        }

        bool IsEditingProcess;


        public ObservableCollection<string> FilterMethoden { get; set; }

        private string selectedFilterMethode;

        public string SelectedFilterMethode
        {
            get { return selectedFilterMethode; }
            set { selectedFilterMethode = value; RaisePropertyChanged(); FilterList(SelectedFilterMethode); }
        }

        //DependencyObject depobj = new DependencyObject();

        private DataHandler dh = new DataHandler();

        public KundenverwaltungVm()
        {
            Kunden = GetAllCustomers();

            EditKundeBtnClick = new RelayCommand(EditKunde);

            SaveKundeBtnClick = new RelayCommand(SaveKunde);

            DeleteKundeBtnClick = new RelayCommand(
                () =>
                {
                    DeleteSelectedKunde(SelectedKunde);
                });

            CancelDataBtnClick = new RelayCommand(CancelData);

            FilterMethoden = new ObservableCollection<string>();
            FilterMethoden.Add("Firmenkunden");
            FilterMethoden.Add("Privatkunden");
            FilterMethoden.Add("Alle");

            IsEditingProcess = false;
        }

        private void EditKunde()
        {
            VorName = SelectedKunde.VorName;
            NachName = SelectedKunde.NachName;
            Geburtsdatum = SelectedKunde.Geburtsdatum;
            EMail = SelectedKunde.EMail;
            Strasse = SelectedKunde.Strasse;
            PLZ = SelectedKunde.PLZ;
            Ort = SelectedKunde.Ort;
            Land = SelectedKunde.Land;
            Passwort = Passwort;
            IsBusinessCustomer = SelectedKunde.IsBusinessCustomer;

            IsEditingProcess = true;
        }

        private void DeleteSelectedKunde(SharedKunde k)
        {

            dh.DeleteCustomer(k);
            RefreshCustomers();
            RaisePropertyChanged("Kunden");
        }

        private void SaveKunde()
        {
            if (IsEditingProcess == true)
            {

                SelectedKunde.VorName = VorName;
                SelectedKunde.NachName = NachName;
                SelectedKunde.Geburtsdatum = Geburtsdatum;
                SelectedKunde.EMail = EMail;
                SelectedKunde.Strasse = Strasse;
                SelectedKunde.PLZ = PLZ;
                SelectedKunde.Ort = Ort;
                SelectedKunde.Land = Land;
                SelectedKunde.IsBusinessCustomer = IsBusinessCustomer;

                dh.UpdateCustomer(SelectedKunde);
                SelectedKunde = null;
                IsEditingProcess = false;
                

            }
            else
            {
                SharedCity customerCity = new SharedCity(PLZ, Ort);
                SharedKunde newCustomer = new SharedKunde()
                {
                    KundenId = Guid.NewGuid(),
                    VorName = VorName,
                    NachName = NachName,
                    Geburtsdatum = Geburtsdatum,
                    EMail = EMail,
                    Strasse = Strasse,
                    PLZ = PLZ,
                    Ort = Ort,
                    Land = Land,
                    IsBusinessCustomer = IsBusinessCustomer
                    //Passwort = PasswordHelper.GetPassword(depobj)
                };
                dh.AddCusomter(newCustomer);
                
            }

            RefreshCustomers();

            VorName = "";
            NachName = "";
            Geburtsdatum = "";
            EMail = "";
            Strasse = "";
            PLZ = 0;
            Ort = "";
            Land = "";
            Passwort = "";  
        }

        private void FilterList(string selectedFilterMethode)
        {
            if (selectedFilterMethode.Equals("Alle"))
            {
                RefreshCustomers();

            }
            else
            {


                if (selectedFilterMethode.Equals("Firmenkunden")) selectedFilterMethode = "Firmenkunde";
                if (selectedFilterMethode.Equals("Privatkunden")) selectedFilterMethode = "Kunde";

                Kunden.Clear();

                foreach (var item in dh.FilterCustomersByType(selectedFilterMethode))
                {
                    Kunden.Add(item);
                }
            }
        }

        public ObservableCollection<SharedKunde> GetAllCustomers()
        {

            return new ObservableCollection<SharedKunde>(dh.GetAllCustomers());

        }

        public void RefreshCustomers()
        {

            Kunden.Clear();
            

            foreach (var item in dh.GetAllCustomers())
            {

                Kunden.Add(item);

            }
        }

        private void CancelData()
        {
            VorName = "";
            NachName = "";
            Geburtsdatum = "";
            EMail = "";
            Strasse = "";
            PLZ = 0;
            Ort = "";
            Land = "";
            Passwort = "";
            IsBusinessCustomer = false;
            SelectedKunde = null;

            IsEditingProcess = false;

        }

    }
}

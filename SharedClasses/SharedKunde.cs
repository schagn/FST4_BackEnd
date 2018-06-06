using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class SharedKunde : ViewModelBase
    {

        public Guid KundenId { get; set; }

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
            set { isBusinessCustomer = value; RaisePropertyChanged(); }
        }

        private float uid;

        public float UID
        {
            get { return uid; }
            set { uid = value; RaisePropertyChanged(); }
        }


        private bool isVisible;
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; RaisePropertyChanged(); }
        }

    }
}

using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class SharedZutat : ViewModelBase
    {

        private string beschreibung;

        public string Beschreibung
        {
            get { return beschreibung; }
            set { beschreibung = value; RaisePropertyChanged(); }
        }

        private double? preis;

        public double? Preis
        {
            get { return preis; }
            set { preis = value; RaisePropertyChanged(); }
        }

        private bool isAvailable;



        public bool IsAvailable
        {
            get { return isAvailable; }
            set { isAvailable = value; RaisePropertyChanged(); }
        }

        private string kategorie;

        public string Kategorie
        {
            get { return kategorie; }
            set { kategorie = value; }
        }


        public Guid ZutatenId { get; set; }
     

    }
}

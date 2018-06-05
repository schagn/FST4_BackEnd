using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private bool? isAvailable;

        public bool? IsAvailable
        {
            get { return isAvailable; }
            set { isAvailable = value; RaisePropertyChanged(); }
        }

        private List<string> kategorie;

        public List<string> Kategorie
        {
            get { return kategorie; }
            set { kategorie = value; }
        }

        public string KategorieString { get; set; }


        public Guid ZutatenId { get; set; }
     

    }
}

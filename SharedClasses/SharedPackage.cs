using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class SharedPackage : ViewModelBase
    {

        public Guid PackageId { get; set; }

        private string beschreibung;

        public string Beschreibung
        {
            get { return beschreibung; }
            set { beschreibung = value; RaisePropertyChanged(); }
        }

        private double preis;

        public double Preis
        {
            get { return preis; }
            set { preis = value; RaisePropertyChanged(); }
        }

        private List<SharedArticle> kuchen;

        public List<SharedArticle> Kuchen
        {
            get { return kuchen; }
            set { kuchen = value; RaisePropertyChanged(); }
        }

    }
}

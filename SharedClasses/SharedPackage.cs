using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private double? preis;

        public double? Preis
        {
            get { return preis; }
            set { preis = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<SharedArticle> kuchen;

        public ObservableCollection<SharedArticle> Kuchen
        {
            get { return kuchen; }
            set { kuchen = value; RaisePropertyChanged(); }
        }

        private bool? visible;

        public bool? Visible
        {
            get { return visible; }
            set { visible = value; RaisePropertyChanged(); }
        }

        private bool? creation;
        public bool? Creation
        {
            get { return creation; }
            set { creation = value; RaisePropertyChanged(); }
        }

    }
}

using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class SharedVerpackung : ViewModelBase
    {
        public Guid VerpackungsId { get; set; }

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; RaisePropertyChanged(); }
        }

        private double price;
        public double Price
        {
            get { return price; }
            set { price = value; RaisePropertyChanged(); }
        }

        private bool creation;
        public bool Creation
        {
            get { return creation; }
            set { creation = value; RaisePropertyChanged(); }
        }

        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set { visible = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<SharedZutat> komponenten;

        public ObservableCollection<SharedZutat> Komponenten
        {
            get { return komponenten; }
            set { komponenten = value; RaisePropertyChanged(); }
        }


    }
}

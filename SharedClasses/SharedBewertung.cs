using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class SharedBewertung : ViewModelBase
    {
        public Guid ArticleId { get; set; }

        public Guid PersonId { get; set; }

        private int? sterne;

        public int? Sterne
        {
            get { return sterne; }
            set { sterne = value; RaisePropertyChanged(); }
        }

        private string kommentar;

        public string Kommentar
        {
            get { return kommentar; }
            set { kommentar = value; RaisePropertyChanged(); }
        }

        private bool? visible;

        public bool? Visible
        {
            get { return visible; }
            set { visible = value; RaisePropertyChanged(); }
        }

        private string artikelName;

        public string ArtikelName
        {
            get { return artikelName; }
            set { artikelName = value; RaisePropertyChanged(); }
        }

        private string kundenName;

        public string KundenName
        {
            get { return kundenName; }
            set { kundenName = value; RaisePropertyChanged(); }
        }


    }
}

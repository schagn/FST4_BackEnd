using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class SharedAngebot : ViewModelBase
    {
        public Guid AngebotId { get; set; }

        private string code;

        public string Code
        {
            get { return code; }
            set { code = value; RaisePropertyChanged(); }
        }

        private DateTime? startDatum;

        public DateTime? StartDatum
        {
            get { return startDatum; }
            set { startDatum = value; RaisePropertyChanged(); }
        }

        private DateTime? endDatum;

        public DateTime? EndDatum
        {
            get { return endDatum; }
            set { endDatum = value; RaisePropertyChanged(); }
        }

        private double? prozent;

        public double? Prozent
        {
            get { return prozent; }
            set { prozent = value; RaisePropertyChanged(); }
        }

       // private SharedArticle produkt;

        /*public SharedArticle Produkt
        {
            get { return produkt; }
            set { produkt = value; RaisePropertyChanged(); }
        }*/


    }
}

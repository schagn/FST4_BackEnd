﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class SharedBestellung : ViewModelBase
    {
        public Guid BestellId { get; set; }

        private DateTime? bestellDatum;

        public DateTime? BestellDatum
        {
            get { return bestellDatum; }
            set { bestellDatum = value; RaisePropertyChanged(); }
        }

        private DateTime? lieferDatum;

        public DateTime? LieferDatum
        {
            get { return lieferDatum; }
            set { lieferDatum = value; RaisePropertyChanged(); }
        }

        private double? gesamtSumme;

        public double? GesamtSumme
        {
            get { return gesamtSumme; }
            set { gesamtSumme = value; RaisePropertyChanged(); }
        }

        private string kundenName;

        public string KundenName
        {
            get { return kundenName; }
            set { kundenName = value; RaisePropertyChanged(); }
        }

        private bool? gutscheinUsed;

        public bool? GutscheinUsed
        {
            get { return gutscheinUsed; }
            set { gutscheinUsed = value; RaisePropertyChanged(); }
        }

        private double? gutscheinWert;

        public double? GutscheinWert
        {
            get { return gutscheinWert; }
            set { gutscheinWert = value; RaisePropertyChanged(); }
        }

        private string bestellstatus;

        public string Bestellstatus
        {
            get { return bestellstatus; }
            set { bestellstatus = value; RaisePropertyChanged(); }
        }

        private List<SharedOrderArticle> artikel;

        public List<SharedOrderArticle> Artikel
        {
            get { return artikel; }
            set { artikel = value; RaisePropertyChanged(); }
        }


    }
}

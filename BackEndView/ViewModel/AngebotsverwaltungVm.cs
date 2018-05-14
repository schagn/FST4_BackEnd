using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEndView.ViewModel
{
    public class AngebotsverwaltungVm : ViewModelBase
    {

        public RelayCommand EditAngebotBtnClick { get; set; }

        public RelayCommand DeleteAngebotBtnClick { get; set; }

        public RelayCommand SaveAngebotBtnClick { get; set; }

        public ObservableCollection<SharedAngebot> Angebote { get; set; }

        private string code;

        public string Code
        {
            get { return code; }
            set { code = value; RaisePropertyChanged(); }
        }

        private double prozent;

        public double Prozent
        {
            get { return prozent; }
            set { prozent = value; RaisePropertyChanged(); }
        }

        private DateTime startDatum;

        public DateTime StartDatum
        {
            get { return startDatum; }
            set { startDatum = value; RaisePropertyChanged(); }
        }

        private DateTime endDatum;

        public DateTime EndDatum
        {
            get { return endDatum; }
            set { endDatum = value; RaisePropertyChanged(); }
        }

        private SharedAngebot selectedAngebot;

        public SharedAngebot SelectedAngebot
        {
            get { return selectedAngebot; }
            set { selectedAngebot = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<string> FilterMethoden { get; set; }

        private string selectedFilterMethode;

        public string SelectedFilterMethode
        {
            get { return selectedFilterMethode; }
            set { selectedFilterMethode = value; RaisePropertyChanged(); LoadNewData(); }
        }

        bool IsEditingProcess;

        public AngebotsverwaltungVm()
        {
            Angebote = new ObservableCollection<SharedAngebot>();

            EditAngebotBtnClick = new RelayCommand(EditAngebot);

            SaveAngebotBtnClick = new RelayCommand(SaveAngebot);

            DeleteAngebotBtnClick = new RelayCommand(
                () =>
                {
                    DeleteSelectedAngebot(SelectedAngebot);
                });

            StartDatum = DateTime.Today;
            EndDatum = DateTime.Today;

            IsEditingProcess = false;

            FilterMethoden = new ObservableCollection<string>();
            FilterMethoden.Add("Aktuell");
            FilterMethoden.Add("Vergangenheit");
            FilterMethoden.Add("Zukunft");
        }

        private void EditAngebot()
        {
            Code = SelectedAngebot.Code;
            Prozent = SelectedAngebot.Prozent;
            StartDatum = SelectedAngebot.StartDatum;
            EndDatum = SelectedAngebot.EndDatum;

            IsEditingProcess = true;
        }

        private void DeleteSelectedAngebot(SharedAngebot a)
        {
            //client.deleteZutat
            //client Zutaten neu abfragen
            Angebote.Remove(a);
            RaisePropertyChanged("Angebote");
        }

        private void SaveAngebot()
        {
            if (IsEditingProcess == true)
            {
                foreach (var item in Angebote)
                {
                    if (item.AngebotId == SelectedAngebot.AngebotId)
                    {
                        item.Code = Code;
                        item.Prozent = Prozent;
                        item.StartDatum = StartDatum;
                        item.EndDatum = EndDatum;
                    }
                }
            }
            else
            {
                Angebote.Add(new SharedAngebot()
                {
                    AngebotId = Guid.NewGuid(),
                    StartDatum = StartDatum,
                    EndDatum = EndDatum,
                    Prozent = Prozent,
                    Code = Code
                });
            }

            Code = "";
            Prozent = 0;
            StartDatum = DateTime.Today;
            EndDatum = DateTime.Today;

            RaisePropertyChanged("Angebote");


            // client. SaveList 

            IsEditingProcess = false;
        }

        private void LoadNewData()
        {
            // db lade neue Daten, abfrage direkt über query
        }
    }
}

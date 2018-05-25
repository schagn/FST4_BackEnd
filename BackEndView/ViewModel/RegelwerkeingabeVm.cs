using DataRepository;
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
    public class RegelwerkeingabeVm : ViewModelBase
    {

        public RelayCommand CancelDataBtnClick { get; set; }
        public RelayCommand EditRegelwerkBtnClick { get; set; }

        public RelayCommand DeleteRegelwerkBtnClick { get; set; }

        public RelayCommand SaveRegelwerkBtnClick { get; set; }

        public ObservableCollection<SharedRegelwerk> Regelwerke { get; set; }

        private string beschreibung;
        public string Beschreibung
        {
            get { return beschreibung; }
            set { beschreibung = value; RaisePropertyChanged(); }
        }

        private bool? visibility;
        public bool? Visibility
        {
            get { return visibility; }
            set { visibility = value; RaisePropertyChanged(); }
        }


        private SharedRegelwerk selectedRegelwerk;

        public SharedRegelwerk SelectedRegelwerk
        {
            get { return selectedRegelwerk; }
            set { selectedRegelwerk = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<string> FilterMethoden { get; set; }

        private string selectedFilterMethode;

        public string SelectedFilterMethode
        {
            get { return selectedFilterMethode; }
            set { selectedFilterMethode = value; RaisePropertyChanged(); RefreshList(SelectedFilterMethode); }
        }

        bool IsEditingProcess;

        private DataHandler dataHandler;

        public RegelwerkeingabeVm()
        {
            dataHandler = new DataHandler();

            FilterMethoden = new ObservableCollection<string>();
            FilterMethoden.Add("verfügbar");
            FilterMethoden.Add("nicht verfügbar");
            FilterMethoden.Add("Alle");

            EditRegelwerkBtnClick = new RelayCommand(EditRegelwerk);

            SaveRegelwerkBtnClick = new RelayCommand(
                () => 
                {
                    if (IsEditingProcess)
                    {
                        SelectedRegelwerk.Beschreibung = Beschreibung;
                        SelectedRegelwerk.IsAvailable = Visibility;
                        dataHandler.UpdateRegel(selectedRegelwerk);
                        Beschreibung = "";
                        Visibility = false;
                        IsEditingProcess = false;
                    }
                    else
                    {
                        dataHandler.CreateRegel((new SharedRegelwerk()
                        {
                            RegelwerkId = Guid.NewGuid(),
                            Beschreibung = Beschreibung,
                            IsAvailable = Visibility
                        }));
                    }
                    Beschreibung = "";
                    RefreshList(SelectedFilterMethode);
                });


            DeleteRegelwerkBtnClick = new RelayCommand(
                () =>
                {
                    DeleteSelectedRegelwerk(SelectedRegelwerk);
                });

            CancelDataBtnClick = new RelayCommand(CancelData);

            IsEditingProcess = false;

            RefreshList(null);
        }

        private void RefreshList(string selected)
        {
            if (selected == null)
            {
                Regelwerke = new ObservableCollection<SharedRegelwerk>(dataHandler.GetRegel());
            }
            else if (selected.Equals("Available"))
            {
                Regelwerke = new ObservableCollection<SharedRegelwerk>(dataHandler.GetRegelAvailable());
            }
            else if (selected.Equals("Non-Available"))
            {
                Regelwerke = new ObservableCollection<SharedRegelwerk>(dataHandler.GetRegelNonAvailable());
            }
            else
            {
                Regelwerke = new ObservableCollection<SharedRegelwerk>(dataHandler.GetRegel());
            }
            RaisePropertyChanged("Regelwerke");
        }

        private void EditRegelwerk()
        {
            Beschreibung = SelectedRegelwerk.Beschreibung;
            Visibility = SelectedRegelwerk.IsAvailable;

            IsEditingProcess = true;
        }

        private void DeleteSelectedRegelwerk(SharedRegelwerk r)
        {
            dataHandler.DeleteRegel(r);
            RefreshList(SelectedFilterMethode);
        }

        private void CancelData()
        {
            Beschreibung = "";
            Visibility = false;
            SelectedRegelwerk = null;

            IsEditingProcess = false;

        }

    }
}

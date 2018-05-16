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

        private bool visibility;
        public bool Visibility
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

        bool IsEditingProcess;

        private DataHandler dataHandler;

        public RegelwerkeingabeVm()
        {
            dataHandler = new DataHandler();

            //änderung datanbank - available
            EditRegelwerkBtnClick = new RelayCommand(EditRegelwerk);

            SaveRegelwerkBtnClick = new RelayCommand(
                () => 
                {
                    if (IsEditingProcess)
                    {
                        SelectedRegelwerk.Beschreibung = Beschreibung;
                        dataHandler.UpdateRegel(selectedRegelwerk);
                        Beschreibung = "";
                        IsEditingProcess = false;
                    }
                    else
                    {
                        dataHandler.CreateRegel((new SharedRegelwerk()
                        {
                            RegelwerkId = Guid.NewGuid(),
                            Beschreibung = Beschreibung
                        }));
                    }
                    Beschreibung = "";
                    RefreshList();
                });


            DeleteRegelwerkBtnClick = new RelayCommand(
                () =>
                {
                    DeleteSelectedRegelwerk(SelectedRegelwerk);
                });


            IsEditingProcess = false;

            RefreshList();
        }

        private void RefreshList()
        {
            Regelwerke = new ObservableCollection<SharedRegelwerk>(dataHandler.GetRegel());
            RaisePropertyChanged("Regelwerke");
        }

        private void EditRegelwerk()
        {
            Beschreibung = SelectedRegelwerk.Beschreibung;

            IsEditingProcess = true;
        }

        private void DeleteSelectedRegelwerk(SharedRegelwerk r)
        {
            dataHandler.DeleteRegel(r);
            RefreshList();
        }

    }
}

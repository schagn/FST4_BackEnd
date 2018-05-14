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

        public RegelwerkeingabeVm()
        {
            Regelwerke = new ObservableCollection<SharedRegelwerk>();

            EditRegelwerkBtnClick = new RelayCommand(EditRegelwerk);

            SaveRegelwerkBtnClick = new RelayCommand(SaveRegelwerk);


            DeleteRegelwerkBtnClick = new RelayCommand(
                () =>
                {
                    DeleteSelectedRegelwerk(SelectedRegelwerk);
                });


            IsEditingProcess = false;
        }

        private void EditRegelwerk()
        {
            Beschreibung = SelectedRegelwerk.Beschreibung;
            Visibility = SelectedRegelwerk.IsAvailable;

            IsEditingProcess = true;
        }

        private void DeleteSelectedRegelwerk(SharedRegelwerk r)
        {
            //client.deleteZutat
            //client Zutaten neu abfragen
            Regelwerke.Remove(r);
            RaisePropertyChanged("Regelwerke");
        }

        private void SaveRegelwerk()
        {
            if (IsEditingProcess == true)
            {
                foreach (var item in Regelwerke)
                {
                    if (item.RegelwerkId == SelectedRegelwerk.RegelwerkId)
                    {
                        item.Beschreibung = Beschreibung;
                        item.IsAvailable = Visibility;

                    }
                }
            }
            else
            {
                Regelwerke.Add(new SharedRegelwerk()
                {
                    Beschreibung = Beschreibung,
                    RegelwerkId = Guid.NewGuid(),
                    IsAvailable = Visibility
                });
            }

            Beschreibung = "";
            Visibility = false;
            
            RaisePropertyChanged("Regelwerke");

            // client. SaveList 

            IsEditingProcess = false;
        }

    }
}

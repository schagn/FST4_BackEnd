using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;

namespace BackEndView.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {
        private NavigationService nav = SimpleIoc.Default.GetInstance<NavigationService>();

        private ViewModelBase currentDetailView;

        public ViewModelBase CurrentDetailView
        {
            get { return currentDetailView; }
            set { currentDetailView = value; RaisePropertyChanged(); }
        }

        public RelayCommand DashboardButton { get; set; }
        public RelayCommand ZutatenButton { get; set; }
        public RelayCommand KuchenButton { get; set; }
        public RelayCommand KuchenZutatenButton { get; set; }
        public RelayCommand VerpackungButton { get; set; }
        public RelayCommand PackageButton { get; set; }
        public RelayCommand AngebotButton { get; set; }
        public RelayCommand KundenButton { get; set; }
        public RelayCommand BewertungButton { get; set; }
        public RelayCommand BestellungButton { get; set; }
        public RelayCommand RegelwerkButton { get; set; }

        public MainViewModel()
        {
            InitialiseButtons();

            
        }

        private void InitialiseButtons()
        {
            DashboardButton = new RelayCommand(
                 () =>
                 {
                     CurrentDetailView = nav.NavigateTo("Dashboard");
                 });

            ZutatenButton = new RelayCommand(
               () =>
               {
                   CurrentDetailView = nav.NavigateTo("Zutaten");
               });

            KuchenButton = new RelayCommand(
                 () =>
                 {
                     CurrentDetailView = nav.NavigateTo("Kuchen");
                 });

            KuchenZutatenButton = new RelayCommand(
                 () =>
                 {
                     CurrentDetailView = nav.NavigateTo("KuchenZutaten");
                 });

            VerpackungButton = new RelayCommand(
                 () =>
                 {
                     CurrentDetailView = nav.NavigateTo("Verpackung");
                 });

            PackageButton = new RelayCommand(
                 () =>
                 {
                     CurrentDetailView = nav.NavigateTo("Package");
                 });

            AngebotButton = new RelayCommand(
                 () =>
                 {
                     CurrentDetailView = nav.NavigateTo("Angebot");
                 });

            KundenButton = new RelayCommand(
                 () =>
                 {
                     CurrentDetailView = nav.NavigateTo("Kunden");
                 });

            BewertungButton = new RelayCommand(
                 () =>
                 {
                     CurrentDetailView = nav.NavigateTo("Bewertung");
                 });

            BestellungButton = new RelayCommand(
                 () =>
                 {
                     CurrentDetailView = nav.NavigateTo("Bestellung");
                 });

            RegelwerkButton = new RelayCommand(
                 () =>
                 {
                     CurrentDetailView = nav.NavigateTo("Regelwerk");
                 });
        }
    }
}
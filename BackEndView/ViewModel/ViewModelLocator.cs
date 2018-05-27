/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:BackEndView"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace BackEndView.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<NavigationService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<DashboardVm>(true);
            SimpleIoc.Default.Register<StammdatenverwaltungVm>(true);
            SimpleIoc.Default.Register<ZutatenverwaltungVm>(true);
            SimpleIoc.Default.Register<KuchenverwaltungVm>(true);
            SimpleIoc.Default.Register<KuchenZutatenVm>(true);
            SimpleIoc.Default.Register<VerpackungsverwaltungVm>(true);
            SimpleIoc.Default.Register<PackageverwaltungVm>(true);
            SimpleIoc.Default.Register<AngebotsverwaltungVm>(true);
            SimpleIoc.Default.Register<KundenverwaltungVm>(true);
            SimpleIoc.Default.Register<BewertungsverwaltungVm>(true);
            SimpleIoc.Default.Register<BestellverwaltungVm>(true);
            SimpleIoc.Default.Register<RegelwerkeingabeVm>(true);
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public DashboardVm Dashboard
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DashboardVm>();
            }
        }

        public StammdatenverwaltungVm Stammdaten
        {
            get
            {
                return ServiceLocator.Current.GetInstance<StammdatenverwaltungVm>();
            }
        }

        public ZutatenverwaltungVm Zutaten
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ZutatenverwaltungVm>();
            }
        }

        public KuchenverwaltungVm Kuchen
        {
            get
            {
                var temp = ServiceLocator.Current.GetInstance<KuchenverwaltungVm>();
                temp.RefreshList();
                return temp;
            }
        }

        public KuchenZutatenVm KuchenZutaten
        {
            get
            {
                var temp = ServiceLocator.Current.GetInstance<KuchenZutatenVm>();
                temp.RefreshForm();
                return temp;
            }
        }

        public VerpackungsverwaltungVm Verpackung
        {
            get
            {
                return ServiceLocator.Current.GetInstance<VerpackungsverwaltungVm>();
            }
        }

        public PackageverwaltungVm Package
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PackageverwaltungVm>();
            }
        }


        public AngebotsverwaltungVm Angebot
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AngebotsverwaltungVm>();
            }
        }

        public KundenverwaltungVm Kunde
        {
            get
            {
                return ServiceLocator.Current.GetInstance<KundenverwaltungVm>();
            }
        }

        public BewertungsverwaltungVm Bewertung
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BewertungsverwaltungVm>();
            }
        }

        public BestellverwaltungVm Bestellung
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BestellverwaltungVm>();
            }
        }

        public RegelwerkeingabeVm Regelwerk
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RegelwerkeingabeVm>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEndView.ViewModel
{
    public class NavigationService
    {
        private string CurrentDetailView = null;

        private string LastDetailView = "";

        public ViewModelBase GoBack()
        {
            if (LastDetailView != "")
            {
                return SimpleFactory(LastDetailView);
            }
            return SimpleFactory(CurrentDetailView);
        }

        private ViewModelBase SimpleFactory(string viewId)
        {
            LastDetailView = CurrentDetailView;

            switch (viewId)
            {
                case "Dashboard":
                    CurrentDetailView = viewId;
                    return SimpleIoc.Default.GetInstance<DashboardVm>();

                case "Stammdaten":
                    CurrentDetailView = viewId;
                    return SimpleIoc.Default.GetInstance<StammdatenverwaltungVm>();

                case "Zutaten":
                    CurrentDetailView = viewId;
                    return SimpleIoc.Default.GetInstance<ZutatenverwaltungVm>();

                case "Kuchen":
                    CurrentDetailView = viewId;
                    return SimpleIoc.Default.GetInstance<KuchenverwaltungVm>();

                case "KuchenZutaten":
                    CurrentDetailView = viewId;
                    return SimpleIoc.Default.GetInstance<KuchenZutatenVm>();

                case "Verpackung":
                    CurrentDetailView = viewId;
                    return SimpleIoc.Default.GetInstance<VerpackungsverwaltungVm>();

                case "Package":
                    CurrentDetailView = viewId;
                    return SimpleIoc.Default.GetInstance<PackageverwaltungVm>();

                case "Angebot":
                    CurrentDetailView = viewId;
                    return SimpleIoc.Default.GetInstance<AngebotsverwaltungVm>();

                case "Kunden":
                    CurrentDetailView = viewId;
                    return SimpleIoc.Default.GetInstance<KundenverwaltungVm>();

                case "Bewertung":
                    CurrentDetailView = viewId;
                    return SimpleIoc.Default.GetInstance<BewertungsverwaltungVm>();

                case "Bestellung":
                    CurrentDetailView = viewId;
                    return SimpleIoc.Default.GetInstance<BestellverwaltungVm>();

                case "Regelwerk":
                    CurrentDetailView = viewId;
                    return SimpleIoc.Default.GetInstance<RegelwerkeingabeVm>();

                default:
                    return SimpleIoc.Default.GetInstance<DashboardVm>();
            }
        }


        public ViewModelBase NavigateTo(string viewId)
        {
            return SimpleFactory(viewId);
        }

    }
}

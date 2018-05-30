using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BackEndView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
     
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MakeAllGrey()
        {
            ButtonDasbboard.Background = new SolidColorBrush(Colors.LightGray);
            ButtonZutaten.Background = new SolidColorBrush(Colors.LightGray);
            ButtonKuchen.Background = new SolidColorBrush(Colors.LightGray);
            ButtonKuchenZutaten.Background = new SolidColorBrush(Colors.LightGray);
            ButtonVerpackung.Background = new SolidColorBrush(Colors.LightGray);
            ButtonPackage.Background = new SolidColorBrush(Colors.LightGray);
            ButtonAngebot.Background = new SolidColorBrush(Colors.LightGray);
            ButtonKunden.Background = new SolidColorBrush(Colors.LightGray);
            ButtonBestellung.Background = new SolidColorBrush(Colors.LightGray);
            ButtonBewertung.Background = new SolidColorBrush(Colors.LightGray);
            ButtonRegelwerk.Background = new SolidColorBrush(Colors.LightGray);
        }

        private void Button_Click_Dashboard(object sender, RoutedEventArgs e)
        {
            MakeAllGrey();
            ButtonDasbboard.Background = new SolidColorBrush(Colors.CadetBlue);
        }

        private void Button_Click_Zutaten(object sender, RoutedEventArgs e)
        {
            MakeAllGrey();
            ButtonZutaten.Background = new SolidColorBrush(Colors.PowderBlue);
        }

        private void Button_Click_Kuchen(object sender, RoutedEventArgs e)
        {
            MakeAllGrey();
            ButtonKuchen.Background = new SolidColorBrush(Colors.SkyBlue);
        }

        private void Button_Click_KuchenZutaten(object sender, RoutedEventArgs e)
        {
            MakeAllGrey();
            ButtonKuchenZutaten.Background = new SolidColorBrush(Colors.SteelBlue);
        }

        private void Button_Click_Verpackung(object sender, RoutedEventArgs e)
        {
            MakeAllGrey();
            ButtonVerpackung.Background = new SolidColorBrush(Colors.LavenderBlush);
        }

        private void Button_Click_Package(object sender, RoutedEventArgs e)
        {
            MakeAllGrey();
            ButtonPackage.Background = new SolidColorBrush(Colors.LavenderBlush);
        }

        private void Button_Click_Angebot(object sender, RoutedEventArgs e)
        {
            MakeAllGrey();
            ButtonAngebot.Background = new SolidColorBrush(Colors.LavenderBlush);
        }

        private void Button_Click_Kunden(object sender, RoutedEventArgs e)
        {
            MakeAllGrey();
            ButtonKunden.Background = new SolidColorBrush(Colors.LavenderBlush);
        }

        private void Button_Click_Bewertung(object sender, RoutedEventArgs e)
        {
            MakeAllGrey();
            ButtonBewertung.Background = new SolidColorBrush(Colors.LavenderBlush);
        }

        private void Button_Click_Bestellung(object sender, RoutedEventArgs e)
        {
            MakeAllGrey();
            ButtonBestellung.Background = new SolidColorBrush(Colors.LavenderBlush);
        }

        private void Button_Click_Regelwerk(object sender, RoutedEventArgs e)
        {
            MakeAllGrey();
            ButtonRegelwerk.Background = new SolidColorBrush(Colors.LavenderBlush);
        }
    }
}

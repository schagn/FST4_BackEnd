using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BackEndView.Converter
{
    public class IntToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int temp = (int) value;

            if (temp == 5)
            {
                return new BitmapImage(new Uri(@"\Images\Sterne5.png", UriKind.Relative));
            }
            if (temp == 4)
            {
                return new BitmapImage(new Uri(@"\Images\Sterne4.png", UriKind.Relative));
            }
            if (temp == 3)
            {
                return new BitmapImage(new Uri(@"\Images\Sterne3.png", UriKind.Relative));
            }
            if (temp == 2)
            {
                return new BitmapImage(new Uri(@"\Images\Sterne2.png", UriKind.Relative));
            }
            if (temp == 1)
            {
                return new BitmapImage(new Uri(@"\Images\Sterne1.png", UriKind.Relative));
            }

            return new BitmapImage(new Uri(@"\Images\Sterne1.png", UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

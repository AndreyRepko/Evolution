using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using Evolution.Game.Model;

namespace Evolution.Presenter.Converters
{
    public class ZavrToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                throw new ArgumentException("Two values are expected");

            var age = (int) values[0];
            var energy = (int) values[1];

            Color color;
            color = new Color();
            if (energy < 1000)
                color = Colors.Red;
            else if (age < 10)
                color = Colors.PaleVioletRed;
            else if (age < 25)
                color = Colors.IndianRed;
            else if (age < 50)
                color = Colors.MediumVioletRed;
            else if (age < 80)
                color = Colors.OrangeRed;
            else
                color = Colors.DarkRed;

            return new SolidColorBrush(color);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
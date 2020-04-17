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
            if (values.Length != 4)
                throw new ArgumentException("4 values are expected");

            var age = (int) values[0];
            var energy = (int) values[1];
            var speed = (int) values[2];
            var sight = (int) values[3];

            Color color;
            color = Color.FromRgb((byte)(50 + 20 * sight), 0, (byte)(50 + 20 * speed));
            if (age == 100)
            {
                color = Color.FromRgb(250, 0, 0);
            }

            return new SolidColorBrush(color);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Evolution.Presenter.Converters
{
    public class EnergyBoxToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 1)
                throw new ArgumentException("Two values are expected");

            var underAggression = (bool)values[0];

            if (underAggression)
                return new SolidColorBrush(Colors.Gold);
            return new SolidColorBrush(Color.FromRgb(0, 100, 0));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

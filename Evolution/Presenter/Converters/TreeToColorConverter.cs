using System;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Data;
using System.Windows.Media;

namespace Evolution.Presenter.Converters
{
    public class TreeToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                throw new ArgumentException("Two values are expected");

            var underAggression = (bool)values[0];
            var age = (int)values[1];

            if (underAggression)
                return new SolidColorBrush(Colors.Gold);
            return new SolidColorBrush(Color.FromRgb(0,(byte)(100+10*age),0));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
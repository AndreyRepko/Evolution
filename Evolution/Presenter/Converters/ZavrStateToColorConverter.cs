using System;
using System.Windows.Data;
using System.Windows.Media;
using Evolution.Game.Model;

namespace Evolution.Presenter.Converters
{
    public class ZavrStateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var isState = value is bool;
            if (!isState)
                throw new ArgumentException("Please convert only Zavrs here", nameof(value));

            if ((bool)value)
                return 0;
            return 0.1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
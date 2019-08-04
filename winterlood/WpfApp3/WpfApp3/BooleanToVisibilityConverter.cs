using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WpfApp3.Converter
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        enum Parameters
        {
            NORMAL, INVERTED
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = (bool)value;
            var strParam = parameter.ToString().ToUpper();
            var direction = (Parameters)Enum.Parse(typeof(Parameters), strParam);

            if (direction == Parameters.INVERTED)
            {
                return !boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
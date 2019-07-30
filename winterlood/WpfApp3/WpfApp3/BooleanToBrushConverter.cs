using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfApp3.Converter
{
    [ValueConversion(typeof(bool), typeof(Brush))]
    public class BooleanToBrushConverter : IValueConverter
    {
        public Brush FalseBrush { get; set; }
        public Brush TrueBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? TrueBrush : FalseBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

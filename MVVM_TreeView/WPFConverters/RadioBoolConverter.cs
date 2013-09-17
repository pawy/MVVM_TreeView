using System;
using System.Globalization;
using System.Windows.Data;

namespace MVVM_TreeView.WPFConverters
{
    [ValueConversion(typeof(bool?), typeof(bool))]
    public class RadioBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool param = bool.Parse(parameter.ToString());

            if (value == null)
                return false;

            return !((bool)value ^ param);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool param = bool.Parse(parameter.ToString());
            return !((bool)value ^ param);
        }
    }
}

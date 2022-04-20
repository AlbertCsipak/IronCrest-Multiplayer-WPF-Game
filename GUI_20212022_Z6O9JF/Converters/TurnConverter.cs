using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_20212022_Z6O9JF.Converters
{
    public class TurnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //bool -> string
            if ((bool)value)
            {
                return "Your turn.";
            }
            else
            {
                return "Enemy's turn.";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

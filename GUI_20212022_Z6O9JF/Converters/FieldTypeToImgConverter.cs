using GUI_20212022_Z6O9JF.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_20212022_Z6O9JF.Converters
{
    public class FieldTypeToImgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((FieldType)value)
            {
                case FieldType.grass:
                    return "\\Resources\\Images\\Menu\\grass_background.png";
                case FieldType.forest:
                    return "\\Resources\\Images\\Menu\\forest_background.gif";
                case FieldType.mountain:
                    return "\\Resources\\Images\\Menu\\mountain_background.gif";
                case FieldType.wheat:
                    return "\\Resources\\Images\\Menu\\wheat_background.gif";
                default: return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

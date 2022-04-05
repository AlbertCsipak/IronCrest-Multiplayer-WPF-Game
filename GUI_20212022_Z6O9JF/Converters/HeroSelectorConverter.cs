using GUI_20212022_Z6O9JF.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_20212022_Z6O9JF.Converters
{
    public class HeroSelectorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Faction -> ImageBrush
            switch ((Faction)value)
            {
                case Faction.Arabian:
                    return "\\Resources\\Images\\Characters\\standing_arabian.png";
                case Faction.Mongolian:
                    return "\\Resources\\Images\\Characters\\standing_mongolian.png";
                case Faction.Crusader:
                    return "\\Resources\\Images\\Characters\\standing_crusader.png";
                case Faction.Viking:
                    return "\\Resources\\Images\\Characters\\standing_viking.png";
                default: return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

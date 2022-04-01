using GUI_20212022_Z6O9JF.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
                    return "\\Resources\\Images\\Menu\\standing_arabian.png";
                case Faction.Mongolian:
                    return "\\Resources\\Images\\Menu\\yellow_icon_mongolian.png";
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

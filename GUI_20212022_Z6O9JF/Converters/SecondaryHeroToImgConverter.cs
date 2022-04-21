using GUI_20212022_Z6O9JF.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GUI_20212022_Z6O9JF.Converters
{
    public class SecondaryHeroToImgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (((Hero)value).FactionType)
            {
                case Faction.Arabian:
                    if (((Hero)value).Name=="Prophet")
                    {
                        return "\\Resources\\Images\\Characters\\prophet.png";
                    }
                    else
                    {
                        return "";
                    }
                case Faction.Mongolian:
                    if (((Hero)value).Name == "Mongolian Mouse")
                    {
                        return "\\Resources\\Images\\Characters\\mongolian_mouse.png";
                    }
                    else
                    {
                        return "";
                    }
                case Faction.Crusader:
                    if(((Hero)value).Name == "Crusader Knight")
                    {
                        return "\\Resources\\Images\\Characters\\white_knight.png";
                    }
                    else
                    {
                        return "";
                    }
                case Faction.Viking:
                    if(((Hero)value).Name == "Sigurd")
                    {
                        return "\\Resources\\Images\\Characters\\sigurd.png";
                    }
                    else
                    {
                        return "";
                    }
                default: return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

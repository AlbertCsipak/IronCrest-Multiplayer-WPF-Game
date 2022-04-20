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
    public class HeroToImgConverter : IValueConverter
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
                        return "\\Resources\\Images\\Characters\\jhin.png";
                    }
                case Faction.Mongolian:
                    if (((Hero)value).Name == "Mongolian Mouse")
                    {
                        return "\\Resources\\Images\\Characters\\mongolian_mouse.png";
                    }
                    else
                    {
                        return "\\Resources\\Images\\Characters\\genghis_khan.png";
                    }
                case Faction.Crusader:
                    if (((Hero)value).Name == "Dark Knight")
                    {
                        return "\\Resources\\Images\\Characters\\dark_knight.png";
                    }
                    else
                    {
                        return "\\Resources\\Images\\Characters\\white_knight.png";
                    }
                case Faction.Viking:
                    if (((Hero)value).Name == "Bjorn")
                    {
                        return "\\Resources\\Images\\Characters\\bjorn.png";
                    }
                    else
                    {
                        return "\\Resources\\Images\\Characters\\sigurd.png";
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

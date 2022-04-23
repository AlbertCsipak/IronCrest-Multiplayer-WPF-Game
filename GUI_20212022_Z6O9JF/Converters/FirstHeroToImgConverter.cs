using GUI_20212022_Z6O9JF.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_20212022_Z6O9JF.Converters
{
    public class FirstHeroToImgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value!=null)
            {
                switch (((Hero)value).FactionType)
                {
                    case Faction.Arabian:
                        if (((Hero)value).Name == "Jhin")
                        {
                            return "\\Resources\\Images\\Characters\\jhin.png";
                        }
                        else
                        {
                            return "";
                        }
                    case Faction.Mongolian:
                        if (((Hero)value).Name == "Genghis Khan")
                        {
                            return "\\Resources\\Images\\Characters\\genghis_khan.png";
                        }
                        else
                        {
                            return "";
                        }
                    case Faction.Crusader:
                        if (((Hero)value).Name == "Dark Knight")
                        {
                            return "\\Resources\\Images\\Characters\\dark_knight.png";
                        }
                        else
                        {
                            return "";
                        }
                    case Faction.Viking:
                        if (((Hero)value).Name == "Bjorn")
                        {
                            return "\\Resources\\Images\\Characters\\bjorn.png";
                        }
                        else
                        {
                            return "";
                        }
                    default: return "";
                }
            }
            else
            {
                return "";
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Tff.Panzer.Models.Geography
{
    public class WeatherZone
    {
        public int WeatherZoneId { get; set; }
        public String WeatherZoneDescription { get; set; }
        public WeatherZoneEnum WeatherZoneEnum
        {
            get
            {
                switch (WeatherZoneId)
                {
                    case 0:
                        return WeatherZoneEnum.Desert;
                    case 1:
                        return WeatherZoneEnum.Mediterrian;
                    case 2:
                        return WeatherZoneEnum.WesternEurope;
                    case 3:
                        return WeatherZoneEnum.EasternEurope;
                    default:
                        return WeatherZoneEnum.WesternEurope;
                }
            }
        }
    }
}

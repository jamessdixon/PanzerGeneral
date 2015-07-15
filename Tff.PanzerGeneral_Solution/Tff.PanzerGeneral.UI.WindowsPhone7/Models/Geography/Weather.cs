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
    public class Weather
    {
        public Int32 WeatherId { get; set; }
        public String WeatherDescription { get; set; }

        public WeatherEnum WeatherEnum
        {
            get
            {
                switch (WeatherId)
                {
                    case 0:
                        return WeatherEnum.Fair;
                    case 1:
                        return WeatherEnum.Cloudy;
                    case 2:
                        return WeatherEnum.Rain;
                    case 3:
                        return WeatherEnum.Snow;
                    default:
                        return WeatherEnum.Fair;
                }
            }
        }
    }
}

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
using System.Xml.Linq;
using System.Linq;
using System.Windows.Resources;
using System.Collections.Generic;
using Tff.Panzer.Models.Geography;

namespace Tff.Panzer.Factories.Geography
{
    public class WeatherFactory
    {
        public List<Weather> Weathers { get; private set; }
        public WeatherZoneFactory WeatherZoneFactory { get; set; }
        public WeatherProbabilityFactory WeatherProbabilityFactory { get; set; }

        public WeatherFactory()
        {
            Weathers = new List<Weather>();
            WeatherZoneFactory = new WeatherZoneFactory();
            WeatherProbabilityFactory = new WeatherProbabilityFactory();

            Uri uri = new Uri(Constants.WeatherDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from wz in applicationXml.Descendants("Weather")
                       select wz;

            Weather weather = null;
            foreach (var d in data)
            {
                weather = new Weather();
                weather.WeatherId = (Int32)d.Element("WeatherId");
                weather.WeatherDescription = (String)d.Element("WeatherDescription");
                Weathers.Add(weather);
            }
        }

        public Weather GetWeather(int weatherId)
        {
            Weather weather = (from w in this.Weathers
                                       where w.WeatherId == weatherId
                                    select w).First();

            return weather;
        }

    }
}

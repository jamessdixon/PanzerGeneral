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
    public class WeatherZoneFactory
    {
        public List<WeatherZone> WeatherZones { get; private set; }

        public WeatherZoneFactory()
        {
            WeatherZones = new List<WeatherZone>();

            Uri uri = new Uri(Constants.WeatherZoneDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from wz in applicationXml.Descendants("WeatherZone")
                       select wz;

            WeatherZone weatherZone = null;
            foreach (var d in data)
            {
                weatherZone = new WeatherZone();
                weatherZone.WeatherZoneId = (Int32)d.Element("WeatherZoneId");
                weatherZone.WeatherZoneDescription = (String)d.Element("WeatherZoneDescription");
                WeatherZones.Add(weatherZone);
            }
        }

        public WeatherZone GetWeatherZone(int weatherZoneId)
        {
            WeatherZone weatherZone = (from wz in this.WeatherZones
                                       where wz.WeatherZoneId == weatherZoneId
                                    select wz).First();

            return weatherZone;
        }

    }
}

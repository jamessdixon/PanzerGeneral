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
    public class WeatherProbabilityFactory
    {
        public List<WeatherProbability> WeatherProbabilities { get; private set; }

        public WeatherProbabilityFactory()
        {
            WeatherProbabilities = new List<WeatherProbability>();

            Uri uri = new Uri(Constants.WeatherProbabilityDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from wz in applicationXml.Descendants("WeatherProbability")
                       select wz;

            WeatherProbability weatherProbability = null;
            foreach (var d in data)
            {
                weatherProbability = new WeatherProbability();
                weatherProbability.WeatherProbabilityId = (Int32)d.Element("WeatherProbabilityId");
                weatherProbability.WeatherZoneId = (Int32)d.Element("WeatherZoneId");
                weatherProbability.Month = (Int32)d.Element("Month");
                weatherProbability.ParcipitationChance = (Double)d.Element("ParcipitationChance");
                weatherProbability.SnowInd = (Boolean)d.Element("SnowInd");
                WeatherProbabilities.Add(weatherProbability);
            }
        }

        public WeatherProbability GetWeatherProbability(int weatherProbabilityId)
        {
            WeatherProbability weatherProbability = (from wz in this.WeatherProbabilities
                                       where wz.WeatherProbabilityId == weatherProbabilityId
                                    select wz).First();

            return weatherProbability;
        }

        public WeatherProbability GetWeatherProbability(int weatherZoneId, int month)
        {
            WeatherProbability q = (from wp in WeatherProbabilities
                     where wp.WeatherZoneId == weatherZoneId
                     && wp.Month == month
                     select wp).FirstOrDefault();
            return q;
        }

        public List<WeatherProbability> GetWeatherProbabilities(int weatherZoneId)
        {
            List<WeatherProbability> returnValue = new List<WeatherProbability>();
            var q = (from wp in WeatherProbabilities
                     where wp.WeatherZoneId == weatherZoneId
                     select wp);

            foreach (WeatherProbability weatherProbability in q)
            {
                returnValue.Add(weatherProbability);
            }

            return returnValue;
        }
    }
}

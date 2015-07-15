using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tff.Panzer.Models.Geography
{
    public class WeatherProbability
    {
        public int WeatherProbabilityId { get; set; }
        public Int32 WeatherZoneId { get; set; }
        public int Month { get; set; }
        public Double ParcipitationChance { get; set; }
        public Boolean SnowInd { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Tff.Panzer.Services
{
    [DataContract]
    public class Scenario
    {
        [DataMember]
        public int ScenarioId { get; set; }
        [DataMember]
        public String ScenarioName { get; set; }
        [DataMember]
        public String ScenarioDescription { get; set; }
        [DataMember]
        public int NumberOfTurns { get; set; }
        [DataMember]
        public DateTime ScenarioStart { get; set; }
        [DataMember]
        public int TurnsPerDay { get; set; }
        [DataMember]
        public int DaysPerTurn { get; set; }
        [DataMember]
        public Int32 StartingWeatherId { get; set; }
        [DataMember]
        public Int32 WeatherZoneId { get; set; }
        [DataMember]
        public int MaxUnitStrength { get; set; }
        [DataMember]
        public int MaxUnitExperience { get; set; }
        [DataMember]
        public bool ActiveInd { get; set; }

    }
}

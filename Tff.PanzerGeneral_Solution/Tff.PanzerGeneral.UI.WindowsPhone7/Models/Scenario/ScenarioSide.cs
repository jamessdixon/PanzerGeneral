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
using Tff.Panzer.Models.Geography;
using System.Collections.Generic;

namespace Tff.Panzer.Models.Scenario
{
    public class ScenarioSide
    {
        public int ScenarioSideId { get; set; }
        public int ScenarioId { get; set; }
        public int SideId { get; set; }
        public int Prestige { get; set; }
        public int CoreLimit { get; set; }
        public int AuxLimit { get; set; }
        public int Stance { get; set; }
        public int Orientation { get; set; }
        public int SeaTransports { get; set; }
        public int SeaTransportTypeId { get; set; }
        public int AirTransports { get; set; }
        public int AirTransportTypeId { get; set; }
        public int NewUnitExperience { get; set; }
    }
}

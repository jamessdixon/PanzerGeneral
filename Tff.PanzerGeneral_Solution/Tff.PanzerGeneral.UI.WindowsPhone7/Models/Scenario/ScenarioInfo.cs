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
using Tff.Panzer.Models.Army;

namespace Tff.Panzer.Models.Scenario
{
    public class ScenarioInfo
    {
        public int ScenarioId { get; set; }
        public String ScenarioName { get; set; }
        public String ScenarioDescription { get; set; }
        public int NumberOfTurns { get; set; }
        public DateTime ScenarioStart { get; set; }
        //1 = 1 day per turn, .5 = 1/2 day per turn, 2 = 2 days per turn 
        public Double TurnIncrement { get; set; }
        public Weather StartingWeather { get; set; }
        public WeatherZone WeatherZone { get; set; }
        public int MaxUnitStrength { get; set; }
        public int MaxUnitExperience { get; set; }

        public override string ToString()
        {
            return String.Format("{0} ({1})",ScenarioName, ScenarioStart.ToShortDateString());
        }

    }
}

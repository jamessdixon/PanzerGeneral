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

namespace Tff.Panzer.Models.Scenario
{
    public class ScenarioTurn
    {
        public int ScenarioTurnId { get; set; }
        public DateTime TurnDate { get; set; }
        public Weather CurrentWeather { get; set; }
        public TerrainCondition CurrentTerrainCondition { get; set; }
        public Weather ForcastedWeather { get; set; }
        public TerrainCondition ForcastedTerrainCondition { get; set; }
    }
}

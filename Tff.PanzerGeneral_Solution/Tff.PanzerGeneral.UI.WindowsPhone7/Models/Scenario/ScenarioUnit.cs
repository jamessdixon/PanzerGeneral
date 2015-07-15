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

namespace Tff.Panzer.Models.Scenario
{
    public class ScenarioUnit
    {
        public int ScenarioUnitId { get; set; }
        public int ScenarioId { get; set; } 
        public int StartingScenarioTileId { get; set; }
        public int EquipmentId { get; set; }
        public int TransportId { get; set; }
        public int SideId { get; set; }
        public int NationId { get; set; }
        public int Strength { get; set; }
        public int Experience { get; set; }
        public int Entrenchment { get; set; }
        public int Fuel { get; set; }
        public int Ammo { get; set; }
        public bool CordInd { get; set; }
    }
}

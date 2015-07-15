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

namespace Tff.Panzer.Models.Army
{
    public class MovementCostModifier
    {
        public int MovementCostModifierId { get; set; }
        public TerrainCondition TerrainCondition { get; set; }
        public MovementType MovementType { get; set; }
        public int RiverPoints { get; set; }
        public int RoadPoints { get; set; }
    }
}

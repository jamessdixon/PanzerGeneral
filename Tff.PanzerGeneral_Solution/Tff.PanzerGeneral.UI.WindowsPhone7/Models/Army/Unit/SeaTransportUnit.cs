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

namespace Tff.Panzer.Models.Army.Unit
{
    public class SeaTransportUnit: BaseUnit, ITransportUnit, ISeaUnit
    {
        public int CurrentFuel { get; set; }
        public LandCombatUnit LandCombatUnit { get; set; }

    }
}

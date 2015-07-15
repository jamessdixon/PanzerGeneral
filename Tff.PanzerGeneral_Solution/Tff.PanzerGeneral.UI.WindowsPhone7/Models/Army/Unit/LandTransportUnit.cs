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
using System.Xml.Serialization;

namespace Tff.Panzer.Models.Army.Unit
{
    public class LandTransportUnit: BaseUnit, ITransportUnit, ILandUnit, IMotorizedUnit
    {
        public int CurrentFuel { get; set; }
        public LandCombatUnit LandCombatUnit { get; set; }
        [XmlIgnore]
        public Boolean IsSuppressed { get; set; }

    }
}

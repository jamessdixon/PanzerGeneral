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
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Tff.Panzer.Models.Army.Unit
{
    [XmlInclude(typeof(MotorizedLandCombatUnit))]
    public class LandCombatUnit: BaseUnit, ICombatUnit, ILandUnit
    {
        public LandCombatUnit()
        {
            AttackableTiles = new List<Tile>();
        }

        public Boolean CanAttack { get; set; }
        [XmlIgnore]
        public List<Tile> AttackableTiles { get; set; }
        public int CurrentAmmo { get; set; }
        [XmlIgnore]
        public ITransportUnit TransportUnit { get; set; }
        public int TransportUnitId
        {
            get
            {
                return TransportUnit.UnitId;
            }
        }
        public int CurrentEntrenchedLevel { get; set; }
        [XmlIgnore]
        public Boolean IsSuppressed { get; set; }

    } 
}

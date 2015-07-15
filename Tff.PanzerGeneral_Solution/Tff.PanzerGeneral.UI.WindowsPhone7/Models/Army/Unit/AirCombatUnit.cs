﻿using System;
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
    public class AirCombatUnit: BaseUnit, ICombatUnit, IAirUnit
    {
        public AirCombatUnit()
        {
            AttackableTiles = new List<Tile>();
        }

        public int CurrentFuel { get; set; }
        public Boolean CanAttack { get; set; }
        [XmlIgnore]
        public List<Tile> AttackableTiles { get; set; }
        public int CurrentAmmo { get; set; }


    }
}

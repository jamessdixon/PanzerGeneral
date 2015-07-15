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
using Tff.Panzer.Controls;
using Tff.Panzer.Models.Army.Unit;
using Tff.Panzer.Models;
using System.Collections.Generic;


namespace Tff.Panzer.Models.Battle
{
    public class BattleOutput
    {
        public BattleOutput()
        {
            Vollies = new List<Volley>();
        }

        public Tile AggressorTile { get; set; }
        public Tile ProtectorTile { get; set; }
        public IUnit AggressorUnit { get; set; }
        public IUnit ProtectorUnit { get; set; }

        public List<Volley> Vollies { get; protected set; }
        public BattleOutcomeEnum BattleOutcomeEnum { get; set; }
    }
}

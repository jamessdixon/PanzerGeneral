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

namespace Tff.Panzer.Models.Battle
{
    public class Volley
    {
        public Tile AttackerTile { get; set; }
        public Tile DefenderTile { get; set; }
        public IUnit AttackerUnit { get; set; }
        public IUnit DefenderUnit { get; set; }
        public VolleyOutcomeEnum VollyOutcomeEnum { get; set; }

    }
}

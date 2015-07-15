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

namespace Tff.Panzer.Models
{
    public class Turn
    {
        public int TurnId { get; set; }
        public DateTime TurnDate { get; set; }
        public Weather CurrentWeather { get; set; }
        public TerrainCondition CurrentTerrainCondition { get; set; }
        public Weather ForcastedWeather { get; set; }
        public TerrainCondition ForcastedTerrainCondition { get; set; }
        public int CurrentAxisPrestige { get; set; }
        public int CurrentAlliedPrestige { get; set; }
        public Boolean ActiveTurn { get; set; }
        public SideEnum CurrentSide { get; set; }

        public int TurnNumber
        {
            get
            {
                return TurnId + 1;
            }
        }

    }
}

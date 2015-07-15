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
using Tff.Panzer.Models.Army;
using System.ComponentModel;
using Tff.Panzer.Controls;
using Tff.Panzer.Models.Army.Unit;
using System.Xml.Serialization;

namespace Tff.Panzer.Models
{
    public class Tile
    {
        //Set once per scenario
        public int TileId { get; set; }
        public int ColumnNumber { get; set; }
        public int RowNumber { get; set; }
        public TileName TileName { get; set; }
        public Terrain Terrain { get; set; }
        public Boolean VictoryIndicator { get; set; }
        public Boolean SupplyIndicator { get; set; }
        public Boolean DeployIndicator { get; set; }
        [XmlIgnore]
        public IGroundUnit GroundUnit { get; set; }
        [XmlIgnore]
        public IAirUnit AirUnit { get; set; }
        public Nation Nation { get; set; }
        public Boolean IsVisible { get; set; }
        [XmlIgnore]
        public int MovementCost { get; set; }
        [XmlIgnore]
        public int Depth { get; set; }
        [XmlIgnore]
        public int Distance { get; set; }
        [XmlIgnore]
        public Boolean UnitCanPassThrough { get; set; }
    }
}

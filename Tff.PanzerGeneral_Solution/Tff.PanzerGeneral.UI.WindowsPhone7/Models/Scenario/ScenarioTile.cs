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
    public class ScenarioTile
    {
        public Int32 ScenarioTileId { get; set; }
        public Int32 ScenarioId { get; set; }
        public Int32 ColumnNumber { get; set; }
        public Int32 RowNumber { get; set; }
        public Int32 TerrainId { get; set; }
        public Int32 NationId { get; set; }
        public Int32 TileNameId { get; set; }
        public Boolean VictoryIndicator { get; set; }
        public Boolean SupplyIndicator { get; set; }
        public Boolean DeployIndicator { get; set; }
        public Int32 SideId { get; set; }
    }
}

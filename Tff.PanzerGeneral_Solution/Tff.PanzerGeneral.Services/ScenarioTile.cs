using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Tff.Panzer.Services
{
    [DataContract]
    public class ScenarioTile
    {
        [DataMember]
        public Int32 ScenarioTileId { get; set; }
        [DataMember]
        public Int32 ScenarioId { get; set; }
        [DataMember]
        public Int32 ColumnNumber { get; set; }
        [DataMember]
        public Int32 RowNumber { get; set; }
        [DataMember]
        public Int32 TerrainId { get; set; }
        [DataMember]
        public Int32 NationId { get; set; }
        [DataMember]
        public Int32 TileNameId { get; set; }
        [DataMember]
        public Boolean VictoryIndicator { get; set; }
        [DataMember]
        public Boolean SupplyIndicator { get; set; }
        [DataMember]
        public Boolean DeployIndicator { get; set; }
        [DataMember]
        public Int32 SideId { get; set; }
    }
}

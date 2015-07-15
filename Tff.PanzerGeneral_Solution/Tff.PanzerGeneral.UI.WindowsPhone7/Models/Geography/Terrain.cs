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

namespace Tff.Panzer.Models.Geography
{
    public class Terrain
    {
        public int TerrainId { get; set; }
        public TerrainType TerrainType { get; set; }
        public bool RiverInd { get; set; }
        public bool RoadInd { get; set; }
        public int ImageXCoordinate { get; set; }
        public int ImageYCoordinate { get; set; }
        [XmlIgnoreAttribute]
        public TerrainTypeEnum TerrainTypeEnum
        {
            get
            {
                return TerrainType.TerrainTypeEnum;
            }
        }
        [XmlIgnoreAttribute]
        public TerrainGroupEnum TerrainGroupEnum
        {
            get
            {
                return TerrainType.TerrainGroupEnum;
            }
        }

        public override string ToString()
        {
            return TerrainTypeEnum.ToString();
        }
    }
}

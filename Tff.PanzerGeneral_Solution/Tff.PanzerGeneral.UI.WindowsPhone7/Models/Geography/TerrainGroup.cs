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
    public class TerrainGroup
    {
        public int TerrainGroupId { get; set; }
        public string TerrainGroupDescription { get; set; }
        [XmlIgnoreAttribute]
        public TerrainGroupEnum TerrainGroupEnum
        {
            get
            {
                switch (TerrainGroupId)
                {
                    case 0:
                        return TerrainGroupEnum.Land;
                    case 1:
                        return TerrainGroupEnum.Sea;
                    case 2:
                        return TerrainGroupEnum.SeaAndLand;
                    default:
                        return TerrainGroupEnum.Land;
                }
            }
        }
    }
}

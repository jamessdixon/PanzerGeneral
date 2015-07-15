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
using System.Windows.Media.Imaging;

namespace Tff.Panzer.Models.Geography
{
    public class TerrainCondition
    {
        public int TerrainConditionId { get; set; }
        public string TerrainConditionDescription { get; set; }
        public TerrainConditionEnum TerrainConditionEnum
        {
            get
            {
                switch (TerrainConditionId)
                {
                    case 0:
                        return TerrainConditionEnum.Dry;
                    case 1:
                        return TerrainConditionEnum.Muddy;
                    case 2:
                        return TerrainConditionEnum.Frozen;
                    default:
                        return TerrainConditionEnum.Dry;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            TerrainCondition terrainConditionObject = (TerrainCondition)obj;
            if (this.TerrainConditionId == terrainConditionObject.TerrainConditionId)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public override int GetHashCode()
        {
            return TerrainConditionId;
        }

        public override string ToString()
        {
            return TerrainConditionId.ToString() + ":" + TerrainConditionDescription;
        }

    }
}

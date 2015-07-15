using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tff.Panzer.Models.Geography
{
    public class TerrainType
    {
        public int TerrainTypeId { get; set; }
        public String TerrainTypeDescription { get; set; }
        public TerrainGroup TerrainGroup { get; set; }
        public int InitiativeCap { get; set; }
        public int ImageXCoordinate { get; set; }
        public int ImageYCoordinate { get; set; }
        public int ImageXScale { get; set; }
        public int ImageYScale { get; set; }
        [XmlIgnoreAttribute]
        public TerrainTypeEnum TerrainTypeEnum
        {
            get
            {
                switch (TerrainTypeId)
                {
                    case 0:
                        return TerrainTypeEnum.Ocean;
                    case 1:
                        return TerrainTypeEnum.Port;
                    case 2:
                        return TerrainTypeEnum.Rough;
                    case 3:
                        return TerrainTypeEnum.Mountain;
                    case 4:
                        return TerrainTypeEnum.City;
                    case 5:
                        return TerrainTypeEnum.Clear;
                    case 6:
                        return TerrainTypeEnum.Forest;
                    case 7:
                        return TerrainTypeEnum.Swamp;
                    case 8:
                        return TerrainTypeEnum.Airfield;
                    case 9:
                        return TerrainTypeEnum.Fortification;
                    case 10:
                        return TerrainTypeEnum.Bocage;
                    case 11:
                        return TerrainTypeEnum.Desert;
                    case 12:
                        return TerrainTypeEnum.RoughDesert;
                    case 13:
                        return TerrainTypeEnum.Escarpment;
                    default:
                        return TerrainTypeEnum.Clear;
                }
            }
        }
        [XmlIgnoreAttribute]
        public TerrainGroupEnum TerrainGroupEnum
        {
            get
            {
                return TerrainGroup.TerrainGroupEnum;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            TerrainType terrainTypeObject = (TerrainType)obj;
            if (this.TerrainTypeId == terrainTypeObject.TerrainTypeId)
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
            return TerrainTypeId;
        }

        public override string ToString()
        {
            return TerrainTypeId.ToString() + ":" + TerrainTypeDescription;
        }
    }
}

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

namespace Tff.Panzer.Models
{
    public class Nation
    {
        public int NationId { get; set; }
        public string NationDescription { get; set; }
        public Side Side { get; set; }
        public int ImageXCoordinate { get; set; }
        public int ImageYCoordinate { get; set; }
        [XmlIgnore]
        public NationEnum NationEnum
        {
            get
            {
                switch (NationId)
                {
                    case -1:
                        return NationEnum.None;
                    case 2:
                        return NationEnum.AlliedForces;
                    case 3:
                        return NationEnum.Bulgaria;
                    case 7:
                        return NationEnum.France;
                    case 8:
                        return NationEnum.German;
                    case 9:
                        return NationEnum.Greece;
                    case 10:
                        return NationEnum.UnitedStates;
                    case 11:
                        return NationEnum.Hungray;
                    case 13:
                        return NationEnum.Italy;
                    case 15:
                        return NationEnum.Norway;
                    case 16:
                        return NationEnum.Poland;
                    case 18:
                        return NationEnum.Romania;
                    case 20:
                        return NationEnum.SovietUnion;
                    case 23:
                        return NationEnum.GreatBritain;
                    case 24:
                        return NationEnum.Yugoslavia;
                    default:
                        return NationEnum.None;
                }
            }
        }
        [XmlIgnore]
        public SideEnum SideEnum
        {
            get
            {
                return Side.SideEnum;
            }
        }
    }
}

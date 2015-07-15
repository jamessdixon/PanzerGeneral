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
    public class Side
    {
        public int SideId { get; set; }
        public string SideDescription { get; set; }
        [XmlIgnore]
        public SideEnum SideEnum
        {
            get
            {
                switch (SideId)
                {
                    case 0:
                        return SideEnum.Axis;
                    case 1:
                        return SideEnum.Allies;
                    case 2:
                        return SideEnum.Neutral;
                    default:
                        return SideEnum.Neutral;
                }
            }
        }
    }
}

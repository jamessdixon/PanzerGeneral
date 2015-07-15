using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tff.Panzer.Models.Army.Unit
{
    public interface ILandUnit: IGroundUnit
    {
        [XmlIgnoreAttribute]
        Boolean IsSuppressed { get; set; }
    }
}

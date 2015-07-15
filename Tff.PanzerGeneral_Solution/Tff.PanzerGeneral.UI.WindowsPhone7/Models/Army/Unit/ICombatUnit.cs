using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tff.Panzer.Models.Army.Unit
{
    public interface ICombatUnit: IUnit
    {
        Boolean CanAttack { get; set; }
        [XmlIgnoreAttribute]
        List<Tile> AttackableTiles { get; set; }
        int CurrentAmmo { get; set; }
    }
}

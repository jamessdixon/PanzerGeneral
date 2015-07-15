using System;
using Tff.Panzer.Models.Army;
using Tff.Panzer.Models;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Tff.Panzer.Models.Army.Unit
{
    public interface IUnit
    {
        int UnitId { get; set; }
        string UnitName { get; set; }
        [XmlIgnoreAttribute]
        List<Tile> MoveableTiles { get; set; }
        [XmlIgnoreAttribute]
        List<Tile> VisibleTiles { get; set; }
        bool CanMove { get; set; }
        bool CanUpdate { get; set; }
        bool CoreIndicator { get; set; }
        int CurrentExperience { get; set; }
        [XmlIgnoreAttribute]
        int CurrentNumberOfStars { get; }
        int CurrentStrength { get; set; }
        [XmlIgnoreAttribute]
        int CurrentAttackPoints { get; set; }
        Equipment Equipment { get; set; }
        [XmlIgnoreAttribute]
        EquipmentClassEnum EquipmentClassEnum { get; }
        [XmlIgnoreAttribute]
        EquipmentGroupEnum EquipmentGroupEnum { get; }
        [XmlIgnoreAttribute]
        EquipmentSubClassEnum EquipmentSubClassEnum { get; }
        [XmlIgnoreAttribute]
        TargetTypeEnum TargetTypeEnum { get; }
        [XmlIgnoreAttribute]
        EquipmentLossCalculationGroupEnum EquipmentLossCalculationGroupEnum { get;  }
        [XmlIgnoreAttribute]
        bool CanCaptureHexes { get; }
        Nation Nation { get; set; }
        [XmlIgnoreAttribute]
        NationEnum NationEnum { get; }
        [XmlIgnoreAttribute]
        SideEnum SideEnum { get; }
        Int32 CurrentTileId { get; set; }

    }
}

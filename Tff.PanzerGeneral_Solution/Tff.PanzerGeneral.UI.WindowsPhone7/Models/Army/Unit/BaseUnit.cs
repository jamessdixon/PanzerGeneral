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
using System.Collections.Generic;
using Tff.Panzer.Models.Geography;
using System.Xml.Serialization;

namespace Tff.Panzer.Models.Army.Unit
{
    public abstract class BaseUnit : IUnit
    {
        public BaseUnit()
        {
            MoveableTiles = new List<Tile>();
            VisibleTiles = new List<Tile>();
        }

        //Static In Each Scenario
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public Boolean CoreIndicator { get; set; }
        public Nation Nation { get; set; }
        public NationEnum NationEnum
        {
            get
            {
                return Nation.NationEnum;
            }

        }
        public SideEnum SideEnum
        {
            get
            {
                return Nation.SideEnum;
            }
        }

        //Changes Occasionally
        public Equipment Equipment { get; set; }
        [XmlIgnoreAttribute]
        public EquipmentSubClassEnum EquipmentSubClassEnum
        {
            get
            {
                return Equipment.EquipmentSubClassEnum;
            }
        }
        [XmlIgnoreAttribute]
        public EquipmentClassEnum EquipmentClassEnum
        {
            get
            {
                return Equipment.EquipmentClassEnum;
            }
        }
        [XmlIgnoreAttribute]
        public EquipmentGroupEnum EquipmentGroupEnum
        {
            get
            {
                return Equipment.EquipmentGroupEnum;
            }

        }
        [XmlIgnoreAttribute]
        public Boolean CanCaptureHexes
        {
            get
            {
                return Equipment.CanCaptureHexes;
            }
        }
        [XmlIgnoreAttribute]
        public TargetTypeEnum TargetTypeEnum
        {
            get
            {
                return Equipment.TargetTypeEnum;
            }
        }
        [XmlIgnoreAttribute]
        public EquipmentLossCalculationGroupEnum EquipmentLossCalculationGroupEnum
        {
            get
            {
                return Equipment.EquipmentLossCalculationGroupEnum;
            }
        }


        //Changes Often
        public int CurrentStrength { get; set; }
        public int CurrentExperience { get; set; }
        [XmlIgnoreAttribute]
        public int CurrentNumberOfStars
        {
            get
            {
                return (Int32)(CurrentExperience * .1);
            }
        }
        [XmlIgnoreAttribute]
        public int CurrentAttackPoints { get; set; }
        [XmlIgnoreAttribute]
        public List<Tile> MoveableTiles { get; set; }
        [XmlIgnoreAttribute]
        public List<Tile> VisibleTiles { get; set; }
        public Boolean CanMove { get; set; }
        public Boolean CanUpdate { get; set; }
        public Int32 CurrentTileId { get; set; }

        public override string ToString()
        {
            return UnitName;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            BaseUnit targetUnit = (BaseUnit)obj;
            if (targetUnit.UnitId == this.UnitId)
                return true;
            else
                return false;

        }

        public override int GetHashCode()
        {
            return this.UnitId;
        }
    }
}

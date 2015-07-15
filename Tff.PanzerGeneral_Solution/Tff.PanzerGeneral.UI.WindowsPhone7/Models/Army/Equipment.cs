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
using Tff.Panzer.Models.Geography;
using Microsoft.Xna.Framework.Audio;

namespace Tff.Panzer.Models.Army
{
    public class Equipment
    {
        public int EquipmentId { get; set; }
        public string EquipmentDescription { get; set; }
        public EquipmentSubClass EquipmentSubClass { get; set; }
        public TargetType TargetType { get; set; }
        public int IconId { get; set; }
        public int StackedIconId { get; set; }
        public int ImageXCoordinate { get; set; }
        public int ImageYCoordinate { get; set; }
        public int StackedImageXCoordinate { get; set; }
        public int StackedImageYCoordinate { get; set; }
        public DateTime StartService { get; set; }
        public DateTime EndService { get; set; }
        public int Experience { get; set; }
        public MovementType MovementType { get; set; }
        public Nation Nation { get; set; }
        public int BaseMovement { get; set; }
        public int BaseStrength { get; set; }
        public int Initative { get; set; }
        public int UnitCost { get; set; }
        public int Spotting { get; set; }
        public int MaxAmmo { get; set; }
        public int MaxFuel { get; set; }
        public int Range { get; set; }
        public int SoftAttack { get; set; }
        public int HardAttack { get; set; }
        public int AirAttack { get; set; }
        public int NavalAttack { get; set; }
        public int AttackRange { get; set; }
        public int GroundDefense { get; set; }
        public int AirDefense { get; set; }
        public int SeaDefense { get; set; }
        public int CloseDefense { get; set; }
        public bool CanBridgeRivers { get; set; }
        public bool JetIndicator { get; set; }
        public bool IgnoresEntrenchment { get; set; }
        public bool CanParadrop { get; set; }
        public bool CanHaveSeaTransport { get; set; }
        public bool CanHaveAirTransport { get; set; }
        public int BomberSpecial { get; set; }
        public bool CanHaveOrganicTransport { get; set; }
        public EquipmentSubClassEnum EquipmentSubClassEnum
        {
            get
            {
                return EquipmentSubClass.EquipmentSubClassEnum;
            }
        }
        public EquipmentClassEnum EquipmentClassEnum
        {
            get
            {
                return EquipmentSubClass.EquipmentClassEnum;
            }
        }
        public EquipmentGroupEnum EquipmentGroupEnum
        {
            get
            {
                return EquipmentSubClass.EquipmentGroupEnum;
            }

        }
        public TargetTypeEnum TargetTypeEnum
        {
            get
            {
                return TargetType.TargetTypeEnum;
            }
        }
        public EquipmentLossCalculationGroupEnum EquipmentLossCalculationGroupEnum
        {
            get
            {
                return EquipmentSubClass.EquipmentLossCalculationGroupEnum;
            }
        }
        public MovementTypeEnum MovementTypeEnum
        {
            get
            {
                return MovementType.MovementTypeEnum;
            }
        }
        public Boolean CanCaptureHexes
        {
            get
            {
                return EquipmentSubClass.CanCaptureHexes;
            }
        }

        public override string ToString()
        {
            return EquipmentDescription;
        }



    }
}

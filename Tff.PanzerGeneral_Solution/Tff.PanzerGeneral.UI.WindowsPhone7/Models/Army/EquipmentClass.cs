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

namespace Tff.Panzer.Models.Army
{
    public class EquipmentClass
    {
        public int EquipmentClassId { get; set; }
        public String EquipmentClassDescription { get; set; }
        public EquipmentGroup EquipmentGroup { get; set; }
        public EquipmentSubClass EquipmentSubClass { get; set; }
        public int EntrenchmentRate { get; set; }
        public EquipmentLossCalculationGroup EquipmentLossCalculationGroup { get; set; }
        public bool CanCaptureHexes { get; set; }
        public EquipmentClassEnum EquipmentClassEnum
        {   
            get
            {
                switch(EquipmentClassId)
                {
                    case 0:
                        return EquipmentClassEnum.Infantry;
                    case 1:
                        return EquipmentClassEnum.Tank;
                    case 2:
                        return EquipmentClassEnum.Recon;
                    case 3:
                        return EquipmentClassEnum.AntiTank;
                    case 4:
                        return EquipmentClassEnum.Artillery;
                    case 5:
                        return EquipmentClassEnum.AntiAir;
                    case 6:
                        return EquipmentClassEnum.AirDefense;
                    case 7:
                        return EquipmentClassEnum.Emplacement;
                    case 8:
                        return EquipmentClassEnum.Fighter;
                    case 9:
                        return EquipmentClassEnum.TacticalBomber;
                    case 10:
                        return EquipmentClassEnum.StrategicBomber;
                    case 11:
                        return EquipmentClassEnum.Submarine;
                    case 12:
                        return EquipmentClassEnum.Destroyer;
                    case 13:
                        return EquipmentClassEnum.CapitalShip;
                    case 14:
                        return EquipmentClassEnum.AircraftCarrier;
                    case 15:
                        return EquipmentClassEnum.GroundTransport;
                    case 16:
                        return EquipmentClassEnum.AirTransport;
                    case 17:
                        return EquipmentClassEnum.SeaTransport;
                    default:
                        return EquipmentClassEnum.Infantry;

                }
            }
        }
        public EquipmentGroupEnum EquipmentGroupEnum
        {
            get
            {
                return EquipmentGroup.EquipmentGroupEnum;
            }
        
        }
        public EquipmentLossCalculationGroupEnum EquipmentLossCalculationGroupEnum
        {
            get
            {
                return EquipmentLossCalculationGroup.EquipmentLossCalculationGroupEnum;
            }
        }

        public override string ToString()
        {
            return EquipmentClassDescription;
        }
    }
}

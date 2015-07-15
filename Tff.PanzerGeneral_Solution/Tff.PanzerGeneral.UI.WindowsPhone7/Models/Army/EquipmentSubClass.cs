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
    public class EquipmentSubClass
    {
        public int EquipmentSubClassId { get; set; }
        public string EquipmentSubClassDescription { get; set; }
        public bool CanCaptureHexes
        {
            get
            {
                return EquipmentClass.CanCaptureHexes;
            }
        }
        public EquipmentClass EquipmentClass { get; set; }
        public EquipmentSubClassEnum EquipmentSubClassEnum
        {
            get
            {
                switch (EquipmentSubClassId)
                {
                    case 0:
                        return EquipmentSubClassEnum.BasicInfantry;
                    case 1:
                        return EquipmentSubClassEnum.Tank;
                    case 2:
                        return EquipmentSubClassEnum.Recon;
                    case 3:
                        return EquipmentSubClassEnum.TowedLightAntiTank;
                    case 4:
                        return EquipmentSubClassEnum.TowedLightArtillery;
                    case 5:
                        return EquipmentSubClassEnum.AntiAir;
                    case 6:
                        return EquipmentSubClassEnum.TowedAirDefense;
                    case 7:
                        return EquipmentSubClassEnum.FortEmplacement;
                    case 8:
                        return EquipmentSubClassEnum.PropFighter;
                    case 9:
                        return EquipmentSubClassEnum.TacticalBomber;
                    case 10:
                        return EquipmentSubClassEnum.StrategicBomber;
                    case 11:
                        return EquipmentSubClassEnum.Submarine;
                    case 12:
                        return EquipmentSubClassEnum.Destroyer;
                    case 13:
                        return EquipmentSubClassEnum.CapitalShip;
                    case 14:
                        return EquipmentSubClassEnum.AircraftCarrier;
                    case 15:
                        return EquipmentSubClassEnum.GroundTransport;
                    case 16:
                        return EquipmentSubClassEnum.AirTransport;
                    case 17:
                        return EquipmentSubClassEnum.SeaTransport;
                    case 18:
                        return EquipmentSubClassEnum.TankDestroyer;
                    case 19:
                        return EquipmentSubClassEnum.SelfPropelledArtillery;
                    case 20:
                        return EquipmentSubClassEnum.SelfPropelledAirDefense;
                    case 21:
                        return EquipmentSubClassEnum.JetFighter;
                    case 22:
                        return EquipmentSubClassEnum.Engineer_PioniereInfantry;
                    case 23:
                        return EquipmentSubClassEnum.Airborne_RangerInfantry;
                    case 24:
                        return EquipmentSubClassEnum.BridgingInfantry;
                    case 25:
                        return EquipmentSubClassEnum.StrongpointEmplacement;
                    case 26:
                        return EquipmentSubClassEnum.TowedHeavyAntiTank;
                    case 27:
                        return EquipmentSubClassEnum.TowedHeavyArtillery;
                    case 28:
                        return EquipmentSubClassEnum.SeaTransport;
                    default:
                        return EquipmentSubClassEnum.BasicInfantry;

                }
            }
        }
        public EquipmentClassEnum EquipmentClassEnum
        {
            get
            {
                return EquipmentClass.EquipmentClassEnum;
            }
        }
        public EquipmentGroupEnum EquipmentGroupEnum
        {
            get
            {
                return EquipmentClass.EquipmentGroupEnum;
            }

        }
        public EquipmentLossCalculationGroupEnum EquipmentLossCalculationGroupEnum
        {
            get
            {
                return EquipmentClass.EquipmentLossCalculationGroupEnum;
            }
        }

    }
}

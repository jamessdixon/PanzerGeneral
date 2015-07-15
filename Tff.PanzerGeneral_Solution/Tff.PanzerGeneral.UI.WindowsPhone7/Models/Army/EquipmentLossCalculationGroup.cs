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
    public class EquipmentLossCalculationGroup
    {
        public int EquipmentLossCalculationGroupId { get; set; }
        public string EquipmentLossCalculationGroupDescription { get; set; }
        public EquipmentLossCalculationGroupEnum EquipmentLossCalculationGroupEnum
        {
            get
            {
                switch(EquipmentLossCalculationGroupId)
                {
                    case 0:
                        return EquipmentLossCalculationGroupEnum.Standard;
                    case 1:
                        return EquipmentLossCalculationGroupEnum.Special;
                    default:
                        return EquipmentLossCalculationGroupEnum.Standard;
                }
            }
        }
    }
}

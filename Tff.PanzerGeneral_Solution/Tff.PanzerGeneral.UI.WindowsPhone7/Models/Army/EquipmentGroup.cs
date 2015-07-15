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
    public class EquipmentGroup
    {
        public int EquipmentGroupId { get; set; }
        public string EquipmentGroupDescription { get; set; }
        public EquipmentGroupEnum EquipmentGroupEnum 
        { 
            get 
                {
                    switch(EquipmentGroupId)
                    {
                        case 0:
                            return EquipmentGroupEnum.Land;
                        case 1:
                            return EquipmentGroupEnum.Air;
                        case 2:
                            return EquipmentGroupEnum.Sea;
                        default:
                            return EquipmentGroupEnum.Land;
                    }
                }
        }

    }
}

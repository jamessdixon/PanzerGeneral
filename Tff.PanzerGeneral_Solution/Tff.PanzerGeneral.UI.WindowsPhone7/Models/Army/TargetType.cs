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
    public class TargetType
    {
        public int TargetTypeId { get; set; }
        public String TargetTypeDescription { get; set; }
        public TargetTypeEnum TargetTypeEnum
        {
            get
            {
                switch (TargetTypeId)
                {
                    case 0:
                        return TargetTypeEnum.SoftGround;
                    case 1:
                        return TargetTypeEnum.HardGround;
                    case 2:
                        return TargetTypeEnum.Air;
                    case 3:
                        return TargetTypeEnum.Sea;
                    default:
                        return TargetTypeEnum.SoftGround;
                }
            }
        }
    }
}

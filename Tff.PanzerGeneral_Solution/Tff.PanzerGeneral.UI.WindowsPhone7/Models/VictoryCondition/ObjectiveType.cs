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

namespace Tff.Panzer.Models.VictoryCondition
{
    public class ObjectiveType
    {
        public int ObjectiveTypeId { get; set; }
        public string ObjectiveTypeDescription { get; set; }
        public ObjectiveTypeEnum ObjectiveTypeEnum
        {
            get
            {
                switch (ObjectiveTypeId)
                {
                    case 0:
                        return ObjectiveTypeEnum.AxisAttacks;
                    case 1:
                        return ObjectiveTypeEnum.AxisDefends;
                    default:
                        return ObjectiveTypeEnum.AxisAttacks;
                }
            }
        }
    }
}

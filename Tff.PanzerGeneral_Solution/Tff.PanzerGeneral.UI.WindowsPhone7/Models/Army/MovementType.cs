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
    public class MovementType
    {
        public int MovementTypeId { get; set; }
        public string MovementTypeDescription { get; set; }
        public bool IsMotorized { get; set; }

        public MovementTypeEnum MovementTypeEnum
        {
            get
            {
                switch (MovementTypeId)
                {
                    case 0:
                        return MovementTypeEnum.Tracked;
                    case 1:
                        return MovementTypeEnum.HalfTracked;
                    case 2:
                        return MovementTypeEnum.Wheeled;
                    case 3:
                        return MovementTypeEnum.Walk;
                    case 4:
                        return MovementTypeEnum.None;
                    case 5:
                        return MovementTypeEnum.Air;
                    case 6:
                        return MovementTypeEnum.Water;
                    case 7:
                        return MovementTypeEnum.AllTerrain;
                    default:
                        return MovementTypeEnum.None;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            MovementType objectMovementType = (MovementType)obj;
            if (this.MovementTypeId == objectMovementType.MovementTypeId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.MovementTypeId;
        }

        public override string ToString()
        {
            return MovementTypeId.ToString() + ":" + MovementTypeDescription;
        }
    }
}

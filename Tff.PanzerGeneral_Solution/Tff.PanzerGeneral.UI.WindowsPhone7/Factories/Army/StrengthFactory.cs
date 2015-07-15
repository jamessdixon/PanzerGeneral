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

namespace Tff.Panzer.Factories.Army
{
    public class StrengthFactory
    {
        public int GetXCoordinate(int strengthAmount)
        {
            return (strengthAmount * -60) + 60;
        }

        public int GetYCoordinate(int sideId)
        {
            if (sideId == 0)
                return 0;
            else
                return -100;

        }
    }
}

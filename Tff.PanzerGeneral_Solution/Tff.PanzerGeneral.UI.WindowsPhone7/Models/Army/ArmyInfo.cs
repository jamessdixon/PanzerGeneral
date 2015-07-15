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
using System.Collections.Generic;
using Tff.Panzer.Models.Army.Unit;

namespace Tff.Panzer.Models.Army
{
    public class ArmyInfo
    {
        public int ArmyId { get; set; }
        public string ArmyDescription { get; set; }
        public List<Nation> Nations { get; set; }
        public List<IUnit> Units { get; set; }
        public int SideId { get; set; }
    }
}

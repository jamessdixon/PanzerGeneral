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

namespace Tff.Panzer.Models.Scenario
{
    public class ScenarioPrestigeAllotment
    {
        public int ScenarioPrestigeAllotmentId { get; set; }
        public int ScenarioId { get; set; }
        public int TurnId { get; set; }
        public int AxisPrestige { get; set; }
        public int AlliedPrestige { get; set; }
    }
}

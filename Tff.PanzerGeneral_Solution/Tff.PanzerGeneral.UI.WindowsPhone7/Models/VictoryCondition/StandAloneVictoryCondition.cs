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
    public class StandAloneVictoryCondition
    {
        public int StandAloneVictoryConditionId { get; set; }
        public int ScenarioId { get; set; }
        public ObjectiveType ObjectiveType { get; set; }
        public int WinAmount { get; set; }
        public int LossAmount { get; set; }
        public ObjectiveTypeEnum ObjectiveTypeEnum
        {
            get
            {
                return ObjectiveType.ObjectiveTypeEnum;
            }
        }
    }
}

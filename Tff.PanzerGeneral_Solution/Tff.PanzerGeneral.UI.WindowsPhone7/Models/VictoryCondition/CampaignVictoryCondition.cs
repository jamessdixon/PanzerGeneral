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
using Tff.Panzer.Models.Scenario;

namespace Tff.Panzer.Models.VictoryCondition
{
    public class CampaignVictoryCondition
    {
        public Int32 CampaignVictoryConditionId { get; set; }
        public Int32 ScenarioId { get; set; }
        public ObjectiveType ObjectiveType { get; set; }
        public Int32 MajorVictoryTurnsRemain { get; set; }
        public Int32 MajorVictoryObjectviesToHold { get; set; }
        public Int32 MinorVactoryTurnsRemain { get; set; }
        public Int32 MinorVictoryObjectviesToHold { get; set; }
        public ObjectiveTypeEnum ObjectiveTypeEnum
        {
            get
            {
                return ObjectiveType.ObjectiveTypeEnum;
            }
        }
    }
}

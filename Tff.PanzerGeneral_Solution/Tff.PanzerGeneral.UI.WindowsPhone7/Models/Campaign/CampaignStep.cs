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

namespace Tff.Panzer.Models.Campaign
{
    public class CampaignStep
    {
        public Int32 CampaignStepId { get; set; }
        public String CampaignStepDescription { get; set; }
        public CampaignBriefing CampaignBriefing { get; set; }
        public Int32 ScenarioId { get; set; }
    }
}

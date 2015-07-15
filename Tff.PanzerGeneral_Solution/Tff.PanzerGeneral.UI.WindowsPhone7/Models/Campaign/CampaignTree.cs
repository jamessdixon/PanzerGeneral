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

namespace Tff.Panzer.Models.Campaign
{
    public class CampaignTree
    {
        public Int32 CampaignTreeId { get; set; }
        public CampaignStep CurrentCampaignStep { get; set; }
        public CampaignStepType CampaignStepType { get; set; }
        public Int32 Prestige { get; set; }
        public CampaignBriefing CampaignBriefing { get; set; }
        public CampaignStep NextCampaignStep { get; set; }
    }
}

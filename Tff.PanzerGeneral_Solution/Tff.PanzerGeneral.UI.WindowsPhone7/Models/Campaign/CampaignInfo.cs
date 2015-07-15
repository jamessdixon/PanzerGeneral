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

namespace Tff.Panzer.Models.Campaign
{
    public class CampaignInfo
    {
        public int CampaignId { get; set; }
        public String CampaignName { get; set; }
        public String CampaignTitle { get; set; }
        public String CampaignDescription { get; set; }
        public int SideId { get; set; }
        public Nation Nation { get; set; } 
        public Boolean FreeEliteReplacements { get; set; }
        public int StartingCampaignStepId { get; set; }
        public CampaignBriefing CampaignBriefing { get; set; }

        public override string ToString()
        {
            return String.Format("{0}-{1}",CampaignName, CampaignTitle);
        }
    }
}

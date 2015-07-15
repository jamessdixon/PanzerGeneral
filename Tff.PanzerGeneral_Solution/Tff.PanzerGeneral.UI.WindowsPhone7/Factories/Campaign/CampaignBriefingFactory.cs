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
using System.Xml.Linq;
using System.Linq;
using System.Windows.Resources;
using System.Collections.Generic;
using Tff.Panzer.Models.Campaign;

namespace Tff.Panzer.Factories.Campaign
{
    public class CampaignBriefingFactory
    {
        public List<CampaignBriefing> CampaignBriefings { get; private set; }

        public CampaignBriefingFactory()
        {
            CampaignBriefings = new List<CampaignBriefing>();

            Uri uri = new Uri(Constants.Campaign_BriefingDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from t in applicationXml.Descendants("Briefing")
                       select t;

            CampaignBriefing campaignBriefing = null;
            foreach (var d in data)
            {
                campaignBriefing = new CampaignBriefing();
                campaignBriefing.CampaignBriefingId = (Int32)d.Element("BriefingId");
                campaignBriefing.CampaignBriefingDescription = (String)d.Element("BriefingDesc");
                CampaignBriefings.Add(campaignBriefing);
            }
        }

        public CampaignBriefing GetCampaignBriefing(int campaignBriefingId) 
        {
            CampaignBriefing briefing = (from b in this.CampaignBriefings
                                         where b.CampaignBriefingId == campaignBriefingId
                                         select b).FirstOrDefault();

            return briefing;
        }

    }
}

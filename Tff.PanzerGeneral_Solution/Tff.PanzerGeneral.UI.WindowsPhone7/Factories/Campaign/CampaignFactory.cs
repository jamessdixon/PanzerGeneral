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
using Tff.Panzer.Models.Scenario;
using Tff.Panzer.Models.Campaign;

namespace Tff.Panzer.Factories.Campaign
{
    public class CampaignFactory
    {
        public List<CampaignInfo> Campaigns { get; private set; }
        public CampaignBriefingFactory CampaignBriefingFactory { get; set; }
        public CampaignStepFactory CampaignStepFactory { get; set; }
        public CampaignStepTypeFactory CampaignStepTypeFactory { get; set; }
        public CampaignTreeFactory CampaignTreeFactory { get; set; }


        public CampaignFactory()
        {
            Campaigns = new List<CampaignInfo>();
            CampaignBriefingFactory = new CampaignBriefingFactory();
            CampaignStepFactory = new CampaignStepFactory();
            CampaignStepTypeFactory = new CampaignStepTypeFactory();
            CampaignTreeFactory = new CampaignTreeFactory();
            PopulateCampaigns();
        }

        private void PopulateCampaigns()
        {
            Uri uri = new Uri(Constants.CampaignDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from wz in applicationXml.Descendants("Campaign")
                       select wz;


            CampaignInfo campaignInfo = null;
            foreach (var d in data)
            {
                campaignInfo = new CampaignInfo();
                campaignInfo.CampaignId = (Int32)d.Element("CampaignId");
                campaignInfo.CampaignDescription = (String)d.Element("CampaignDescription");
                campaignInfo.CampaignName = (String)d.Element("CampaignName");
                campaignInfo.CampaignTitle = (String)d.Element("CampaignTitle");
                campaignInfo.Nation = Game.NationFactory.GetNation((Int32)d.Element("Nation"));
                campaignInfo.FreeEliteReplacements = (Boolean)d.Element("FreeEliteReplacement");
                campaignInfo.SideId = (Int32)d.Element("Side");
                campaignInfo.StartingCampaignStepId = (Int32)d.Element("StartingCampaignStepId");
                Campaigns.Add(campaignInfo);
            }
        }


        public CampaignInfo GetCampaignInfo(int campaignId)
        {
            CampaignInfo campaignInfo = (from ci in this.Campaigns
                                         where ci.CampaignId == campaignId
                                         select ci).First();
            return campaignInfo;
        }

    }
}

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
    public class CampaignTreeFactory
    {
        public List<CampaignTree> CampaignTrees { get; private set; }
        public CampaignStepFactory CampaignStepFactory { get; private set; }
        public CampaignStepTypeFactory CampaignStepTypeFactory { get; private set; }
        public CampaignBriefingFactory CampaignBriefingFactory { get; private set; }

        public CampaignTreeFactory()
        {
            CampaignTrees = new List<CampaignTree>();
            CampaignStepFactory = new CampaignStepFactory();
            CampaignStepTypeFactory = new CampaignStepTypeFactory();
            CampaignBriefingFactory = new CampaignBriefingFactory();

            Uri uri = new Uri(Constants.Campaign_TreeDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from t in applicationXml.Descendants("Campaign_Tree")
                       select t;

            CampaignTree campaignTree = null;
            foreach (var d in data)
            {
                campaignTree = new CampaignTree();
                campaignTree.CampaignTreeId = (Int32)d.Element("CampaignTreeId");
                campaignTree.CurrentCampaignStep = CampaignStepFactory.GetCampaignStep((Int32)d.Element("CurrentCampaignStepId"));
                campaignTree.CampaignStepType = CampaignStepTypeFactory.GetCampaignStepType((Int32)d.Element("CampaignStepTypeId"));
                campaignTree.Prestige = (Int32)d.Element("Prestige");
                campaignTree.CampaignBriefing = CampaignBriefingFactory.GetCampaignBriefing((Int32)d.Element("BriefingId"));
                campaignTree.NextCampaignStep = CampaignStepFactory.GetCampaignStep((Int32)d.Element("NextCampaignStepId"));
                CampaignTrees.Add(campaignTree);
            }
        }

        public CampaignTree GetCampaignTree(int campaignTreeId)
        {
            CampaignTree campaignTree = (from ct in this.CampaignTrees
                                         where ct.CampaignTreeId == campaignTreeId
                                         select ct).First();

            return campaignTree;
        }

        public CampaignTree GetCampaignTree(int campaignStepId, int campaignStepTypeId)
        {
            CampaignTree campaignTree = (from ct in this.CampaignTrees
                                         where ct.CurrentCampaignStep.CampaignStepId == campaignStepId
                                         && ct.CampaignStepType.CampaignStepTypeId == campaignStepTypeId
                                         select ct).First();
            return campaignTree;
        }

        public CampaignTree GetCampaignTree(int campaignStepId, CampaignStepTypeEnum campaignStepTypeEnum)
        {
            return GetCampaignTree(campaignStepId, campaignStepTypeEnum.GetHashCode());
        }

        public CampaignTree GetCampaignTree(CampaignStep campaignStep, CampaignStepTypeEnum campaignStepTypeEnum)
        {
            return GetCampaignTree(campaignStep.CampaignStepId, campaignStepTypeEnum);
        }
    }
}

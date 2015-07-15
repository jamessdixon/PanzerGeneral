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
using Tff.Panzer.Models.Geography;
using Tff.Panzer.Models;
using Tff.Panzer.Models.Campaign;

namespace Tff.Panzer.Factories.Campaign
{
    public class CampaignStepFactory
    {
        public List<CampaignStep> CampaignSteps { get; private set; }
        public CampaignBriefingFactory CampaignBriefingFactory { get; private set; }


        public CampaignStepFactory()
        {
            CampaignSteps = new List<CampaignStep>();
            CampaignBriefingFactory = new CampaignBriefingFactory();

            Uri uri = new Uri(Constants.Campaign_StepDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from t in applicationXml.Descendants("Campaign_Step")
                       select t;

            CampaignStep campaignStep = null;
            foreach (var d in data)
            {
                campaignStep = new CampaignStep();
                campaignStep.CampaignStepId = (Int32)d.Element("CampaignStepId");
                campaignStep.CampaignStepDescription = (String)d.Element("CampaignStepDesc");
                campaignStep.CampaignBriefing = CampaignBriefingFactory.GetCampaignBriefing((Int32)d.Element("BriefingId"));
                campaignStep.ScenarioId = ((Int32)d.Element("ScenarioId"));
                CampaignSteps.Add(campaignStep);
            }
        }

        public CampaignStep GetCampaignStep(int campaignStepId) 
        {
            CampaignStep step = (from b in this.CampaignSteps
                                 where b.CampaignStepId == campaignStepId
                                 select b).FirstOrDefault();

            return step;
        }
    }
}

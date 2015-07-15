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
    public class CampaignStepTypeFactory
    {
        public List<CampaignStepType> CampaignStepTypes { get; private set; }

        public CampaignStepTypeFactory()
        {
            CampaignStepTypes = new List<CampaignStepType>();

            Uri uri = new Uri(Constants.Campaign_StepTypeDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from t in applicationXml.Descendants("Campaign_StepType")
                       select t;

            CampaignStepType campaignStepType = null;
            foreach (var d in data)
            {
                campaignStepType = new CampaignStepType();
                campaignStepType.CampaignStepTypeId = (Int32)d.Element("CampaignStepTypeId");
                campaignStepType.CampaignStepTypeDescription = (String)d.Element("CampaginStepTypeDesc");
                CampaignStepTypes.Add(campaignStepType);
            }
        }

        public CampaignStepType GetCampaignStepType(int campaignStepTypeId) 
        {
            CampaignStepType campaignStepType = (from tt in this.CampaignStepTypes
                      where tt.CampaignStepTypeId == campaignStepTypeId
                      select tt).First();

            return campaignStepType;
        }
    }
}

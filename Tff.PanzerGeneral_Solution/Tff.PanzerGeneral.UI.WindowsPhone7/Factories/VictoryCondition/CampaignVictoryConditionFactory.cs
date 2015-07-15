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
using Tff.Panzer.Models;
using Tff.Panzer.Models.VictoryCondition;

namespace Tff.Panzer.Factories.VictoryCondition
{
    public class CampaignVictoryConditionFactory
    {
        public List<CampaignVictoryCondition> CampaignVictoryConditions { get; private set; }
        public ObjectiveTypeFactory ObjectiveTypeFactory { get; private set; }

        public CampaignVictoryConditionFactory()
        {
            CampaignVictoryConditions = new List<CampaignVictoryCondition>();
            ObjectiveTypeFactory = new ObjectiveTypeFactory();

            Uri uri = new Uri(Constants.CampaignVictoryConditionDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from t in applicationXml.Descendants("VictoryConditionCampaign")
                       select t;

            CampaignVictoryCondition campaignVictoryCondition = null;
            foreach (var d in data)
            {
                campaignVictoryCondition = new CampaignVictoryCondition();
                campaignVictoryCondition.CampaignVictoryConditionId = (Int32)d.Element("VictoryConditionCampaignId");
                campaignVictoryCondition.ScenarioId = ((Int32)d.Element("ScenarioId"));
                campaignVictoryCondition.ObjectiveType = ObjectiveTypeFactory.GetObjectiveType((Int32)d.Element("ObjectiveTypeId"));
                campaignVictoryCondition.MajorVictoryObjectviesToHold = (Int32)d.Element("MajorVictoryNumberOfObjectivesToHold");
                campaignVictoryCondition.MajorVictoryTurnsRemain = (Int32)d.Element("MajorVictoryTurnsRemain");
                campaignVictoryCondition.MinorVictoryObjectviesToHold = (Int32)d.Element("MinorVictoryNumberOfObjectivesToHold");
                campaignVictoryCondition.MinorVactoryTurnsRemain = (Int32)d.Element("MinorVictoryTurnsRemain");
                CampaignVictoryConditions.Add(campaignVictoryCondition);
            }
        }

        public CampaignVictoryCondition GetCampaignVictoryCondition(int campaignVictoryConditionId) 
        {
            CampaignVictoryCondition campaignVictoryCondition = (from tt in this.CampaignVictoryConditions
                                                                 where tt.CampaignVictoryConditionId == campaignVictoryConditionId
                                                                 select tt).FirstOrDefault();

            return campaignVictoryCondition;
        }

        public CampaignVictoryCondition GetCampaignVictoryConditionForAScenario(int scenarioId)
        {
            return CampaignVictoryConditions.Where(cvc => cvc.ScenarioId == scenarioId).FirstOrDefault();
        }



    }
}

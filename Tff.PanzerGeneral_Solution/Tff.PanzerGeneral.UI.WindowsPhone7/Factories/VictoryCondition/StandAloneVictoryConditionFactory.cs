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
using Tff.Panzer.Models;
using Tff.Panzer.Models.VictoryCondition;

namespace Tff.Panzer.Factories.VictoryCondition
{
    public class StandAloneVictoryConditionFactory
    {
        public List<StandAloneVictoryCondition> StandAloneVictoryConditions { get; private set; }
        public ObjectiveTypeFactory ObjectiveTypeFactory { get; private set; }

        public StandAloneVictoryConditionFactory()
        {
            StandAloneVictoryConditions = new List<StandAloneVictoryCondition>();
            ObjectiveTypeFactory = new ObjectiveTypeFactory();

            Uri uri = new Uri(Constants.StandAloneVictoryConditionDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from wz in applicationXml.Descendants("VictoryConditionStandAlone")
                       select wz;

            StandAloneVictoryCondition standAloneVictoryCondition = null;
            foreach (var d in data)
            {
                standAloneVictoryCondition = new StandAloneVictoryCondition();
                standAloneVictoryCondition.StandAloneVictoryConditionId = (Int32)d.Element("VictoryConditionStandAloneId");
                standAloneVictoryCondition.ScenarioId = (Int32)d.Element("ScenarioId");
                standAloneVictoryCondition.ObjectiveType = ObjectiveTypeFactory.GetObjectiveType((Int32)d.Element("ObjectiveTypeId"));
                standAloneVictoryCondition.WinAmount = (Int32)d.Element("WinAmount");
                standAloneVictoryCondition.LossAmount = (Int32)d.Element("LossAmount");
                StandAloneVictoryConditions.Add(standAloneVictoryCondition);
            }
        }

        public StandAloneVictoryCondition GetStandAloneVictoryCondition(int standAloneVictoryConditionId)
        {
            StandAloneVictoryCondition standAloneVictoryCondition = (
                                    from scp in this.StandAloneVictoryConditions
                                    where scp.StandAloneVictoryConditionId == standAloneVictoryConditionId
                                    select scp).First();

            return standAloneVictoryCondition;
        }

        public StandAloneVictoryCondition GetStandAloneVictoryConditionForAScenario(int scenarioId)
        {
            return StandAloneVictoryConditions.Where(savc => savc.ScenarioId == scenarioId).FirstOrDefault();
        }


    }
}
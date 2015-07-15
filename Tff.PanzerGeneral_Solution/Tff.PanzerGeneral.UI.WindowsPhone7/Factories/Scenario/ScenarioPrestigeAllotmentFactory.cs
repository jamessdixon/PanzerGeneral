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

namespace Tff.Panzer.Factories.Scenario
{
    public class ScenarioPrestigeAllotmentFactory
    {
        public List<ScenarioPrestigeAllotment> ScenarioPrestigeAllotments { get; private set; }

        public ScenarioPrestigeAllotmentFactory()
        {
            ScenarioPrestigeAllotments = new List<ScenarioPrestigeAllotment>();

            Uri uri = new Uri(Constants.Scenario_PrestigeAllotmentDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from wz in applicationXml.Descendants("Scenario_PrestigeAllotment")
                       select wz;

            ScenarioPrestigeAllotment scenarioPrestigeAllotment = null;
            foreach (var d in data)
            {
                scenarioPrestigeAllotment = new ScenarioPrestigeAllotment();
                scenarioPrestigeAllotment.ScenarioPrestigeAllotmentId = (Int32)d.Element("ScenarioPrestigeAllotmentId");
                scenarioPrestigeAllotment.ScenarioId = (Int32)d.Element("ScenarioId");
                scenarioPrestigeAllotment.TurnId = (Int32)d.Element("TurnId");
                scenarioPrestigeAllotment.AxisPrestige = (Int32)d.Element("AxisPrestige");
                scenarioPrestigeAllotment.AlliedPrestige = (Int32)d.Element("AlliedPrestige");
                ScenarioPrestigeAllotments.Add(scenarioPrestigeAllotment);
            }
        }

        public ScenarioPrestigeAllotment GetScenarioPrestigeAllotment(int scenarioPrestigeAllotmentId)
        {
            ScenarioPrestigeAllotment scenarioPrestigeAllotment = (
                                    from spa in this.ScenarioPrestigeAllotments
                                    where spa.ScenarioPrestigeAllotmentId == scenarioPrestigeAllotmentId
                                    select spa).First();

            return scenarioPrestigeAllotment;
        }

        public ScenarioPrestigeAllotment GetScenarioPrestigeAllotment(int scenarioId, int turnId)
        {
            ScenarioPrestigeAllotment scenarioPrestigeAllotment = (
                                    from spa in this.ScenarioPrestigeAllotments
                                    where spa.ScenarioId == scenarioId
                                    && spa.TurnId == turnId
                                    select spa).FirstOrDefault();

            return scenarioPrestigeAllotment;
        }

        
        public List<ScenarioPrestigeAllotment> GetScenarioPrestigeAllotments(int scenarioId)
        {
            List<ScenarioPrestigeAllotment> scenarioPrestigeAllotments = new List<ScenarioPrestigeAllotment>();

            var query = from spa in this.ScenarioPrestigeAllotments
                        where spa.ScenarioId == scenarioId
                        select spa;
            foreach (ScenarioPrestigeAllotment scenarioPrestigeAllotment in query)
            {
                scenarioPrestigeAllotments.Add(scenarioPrestigeAllotment);
            }

            return scenarioPrestigeAllotments;
        }

    }
}

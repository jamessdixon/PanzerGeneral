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
using Tff.Panzer.Models.Geography;

namespace Tff.Panzer.Factories.Scenario
{
    public class ScenarioSideFactory
    {
        public List<ScenarioSide> ScenarioSides { get; private set; }

        public ScenarioSideFactory()
        {
            LoadScenarioSides();
            
        }

        private void LoadScenarioSides()
        {
            ScenarioSides = new List<ScenarioSide>();

            Uri uri = new Uri(Constants.Scenario_SideDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from ss in applicationXml.Descendants("Scenario_Side")
                       select ss;

            ScenarioSide scenarioSide = null;
            foreach (var d in data)
            {
                scenarioSide = new ScenarioSide();
                scenarioSide.ScenarioSideId = (Int32)d.Element("ScenarioSideId");
                scenarioSide.ScenarioId = (Int32)d.Element("ScenarioId");
                scenarioSide.SideId = (Int32)d.Element("SideId");
                scenarioSide.Prestige = (Int32)d.Element("Prestige");
                scenarioSide.CoreLimit = (Int32)d.Element("CoreLimit");
                scenarioSide.AuxLimit = (Int32)d.Element("AuxLimit");
                scenarioSide.Stance = (Int32)d.Element("Stance");
                scenarioSide.Orientation = (Int32)d.Element("Orientation");
                scenarioSide.SeaTransports = (Int32)d.Element("SeaTransports");
                scenarioSide.SeaTransportTypeId = (Int32)d.Element("SeaTransportTypeId");
                scenarioSide.AirTransports = (Int32)d.Element("AirTransports");
                scenarioSide.AirTransportTypeId = (Int32)d.Element("AirTransportTypeId");
                scenarioSide.NewUnitExperience = (Int32)d.Element("NewUnitExperience");
                ScenarioSides.Add(scenarioSide);
            }
        }

        public ScenarioSide GetScenarioSide(int scenarioSideId)
        {
            ScenarioSide scenarioSide = (
                                    from ss in this.ScenarioSides
                                    where ss.ScenarioSideId == scenarioSideId
                                    select ss).First();

            return scenarioSide;
        }

        public List<ScenarioSide> GetScenarioSides(int scenarioId)
        {
            List<ScenarioSide> scenarioSides = new List<ScenarioSide>();

            var query = from ss in this.ScenarioSides
                        where ss.ScenarioId == scenarioId
                        select ss;
            foreach (ScenarioSide scenarioSide in query)
            {
                scenarioSides.Add(scenarioSide);
            }

            return scenarioSides;
        }

        public ScenarioSide GetScenarioSide(int scenarioId, SideEnum sideEnum)
        {
            List<ScenarioSide> scenarioSides = ScenarioSides.Where(ss => ss.ScenarioId == scenarioId).ToList();
            if (sideEnum == SideEnum.Axis)
            {
                return scenarioSides[0];
            }
            else
            {
                return scenarioSides[1];
            }
        }

    }
}

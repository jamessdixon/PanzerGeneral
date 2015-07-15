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
    public class ScenarioFactory
    {
        public List<ScenarioInfo> Scenarios { get; private set; }
        public event EventHandler<EventArgs> ScenariosLoaded;

        public ScenarioClassPurchaseFactory ScenarioClassPurchaseFactory { get; set; }
        public ScenarioPrestigeAllotmentFactory ScenarioPrestigeAllotmentFactory { get; set; }
        public ScenarioSideFactory ScenarioSideFactory { get; set; }
        public ScenarioTileFactory ScenarioTileFactory { get; set; }
        public ScenarioUnitFactory ScenarioUnitFactory { get; set; }
        public ScenarioTurnFactory ScenarioTurnFactory { get; set; }

        public ScenarioFactory()
        {
            ScenarioClassPurchaseFactory = new ScenarioClassPurchaseFactory();
            ScenarioPrestigeAllotmentFactory = new ScenarioPrestigeAllotmentFactory();
            ScenarioSideFactory = new ScenarioSideFactory();
            ScenarioTileFactory = new ScenarioTileFactory();
            ScenarioUnitFactory = new ScenarioUnitFactory();
            ScenarioTurnFactory = new ScenarioTurnFactory();
            Scenarios = new List<ScenarioInfo>();
        }

        public void PopulateScenariosLocally()
        {
            Uri uri = new Uri(Constants.ScenarioDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from wz in applicationXml.Descendants("Scenario")
                       select wz;

            int startYear = 0;
            int startMonth = 0;
            int startDay = 0;
            int turnsPerDay = 0;
            int daysPerTurn = 0;

            ScenarioInfo scenarioInfo = null;
            foreach (var d in data)
            {
                scenarioInfo = new ScenarioInfo();
                scenarioInfo.ScenarioId = (Int32)d.Element("ScenarioId");
                scenarioInfo.ScenarioName = (String)d.Element("ScenarioName");
                scenarioInfo.ScenarioDescription = (String)d.Element("ScenarioDescription");
                scenarioInfo.NumberOfTurns = (Int32)d.Element("Turns");
                startYear = (Int32)d.Element("Year");
                startMonth = (Int32)d.Element("Month");
                startDay = (Int32)d.Element("Day");
                scenarioInfo.ScenarioStart = new DateTime(startYear, startMonth, startDay);
                turnsPerDay = (Int32)d.Element("TurnsPerDay");
                daysPerTurn = (Int32)d.Element("DaysPerTurn");
                if (turnsPerDay == 0 && daysPerTurn == 0)
                {
                    scenarioInfo.TurnIncrement = 1;
                }
                else if (turnsPerDay == 0 && daysPerTurn != 0)
                {
                    scenarioInfo.TurnIncrement = daysPerTurn;
                }
                else if (turnsPerDay != 0 && daysPerTurn == 0)
                {
                    scenarioInfo.TurnIncrement = 1/turnsPerDay;
                }
                scenarioInfo.StartingWeather = Game.WeatherFactory.GetWeather((Int32)d.Element("CurrentWeather"));
                scenarioInfo.WeatherZone = Game.WeatherFactory.WeatherZoneFactory.GetWeatherZone((Int32)d.Element("WeatherZone"));
                scenarioInfo.MaxUnitStrength = (Int32)d.Element("MaxUnitStrength");
                scenarioInfo.MaxUnitExperience = (Int32)d.Element("MaxUnitExperience");
                Scenarios.Add(scenarioInfo);
            }
            ScenariosLoaded(null, null);
        }

        public void PopulateScenariosFromWebService()
        {
            PanzerProxy.PanzerServiceClient panzerClient = new PanzerProxy.PanzerServiceClient();
            panzerClient.GetActiveScenariosCompleted += new EventHandler<PanzerProxy.GetActiveScenariosCompletedEventArgs>(panzerClient_GetActiveScenariosCompleted);
            panzerClient.GetActiveScenariosAsync();

        }

        void panzerClient_GetActiveScenariosCompleted(object sender, PanzerProxy.GetActiveScenariosCompletedEventArgs e)
        {
            List<PanzerProxy.Scenario> proxyScenarios = e.Result as List<PanzerProxy.Scenario>;
            foreach (PanzerProxy.Scenario proxyScenario in proxyScenarios)
            {
                Scenarios.Add(ConvertProxyScenarioToScenario(proxyScenario));
            }
            ScenariosLoaded(null, null);

        }

        private ScenarioInfo ConvertProxyScenarioToScenario(PanzerProxy.Scenario proxyScenario)
        {
            ScenarioInfo scenarioInfo = new ScenarioInfo();
            scenarioInfo.MaxUnitExperience = proxyScenario.MaxUnitExperience;
            scenarioInfo.MaxUnitStrength = proxyScenario.MaxUnitStrength;
            scenarioInfo.NumberOfTurns = proxyScenario.NumberOfTurns;
            scenarioInfo.ScenarioDescription = proxyScenario.ScenarioDescription;
            scenarioInfo.ScenarioId = proxyScenario.ScenarioId;
            scenarioInfo.ScenarioName = proxyScenario.ScenarioName;
            scenarioInfo.ScenarioStart = proxyScenario.ScenarioStart;
            scenarioInfo.StartingWeather = Game.WeatherFactory.GetWeather(proxyScenario.StartingWeatherId);
            if (proxyScenario.TurnsPerDay == 0 && proxyScenario.DaysPerTurn == 0)
            {
                scenarioInfo.TurnIncrement = 1;
            }
            else if (proxyScenario.TurnsPerDay == 0 && proxyScenario.DaysPerTurn != 0)
            {
                scenarioInfo.TurnIncrement = proxyScenario.DaysPerTurn;
            }
            else if (proxyScenario.TurnsPerDay != 0 && proxyScenario.DaysPerTurn == 0)
            {
                scenarioInfo.TurnIncrement = 1 / proxyScenario.TurnsPerDay;
            }
            scenarioInfo.WeatherZone = Game.WeatherFactory.WeatherZoneFactory.GetWeatherZone(proxyScenario.WeatherZoneId);
            return scenarioInfo;
        }

        public ScenarioInfo GetScenarioInfo(int scenarioId)
        {
            ScenarioInfo scenarioInfo = (from si in this.Scenarios
                               where si.ScenarioId == scenarioId
                               select si).First();

            return scenarioInfo;
        }


    }
}

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
using Tff.Panzer.Models.Scenario;

namespace Tff.Panzer.Factories.Scenario
{
    public class ScenarioUnitFactory
    {
        public List<ScenarioUnit> ScenarioUnits { get; private set; }

        public ScenarioUnitFactory()
        {
            ScenarioUnits = new List<ScenarioUnit>();

            Uri uri = new Uri(Constants.Scenario_UnitDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from wz in applicationXml.Descendants("Scenario_Unit")
                       select wz;

            ScenarioUnit scenarioUnit = null;
            foreach (var d in data)
            {
                scenarioUnit = new ScenarioUnit();
                scenarioUnit.ScenarioUnitId = (Int32)d.Element("ScenarioUnitId");
                scenarioUnit.ScenarioId = (Int32)d.Element("ScenarioId");
                scenarioUnit.StartingScenarioTileId = (Int32)d.Element("StartingScenarioTileId");
                scenarioUnit.EquipmentId = (Int32)d.Element("EquipmentId");
                scenarioUnit.TransportId = (Int32)d.Element("TransportId");
                scenarioUnit.SideId = (Int32)d.Element("SideId");
                scenarioUnit.NationId = (Int32)d.Element("FlagId");
                scenarioUnit.Strength = (Int32)d.Element("Strength");
                scenarioUnit.Experience = (Int32)d.Element("Experience");
                scenarioUnit.Entrenchment = (Int32)d.Element("Entrenchment");
                scenarioUnit.CordInd = !(Boolean)d.Element("AuxiliaryInd");
                ScenarioUnits.Add(scenarioUnit);
            }
        }

        public ScenarioUnit GetScenarioUnit(int scenarioUnitId)
        {
            ScenarioUnit scenarioUnit = (from su in this.ScenarioUnits
                                       where su.ScenarioUnitId == scenarioUnitId
                                    select su).First();

            return scenarioUnit;
        }

        internal List<ScenarioUnit> GetScenarioUnits(int scenarioId)
        {
            List<ScenarioUnit> scenarioUnits = new List<ScenarioUnit>();

            var query = from su in this.ScenarioUnits
                        where su.ScenarioId == scenarioId
                        select su;
            foreach (ScenarioUnit scenarioUnit in query)
            {
                scenarioUnits.Add(scenarioUnit);
            }

            return scenarioUnits;
        }
    }
}

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
    public class ScenarioClassPurchaseFactory
    {
        public List<ScenarioClassPurchase> ScenarioClassPurchases { get; private set; }

        public ScenarioClassPurchaseFactory()
        {
            ScenarioClassPurchases = new List<ScenarioClassPurchase>();

            Uri uri = new Uri(Constants.Scenario_ClassPurchaseDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from wz in applicationXml.Descendants("Scenario_ClassPurchase")
                       select wz;

            ScenarioClassPurchase scenarioClassPurchase = null;
            foreach (var d in data)
            {
                scenarioClassPurchase = new ScenarioClassPurchase();
                scenarioClassPurchase.ScenarioClassPurchaseId = (Int32)d.Element("ScenarioClassPurchaseId");
                scenarioClassPurchase.ScenarioId = (Int32)d.Element("ScenarioId");
                scenarioClassPurchase.ClassId = (Int32)d.Element("ClassId");
                scenarioClassPurchase.CanPurchaseAmount = (Int32)d.Element("CanPurchaseAmount");
                ScenarioClassPurchases.Add(scenarioClassPurchase);
            }
        }

        public ScenarioClassPurchase GetScenarioClassPurchase(int scenarioClassPurchaseId)
        {
            ScenarioClassPurchase scenarioClassPurchase = (
                                    from scp in this.ScenarioClassPurchases
                                    where scp.ScenarioClassPurchaseId == scenarioClassPurchaseId
                                    select scp).First();

            return scenarioClassPurchase;
        }
        
        public List<ScenarioClassPurchase> GetScenarioClassPurchases(int scenarioId)
        {
            List<ScenarioClassPurchase> scenarioClassPurchases = new List<ScenarioClassPurchase>();

            var query = from scp in this.ScenarioClassPurchases
                        where scp.ScenarioId == scenarioId
                        select scp;
            foreach (ScenarioClassPurchase scenarioClassPurchase in query)
            {
                scenarioClassPurchases.Add(scenarioClassPurchase);
            }

            return scenarioClassPurchases;
        }
    }
}

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
using Tff.Panzer.Controls;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Windows.Resources;
using System.Diagnostics;
using Tff.Panzer.Models.Geography;
using Tff.Panzer.Models.Scenario;
using System.Threading;
using System.ComponentModel;


namespace Tff.Panzer.Factories.Scenario
{
    public class ScenarioTileFactory
    {

        public List<ScenarioTile> ScenarioTiles { get; private set; }
        public event EventHandler<EventArgs> ScenarioTilesLoaded;

        public void PopulateScenarioTiles(int scenarioId)
        {
            if (scenarioId < 2)
            {
                PopulateScenarioTilesLocally(scenarioId);
            }
            else
            {
                PopulateScenarioTilesFromWebService(scenarioId);
            }
        }

        private void PopulateScenarioTilesLocally(int scenarioId)
        {
            ScenarioTiles = new List<ScenarioTile>();

            Uri uri = new Uri(Constants.Scenario_TileDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from st in applicationXml.Descendants("ScenarioTile")
                       where st.Element("ScenarioId").Value == scenarioId.ToString()
                       select st;

            ScenarioTile scenarioTile = null;
            foreach (var d in data)
            {
                scenarioTile = new ScenarioTile();
                scenarioTile.ScenarioTileId = (Int32)d.Element("ScenarioTileId");
                scenarioTile.ScenarioId = (Int32)d.Element("ScenarioId");
                scenarioTile.ColumnNumber = (Int32)d.Element("ColumnNumber");
                scenarioTile.RowNumber = (Int32)d.Element("RowNumber");
                scenarioTile.TerrainId = (Int32)d.Element("TerrainId");
                scenarioTile.TileNameId = (Int32)d.Element("TileNameId");
                scenarioTile.NationId = (Int32)d.Element("NationId");
                scenarioTile.SideId = (Int32)d.Element("SideId");
                scenarioTile.DeployIndicator = (Boolean)d.Element("DeployTileInd");
                scenarioTile.SupplyIndicator = (Boolean)d.Element("SupplyTileInd");
                scenarioTile.VictoryIndicator = (Boolean)d.Element("VictoryTileInd");
                ScenarioTiles.Add(scenarioTile);
            }
            ScenarioTilesLoaded(null,null);

        }

        private void PopulateScenarioTilesFromWebService(int scenarioId)
        {
            PanzerProxy.PanzerServiceClient panzerClient = new PanzerProxy.PanzerServiceClient();
            panzerClient.GetScenarioTilesCompleted += new EventHandler<PanzerProxy.GetScenarioTilesCompletedEventArgs>(panzerClient_GetScenarioTilesCompleted);
            panzerClient.GetScenarioTilesAsync(scenarioId);
        }

        void panzerClient_GetScenarioTilesCompleted(object sender, PanzerProxy.GetScenarioTilesCompletedEventArgs e)
        {
            ScenarioTiles = new List<ScenarioTile>();
            List<PanzerProxy.ScenarioTile> proxyTiles = e.Result as List<PanzerProxy.ScenarioTile>;
            foreach (PanzerProxy.ScenarioTile proxyScenarioTile in proxyTiles)
            {
                ScenarioTiles.Add(ConvertProxyScenarioTileToScenarioTile(proxyScenarioTile));
            }
            ScenarioTilesLoaded(null, null);
        }

        private ScenarioTile ConvertProxyScenarioTileToScenarioTile(PanzerProxy.ScenarioTile proxyScenarioTile)
        {
            ScenarioTile scenarioTile = new ScenarioTile()
            {
                ColumnNumber = proxyScenarioTile.ColumnNumber,
                DeployIndicator = proxyScenarioTile.DeployIndicator,
                NationId = proxyScenarioTile.NationId,
                RowNumber = proxyScenarioTile.RowNumber,
                ScenarioId = proxyScenarioTile.ScenarioId,
                ScenarioTileId = proxyScenarioTile.ScenarioTileId,
                SideId = proxyScenarioTile.SideId,
                SupplyIndicator = proxyScenarioTile.SupplyIndicator,
                TerrainId = proxyScenarioTile.TerrainId,
                TileNameId = proxyScenarioTile.TileNameId,
                VictoryIndicator = proxyScenarioTile.VictoryIndicator
            };
            return scenarioTile;
        }

        public ScenarioTile GetScenarioTile(int scenarioTileId)
        {
            ScenarioTile scenarioTile = (from st in this.ScenarioTiles
                                       where st.ScenarioTileId == scenarioTileId
                                       select st).FirstOrDefault();
            return scenarioTile;
        }





    }


}

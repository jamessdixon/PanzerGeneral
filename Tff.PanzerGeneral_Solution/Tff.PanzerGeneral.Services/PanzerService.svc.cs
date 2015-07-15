using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Tff.Panzer.Services
{
    public class PanzerService : IPanzerService
    {

        public ScenarioTile GetScenarioTile(int scenarioTileid)
        {
            PanzerEntities entities = new PanzerEntities();
            var panzer_ScenarioTile = entities.Panzer_ScenarioTile.Where(pst => pst.ScenarioTileId == scenarioTileid).FirstOrDefault();
            return CreateScenarioTile(panzer_ScenarioTile);
        }

        public List<ScenarioTile> GetScenarioTiles(int scenarioid)
        {
            List<ScenarioTile> scenarioTiles = new List<ScenarioTile>();
            PanzerEntities entities = new PanzerEntities();
            var panzer_ScenarioTiles = entities.Panzer_ScenarioTile.Where(pst => pst.ScenarioId == scenarioid).ToList();
            foreach (Panzer_ScenarioTile panzer_ScenarioTile in panzer_ScenarioTiles)
            {
                scenarioTiles.Add(CreateScenarioTile(panzer_ScenarioTile));
            }
            return scenarioTiles;
        }

        private ScenarioTile CreateScenarioTile(Panzer_ScenarioTile panzer_ScenarioTile)
        {
            ScenarioTile scenarioTile = new ScenarioTile()
            {
                ScenarioTileId = panzer_ScenarioTile.ScenarioTileId,
                ColumnNumber = panzer_ScenarioTile.ColumnNumber,
                DeployIndicator = panzer_ScenarioTile.DeployTileInd,
                NationId = panzer_ScenarioTile.NationId,
                RowNumber = panzer_ScenarioTile.RowNumber,
                ScenarioId = panzer_ScenarioTile.ScenarioId,
                SideId = panzer_ScenarioTile.SideId,
                SupplyIndicator = panzer_ScenarioTile.SupplyTileInd,
                TerrainId = panzer_ScenarioTile.TerrainId,
                TileNameId = panzer_ScenarioTile.TileNameId,
                VictoryIndicator = panzer_ScenarioTile.VictoryTileInd
            };
            return scenarioTile;
        }


        public List<Scenario> GetActiveScenarios()
        {
            List<Scenario> scenarios = new List<Scenario>();
            PanzerEntities entities = new PanzerEntities();
            var query = entities.Panzer_Scenario.Where(ps => ps.ActiveInd == true);
            foreach (var panzer_scenario in query)
            {
                scenarios.Add(CreateScenario(panzer_scenario));
            }
            return scenarios;

        }

        private Scenario CreateScenario(Panzer_Scenario panzer_Scenario)
        {
            Scenario scenario = new Scenario();
            scenario.ActiveInd = panzer_Scenario.ActiveInd;
            scenario.MaxUnitExperience = panzer_Scenario.MaxUnitStrength;
            scenario.MaxUnitStrength = panzer_Scenario.MaxUnitStrength;
            scenario.NumberOfTurns = panzer_Scenario.Turns;
            scenario.ScenarioDescription = panzer_Scenario.ScenarioDescription;
            scenario.ScenarioId = panzer_Scenario.ScenarioId;
            scenario.ScenarioName = panzer_Scenario.ScenarioName;
            scenario.ScenarioStart = new DateTime(panzer_Scenario.Year, panzer_Scenario.Month, panzer_Scenario.Day);
            scenario.StartingWeatherId = panzer_Scenario.CurrentWeather;
            scenario.DaysPerTurn = panzer_Scenario.DaysPerTurn;
            scenario.TurnsPerDay = panzer_Scenario.TurnsPerDay;
            scenario.WeatherZoneId = panzer_Scenario.WeatherZone;
            return scenario;
        }
    }
}

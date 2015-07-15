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
using Tff.Panzer.Models;
using Tff.Panzer.Factories.Scenario;
using Tff.Panzer.Models.Scenario;
using System.Collections.Generic;

namespace Tff.Panzer.Factories
{
    public class TurnFactory
    {   
        public List<Turn> GetTurnsForAScenario(int scenarioId)
        {
            List<Turn> turns = new List<Turn>();
            ScenarioTurnFactory scenarioTurnFactory = Game.ScenarioFactory.ScenarioTurnFactory;
            Turn turn = null;
            foreach (ScenarioTurn scenarioTurn in scenarioTurnFactory.GetScenarioTurns(scenarioId))
            {
                turn = new Turn()
                {
                    TurnId = scenarioTurn.ScenarioTurnId,
                    TurnDate = scenarioTurn.TurnDate,
                    CurrentWeather = scenarioTurn.CurrentWeather,
                    CurrentTerrainCondition = scenarioTurn.CurrentTerrainCondition,
                    ForcastedWeather = scenarioTurn.ForcastedWeather,
                    ForcastedTerrainCondition = scenarioTurn.ForcastedTerrainCondition
                };
                turns.Add(turn);
            }

            //TODO: Plug for testing save and load - 1st turn has frozen terrrain
            //Models.Geography.TerrainCondition terrainCondition = new Models.Geography.TerrainCondition();
            //terrainCondition.TerrainConditionId = 2;
            //turns[0].CurrentTerrainCondition = terrainCondition; 

            return turns;
        
        }



    }
}

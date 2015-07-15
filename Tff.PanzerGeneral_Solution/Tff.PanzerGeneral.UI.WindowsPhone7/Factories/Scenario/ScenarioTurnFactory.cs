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
using Tff.Panzer.Factories.Geography;


namespace Tff.Panzer.Factories.Scenario
{
    public class ScenarioTurnFactory
    {
        TerrainConditionFactory terrainConditionFactory = new TerrainConditionFactory();

        public List<ScenarioTurn> GetScenarioTurns(int numberOfTurns, DateTime startDate, Double turnIncrement, int weatherZoneId)
        {
            int numberOfSnowDays = 0;
            int numberOfRainDays = 0;
            DateTime currentDate = startDate;
            List<ScenarioTurn> scenarioTurns = new List<ScenarioTurn>();
            for (int i = 0; i < numberOfTurns; i++)
            {
                ScenarioTurn scenarioTurn = new ScenarioTurn();
                scenarioTurn.ScenarioTurnId = i;
                scenarioTurn.TurnDate = currentDate;
                //Current Weather
                WeatherProbability weatherProbability = Game.WeatherFactory.WeatherProbabilityFactory.GetWeatherProbability(weatherZoneId, currentDate.Month);
                Random random = new Random(0);
                double randomValue = random.NextDouble();
                if (randomValue < weatherProbability.ParcipitationChance)
                {
                    if (weatherProbability.SnowInd == true)
                    {
                        scenarioTurn.CurrentWeather = Game.WeatherFactory.GetWeather(2);
                        numberOfSnowDays++;
                    }
                    else
                    {
                        scenarioTurn.CurrentWeather = Game.WeatherFactory.GetWeather(1);
                        numberOfRainDays++;
                    }
                }
                else
                {
                    scenarioTurn.CurrentWeather = Game.WeatherFactory.GetWeather(0);
                    numberOfSnowDays = 0;
                    numberOfRainDays = 0;
                }
                //Current Conditions
                if (numberOfRainDays < 2 && numberOfSnowDays < 2)
                {
                    scenarioTurn.CurrentTerrainCondition = terrainConditionFactory.GetTerrainCondition(0);
                }
                else
                {
                    if (numberOfRainDays >= 2)
                    {
                        scenarioTurn.CurrentTerrainCondition = terrainConditionFactory.GetTerrainCondition(1);
                    }
                    else
                    {
                        scenarioTurn.CurrentTerrainCondition = terrainConditionFactory.GetTerrainCondition(2);
                    }
                }
                //Plug for future conditions
                scenarioTurn.ForcastedWeather = scenarioTurn.CurrentWeather;
                scenarioTurn.ForcastedTerrainCondition = scenarioTurn.CurrentTerrainCondition;
                scenarioTurns.Add(scenarioTurn);
                currentDate = currentDate.AddDays(turnIncrement);
            }
            return scenarioTurns;
        }

        public List<ScenarioTurn> GetScenarioTurns(int scenarioId)
        {
            ScenarioInfo scenarioInfo = Game.ScenarioFactory.GetScenarioInfo(scenarioId);
            return GetScenarioTurns(scenarioInfo.NumberOfTurns, scenarioInfo.ScenarioStart, scenarioInfo.TurnIncrement, scenarioInfo.WeatherZone.WeatherZoneId);
        }

    }
}

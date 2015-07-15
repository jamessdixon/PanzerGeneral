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
using System.Collections.Generic;
using System.Linq;
using Tff.Panzer.Models.Scenario;
using Tff.Panzer.Models;
using Tff.Panzer.Models.Army;
using Tff.Panzer.Models.Geography;

namespace Tff.Panzer.Factories.Army
{

    public class ArmyFactory
    {
        
        public ArmyInfo CreateArmy(int scenarioId, int sideId)
        {
            ArmyInfo armyInfo = new ArmyInfo();
            armyInfo.SideId = sideId;

            List<ScenarioUnit> scenarioUnits = Game.ScenarioFactory.ScenarioUnitFactory.GetScenarioUnits(scenarioId);
            foreach (ScenarioUnit scenarioUnit in scenarioUnits)
            {
                int unitId = 0;
                if (scenarioUnit.SideId == sideId)
                {
                    armyInfo.Units.Add(Game.UnitFactory.CreateUnit(unitId, scenarioUnit));
                    unitId++;
                }
            }

            return armyInfo;
        }

        

    }
}

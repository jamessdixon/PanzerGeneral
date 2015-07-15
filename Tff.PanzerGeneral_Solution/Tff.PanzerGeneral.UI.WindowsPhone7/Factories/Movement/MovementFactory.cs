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
using Tff.Panzer.Models.Geography;
using Tff.Panzer.Models.Army;
using System.Linq;
using Tff.Panzer.Models.Army.Unit;
using Tff.Panzer.Models;
using System.Collections.Generic;
using Tff.Panzer.Models.Movement;

namespace Tff.Panzer.Factories.Movement
{
    public class MovementFactory
    {
        public MovementCostFactory MovementCostFactory { get; private set; }
        public MovementCostModifierFactory MovementCostModifierFactory { get; private set; }
        
        public MovementFactory()
        {
            MovementCostFactory = new MovementCostFactory();
            MovementCostModifierFactory = new MovementCostModifierFactory();
        }

        public int CalculateMovementCost(IUnit unit, Terrain terrain, TerrainCondition terrainCondition)
        {
            int returnValue = 0;
            if (terrain.RiverInd == false && terrain.RoadInd == false)
            {
                MovementCost movementCost = (from mc in MovementCostFactory.MovementCosts
                         where mc.TerrainCondition.TerrainConditionEnum == terrainCondition.TerrainConditionEnum
                         && mc.TerrainType.TerrainTypeEnum == terrain.TerrainTypeEnum
                         && mc.MovementType.MovementTypeEnum == unit.Equipment.MovementType.MovementTypeEnum
                         select mc).FirstOrDefault();

                if (movementCost.MovementPoints == -1)
                {
                    returnValue = 99;
                }
                else
                {
                    returnValue = movementCost.MovementPoints;
                }
            }
            else
            {
                MovementCostModifier movementCostModifier = (from mcm in MovementCostModifierFactory.MovementCostModifiers
                         where mcm.TerrainCondition.TerrainConditionEnum == terrainCondition.TerrainConditionEnum
                         && mcm.MovementType.MovementTypeEnum == unit.Equipment.MovementType.MovementTypeEnum
                         select mcm).FirstOrDefault();
                if (terrain.RoadInd == true)
                {
                    returnValue = movementCostModifier.RoadPoints;
                }
                else
                {
                    returnValue = movementCostModifier.RiverPoints;
                }
            }
            return returnValue;

        }
        
        public MovementAxisEnum DetermineMovementAxis(Tile startTile, Tile endTile)
        {
            int columnDifference = endTile.ColumnNumber - startTile.ColumnNumber;
            int rowDifference = endTile.RowNumber - startTile.RowNumber;

            if (rowDifference > 0 && columnDifference == 0)
            {
                return MovementAxisEnum.North_South;
            }
            else if (rowDifference < 0 && columnDifference == 0)
            {
                return MovementAxisEnum.North_South;
            }
            else if (rowDifference > 0 && columnDifference > 0)
            {
                return MovementAxisEnum.NorthWest_SouthEast;
            }
            else if (rowDifference > 0 && columnDifference < 0)
            {
                return MovementAxisEnum.SouthWest_NorthEast;
            }
            else if (rowDifference < 0 && columnDifference < 0)
            {
                return MovementAxisEnum.NorthWest_SouthEast;
            }
            else if (rowDifference < 0 && columnDifference > 0)
            {
                return MovementAxisEnum.SouthWest_NorthEast;
            }

            return MovementAxisEnum.North_South;
        }

        public CompassPointEnum DetermineMovementDirection(Tile startTile, Tile endTile)
        {
            int columnDifference = endTile.ColumnNumber - startTile.ColumnNumber;
            int rowDifference = endTile.RowNumber - startTile.RowNumber;

            if (rowDifference > 0 && columnDifference == 0)
            {
                return CompassPointEnum.South;
            }
            else if (rowDifference < 0 && columnDifference == 0)
            {
                return CompassPointEnum.North;
            }
            else if (rowDifference > 0 && columnDifference > 0)
            {
                return CompassPointEnum.SouthEast;
            }
            else if (rowDifference > 0 && columnDifference < 0)
            {
                return CompassPointEnum.SouthWest; 
            }
            else if (rowDifference < 0 && columnDifference < 0)
            {
                return CompassPointEnum.NorthWest;
            }
            else if (rowDifference < 0 && columnDifference > 0)
            {
                return CompassPointEnum.NorthEast;
            }

            return CompassPointEnum.North;
        }



        
    }
}

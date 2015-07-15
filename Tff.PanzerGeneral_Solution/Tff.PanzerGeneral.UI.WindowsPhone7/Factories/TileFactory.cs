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
using System.Collections.Generic;
using Tff.Panzer.Models.Scenario;
using System.Linq;
using Tff.Panzer.Models.Army;
using Tff.Panzer.Factories.Army;
using System.Diagnostics;
using Tff.Panzer.Models.Geography;
using Tff.Panzer.Models.Army.Unit;
using Tff.Panzer.Factories.Geography;


namespace Tff.Panzer.Factories
{
    public class TileFactory
    {
        public TileNameFactory TileNameFactory { get; private set; }
        public event EventHandler<EventArgs> TilesLoaded;

        public List<Tile> Tiles { get; set; }

        public TileFactory()
        {
            Tiles = new List<Tile>();
            TileNameFactory = new TileNameFactory();
            Game.ScenarioFactory.ScenarioTileFactory.ScenarioTilesLoaded += new EventHandler<EventArgs>(ScenarioTileFactory_ScenarioTilesLoaded);
        }

        public void PopulateTiles(int scenarioId)
        {
            Tiles.Clear();
            Game.ScenarioFactory.ScenarioTileFactory.PopulateScenarioTiles(scenarioId);
        }

        void ScenarioTileFactory_ScenarioTilesLoaded(object sender, EventArgs e)
        {
            Tile tile = null;
            List<ScenarioTile> scenarioTiles = Game.ScenarioFactory.ScenarioTileFactory.ScenarioTiles;
            foreach (ScenarioTile scenarioTile in scenarioTiles)
            {
                tile = new Tile();
                tile.TileId = scenarioTile.ScenarioTileId;
                tile.ColumnNumber = scenarioTile.ColumnNumber;
                tile.RowNumber = scenarioTile.RowNumber;
                tile.Terrain = Game.TerrainFactory.GetTerrain(scenarioTile.TerrainId);
                tile.Nation = Game.NationFactory.GetNation(scenarioTile.NationId);
                tile.TileName = TileNameFactory.GetTileName(scenarioTile.TileNameId);
                tile.DeployIndicator = scenarioTile.DeployIndicator;
                tile.SupplyIndicator = scenarioTile.SupplyIndicator;
                tile.VictoryIndicator = scenarioTile.VictoryIndicator;
                Tiles.Add(tile);
            }
            AddUnitsToTile();
            TilesLoaded(null, null);
        }

        private void AddUnitsToTile()
        {
            IUnit unit = null;
            int unitId = 1;
            foreach (ScenarioUnit scenarioUnit in Game.ScenarioFactory.ScenarioUnitFactory.GetScenarioUnits(Game.CurrentScenarioId))
            {
                Tile currentTile = Tiles.Where(t => t.TileId == scenarioUnit.StartingScenarioTileId).FirstOrDefault();
                unit = Game.UnitFactory.CreateUnit(unitId, scenarioUnit);

                //TODO: plug for testing reinforcing computer's units
                //if (scenarioUnit.ScenarioUnitId == 12)
                //{
                //    //unit.CurrentStrength = 5;
                //    LandCombatUnit lcu = (LandCombatUnit)unit;
                //    lcu.CurrentAmmo = 0;
                //}
                
                if (unit is AirCombatUnit)
                {
                    currentTile.AirUnit = (AirCombatUnit)unit;
                }
                else if (unit is SeaCombatUnit)
                {
                    currentTile.GroundUnit = (SeaCombatUnit)unit;
                }
                else if (unit is LandCombatUnit)
                {
                    if (currentTile.Terrain.TerrainGroupEnum == TerrainGroupEnum.Sea)
                    {
                        LandCombatUnit landCombatUnit = (LandCombatUnit)unit;
                        if (unit.EquipmentSubClassEnum == EquipmentSubClassEnum.Airborne_RangerInfantry)
                        {
                            AirTransportUnit airTransportUnit = Game.UnitFactory.CreateAirTransportUnit(unitId++, unit.Nation, currentTile.TileId);
                            landCombatUnit.TransportUnit = airTransportUnit;
                            airTransportUnit.LandCombatUnit = landCombatUnit;
                            currentTile.AirUnit = airTransportUnit;
                        }
                        else
                        {
                            SeaTransportUnit seaTransportUnit = Game.UnitFactory.CreateSeaTransportUnit(unitId++, unit.Nation, currentTile.TileId);
                            landCombatUnit.TransportUnit = seaTransportUnit;
                            seaTransportUnit.LandCombatUnit = landCombatUnit;
                            currentTile.GroundUnit = seaTransportUnit;
                        }

                    }
                    else
                    {
                        currentTile.GroundUnit = (LandCombatUnit)unit;
                        //TODO: plug for testing adding computer units
                        //if (currentTile.TileId == 38)
                        //{
                        //    currentTile.GroundUnit = null;
                        //}
                    }

                }
                unitId++;
            }
        }

        public Tile GetTile(int tileId)
        {
            Tile targetTile = (from tile in Game.BoardFactory.Tiles
                               where tile.TileId == tileId
                               select tile).FirstOrDefault();
            if (targetTile != null)
                return targetTile;
            else
                return null;

        }

        public Tile GetTile(int columnNumber, int rowNumber)
        {
            var q = (from Tile t in Game.BoardFactory.Tiles
                     where t.RowNumber == rowNumber &&
                     t.ColumnNumber == columnNumber
                     select t).FirstOrDefault();
            return q;
        }

        public Tile CreateTile(TerrainTypeEnum terrainTypeEnum)
        {
            Tile tile = new Tile();
            tile.TileId = 0;
            tile.ColumnNumber = 0;
            tile.RowNumber = 0;
            tile.Terrain = Game.TerrainFactory.Terrains.Where(t => t.TerrainTypeEnum == terrainTypeEnum).FirstOrDefault();
            tile.Nation = null;
            tile.TileName = TileNameFactory.GetTileName(0);
            tile.DeployIndicator = false;
            tile.SupplyIndicator = false;
            tile.VictoryIndicator = false;
            return tile;
        }

        public void SetVisibleTiles(IUnit unit, Tile tile)
        {
            unit.VisibleTiles = GetAdjacentTiles(tile, unit.Equipment.Spotting, true);
        }

        public void SetMovableTiles(IUnit unit, Tile tile)
        {
            if (unit is IAirUnit)
            {
                IAirUnit airUnit = (IAirUnit)unit;
                unit.MoveableTiles = GetAirMoveableTiles(airUnit, tile);
            }
            else if (unit is ISeaUnit)
            {
                ISeaUnit seaUnit = (ISeaUnit)unit;
                unit.MoveableTiles = GetSeaMoveableTiles(seaUnit, tile);
            }
            else if (unit is ILandUnit)
            {
                ILandUnit landUnit = (ILandUnit)unit;
                unit.MoveableTiles = GetLandMoveableTiles(landUnit, tile);
            }
        }

        public void SetAttackableTiles(ICombatUnit unit, Tile tile)
        {
            if (unit is AirCombatUnit)
            {
                AirCombatUnit airUnit = (AirCombatUnit)unit;
                unit.AttackableTiles = GetAirAttackableTiles(airUnit, tile);
            }
            else if (unit is SeaCombatUnit)
            {
                SeaCombatUnit seaUnit = (SeaCombatUnit)unit;
                unit.AttackableTiles = GetSeaAttackableTiles(seaUnit, tile);
            }
            else if (unit is LandCombatUnit)
            {
                LandCombatUnit landUnit = (LandCombatUnit)unit;
                unit.AttackableTiles = GetLandAttackableTiles(landUnit, tile);
            }
        }

        private List<Tile> GetLandAttackableTiles(LandCombatUnit landUnit, Tile tile)
        {
            List<Tile> tiles = new List<Tile>();
            switch (landUnit.EquipmentClassEnum)
            {
                case EquipmentClassEnum.AirDefense:
                    tiles = GetAdjacentTiles(tile, landUnit.Equipment.Range, true);
                    tiles.Add(tile);
                    tiles = tiles.Where(t => t.AirUnit != null).ToList();
                    tiles = tiles.Where(t => t.AirUnit.SideEnum != landUnit.SideEnum).ToList();
                    break;
                case EquipmentClassEnum.AntiAir:
                    tiles = GetAdjacentTiles(tile, landUnit.Equipment.Range, true);
                    tiles.Add(tile);
                    tiles = tiles.Where(t => t.AirUnit != null).ToList();
                    tiles = tiles.Where(t => t.AirUnit.SideEnum != landUnit.SideEnum).ToList();
                    List<Tile> landTiles = new List<Tile>();
                    landTiles = GetAdjacentTiles(tile, 1, true);
                    landTiles = tiles.Where(t => t.GroundUnit != null).ToList();
                    landTiles = tiles.Where(t => t.GroundUnit.SideEnum != landUnit.SideEnum).ToList();
                    tiles.AddRange(landTiles);
                    break;
                case EquipmentClassEnum.Artillery:
                    tiles = GetAdjacentTiles(tile, landUnit.Equipment.Range, true);
                    tiles = tiles.Where(t => t.GroundUnit != null).ToList();
                    tiles = tiles.Where(t => t.GroundUnit.SideEnum != landUnit.SideEnum).ToList();
                    break;
                default:
                    tiles = GetAdjacentTiles(tile, 1, true);
                    tiles = tiles.Where(t => t.GroundUnit != null).ToList();
                    tiles = tiles.Where(t => t.GroundUnit.SideEnum != landUnit.SideEnum).ToList();
                    break;
            }

            return tiles;

        }

        private List<Tile> GetSeaAttackableTiles(SeaCombatUnit seaUnit, Tile tile)
        {
            List<Tile> tiles = new List<Tile>();
            switch(seaUnit.EquipmentClassEnum)
            {
                case EquipmentClassEnum.Submarine:
                    tiles = GetAdjacentTiles(tile, 1, true);
                    tiles = tiles.Where(t => t.GroundUnit != null).ToList();
                    tiles = tiles.Where(t => t.GroundUnit.SideEnum != seaUnit.SideEnum).ToList();
                    tiles = tiles.Where(t => t.Terrain.TerrainGroupEnum != TerrainGroupEnum.Land).ToList();
                    tiles = tiles.Where(t => t.GroundUnit.EquipmentClassEnum != EquipmentClassEnum.Submarine).ToList();
                    break;
                case EquipmentClassEnum.Destroyer:
                    tiles = GetAdjacentTiles(tile, 1, true);
                    tiles = tiles.Where(t => t.GroundUnit != null).ToList();
                    tiles = tiles.Where(t => t.GroundUnit.SideEnum != seaUnit.SideEnum).ToList();
                    tiles = tiles.Where(t => t.Terrain.TerrainGroupEnum != TerrainGroupEnum.Land).ToList();
                    break;
                case EquipmentClassEnum.CapitalShip:
                    tiles = GetAdjacentTiles(tile, seaUnit.Equipment.Range, true);
                    tiles = tiles.Where(t => t.GroundUnit != null).ToList();
                    tiles = tiles.Where(t => t.GroundUnit.SideEnum != seaUnit.SideEnum).ToList();
                    tiles = tiles.Where(t => t.GroundUnit.EquipmentClassEnum != EquipmentClassEnum.Submarine).ToList();
                    break;
            }
            return tiles;
        }

        private List<Tile> GetAirAttackableTiles(AirCombatUnit airUnit, Tile tile)
        {
            List<Tile> tiles = new List<Tile>();
            
            switch(airUnit.EquipmentClassEnum)
            {
                case EquipmentClassEnum.Fighter:
                    //Air Target
                    tiles = GetAdjacentTiles(tile, 1, true);
                    tiles = tiles.Where(t => t.AirUnit != null).ToList();
                    tiles = tiles.Where(t => t.AirUnit.SideEnum != airUnit.SideEnum).ToList();
                    //Ground Target
                    if (tile.GroundUnit != null)
                    {
                        if (tile.GroundUnit.SideEnum != airUnit.SideEnum)
                        {
                            tiles.Add(tile);
                        }
                    }
                    break;
                case EquipmentClassEnum.TacticalBomber:
                    //Air Target
                    tiles = GetAdjacentTiles(tile, 1, true);
                    tiles = tiles.Where(t => t.AirUnit != null).ToList();
                    tiles = tiles.Where(t => t.AirUnit.SideEnum != airUnit.SideEnum).ToList();
                    //Ground Target
                    if (tile.GroundUnit != null)
                    {
                        if (tile.GroundUnit.SideEnum != airUnit.SideEnum)
                        {
                            tiles.Add(tile);
                        }
                    }
                    break;
                case EquipmentClassEnum.StrategicBomber:
                    //Ground Target
                    if (tile.GroundUnit != null)
                    {
                        if (tile.GroundUnit.SideEnum != airUnit.SideEnum)
                        {
                            tiles.Add(tile);
                        }
                    }
                    //Victory Tile Target
                    if (tile.VictoryIndicator == true)
                    {
                        if (tile.Nation.SideEnum != airUnit.Nation.SideEnum)
                        {
                            tiles.Add(tile);
                        }
                    }
                    break;
            }
            return tiles;
        }

        private List<Tile> GetLandMoveableTiles(ILandUnit unit, Tile tile)
        {
            List<Tile> tiles;
            int movementPoints = GetMaximumMovementPointsForAUnit(unit);
            tiles = GetAdjacentTiles(tile, movementPoints, true);
            tiles = AddMovementPointsToTiles(tiles,unit);
            tiles = CalculateGroundReachableTiles(unit, tile, movementPoints);
            tiles = RemoveUnitsFromLandReachableTiles(tiles);
            return tiles;

        }

        private List<Tile> GetAirMoveableTiles(IAirUnit unit, Tile tile)
        {
            List<Tile> tiles;
            int movementPoints = GetMaximumMovementPointsForAUnit(unit);
            tiles = GetAdjacentTiles(tile, movementPoints, true);
            tiles = RemoveUnitsFromAirReachableTiles(tiles);
            return tiles;
        }

        private List<Tile> GetSeaMoveableTiles(ISeaUnit unit, Tile tile)
        {
            List<Tile> tiles;
            int movementPoints = GetMaximumMovementPointsForAUnit(unit);
            tiles = GetAdjacentTiles(tile, movementPoints, true);
            tiles = AddMovementPointsToTiles(tiles, unit);
            tiles = CalculateGroundReachableTiles(unit, tile, movementPoints);
            tiles = RemoveUnitsFromLandReachableTiles(tiles);
            return tiles;
        }

        private int GetMaximumMovementPointsForAUnit(IUnit unit)
        {
            int unitTotalMovementPoints = unit.Equipment.BaseMovement;
            if(unit.Equipment.MovementType.IsMotorized)
            {
                IMotorizedUnit motorizedUnit = (IMotorizedUnit)unit;
                if (motorizedUnit.CurrentFuel < unit.Equipment.BaseMovement)
                {
                    unitTotalMovementPoints = motorizedUnit.CurrentFuel;
                }
            
            }

            return unitTotalMovementPoints;
        }

        public List<Tile> GetAdjacentTiles(Tile startTile, Int32 maxDepth, bool calculateDepth)
        {
            List<Tile> tiles = new List<Tile>();
            int startingColumn = startTile.ColumnNumber - maxDepth;
            int endingColumn = startTile.ColumnNumber + maxDepth;
            AdjustColumnsForGridSize(ref startingColumn, ref endingColumn);

            int startingRow = startTile.RowNumber - maxDepth;
            int endingRow = startTile.RowNumber + maxDepth;
            AdjustRowsForGridSize(ref startingRow, ref endingRow);

            Tile tile = null;
            int depth = 0;
            for (int i = startingColumn; i <= endingColumn; i++)
            {
                for (int j = startingRow; j <= endingRow; j++)
                {
                    tile = GetTile(i, j);
                    if (!tiles.Contains(tile))
                    {
                        depth = GetDepthBetweenTwoTiles(startTile, tile);
                        if (depth <= maxDepth)
                        {
                            if (calculateDepth)
                            {
                                tile.Depth = depth;
                            }
                            tiles.Add(tile);
                        }
                    }
                }
            }
            tiles.Remove(startTile);

            return tiles;
        }
        
        private List<Tile> AddMovementPointsToTiles(List<Tile> tiles, IUnit unit)
        {
            foreach (Tile tile in tiles)
            {
                tile.MovementCost = Game.MovementFactory.CalculateMovementCost(unit, tile.Terrain, Game.CurrentTurn.CurrentTerrainCondition);

                tile.UnitCanPassThrough = true;
                if (tile.GroundUnit != null)
                {
                    if (tile.Terrain.RiverInd && tile.GroundUnit.SideEnum == unit.SideEnum && tile.GroundUnit.Equipment.CanBridgeRivers && tile.GroundUnit is LandCombatUnit)
                    {
                        tile.MovementCost = 1;
                    }
                }
            }
            return tiles;

        }

        private void AdjustRowsForGridSize(ref int startingRow, ref int endingRow)
        {
            if (startingRow < 0)
            {
                startingRow = 0;
            }
            if (endingRow > Game.CurrentBoard.NumberOfRows)
            {
                endingRow = Game.CurrentBoard.NumberOfRows;
            }
        }

        private void AdjustColumnsForGridSize(ref int startingColumn, ref int endingColumn)
        {
            if (startingColumn < 0)
            {
                startingColumn = 0;
            }
            if (endingColumn > Game.CurrentBoard.NumberOfColumns)
            {
                endingColumn = Game.CurrentBoard.NumberOfColumns;
            }
        }

        private int GetDepthBetweenTwoTiles(Tile startTile, Tile endTile)
        {
            int startColumnNumber = startTile.ColumnNumber;
            int endColumnNumber = endTile.ColumnNumber;
            int startRowNumber = startTile.RowNumber;
            int endRowNumber = endTile.RowNumber;

            startRowNumber = AdjustRowNumberForDiagnalGrid(startColumnNumber, startRowNumber);
            endRowNumber = AdjustRowNumberForDiagnalGrid(endColumnNumber, endRowNumber);

            return CalculateDepthBetweenTwoTiles(startColumnNumber, endColumnNumber, startRowNumber, endRowNumber);


        }

        private int CalculateDepthBetweenTwoTiles(int startColumnNumber, int endColumnNumber, int startRowNumber, int endRowNumber)
        {
             List<Int32> depths = new List<int>();
            int columnDepth = Math.Abs(startColumnNumber - endColumnNumber);
            int rowDepth = Math.Abs(startRowNumber - endRowNumber);
            int cumulativeDepth = Math.Abs((startColumnNumber - endColumnNumber) + (startRowNumber - endRowNumber));
            depths.Add(columnDepth);
            depths.Add(rowDepth);
            depths.Add(cumulativeDepth);

            var q = (from d in depths
                     select d).Max();
            return q;
        }

        private int AdjustRowNumberForDiagnalGrid(int columnNumber, int rowNumber)
        {
            int rowAdjustment = (Int32)columnNumber / 2;
            return rowNumber -= rowAdjustment;
        }

        private int AdjustRowNumberForStandardGrid(int columnNumber, int rowNumber)
        {
            int rowAdjustment = (Int32)columnNumber / 2;
            return rowNumber += rowAdjustment;
        }

        private List<Tile> CalculateGroundReachableTiles(IGroundUnit unit, Tile startTile, int movementPoints)
        {
            List<Tile> reachableTiles = new List<Tile>();
            List<Tile> tilesForEvaluation = new List<Tile>();
            List<Tile> outsideTiles = new List<Tile>();
            List<Tile> adjacentTiles = GetAdjacentTiles(startTile, 1, false);
            List<Tile> currentTileAdjacentTiles = null;
            List<Tile> currentTileOtherUnits = null;
            List<Tile> otherUnitAdjactentTiles = null;
            //Level 1
            foreach (Tile tile in adjacentTiles)
            {
                if (tile.MovementCost <= movementPoints)
                {
                    tile.Distance = tile.MovementCost;
                    reachableTiles.Add(tile);
                }
                if (tile.MovementCost == 99)
                {
                    tile.UnitCanPassThrough = false;
                    tile.Distance = tile.MovementCost;
                    reachableTiles.Add(tile);
                }
            }
            //Enemy ZOC
            currentTileOtherUnits = adjacentTiles.Where(t => t.GroundUnit != null).ToList();
            currentTileOtherUnits = currentTileOtherUnits.Where(t => t.GroundUnit.SideEnum != unit.SideEnum).ToList();
            foreach (Tile tile in currentTileOtherUnits)
            {
                otherUnitAdjactentTiles = GetAdjacentTiles(tile, 1, true);
                if (tile.GroundUnit == null)
                {
                    tile.UnitCanPassThrough = false;
                }
            }

            //Level 2-n
            for (int i = 1; i < movementPoints; i++)
            {
                tilesForEvaluation.Clear();
                tilesForEvaluation.AddRange(reachableTiles.Where(t => t.Depth == i && t.UnitCanPassThrough));
                foreach (Tile tile in tilesForEvaluation)
                {
                    outsideTiles.Clear();
                    adjacentTiles = GetAdjacentTiles(tile, 1, false);
                    outsideTiles.AddRange(adjacentTiles.Where(t => t.Depth == i+1));
                    foreach (Tile currentTile in outsideTiles)
                    {
                        int cumulativeMovementCost = tile.Distance;
                        currentTileAdjacentTiles = GetAdjacentTiles(currentTile, 1, false);
                        currentTileOtherUnits = currentTileAdjacentTiles.Where(t => t.GroundUnit != null).ToList();
                        foreach (Tile otherUnitTile in currentTileOtherUnits)
                        {
                            if (unit.SideEnum != otherUnitTile.GroundUnit.SideEnum)
                            {
                                currentTile.UnitCanPassThrough = false;
                            }
                        }

                        if (!reachableTiles.Contains(currentTile))
                        {
                            if (currentTile.MovementCost + cumulativeMovementCost <= movementPoints)
                            {
                                currentTile.Distance = tile.MovementCost + cumulativeMovementCost;
                                reachableTiles.Add(currentTile);
                            }
                        }
                    }
                }
            }
            return reachableTiles;

        }

        private List<Tile> RemoveUnitsFromLandReachableTiles(List<Tile> tiles)
        {
            return tiles.Where(t => t.GroundUnit == null).ToList();
        }

        private List<Tile> RemoveUnitsFromAirReachableTiles(List<Tile> tiles)
        {
            return tiles.Where(t => t.AirUnit == null).ToList();
        }

        public Tile DetermineRetreatTile(Tile startTile, Tile endTile)
        {
            Tile retreatTile = null;
            Tile tile = null;
            List<Tile> adjacentTiles = GetAdjacentTiles(endTile, 1, true);
            List<Int32> retreatPaths = new List<int>();

            int attackPathNumber = 0;
            for (int i = 0; i < adjacentTiles.Count; i++)
            {
                tile = adjacentTiles[i];
                if (tile == startTile)
                {
                    attackPathNumber = i;
                }
            }

            switch (attackPathNumber)
            {
                case 0:
                    int[] path0 = new int[] { 3, 2, 4, 1, 5 };
                    retreatPaths.AddRange(path0);
                    break;
                case 1:
                    int[] path1 = new int[] { 4, 3, 5, 2, 0 };
                    retreatPaths.AddRange(path1);
                    break;
                case 2:
                    int[] path2 = new int[] { 5, 0, 4, 1, 3 };
                    retreatPaths.AddRange(path2);
                    break;
                case 3:
                    int[] path3 = new int[] { 0, 1, 5, 2, 4 };
                    retreatPaths.AddRange(path3);
                    break;
                case 4:
                    int[] path4 = new int[] { 1, 2, 0, 3, 5 };
                    retreatPaths.AddRange(path4);
                    break;
                case 5:
                    int[] path5 = new int[] { 2, 1, 3, 0, 4 };
                    retreatPaths.AddRange(path5);
                    break;
            }

            foreach (Int32 retreatHexNumber in retreatPaths)
            {
                if (adjacentTiles[retreatHexNumber].GroundUnit == null)
                    retreatTile = adjacentTiles[retreatHexNumber];
                break;
            }

            return retreatTile;
        }

    }
}

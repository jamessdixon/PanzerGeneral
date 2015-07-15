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
using Tff.Panzer.Models.Army;
using Tff.Panzer.Controls;
using System.Linq;
using System.Collections.Generic;
using Tff.Panzer.Models.Army.Unit;
using Tff.Panzer.Models;
using Tff.Panzer.Factories.Army;
using Tff.Panzer.Models.VictoryCondition;
using Tff.Panzer.Models.Geography;

namespace Tff.Panzer.Factories
{
    public class ComputerPlayerFactory
    {
        public static Int32 NumberOfUnitsInArmy { get; set; }

        public void CalculateTurn()
        {
            List<AirCombatUnit> airCombatUnits = GetAirCombatUnits();
            airCombatUnits = ReinforceAirCombatUnits(airCombatUnits);
            airCombatUnits = ResupplyAirCombatUnits(airCombatUnits);
            airCombatUnits = MoveAirCombatUnits(airCombatUnits);
            airCombatUnits = AttackWithAirCombatUnits(airCombatUnits);

            List<SeaCombatUnit> seaCombatUnits = GetSeaCombatUnits();
            seaCombatUnits = AttackWithSeaCombatUnits(seaCombatUnits);
            seaCombatUnits = MoveSeaCombatUnits(seaCombatUnits);

            List<LandCombatUnit> landCombatUnits = GetLandCombatUnits();
            landCombatUnits = ReinforceLandCombatUnits(landCombatUnits);
            landCombatUnits = ResupplyLandCombatUnits(landCombatUnits);
            landCombatUnits = AttackWithLandCombatUnits(landCombatUnits);
            landCombatUnits = MoveLandCombatUnits(landCombatUnits);
            ProtectVictoryTiles();
            ProtectSupplyTiles();
        }

        private List<AirCombatUnit> GetAirCombatUnits()
        {
            List<AirCombatUnit> airCombatUnits = new List<AirCombatUnit>();
            foreach (Tile tile in Game.BoardFactory.Tiles)
            {
                if (tile.AirUnit != null)
                {
                    if (tile.AirUnit.SideEnum == SideEnum.Allies && tile.AirUnit is AirCombatUnit)
                    {
                        airCombatUnits.Add((AirCombatUnit)tile.AirUnit);
                    }
                }
            }
            NumberOfUnitsInArmy = airCombatUnits.Count;
            return airCombatUnits;
        }

        private List<AirCombatUnit> ResupplyAirCombatUnits(List<AirCombatUnit> airCombatUnits)
        {
            airCombatUnits = airCombatUnits.Where(lcu => lcu.CanAttack || lcu.CanMove).ToList();
            foreach (AirCombatUnit airCombatUnit in airCombatUnits)
            {
                Tile currentTile = Game.BoardFactory.Tiles.Where(t => t.AirUnit == airCombatUnit).FirstOrDefault();
                if (airCombatUnit.CurrentAmmo < 2 && currentTile.Terrain.TerrainTypeEnum == TerrainTypeEnum.Airfield)
                {
                    Game.UnitFactory.ResupplyUnit(airCombatUnit);
                    airCombatUnit.CanAttack = false;
                    airCombatUnit.CanMove = false;
                }
                if (airCombatUnit.CurrentFuel <= 5 && currentTile.Terrain.TerrainTypeEnum == TerrainTypeEnum.Airfield)
                {
                    Game.UnitFactory.ResupplyUnit(airCombatUnit);
                    airCombatUnit.CanAttack = false;
                    airCombatUnit.CanMove = false;
                }
            }

            return airCombatUnits;
        
        }

        private List<AirCombatUnit> ReinforceAirCombatUnits(List<AirCombatUnit> airCombatUnits)
        {
            airCombatUnits = airCombatUnits.Where(lcu => lcu.CanAttack || lcu.CanMove).ToList();

            foreach (AirCombatUnit airCombatUnit in airCombatUnits)
            {
                Tile currentTile = Game.BoardFactory.Tiles.Where(t => t.AirUnit == airCombatUnit).FirstOrDefault();
                if (airCombatUnit.CurrentStrength < 10 && Game.CurrentTurn.CurrentAlliedPrestige > 0 && currentTile.Terrain.TerrainTypeEnum == TerrainTypeEnum.Airfield)
                {
                    Game.UnitFactory.ReinforceUnit(airCombatUnit, false);
                    airCombatUnit.CanAttack = false;
                    airCombatUnit.CanMove = false;
                }
            }

            return airCombatUnits;
        }

        private List<AirCombatUnit> MoveAirCombatUnits(List<AirCombatUnit> airCombatUnits)
        {
            airCombatUnits = airCombatUnits.Where(lcu => lcu.CanAttack || lcu.CanMove).ToList();
            foreach (AirCombatUnit airCombatUnit in airCombatUnits)
            {
                Tile currentTile = Game.BoardFactory.Tiles.Where(t => t.AirUnit == airCombatUnit).FirstOrDefault();
                Game.TileFactory.SetMovableTiles(airCombatUnit, currentTile);
                if (airCombatUnit.MoveableTiles.Count > 0)
                {
                    MoveAirCombatUnit(airCombatUnit, currentTile);
                }
            }
            return airCombatUnits;
        }

        private static void MoveAirCombatUnit(AirCombatUnit airCombatUnit, Tile currentTile)
        {
            Tile targetTile = null;
            TargetAndJumpOffTile targetAndJumpOffTile = null;
            targetAndJumpOffTile = DetermineAirAttackJumpOffTile(airCombatUnit, currentTile);
            if (airCombatUnit.CanAttack && targetAndJumpOffTile != null && airCombatUnit.CurrentFuel >= airCombatUnit.Equipment.MaxFuel*.5)
            {
                Game.BoardFactory.ActiveTile = currentTile;
                Game.BoardFactory.ActiveUnit = airCombatUnit;
                Game.BoardFactory.MoveUnitForComputerPlayer(airCombatUnit, currentTile, targetAndJumpOffTile.JumpOffTile);
                Game.BoardFactory.ActiveTile = targetAndJumpOffTile.JumpOffTile;
                Game.BoardFactory.CalculateBattleForComputerPlayer(targetAndJumpOffTile.TargetTile);
                return;
            }
            targetTile = DetermineAirWithdrawlTile(airCombatUnit);
            if (targetTile != null)
            {
                Game.BoardFactory.ActiveTile = currentTile;
                Game.BoardFactory.ActiveUnit = airCombatUnit;
                Game.BoardFactory.MoveUnitForComputerPlayer(airCombatUnit, currentTile, targetTile);
                return;
            }

        }

        private static Tile DetermineAirWithdrawlTile(AirCombatUnit airCombatUnit)
        {
            foreach (Tile tile in airCombatUnit.MoveableTiles)
            {
                if(tile.Terrain.TerrainTypeEnum == TerrainTypeEnum.Airfield && tile.Nation.SideEnum == SideEnum.Allies)
                {
                    return tile;
                }
            }
            return null;
        }

        private static TargetAndJumpOffTile DetermineAirAttackJumpOffTile(AirCombatUnit airCombatUnit, Tile currentTile)
        {
            List<Tile> adjacentTiles = null;
            List<TargetAndJumpOffTile> targetAndJumpOffTiles = new List<TargetAndJumpOffTile>();
            TargetAndJumpOffTile targetAndJumpOffTile = null;

            if (airCombatUnit.EquipmentClassEnum == EquipmentClassEnum.Fighter)
            {
                foreach (Tile tile in airCombatUnit.MoveableTiles)
                {
                    adjacentTiles = Game.TileFactory.GetAdjacentTiles(tile, 1, false);
                    adjacentTiles = adjacentTiles.Where(t => t.AirUnit != null).ToList();
                    adjacentTiles = adjacentTiles.Where(t => t.AirUnit.SideEnum == SideEnum.Axis).ToList();
                    foreach (Tile potentialTargetTile in adjacentTiles)
                    {
                        targetAndJumpOffTile = new TargetAndJumpOffTile();
                        targetAndJumpOffTile.TargetTile = potentialTargetTile;
                        targetAndJumpOffTile.JumpOffTile = tile;
                        targetAndJumpOffTiles.Add(targetAndJumpOffTile);
                    }
                }
                return DetermineTarget(targetAndJumpOffTiles, TargetTypeEnum.Air);
            }
            else if(airCombatUnit.EquipmentClassEnum == EquipmentClassEnum.TacticalBomber)
            {
                foreach (Tile tile in airCombatUnit.MoveableTiles)
                {
                    if (tile.GroundUnit != null)
                    {
                        if (tile.GroundUnit.SideEnum == SideEnum.Axis)
                        {
                            targetAndJumpOffTile = new TargetAndJumpOffTile();
                            targetAndJumpOffTile.TargetTile = tile;
                            targetAndJumpOffTile.JumpOffTile = tile;
                            targetAndJumpOffTiles.Add(targetAndJumpOffTile);
                        }
                    }
                }

            }
            else if (airCombatUnit.EquipmentClassEnum == EquipmentClassEnum.StrategicBomber)
            {
                foreach (Tile tile in airCombatUnit.MoveableTiles)
                {
                    if (tile.Nation.SideEnum == SideEnum.Axis && tile.VictoryIndicator == true)
                    {
                        targetAndJumpOffTile = new TargetAndJumpOffTile();
                        targetAndJumpOffTile.TargetTile = tile;
                        targetAndJumpOffTile.JumpOffTile = tile;
                        targetAndJumpOffTiles.Add(targetAndJumpOffTile);
                    }
                }
            }

            return null;
        }

        private List<AirCombatUnit> AttackWithAirCombatUnits(List<AirCombatUnit> airCombatUnits)
        {
            return null;  
        }

        private static TargetAndJumpOffTile DetermineLandAttackJumpOffTile(AirCombatUnit airCombatUnit, Tile currentTile)
        {
            List<Tile> adjacentTiles = null;
            List<TargetAndJumpOffTile> targetAndJumpOffTiles = new List<TargetAndJumpOffTile>();
            TargetAndJumpOffTile targetAndJumpOffTile = null;

            foreach (Tile tile in airCombatUnit.MoveableTiles)
            {
                adjacentTiles = Game.TileFactory.GetAdjacentTiles(tile, 1, false);
                adjacentTiles = adjacentTiles.Where(t => t.GroundUnit != null).ToList();
                adjacentTiles = adjacentTiles.Where(t => t.GroundUnit.SideEnum == SideEnum.Axis).ToList();
                foreach (Tile potentialTargetTile in adjacentTiles)
                {
                    targetAndJumpOffTile = new TargetAndJumpOffTile();
                    targetAndJumpOffTile.TargetTile = potentialTargetTile;
                    targetAndJumpOffTile.JumpOffTile = tile;
                    targetAndJumpOffTiles.Add(targetAndJumpOffTile);
                }
            }

            return DetermineTarget(targetAndJumpOffTiles, TargetTypeEnum.SoftGround);
        }

        private List<SeaCombatUnit> AttackWithSeaCombatUnits(List<SeaCombatUnit> seaCombatUnits)
        {
            seaCombatUnits = seaCombatUnits.Where(lcu => lcu.CanAttack || lcu.CanMove).ToList();
            foreach (SeaCombatUnit seaCombatUnit in seaCombatUnits)
            {
                Tile currentTile = Game.BoardFactory.Tiles.Where(t => t.GroundUnit == seaCombatUnit).FirstOrDefault();
                Game.TileFactory.SetAttackableTiles(seaCombatUnit, currentTile);
                if (seaCombatUnit.AttackableTiles.Count > 0)
                {
                    Tile targetTile = DetermineTarget(seaCombatUnit.AttackableTiles, TargetTypeEnum.Sea);
                    Game.BoardFactory.ActiveTile = currentTile;
                    Game.BoardFactory.ActiveUnit = seaCombatUnit;
                    Game.BoardFactory.CalculateBattleForComputerPlayer(targetTile);

                }
            }
            return seaCombatUnits;


        }

        private List<SeaCombatUnit> MoveSeaCombatUnits(List<SeaCombatUnit> seaCombatUnits)
        {
            seaCombatUnits = seaCombatUnits.Where(lcu => lcu.CanMove).ToList();
            foreach (SeaCombatUnit seaCombatUnit in seaCombatUnits)
            {
                Tile currentTile = Game.BoardFactory.Tiles.Where(t => t.GroundUnit == seaCombatUnit).FirstOrDefault();
                Game.TileFactory.SetMovableTiles(seaCombatUnit, currentTile);
                if (seaCombatUnit.MoveableTiles.Count > 0)
                {
                    MoveSeaCombatUnit(seaCombatUnit, currentTile);
                }
            }
            return seaCombatUnits;            
        }

        private void MoveSeaCombatUnit(SeaCombatUnit seaCombatUnit, Tile currentTile)
        {
            Tile targetTile = null;
            TargetAndJumpOffTile targetAndJumpOffTile = null;
            targetAndJumpOffTile = DetermineSeaAttackJumpOffTile(seaCombatUnit, currentTile);
            if (seaCombatUnit.CanAttack && targetAndJumpOffTile != null)
            {
                Game.BoardFactory.ActiveTile = currentTile;
                Game.BoardFactory.ActiveUnit = seaCombatUnit;
                Game.BoardFactory.MoveUnitForComputerPlayer(seaCombatUnit, currentTile, targetAndJumpOffTile.JumpOffTile);
                Game.BoardFactory.ActiveTile = targetAndJumpOffTile.JumpOffTile;
                if (seaCombatUnit.CanAttack)
                {
                    Game.BoardFactory.CalculateBattleForComputerPlayer(targetAndJumpOffTile.TargetTile);
                }
                return;
            }
            targetTile = DetermineSeaWithdrawlTile(seaCombatUnit);
            if (targetTile != null)
            {
                Game.BoardFactory.ActiveTile = currentTile;
                Game.BoardFactory.ActiveUnit = seaCombatUnit;
                Game.BoardFactory.MoveUnitForComputerPlayer(seaCombatUnit, currentTile, targetTile);
                return;
            }


        }

        private Tile DetermineSeaWithdrawlTile(SeaCombatUnit seaCombatUnit)
        {
            Tile targetTile = null;
            List<Tile> adjacentTiles = null;

            foreach (Tile tile in seaCombatUnit.MoveableTiles)
            {
                adjacentTiles = Game.TileFactory.GetAdjacentTiles(tile, 1, false);
                adjacentTiles = adjacentTiles.Where(t => t.VictoryIndicator).ToList();
                targetTile = adjacentTiles.Where(t => t.GroundUnit.SideEnum == SideEnum.Allies).FirstOrDefault();
                if (targetTile != null)
                {
                    return targetTile;
                }
            }
            return null;            
        }

        private TargetAndJumpOffTile DetermineSeaAttackJumpOffTile(SeaCombatUnit seaCombatUnit, Tile currentTile)
        {
            List<Tile> adjacentTiles = null;
            List<TargetAndJumpOffTile> targetAndJumpOffTiles = new List<TargetAndJumpOffTile>();
            TargetAndJumpOffTile targetAndJumpOffTile = null;

            foreach (Tile tile in seaCombatUnit.MoveableTiles)
            {
                Game.TileFactory.SetAttackableTiles(seaCombatUnit, tile);
                adjacentTiles = seaCombatUnit.AttackableTiles;
                adjacentTiles = adjacentTiles.Where(t => t.GroundUnit != null).ToList();
                adjacentTiles = adjacentTiles.Where(t => t.GroundUnit.SideEnum == SideEnum.Axis).ToList();
                foreach (Tile potentialTargetTile in adjacentTiles)
                {
                    targetAndJumpOffTile = new TargetAndJumpOffTile();
                    targetAndJumpOffTile.TargetTile = potentialTargetTile;
                    targetAndJumpOffTile.JumpOffTile = tile;
                    targetAndJumpOffTiles.Add(targetAndJumpOffTile);
                }
            }

            return DetermineTarget(targetAndJumpOffTiles, TargetTypeEnum.SoftGround);
        }

        private List<SeaCombatUnit> GetSeaCombatUnits()
        {
            List<SeaCombatUnit> seaCombatUnits = new List<SeaCombatUnit>();
            foreach (Tile tile in Game.BoardFactory.Tiles)
            {
                if (tile.GroundUnit != null)
                {
                    if (tile.GroundUnit.SideEnum == SideEnum.Allies && tile.GroundUnit is SeaCombatUnit)
                    {
                        seaCombatUnits.Add((SeaCombatUnit)tile.GroundUnit);
                    }
                }
            }
            NumberOfUnitsInArmy = seaCombatUnits.Count;
            return seaCombatUnits;
        }

        private static List<LandCombatUnit> MoveLandCombatUnits(List<LandCombatUnit> landCombatUnits)
        {
            landCombatUnits = landCombatUnits.Where(lcu => lcu.CanMove).ToList();
            foreach (LandCombatUnit landCombatUnit in landCombatUnits)
            {
                Tile currentTile = Game.BoardFactory.Tiles.Where(t => t.GroundUnit == landCombatUnit).FirstOrDefault();
                if (currentTile != null)
                {
                    if (currentTile.VictoryIndicator != true && currentTile.SupplyIndicator != true && currentTile.Terrain.TerrainTypeEnum != TerrainTypeEnum.Fortification)
                    {
                        Game.TileFactory.SetMovableTiles(landCombatUnit, currentTile);
                        if (landCombatUnit.MoveableTiles.Count > 0)
                        {
                            MoveLandCombatUnit(landCombatUnit, currentTile);
                        }
                    }
                }
            }
            return landCombatUnits;

        }

        private static void MoveLandCombatUnit(LandCombatUnit landCombatUnit, Tile currentTile)
        {
            Tile targetTile = null;
            TargetAndJumpOffTile targetAndJumpOffTile = null;
            targetTile = DetermineCaptureTile(landCombatUnit);
            if (targetTile != null)
            {
                Game.BoardFactory.ActiveTile = currentTile;
                Game.BoardFactory.ActiveUnit = landCombatUnit;
                Game.BoardFactory.MoveUnitForComputerPlayer(landCombatUnit, currentTile, targetTile);
                return;
            }
            targetAndJumpOffTile = DetermineLandAttackJumpOffTile(landCombatUnit, currentTile);
            if (landCombatUnit.CanAttack && targetAndJumpOffTile != null)
            {
                Game.BoardFactory.ActiveTile = currentTile;
                Game.BoardFactory.ActiveUnit = landCombatUnit;
                Game.BoardFactory.MoveUnitForComputerPlayer(landCombatUnit, currentTile, targetAndJumpOffTile.JumpOffTile);
                Game.BoardFactory.ActiveTile = targetAndJumpOffTile.JumpOffTile;
                if (landCombatUnit.CanAttack)
                {
                    Game.BoardFactory.CalculateBattleForComputerPlayer(targetAndJumpOffTile.TargetTile);
                }
                return;
            }
            targetTile = DetermineLandWithdrawlTile(landCombatUnit);
            if(targetTile != null)
            {
                Game.BoardFactory.ActiveTile = currentTile;
                Game.BoardFactory.ActiveUnit = landCombatUnit;
                Game.BoardFactory.MoveUnitForComputerPlayer(landCombatUnit, currentTile, targetTile);
                return;
            }
        }

        private static Tile DetermineLandWithdrawlTile(LandCombatUnit landCombatUnit)
        {
            Tile targetTile = null;
            List<Tile> adjacentTiles = null;

            foreach (Tile tile in landCombatUnit.MoveableTiles)
            {
                adjacentTiles = Game.TileFactory.GetAdjacentTiles(tile, 1, false);
                adjacentTiles = adjacentTiles.Where(t => t.VictoryIndicator).ToList();
                targetTile = adjacentTiles.Where(t => t.GroundUnit.SideEnum == SideEnum.Allies).FirstOrDefault();
                if (targetTile != null)
                {
                    return targetTile;
                }
            }
            return null;

        }

        private static TargetAndJumpOffTile DetermineLandAttackJumpOffTile(LandCombatUnit landCombatUnit, Tile currentTile)
        {
            List<Tile> adjacentTiles = null;
            List<TargetAndJumpOffTile> targetAndJumpOffTiles = new List<TargetAndJumpOffTile>();
            TargetAndJumpOffTile targetAndJumpOffTile = null;

            foreach (Tile tile in landCombatUnit.MoveableTiles)
            {
                adjacentTiles = Game.TileFactory.GetAdjacentTiles(tile, 1, false);
                adjacentTiles = adjacentTiles.Where(t => t.GroundUnit != null).ToList();
                adjacentTiles = adjacentTiles.Where(t => t.GroundUnit.SideEnum == SideEnum.Axis).ToList();
                foreach (Tile potentialTargetTile in adjacentTiles)
                {
                    targetAndJumpOffTile = new TargetAndJumpOffTile();
                    targetAndJumpOffTile.TargetTile = potentialTargetTile;
                    targetAndJumpOffTile.JumpOffTile = tile;
                    targetAndJumpOffTiles.Add(targetAndJumpOffTile);
                }
            }

            return DetermineTarget(targetAndJumpOffTiles, TargetTypeEnum.SoftGround);
        }

        private static Tile DetermineCaptureTile(LandCombatUnit landCombatUnit)
        {
            if(!landCombatUnit.CanCaptureHexes)
            {
                return null;
            }

            Tile targetTile = null;
            foreach (Tile tile in landCombatUnit.MoveableTiles)
            {
                if (tile.VictoryIndicator == true && tile.GroundUnit == null)
                    return targetTile;
                if (tile.SupplyIndicator == true && tile.GroundUnit == null)
                    return targetTile;
            }
            return null;
        }

        private static void ProtectSupplyTiles()
        {
            foreach (Tile tile in Game.BoardFactory.Tiles.Where(t => t.SupplyIndicator == true))
            {
                if (tile.Nation.SideEnum == SideEnum.Allies && tile.GroundUnit == null)
                {
                    LandCombatUnit unitToBePurchased = Game.UnitFactory.CreateDefaultAntiTankUnit(-1, tile.Nation, tile.TileId);
                    if (unitToBePurchased.Equipment.UnitCost < Game.CurrentTurn.CurrentAlliedPrestige)
                    {
                        unitToBePurchased.UnitId = NumberOfUnitsInArmy;
                        Game.BoardFactory.AddNewUnitForComputerPlayer(unitToBePurchased, tile);
                        NumberOfUnitsInArmy++;
                    }
                }
            }

        }

        private static void ProtectVictoryTiles()
        {
            foreach (Tile tile in Game.BoardFactory.Tiles.Where(t => t.VictoryIndicator == true))
            {
                if (tile.Nation.SideEnum == SideEnum.Allies && tile.GroundUnit == null)
                {
                    LandCombatUnit unitToBePurchased = Game.UnitFactory.CreateDefaultAntiTankUnit(-1, tile.Nation, tile.TileId);
                    if (unitToBePurchased.Equipment.UnitCost < Game.CurrentTurn.CurrentAlliedPrestige)
                    {
                        unitToBePurchased.UnitId = NumberOfUnitsInArmy;
                        Game.BoardFactory.AddNewUnitForComputerPlayer(unitToBePurchased, tile);
                        NumberOfUnitsInArmy++;
                    }
                }
            }
        }

        private static List<LandCombatUnit> AttackWithLandCombatUnits(List<LandCombatUnit> landCombatUnits)
        {
            landCombatUnits = landCombatUnits.Where(lcu => lcu.CanAttack || lcu.CanMove).ToList();
            foreach (LandCombatUnit landCombatUnit in landCombatUnits)
            {
                Tile currentTile = Game.BoardFactory.Tiles.Where(t => t.GroundUnit == landCombatUnit).FirstOrDefault();
                Game.TileFactory.SetAttackableTiles(landCombatUnit, currentTile);
                if (landCombatUnit.AttackableTiles.Count > 0)
                {
                    Tile targetTile = null;
                    if (landCombatUnit.EquipmentClassEnum == EquipmentClassEnum.AntiAir)
                    {
                        targetTile = DetermineTarget(landCombatUnit.AttackableTiles,TargetTypeEnum.Air);

                    }
                    else
                    {
                        targetTile = DetermineTarget(landCombatUnit.AttackableTiles, TargetTypeEnum.SoftGround);
                    }

                    Game.BoardFactory.ActiveTile = currentTile;
                    Game.BoardFactory.ActiveUnit = landCombatUnit;
                    Game.BoardFactory.CalculateBattleForComputerPlayer(targetTile);

                }
            }
            return landCombatUnits;
        }

        private static List<LandCombatUnit> ResupplyLandCombatUnits(List<LandCombatUnit> landCombatUnits)
        {
            landCombatUnits = landCombatUnits.Where(lcu => lcu.CanAttack || lcu.CanMove).ToList();
            foreach (LandCombatUnit landCombatUnit in landCombatUnits)
            {
                if (landCombatUnit.CurrentAmmo < 2)
                {
                    Game.UnitFactory.ResupplyUnit(landCombatUnit);
                    landCombatUnit.CanAttack = false;
                    landCombatUnit.CanMove = false;
                }
            }

            landCombatUnits = landCombatUnits.Where(lcu => lcu.CanAttack || lcu.CanMove).ToList();
            foreach (LandCombatUnit landCombatUnit in landCombatUnits)
            {
                if (landCombatUnit is MotorizedLandCombatUnit)
                {
                    MotorizedLandCombatUnit motorizedLandCombatUnit = (MotorizedLandCombatUnit)landCombatUnit;
                    if (motorizedLandCombatUnit.CurrentFuel <= 5)
                    {
                        Game.UnitFactory.ResupplyUnit(landCombatUnit);
                        landCombatUnit.CanAttack = false;
                        landCombatUnit.CanMove = false;
                    }
                }
            }
            return landCombatUnits;
        }

        private static List<LandCombatUnit> ReinforceLandCombatUnits(List<LandCombatUnit> landCombatUnits)
        {
            landCombatUnits = landCombatUnits.Where(lcu => lcu.CanAttack || lcu.CanMove).ToList();

            foreach (LandCombatUnit landCombatUnit in landCombatUnits)
            {
                if (landCombatUnit.CurrentStrength < 10 && Game.CurrentTurn.CurrentAlliedPrestige > 0)
                {
                    Game.UnitFactory.ReinforceUnit(landCombatUnit, false);
                    landCombatUnit.CanAttack = false;
                    landCombatUnit.CanMove = false;
                }
            }

            return landCombatUnits;
        }

        private static List<LandCombatUnit> GetLandCombatUnits()
        {
            List<LandCombatUnit> landCombatUnits = new List<LandCombatUnit>();
            foreach (Tile tile in Game.BoardFactory.Tiles)
            {
                if (tile.GroundUnit != null)
                {
                    if (tile.GroundUnit.SideEnum == SideEnum.Allies && tile.GroundUnit is LandCombatUnit)
                    {
                        landCombatUnits.Add((LandCombatUnit)tile.GroundUnit);
                    }
                }
            }
            NumberOfUnitsInArmy = landCombatUnits.Count;
            return landCombatUnits;
        }

        private static Tile DetermineTarget(List<Tile> tiles, TargetTypeEnum targetTypeEnum)
        {
            Tile targetTile = null;
            foreach (Tile tile in tiles)
            {
                if (targetTile == null)
                {
                    targetTile = tile;
                }
                else
                {
                    if (targetTypeEnum == TargetTypeEnum.Air)
                    {
                        if (tile.AirUnit is ITransportUnit)
                        {
                            return tile;
                        }
                        else
                        {
                            if (tile.AirUnit.CurrentStrength < targetTile.AirUnit.CurrentStrength)
                            {
                                targetTile = tile;
                            }
                        }

                    }
                    else
                    {
                        if (tile.GroundUnit is ITransportUnit)
                        {
                            return tile;
                        }
                        else
                        {
                            if (tile.GroundUnit.CurrentStrength < targetTile.GroundUnit.CurrentStrength)
                            {
                                targetTile = tile;
                            }
                        }

                    }
                }
            }
            return targetTile;


        }

        private static TargetAndJumpOffTile DetermineTarget(List<TargetAndJumpOffTile> targetAndJumpOffTiles, TargetTypeEnum targetTypeEnum)
        {
            TargetAndJumpOffTile returnValue = null;
            foreach (TargetAndJumpOffTile targetAndJumpOffTile in targetAndJumpOffTiles)
            {
                if (returnValue == null)
                {
                    returnValue = targetAndJumpOffTile;
                }
                else
                {
                    if (targetTypeEnum == TargetTypeEnum.Air)
                    {
                        if (targetAndJumpOffTile.TargetTile.AirUnit is ITransportUnit)
                        {
                            return targetAndJumpOffTile;
                        }
                        else
                        {
                            if (targetAndJumpOffTile.TargetTile.AirUnit.CurrentStrength < returnValue.TargetTile.AirUnit.CurrentStrength)
                            {
                                returnValue = targetAndJumpOffTile;
                            }
                        }

                    }
                    else
                    {
                        if (targetAndJumpOffTile.TargetTile.GroundUnit is ITransportUnit)
                        {
                            return targetAndJumpOffTile;
                        }
                        else
                        {
                            if (targetAndJumpOffTile.TargetTile.GroundUnit.CurrentStrength < returnValue.TargetTile.GroundUnit.CurrentStrength)
                            {
                                returnValue = targetAndJumpOffTile;
                            }
                        }

                    }
                }
            }
            return returnValue;


        }

        private class TargetAndJumpOffTile
        {
            public Tile TargetTile { get; set; }
            public Tile JumpOffTile { get; set; }
        }
    
    
    }
}

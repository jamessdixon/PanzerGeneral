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
using Tff.Panzer.Factories.Scenario;
using Tff.Panzer.Models.Scenario;
using Tff.Panzer.Models.Army;
using Tff.Panzer.Models;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Shell;
using Tff.Panzer.Models.Army.Unit;
using Tff.Panzer.Factories.Battle;
using Tff.Panzer.Models.Battle;
using System.ComponentModel;
using System.Threading;


namespace Tff.Panzer.Factories
{
    public class BoardFactory
    {
        public Board Board { get; private set; }

        public List<Tile> Tiles { get; set; }
        public List<Hex> Hexes { get; set; }
        public Tile ActiveTile { get; set; }
        public IUnit ActiveUnit { get; set; }
        private BackgroundWorker backgroundWorker = null;
        private BattleOutput currentBattleOutput = null;
        public event EventHandler<EventArgs> BoardLoaded;

        public BoardFactory()
        {
            Board = new Board();
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            Game.TileFactory.TilesLoaded += new EventHandler<EventArgs>(TileFactory_TilesLoaded);
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BattleOutput battleOutput = currentBattleOutput;

            switch (battleOutput.BattleOutcomeEnum)
            {
                case BattleOutcomeEnum.AggressorDestroyed_ProtectorDestroyed:
                    Game.UpdatePrestigeAmount(battleOutput.AggressorUnit, battleOutput.ProtectorUnit);
                    Game.UpdatePrestigeAmount(battleOutput.ProtectorUnit, battleOutput.AggressorUnit);
                    RemoveUnitFromGame(battleOutput.AggressorUnit, battleOutput.AggressorTile);
                    RemoveUnitFromGame(battleOutput.ProtectorUnit, battleOutput.ProtectorTile);
                    break;
                case BattleOutcomeEnum.AggressorDestroyed_ProtectorHolds:
                    Game.UpdatePrestigeAmount(battleOutput.ProtectorUnit, battleOutput.AggressorUnit);
                    RemoveUnitFromGame(battleOutput.AggressorUnit, battleOutput.AggressorTile);
                    break;
                case BattleOutcomeEnum.AggressorSurvives_ProtectorDestroyed:
                    Game.UpdatePrestigeAmount(battleOutput.AggressorUnit, battleOutput.ProtectorUnit);
                    RemoveUnitFromGame(battleOutput.ProtectorUnit, battleOutput.ProtectorTile);
                    break;
                case BattleOutcomeEnum.AggressorSurvives_ProtectorRetreats:
                    Tile retreatTile = Game.TileFactory.DetermineRetreatTile(battleOutput.AggressorTile, battleOutput.ProtectorTile);
                    if (retreatTile == null)
                    {
                        Game.UpdatePrestigeAmount(battleOutput.AggressorUnit, battleOutput.ProtectorUnit);
                        RemoveUnitFromGame(battleOutput.ProtectorUnit, battleOutput.ProtectorTile);
                    }
                    {
                        MoveUnit(battleOutput.ProtectorUnit, battleOutput.ProtectorTile, retreatTile);
                    }
                    break;
            }

            MakeUnitInactive(battleOutput.AggressorUnit);
            MakeTileInactive(battleOutput.AggressorTile);
            this.ActiveUnit = null;
            this.ActiveTile = null;
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BattleOutput battleOutput = (BattleOutput)e.Argument;

            Game.HexFactory.TriggerCrosshairs(GetHexFromTile(battleOutput.AggressorTile));
            Game.HexFactory.TriggerCrosshairs(GetHexFromTile(battleOutput.ProtectorTile));

            foreach (Volley volley in battleOutput.Vollies)
            {
                switch (volley.VollyOutcomeEnum)
                {
                    case VolleyOutcomeEnum.AttackerHurt_DefenderHurt:
                        Game.SoundFactory.PlayBattleSound();
                        Game.HexFactory.TriggerExplosion(GetHexFromTile(volley.AttackerTile));
                        Game.HexFactory.TriggerExplosion(GetHexFromTile(volley.DefenderTile));
                        Game.HexFactory.TriggerStrength(GetHexFromTile(volley.AttackerTile), volley.AttackerUnit);
                        Game.HexFactory.TriggerStrength(GetHexFromTile(volley.DefenderTile), volley.DefenderUnit);
                        break;
                    case VolleyOutcomeEnum.AttackerHurt_DefenderUnhurt:
                        Game.SoundFactory.PlayBattleSound();
                        Game.HexFactory.TriggerExplosion(GetHexFromTile(volley.AttackerTile));
                        Game.HexFactory.TriggerStrength(GetHexFromTile(volley.AttackerTile), volley.AttackerUnit);
                        break;
                    case VolleyOutcomeEnum.AttackerUnhurt_DefenderHurt:
                        Game.SoundFactory.PlayBattleSound();
                        Game.HexFactory.TriggerExplosion(GetHexFromTile(volley.DefenderTile));
                        Game.HexFactory.TriggerStrength(GetHexFromTile(volley.DefenderTile), volley.DefenderUnit);
                        break;
                }
            }

        }

        public void PopulateBoard(int scenarioId)
        {
            Board.MainCanvas.Children.Clear();
            Game.TileFactory.PopulateTiles(scenarioId);
        }

        void TileFactory_TilesLoaded(object sender, EventArgs e)
        {
            LoadBoard();
        }

        public void LoadBoard()
        {
            Tiles = Game.TileFactory.Tiles;
            Hexes = Game.HexFactory.GetHexes(Tiles);
            Board.MainCanvas.Children.Clear();
            foreach (Hex hex in Hexes)
            {
                Board.MainCanvas.Children.Add(hex);
            }
            SetBoardDimensions(Board, Tiles);
            if (BoardLoaded != null)
            {
                BoardLoaded(null, null);
            }
        }

        public void SetBoardDimensions(Board board, List<Tile> tiles)
        {
            if (tiles.Count > 0)
            {
                int maxNumberOfColumns = (from t in tiles
                                          select t.ColumnNumber).Max();
                int maxNumberOfRows = (from t in tiles
                                       select t.RowNumber).Max();

                board.NumberOfColumns = maxNumberOfColumns;
                board.NumberOfRows = maxNumberOfRows;

                board.MainCanvasHeight = maxNumberOfRows * Constants.BaseBoardRowPadding * Game.CurrentViewLevel;
                board.MainCanvasWidth = maxNumberOfColumns * Constants.BaseBoardColumnPadding * Game.CurrentViewLevel;

                board.MainCanvas.Height = board.MainCanvasHeight;
                board.MainCanvas.Width = board.MainCanvasWidth;
            }

        }

        public Hex GetClosestHex(Point point)
        {
            double testDistance = 0;
            double finalDistance = double.MaxValue;
            Hex closestHex = null;
            double a = 0;
            double b = 0;

            foreach (Hex hex in Hexes)
            {
                a = point.X - hex.CenterPoint.X;
                b = point.Y - hex.CenterPoint.Y;
                testDistance = Math.Sqrt(a * a + b * b);
                if (testDistance < finalDistance)
                {
                    finalDistance = testDistance;
                    closestHex = hex;
                }
            }

            return closestHex;
        }

        public void UpdateBoardViewLevel(double distanceRatio)
        {
            if (distanceRatio < 1)
            {
                if (Game.CurrentViewLevel > 1)
                {
                    Game.CurrentViewLevel--;
                    ChangeSizeOfHexes();
                    ChangeLocationOfHexes();

                }
            }
            else
            {
                if (Game.CurrentViewLevel < 3)
                {
                    Game.CurrentViewLevel++;
                    ChangeSizeOfHexes();
                    ChangeLocationOfHexes();
                }
            }

        }

        public void UpdateBoardOrientation(Board board)
        {
            double newHeight = board.MainCanvasWidth;
            double newWidth = board.MainCanvasHeight;

            board.MainCanvasHeight = newHeight;
            board.MainCanvasWidth = newWidth;

            board.MainCanvas.Height = board.MainCanvasHeight;
            board.MainCanvas.Width = board.MainCanvasWidth;
        }

        private void ChangeSizeOfHexes()
        {
            foreach (Hex hex in this.Hexes)
            {
                Game.HexFactory.ResizeTerrain(hex);
                Game.HexFactory.ResizeUnit(hex);
                Game.HexFactory.ResizeColor(hex);
                Game.HexFactory.ResizeHexInfo(hex);
                Game.HexFactory.ResizeNation(hex);
                Game.HexFactory.ResizeStrength(hex);
                Game.HexFactory.ResizeStackedUnit(hex);
            }
        }

        private void ChangeLocationOfHexes()
        {
            Hex hex = null;
            foreach(Tile tile in this.Tiles)
            {
                hex = GetHexFromTile(tile);
                Game.HexFactory.AssignBoardCoordinates(tile, hex);
                hex.SetValue(Canvas.LeftProperty, (Double)hex.BoardXCoordinate);
                hex.SetValue(Canvas.TopProperty, (Double)hex.BoardYCoordinate);
            }
        }

        public void HandleTapEvent(Hex targetHex)
        {
            Tile targetTile = GetTileFromHex(targetHex);
            if (this.ActiveTile == null)
            {
                MakeTileActive(targetTile);
                UpdateMenus(this.ActiveTile, this.ActiveUnit);
                DeactivateIcons();
            }
            else
            {
                if (this.ActiveTile == targetTile)
                {
                    if (this.ActiveUnit != null)
                    {
                        UpdateMenus(this.ActiveTile, this.ActiveUnit);
                        MakeUnitInactive(this.ActiveUnit);
                        this.ActiveUnit = null;
                    }
                    MakeTileInactive(this.ActiveTile);
                    this.ActiveTile = null;
                    ActivateIcons();
                }
            }

        }

        public void HandleDragEvent(Hex targetHex)
        {
            Tile endingTile = GetTileFromHex(targetHex);
            if (this.ActiveUnit is ICombatUnit)
            {
                ICombatUnit combatUnit = (ICombatUnit)this.ActiveUnit;
                if (combatUnit.MoveableTiles.Contains(endingTile) && combatUnit.AttackableTiles.Contains(endingTile))
                {
                    CalculateBattle(endingTile);
                }
                else if (combatUnit.MoveableTiles.Contains(endingTile))
                {
                    MoveGameActiveUnit(endingTile);

                }
                else if (combatUnit.AttackableTiles.Contains(endingTile))
                {
                    CalculateBattle(endingTile);

                }

            }
            else
            {
                if (this.ActiveUnit.MoveableTiles.Contains(endingTile))
                {
                    MoveGameActiveUnit(endingTile);
                }
            }
        }

        private void UpdateMenus(Tile targetTile, IUnit targetUnit)
        {
            Game.MenuFactory.DeactivateAllMenuItems();
            Game.MenuFactory.ShowBoardInformation = true;

            
            if (targetUnit is LandTransportUnit && targetUnit.CanMove)
            {
                Game.MenuFactory.ShowMount = true;
            }

            if (targetUnit == null)
            {
                if (targetTile.Terrain.TerrainTypeEnum == TerrainTypeEnum.City && targetTile.GroundUnit == null)
                {
                    Game.MenuFactory.ShowPurchase = true;
                }
                if (targetTile.Terrain.TerrainTypeEnum == TerrainTypeEnum.Port && targetTile.GroundUnit == null)
                {
                    Game.MenuFactory.ShowPurchase = true;
                }
                if (targetTile.Terrain.TerrainTypeEnum == TerrainTypeEnum.Airfield && targetTile.AirUnit == null)
                {
                    Game.MenuFactory.ShowPurchase = true;
                }
            }
            else
            {
                if (targetTile.Terrain.TerrainType.TerrainTypeEnum == TerrainTypeEnum.Airfield
                    && targetUnit.Equipment.CanHaveAirTransport
                    && targetUnit.CanMove)
                {
                    Game.MenuFactory.ShowEmbark = true;
                }


                if (targetUnit is LandCombatUnit
                    && targetUnit.CanMove)
                {
                    LandCombatUnit landCombatUnit = (LandCombatUnit)targetUnit;
                    if (landCombatUnit.CanAttack)
                    {
                        if (landCombatUnit.TransportUnit != null)
                        {
                            Game.MenuFactory.ShowMount = true;
                        }
                        Game.MenuFactory.ShowSupply = true;
                        Game.MenuFactory.ShowRegularReplacements = true;
                        Game.MenuFactory.ShowEliteReplacements = true;
                        Game.MenuFactory.ShowDisband = true;
                    }
                }
                if (targetUnit is AirCombatUnit
                    && targetTile.Terrain.TerrainType.TerrainTypeEnum == TerrainTypeEnum.Airfield
                    && targetUnit.CanMove)
                {
                    AirCombatUnit airCombatUnit = (AirCombatUnit)targetUnit;
                    if (airCombatUnit.CanAttack)
                    {
                        Game.MenuFactory.ShowSupply = true;
                        Game.MenuFactory.ShowRegularReplacements = true;
                        Game.MenuFactory.ShowEliteReplacements = true;
                        Game.MenuFactory.ShowUpgrade = true;
                    }
                }
                if (targetUnit is SeaCombatUnit
                    && targetTile.Terrain.TerrainType.TerrainTypeEnum == TerrainTypeEnum.Port
                    && targetUnit.CanMove)
                {
                    SeaCombatUnit seaCombatUnit = (SeaCombatUnit)targetUnit;
                    if (seaCombatUnit.CanAttack)
                    {
                        Game.MenuFactory.ShowSupply = true;
                        Game.MenuFactory.ShowRegularReplacements = true;
                        Game.MenuFactory.ShowEliteReplacements = true;
                    }
                }
                if (targetUnit is LandCombatUnit
                    && targetTile.Terrain.TerrainTypeEnum == TerrainTypeEnum.Port
                    && targetUnit.CanMove)
                {
                    LandCombatUnit landCombatUnit = (LandCombatUnit)targetUnit;
                    if (landCombatUnit.CanAttack)
                    {
                        Game.MenuFactory.ShowUpgrade = true;
                    }
                }
                if (targetUnit is LandCombatUnit
                    && targetTile.Terrain.TerrainType.TerrainTypeEnum == TerrainTypeEnum.City
                    && targetUnit.CanMove)
                {
                    LandCombatUnit landCombatUnit = (LandCombatUnit)targetUnit;
                    if (landCombatUnit.CanAttack)
                    {
                        Game.MenuFactory.ShowUpgrade = true;
                    }
                }
                if (targetUnit is AirCombatUnit
                    && targetUnit.CanMove)
                {
                    AirCombatUnit airCombatUnit = (AirCombatUnit)targetUnit;
                    if (airCombatUnit.CanAttack)
                    {
                        Game.MenuFactory.ShowDisband = true;
                    }
                }
                if (targetUnit is SeaCombatUnit
                    && targetUnit.CanMove)
                {
                    SeaCombatUnit seaCombatUnit = (SeaCombatUnit)targetUnit;
                    if (seaCombatUnit.CanAttack)
                    {
                        Game.MenuFactory.ShowDisband = true;
                    }
                }
                if (targetUnit is AirTransportUnit
                    && targetTile.Terrain.TerrainGroupEnum == TerrainGroupEnum.Land
                    && targetUnit.CanMove)
                {
                    AirTransportUnit airTransportUnit = (AirTransportUnit)targetUnit;
                    if (airTransportUnit.LandCombatUnit.Equipment.CanParadrop)
                    {
                        Game.MenuFactory.ShowEmbark = true;
                    }
                }
                if (targetUnit is AirTransportUnit
                    && targetTile.Terrain.TerrainTypeEnum == TerrainTypeEnum.Airfield
                    && targetUnit.CanMove)
                {
                    Game.MenuFactory.ShowEmbark = true;
                }
            }
        }

        private void MoveGameActiveUnit(Tile endTile)
        {
            MoveUnit(this.ActiveUnit, this.ActiveTile, endTile);
            UpdateMenusAndIconsForMovingUnit(this.ActiveUnit, endTile);
            this.ActiveUnit = null;
            this.ActiveTile = null;
        }

        private void MoveUnit(IUnit unit, Tile startTile, Tile endTile)
        {
            UpdateStartingTileForMovingUnit(unit, startTile);
            UpdateStartingHexForMovingUnit(startTile);
            MakeTileInactive(startTile);
            MakeUnitInactive(unit);
            unit = ConvertMovingUnit(endTile, unit);
            ImmobilizeUnit(unit);
            UpdateEndingTileForMovingUnit(endTile, unit);
            UpdateGamePrestigeForCapturedTile(endTile, unit);
            CallSoundForMovingUnit(unit);
            Game.TileFactory.SetVisibleTiles(unit, endTile);
            Hex endingHex = GetHexFromTile(endTile);
            UpdateFuelForMovingUnit(unit, endTile);
            UpdateEndHexForMovingUnit(endingHex, endTile);
            unit.CurrentTileId = endTile.TileId;
        }

        public void MoveUnitForComputerPlayer(IUnit unit, Tile startTile, Tile endTile)
        {
            UpdateStartingTileForMovingUnit(unit, startTile);
            UpdateStartingHexForMovingUnit(startTile);
            MakeTileInactive(startTile);
            MakeUnitInactive(unit);
            ImmobilizeUnit(unit);
            UpdateEndingTileForMovingUnit(endTile, unit);
            UpdateGamePrestigeForCapturedTile(endTile, unit);
            Hex endingHex = GetHexFromTile(endTile);
            UpdateFuelForMovingUnit(unit, endTile);
            UpdateEndHexForMovingUnit(endingHex, endTile);
            unit.CurrentTileId = endTile.TileId;
        }

        private static void UpdateFuelForMovingUnit(IUnit unit, Tile endTile)
        {
            if (unit is IMotorizedUnit)
            {
                IMotorizedUnit motorizedUnit = (IMotorizedUnit)unit;
                motorizedUnit.CurrentFuel -= endTile.Distance;
            }
        }

        private void UpdateMenusAndIconsForMovingUnit(IUnit unit, Tile endTile)
        {
            UpdateMenus(endTile, unit);
            ActivateIcons();
        }

        private static void ImmobilizeUnit(IUnit unit)
        {
            if (unit.EquipmentClassEnum == EquipmentClassEnum.Artillery || unit.EquipmentClassEnum == EquipmentClassEnum.AirDefense)
            {
                ICombatUnit combatUnit = (ICombatUnit)unit;
                combatUnit.CanAttack = false;
            }
            unit.CanMove = false;
            unit.CanUpdate = false;
        }

        private void CalculateBattle(Tile endingTile)
        {
            BattleInput battleInput = new BattleInput();
            battleInput.AggressorTile = this.ActiveTile;
            battleInput.ProtectorTile = endingTile;
            battleInput.AggressorUnit = this.ActiveUnit;
            battleInput.ProtectorUnit = DetermineProtectingUnitForBattle(this.ActiveUnit, endingTile);
            battleInput.TerrainCondition = Game.CurrentTurn.CurrentTerrainCondition;
            currentBattleOutput = Game.BattleFactory.CalculateBattle(battleInput);
            backgroundWorker.RunWorkerAsync(currentBattleOutput);
            UpdateMenusAndIconsForAttackingUnit(currentBattleOutput);
        }

        public void CalculateBattleForComputerPlayer(Tile endingTile)
        {
            BattleInput battleInput = new BattleInput();
            battleInput.AggressorTile = this.ActiveTile;
            battleInput.ProtectorTile = endingTile;
            battleInput.AggressorUnit = this.ActiveUnit;
            battleInput.ProtectorUnit = DetermineProtectingUnitForBattle(this.ActiveUnit, endingTile);
            battleInput.TerrainCondition = Game.CurrentTurn.CurrentTerrainCondition;
            currentBattleOutput = Game.BattleFactory.CalculateBattle(battleInput);
            BattleOutput battleOutput = currentBattleOutput;

            switch (battleOutput.BattleOutcomeEnum)
            {
                case BattleOutcomeEnum.AggressorDestroyed_ProtectorDestroyed:
                    Game.UpdatePrestigeAmount(battleOutput.AggressorUnit, battleOutput.ProtectorUnit);
                    Game.UpdatePrestigeAmount(battleOutput.ProtectorUnit, battleOutput.AggressorUnit);
                    RemoveUnitFromGame(battleOutput.AggressorUnit, battleOutput.AggressorTile);
                    RemoveUnitFromGame(battleOutput.ProtectorUnit, battleOutput.ProtectorTile);
                    break;
                case BattleOutcomeEnum.AggressorDestroyed_ProtectorHolds:
                    Game.UpdatePrestigeAmount(battleOutput.ProtectorUnit, battleOutput.AggressorUnit);
                    RemoveUnitFromGame(battleOutput.AggressorUnit, battleOutput.AggressorTile);
                    break;
                case BattleOutcomeEnum.AggressorSurvives_ProtectorDestroyed:
                    Game.UpdatePrestigeAmount(battleOutput.AggressorUnit, battleOutput.ProtectorUnit);
                    RemoveUnitFromGame(battleOutput.ProtectorUnit, battleOutput.ProtectorTile);
                    break;
                case BattleOutcomeEnum.AggressorSurvives_ProtectorRetreats:
                    Tile retreatTile = Game.TileFactory.DetermineRetreatTile(battleOutput.AggressorTile, battleOutput.ProtectorTile);
                    if (retreatTile == null)
                    {
                        Game.UpdatePrestigeAmount(battleOutput.AggressorUnit, battleOutput.ProtectorUnit);
                        RemoveUnitFromGame(battleOutput.ProtectorUnit, battleOutput.ProtectorTile);
                    }
                    {
                        MoveUnit(battleOutput.ProtectorUnit, battleOutput.ProtectorTile, retreatTile);
                    }
                    break;
            }

            MakeUnitInactive(battleOutput.AggressorUnit);
            MakeTileInactive(battleOutput.AggressorTile);
            this.ActiveUnit = null;
            this.ActiveTile = null;

        }

        private void UpdateMenusAndIconsForAttackingUnit(BattleOutput battleOutput)
        {
            if (this.ActiveUnit.EquipmentClassEnum == EquipmentClassEnum.Artillery || this.ActiveUnit.EquipmentClassEnum == EquipmentClassEnum.AirDefense)
            {
                this.ActiveUnit.CanMove = false;
                this.ActiveUnit.CanUpdate = false;
            }
            if (this.ActiveUnit is ICombatUnit)
            {
                ICombatUnit combatUnit = (ICombatUnit)this.ActiveUnit;
                combatUnit.CanAttack = false;
            }

            UpdateMenus(battleOutput.ProtectorTile, battleOutput.AggressorUnit);
            ActivateIcons();
        }

        public void RemoveUnitFromGame(IUnit unit, Tile tile)
        {
            MakeTileInactive(this.ActiveTile);
            MakeUnitInactive(this.ActiveUnit);
            ImmobilizeUnit(this.ActiveUnit);
            RemoveUnitFromTile(unit, tile);
            UpdateStartingHexForMovingUnit(tile);
            UpdateMenus(this.ActiveTile, this.ActiveUnit);
            SetActivesToNull();

            if (Game.InCampaign)
            {
                Game.CampaignArmy.Remove(unit);
            }
        }

        private static void RemoveUnitFromTile(IUnit unit, Tile tile)
        {
            if (tile.GroundUnit == unit)
            {
                if (tile.GroundUnit is ITransportUnit)
                {
                    ITransportUnit transportUnit = (ITransportUnit)unit;
                    transportUnit.LandCombatUnit = null;
                }
                tile.GroundUnit = null;
            }
            else
            {
                if (tile.AirUnit is ITransportUnit)
                {
                    ITransportUnit transportUnit = (ITransportUnit)unit;
                    transportUnit.LandCombatUnit = null;
                }
                tile.AirUnit = null;
            }
        }

        private void UpdatePrestigeAmountForBattle(IUnit winningUnit, IUnit destroyedUnit)
        {
            Game.UpdatePrestigeAmount(winningUnit, destroyedUnit);
        }

        private void MakeTileInactive(Tile tile)
        {
            Hex hex = GetHexFromTile(tile);
            Game.HexFactory.RemoveColor(hex);
            Game.HexFactory.RemoveHexInfo(hex);

        }

        private void MakeUnitInactive(IUnit unit)
        {
            Hex hex = null;
            foreach (Tile tile in unit.MoveableTiles)
            {
                hex = GetHexFromTile(tile);
                Game.HexFactory.RemoveColor(hex);
            }
            if (unit is ICombatUnit)
            {
                ICombatUnit combatUnit = (ICombatUnit)unit;
                foreach (Tile tile in combatUnit.AttackableTiles)
                {
                    hex = GetHexFromTile(tile);
                    Game.HexFactory.RemoveColor(hex);
                }
            }
            
        }

        private void MakeTileActive(Tile targetTile)
        {
            Hex hex = GetHexFromTile(targetTile);
            Game.HexFactory.AddColor(hex, Colors.LightGray);
            this.ActiveTile = targetTile;

            if (targetTile.GroundUnit == null && targetTile.AirUnit == null)
            {
                Game.HexFactory.AddHexInfo(targetTile, hex);
            }
            else if (targetTile.GroundUnit != null && targetTile.AirUnit == null)
            {
                //TODO: Remove the Axis test to manipulate Allied Units
                if (targetTile.GroundUnit.SideEnum == SideEnum.Axis)
                {
                    MakeUnitActive(targetTile.GroundUnit);
                }
            }
            else if (targetTile.GroundUnit == null && targetTile.AirUnit != null)
            {
                //TODO: Remove the Axis test to manipulate Allied Units
                if (targetTile.AirUnit.SideEnum == SideEnum.Axis)
                {
                    MakeUnitActive(targetTile.AirUnit);
                }

            }
            else
            {
                if (Game.InGroundMode)
                {
                    //TODO: Remove the Axis test to manipulate Allied Units
                    if (targetTile.AirUnit.SideEnum == SideEnum.Axis)
                    {
                        MakeUnitActive(targetTile.GroundUnit);
                    }
                }
                else
                {
                    //TODO: Remove the Axis test to manipulate Allied Units
                    if (targetTile.AirUnit.SideEnum == SideEnum.Axis)
                    {
                        MakeUnitActive(targetTile.AirUnit);
                    }                    
                }

            }
            

        }

        private void MakeUnitActive(IUnit targetUnit)
        {
            Hex hex = null;
            if (targetUnit.CanMove)
            {
                Game.TileFactory.SetMovableTiles(targetUnit, this.ActiveTile);
                foreach (Tile tile in targetUnit.MoveableTiles)
                {
                    hex = GetHexFromTile(tile);
                    Game.HexFactory.AddColor(hex, Colors.Green);
                }
            }

            if (targetUnit is ICombatUnit)
            {
                ICombatUnit combatUnit = targetUnit as ICombatUnit;
                if (combatUnit.CanAttack)
                {
                    Game.TileFactory.SetAttackableTiles(combatUnit, this.ActiveTile);
                    foreach (Tile tile in combatUnit.AttackableTiles)
                    {
                        hex = GetHexFromTile(tile);
                        Game.HexFactory.RemoveColor(hex);
                        Game.HexFactory.AddColor(hex, Colors.Red);
                    }
                }
            }
            this.ActiveUnit = targetUnit;
        }

        private void ActivateIcons()
        {
            GameBoard currentGameBoard = (GameBoard)this.Board.Parent;
            foreach (ApplicationBarIconButton button in currentGameBoard.ApplicationBar.Buttons)
            {
                button.IsEnabled = true;
            }
        }

        private void DeactivateIcons()
        {
            GameBoard currentGameBoard = (GameBoard)this.Board.Parent;
            foreach (ApplicationBarIconButton button in currentGameBoard.ApplicationBar.Buttons)
            {
                button.IsEnabled = false;
            }
        }

        public Boolean AllowDragEvent(Hex startHex, Hex targetHex)
        {
            if (this.ActiveTile == null)
                return false;
            if (this.ActiveUnit == null)
                return false;
            Tile beginningTile = GetTileFromHex(startHex);
            if (this.ActiveTile != beginningTile)
                return false;

            Tile endingTile = GetTileFromHex(targetHex);

            bool canMove = this.ActiveUnit.MoveableTiles.Contains(endingTile) && this.ActiveUnit.CanMove;
            bool canAttack = false;
            if (this.ActiveUnit is ICombatUnit)
            {
                ICombatUnit combatUnit = (ICombatUnit)this.ActiveUnit;
                canAttack = combatUnit.AttackableTiles.Contains(endingTile) && this.ActiveUnit is ICombatUnit;
            }

            if (canMove == false && canAttack == false)
                return false;

            return true;
        }

        private IUnit ConvertMovingUnit(Tile endingTile, IUnit movingUnit)
        {

            if (movingUnit is LandCombatUnit && endingTile.Terrain.TerrainGroupEnum == TerrainGroupEnum.Sea)
            {
                SeaTransportUnit seaTransportUnit = Game.UnitFactory.CreateSeaTransportUnit(0, movingUnit.Nation, endingTile.TileId);
                seaTransportUnit.LandCombatUnit = movingUnit as LandCombatUnit;
                return seaTransportUnit;
            }
            else if (movingUnit is SeaTransportUnit && endingTile.Terrain.TerrainGroupEnum == TerrainGroupEnum.Land)
            {
                SeaTransportUnit seaTransportUnit = movingUnit as SeaTransportUnit;
                return seaTransportUnit.LandCombatUnit;
            }
            else if (movingUnit is SeaTransportUnit && endingTile.Terrain.TerrainGroupEnum == TerrainGroupEnum.SeaAndLand)
            {
                SeaTransportUnit seaTransportUnit = movingUnit as SeaTransportUnit;
                return seaTransportUnit.LandCombatUnit;
            }
            else
                return movingUnit;
        }

        private void UpdateStartingTileForMovingUnit(IUnit unit, Tile startTile)
        {
            if (unit == startTile.GroundUnit)
            {
                startTile.GroundUnit = null;
            }
            else if (unit == startTile.AirUnit)
            {
                startTile.AirUnit = null;
            }
        }

        private void UpdateStartingHexForMovingUnit(Tile startTile)
        {

            Hex startingHex = GetHexFromTile(startTile);
            Game.HexFactory.RemoveUnit(startingHex);
            Game.HexFactory.RemoveStrength(startingHex);
            Game.HexFactory.RemoveStackedUnit(startingHex);

            if (startTile.GroundUnit != null)
            {
                Game.HexFactory.AddUnit(startingHex, startTile.GroundUnit);
                Game.HexFactory.AddStrength(startingHex, startTile.GroundUnit);
            }
            else if (startTile.AirUnit != null)
            {
                Game.HexFactory.AddUnit(startingHex, startTile.AirUnit);
                Game.HexFactory.AddStrength(startingHex, startTile.AirUnit);
            }
        }

        private void CallSoundForMovingUnit(IUnit movingUnit)
        {
            Game.SoundFactory.PlayEquipmentSound(movingUnit.Equipment);
        }

        private void UpdateEndingTileForMovingUnit(Tile endingTile, IUnit movingUnit)
        {
            if (movingUnit is LandCombatUnit)
            {
                LandCombatUnit unit = movingUnit as LandCombatUnit;
                endingTile.GroundUnit = unit;

            }
            else if (movingUnit is LandTransportUnit)
            {
                endingTile.GroundUnit = movingUnit as LandTransportUnit;
            }

            else if (movingUnit is IAirUnit)
            {
                endingTile.AirUnit = movingUnit as IAirUnit;
            }
            else if (movingUnit is ISeaUnit)
            {
                endingTile.GroundUnit = movingUnit as ISeaUnit;
            }

        }

        private static void UpdateGamePrestigeForCapturedTile(Tile endingTile, IUnit unit)
        {
            if (unit is LandCombatUnit)
            {
                if (unit.CanCaptureHexes)
                {
                    if (endingTile.VictoryIndicator || endingTile.SupplyIndicator)
                    {
                        Game.UpdatePrestigeAmount(unit, endingTile);
                    }
                }
            }
        }

        private void UpdateEndHexForMovingUnit(Hex endingHex, Tile endingTile)
        {
            Game.HexFactory.RemoveUnit(endingHex);
            Game.HexFactory.RemoveColor(endingHex);
            

            //Add Nation
            if (endingTile.GroundUnit != null)
            {
                if (endingTile.GroundUnit.CanCaptureHexes)
                {
                    if (endingTile.VictoryIndicator || endingTile.SupplyIndicator)
                    {
                        Game.HexFactory.RemoveNation(endingHex);
                        Game.HexFactory.AddNation(endingHex, endingTile.GroundUnit.Nation);
                    }
                }
            }

            //Add Units
            //Stacked
            if (endingTile.AirUnit != null && endingTile.GroundUnit != null)
            {
                if (Game.InGroundMode)
                {
                    Game.HexFactory.AddUnit(endingHex, endingTile.GroundUnit);
                    Game.HexFactory.AddStackedUnit(endingHex, endingTile.AirUnit);

                }
                else
                {
                    Game.HexFactory.AddUnit(endingHex, endingTile.AirUnit);
                    Game.HexFactory.AddStackedUnit(endingHex, endingTile.GroundUnit);

                }
            }
            //Air Only
            if (endingTile.AirUnit != null && endingTile.GroundUnit == null)
            {
                Game.HexFactory.AddUnit(endingHex, endingTile.AirUnit);
                Game.HexFactory.AddStrength(endingHex, endingTile.AirUnit);

            }
            //Ground Only
            if (endingTile.AirUnit == null && endingTile.GroundUnit != null)
            {
                Game.HexFactory.AddUnit(endingHex, endingTile.GroundUnit);
                Game.HexFactory.AddStrength(endingHex, endingTile.GroundUnit);
            }

        }

        private IUnit DetermineProtectingUnitForBattle(IUnit aggressorUnit, Tile protectorTile)
        {
            IUnit defendingUnit = null;
            if (protectorTile.GroundUnit != null && protectorTile.AirUnit == null)
            {
                defendingUnit = protectorTile.GroundUnit;
            }
            else if (protectorTile.GroundUnit == null && protectorTile.AirUnit != null)
            {
                defendingUnit = protectorTile.AirUnit;
            }
            else if (protectorTile.GroundUnit != null && protectorTile.AirUnit != null)
            {
                switch(aggressorUnit.EquipmentClassEnum)
                {
                    case EquipmentClassEnum.Fighter:
                        defendingUnit = protectorTile.AirUnit;
                        break;
                    case EquipmentClassEnum.TacticalBomber:
                        defendingUnit = protectorTile.AirUnit;
                        break;
                    case EquipmentClassEnum.AirDefense:
                        defendingUnit = protectorTile.AirUnit;
                        break;
                    case EquipmentClassEnum.AntiAir:
                        if (Game.InGroundMode)
                        {
                            defendingUnit = protectorTile.GroundUnit;
                        }
                        else
                        {
                            defendingUnit = protectorTile.AirUnit;
                        }
                        break;
                    default:
                        defendingUnit = protectorTile.GroundUnit;
                        break;
                }
            }

            return defendingUnit;
        }

        private Tile GetTileFromHex(Hex hex)
        {
            Tile tile = (from t in Tiles
                     where t.TileId == hex.TileId
                     select t).FirstOrDefault();
            return tile;
        }

        private Hex GetHexFromTile(Tile tile)
        {
            Hex hex = (from h in Hexes
                       where h.TileId == tile.TileId
                         select h).FirstOrDefault();
            return hex;

        }

        public void ToggleAirAndGroundUnits()
        {
            foreach (Tile tile in Tiles)
            {
                if (tile.GroundUnit != null && tile.AirUnit != null)
                {
                    Hex hex = GetHexFromTile(tile);
                    Game.HexFactory.RemoveStackedUnit(hex);
                    Game.HexFactory.RemoveUnit(hex);

                    if (Game.InGroundMode)
                    {
                        Game.HexFactory.AddUnit(hex,tile.GroundUnit);
                        Game.HexFactory.AddStackedUnit(hex,tile.AirUnit);
                    }
                    else
                    {
                        Game.HexFactory.AddUnit(hex, tile.AirUnit);
                        Game.HexFactory.AddStackedUnit(hex, tile.GroundUnit);
                    }

                }
            }
        }

        public void ToggleMountAndDismount()
        {
            Tile tile = this.ActiveTile;
            IUnit unit = this.ActiveUnit;
            MakeTileInactive(tile);
            MakeUnitInactive(unit);
            Hex hex = GetHexFromTile(tile);
            Game.HexFactory.RemoveUnit(hex);
            LandCombatUnit landCombatUnit = unit as LandCombatUnit;
            LandTransportUnit landTransportUnit = unit as LandTransportUnit;
            if (landCombatUnit != null)
            {
                landTransportUnit = (LandTransportUnit)landCombatUnit.TransportUnit;
                Game.HexFactory.AddUnit(hex, landTransportUnit);
                tile.GroundUnit = landTransportUnit;
            }
            else
            {
                landCombatUnit = landTransportUnit.LandCombatUnit;
                Game.HexFactory.AddUnit(hex, landCombatUnit);
                tile.GroundUnit = landCombatUnit;
            }

            MakeTileActive(tile);
        }

        public void ToggleEmbarkAndDisembark()
        {
            Tile tile = this.ActiveTile;
            IUnit unit = this.ActiveUnit;
            MakeUnitInactive(unit);
            MakeTileInactive(tile);
            Hex hex = GetHexFromTile(tile);
            Game.HexFactory.RemoveUnit(hex);
            LandCombatUnit landCombatUnit = unit as LandCombatUnit;
            AirTransportUnit airTransportUnit = unit as AirTransportUnit;
            if (landCombatUnit != null)
            {
                airTransportUnit = Game.UnitFactory.CreateAirTransportUnit(0, landCombatUnit.Nation, tile.TileId);
                airTransportUnit.LandCombatUnit = landCombatUnit;
                Game.HexFactory.AddUnit(hex, airTransportUnit);
                tile.AirUnit = airTransportUnit;
                tile.GroundUnit = null;
                MakeTileActive(tile);
            }
            else
            {
                landCombatUnit = airTransportUnit.LandCombatUnit;
                Game.HexFactory.AddUnit(hex, landCombatUnit);
                tile.GroundUnit = landCombatUnit;
                tile.AirUnit = null;
                UpdateMenus(tile, unit);
            }
        }

        public void ResupplyActiveUnit()
        {
            Game.UnitFactory.ResupplyUnit(this.ActiveUnit);
            MakeTileInactive(this.ActiveTile);
            MakeUnitInactive(this.ActiveUnit);
            ImmobilizeUnit(this.ActiveUnit);
            UpdateMenus(this.ActiveTile, this.ActiveUnit);
            SetActivesToNull();
        }

        private void SetActivesToNull()
        {
            this.ActiveUnit = null;
            this.ActiveTile = null;
        }

        internal void ReinforceActiveUnit(bool useEliteReplacements)
        {
            Game.UnitFactory.ReinforceUnit(this.ActiveUnit, useEliteReplacements);
            MakeTileInactive(this.ActiveTile);
            MakeUnitInactive(this.ActiveUnit);
            ImmobilizeUnit(this.ActiveUnit);
            UpdateMenus(this.ActiveTile, this.ActiveUnit);
            SetActivesToNull();
        }

        public void NavigateToBoardInformationPage()
        {
            Game.CurrentUnit = this.ActiveUnit;
            Game.BoardFactory.ActiveTile = this.ActiveTile;
            GameBoard gameBoard = (GameBoard)Game.CurrentBoard.Parent;
            gameBoard.NavigationService.Navigate(new Uri(@"/BoardInformation.xaml", UriKind.Relative));
        }

        public Int32 GetNumberOfVictoryTiles(SideEnum sideEnum)
        {
            return Tiles.Where(t => t.VictoryIndicator && t.Nation.SideEnum == sideEnum).Count();
        }

        public Int32 GetNumberOfVictoryTiles()
        {
            return Tiles.Where(t => t.VictoryIndicator == true).Count();
        }

        public bool AllTilesCapturedByOneSide()
        {
            int totalVictoryTilesCount = GetNumberOfVictoryTiles();
            int totalAxisVictoryTilesCount = GetNumberOfVictoryTiles(SideEnum.Axis);
            int totalAlliedVictoryTilesCount = GetNumberOfVictoryTiles(SideEnum.Allies);
            if (totalVictoryTilesCount == totalAxisVictoryTilesCount)
            {
                return true;
            }
            else if (totalVictoryTilesCount == totalAlliedVictoryTilesCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddNewUnitToActiveTile(IUnit unit, Tile tile)
        {
            UpdateStartingTileForMovingUnit(unit, tile);
            UpdateStartingHexForMovingUnit(tile);
            MakeUnitInactive(unit);
            MakeTileInactive(tile);
            ImmobilizeUnit(unit);

            UpdateEndingTileForMovingUnit(tile, unit);
            Game.TileFactory.SetVisibleTiles(unit, tile);
            Hex endingHex = GetHexFromTile(tile);
            UpdateEndHexForMovingUnit(endingHex, tile);

            if (Game.UnitUpgrade)
            {
                Game.UnitUpgrade = false;
                unit.CanMove = false;
                unit.CanUpdate = false;
            }

            this.ActiveUnit = unit;
            this.ActiveTile = tile;
        }

        public void AddNewUnitForComputerPlayer(IUnit unit, Tile tile)
        {
            UpdateEndingTileForMovingUnit(tile, unit);
            Hex endingHex = GetHexFromTile(tile);
            UpdateEndHexForMovingUnit(endingHex, tile);

        }

    }

}

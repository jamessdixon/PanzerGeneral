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
using System.IO.IsolatedStorage;
using System.IO;

using Tff.Panzer.Controls;

using Tff.Panzer.Factories;
using Tff.Panzer.Factories.Campaign;
using Tff.Panzer.Factories.Scenario;
using Tff.Panzer.Factories.Geography;
using Tff.Panzer.Factories.Army;
using Tff.Panzer.Factories.Movement;
using Tff.Panzer.Factories.Battle;

using Tff.Panzer.Models.Campaign;
using Tff.Panzer.Models.Scenario;
using Tff.Panzer.Models.Geography;
using Tff.Panzer.Models.Army;
using Tff.Panzer.Models;
using Tff.Panzer.Models.Army.Unit;
using Tff.Panzer.Factories.VictoryCondition;

using System.Xml.Serialization;
using System.Xml;

namespace Tff.Panzer
{
    //TODO: Write Help Pages on Ten Fingers Free

    //TODO: Handle Landscape
    //TODO: Scenario - experience cap
    //TODO: Scenario - aggressorUnit purchase Cap
    //TODO: Move all of the code from GameSummary.xaml.cs into a Factory class
    //TODO: Campaign Scenario - Deploy Hexes
    //TODO: Why changing Path.Height and Width slows thing down so much?
    //TODO: Need a aggressorUnit movement undo

    public static class Game
    {
        public static Board CurrentBoard { get; set; }
        public static Int32 CurrentViewLevel { get; set; }
        public static Int32 CurrentScenarioId { get; set; }
        public static Int32 CurrentCampaignId { get; set; }
        public static Boolean InCampaign
        {
            get
            {
                if (CurrentCampaignId < 0)
                {
                    return false;
                }
                else 
                {
                    return true;
                }

            }
        }
        public static CampaignStep CurrentCampaignStep { get; set; } 
        public static List<Turn> Turns { get; set; }
        public static Turn CurrentTurn
        {
            get
            {
                return Turns.Where(t => t.ActiveTurn == true).FirstOrDefault();
            }
        } 
        public static Boolean InGroundMode { get; set; }
        public static Boolean UnitUpgrade { get; set; }

        public static Side CurrentSide { get; set; }
        public static IUnit CurrentUnit { get; set; }

        public static SolidColorBrush HexOutline { get; set; }
        public static Boolean ShowTileInformation { get; set; }
        public static List<IUnit> CampaignArmy { get; set; }

        public static BoardFactory BoardFactory { get; set; }
        public static MenuFactory MenuFactory { get; set; }
        public static ImageFactory ImageFactory { get; set; }
        public static SoundFactory SoundFactory { get; set; }
        public static TerrainFactory TerrainFactory { get; set; }
        public static StrengthFactory StrengthFactory { get; set; } 
        public static NationFactory NationFactory { get; set; }
        public static WeatherFactory WeatherFactory { get; set; }
        public static MovementFactory MovementFactory { get; set; }
        public static BattleFactory BattleFactory { get; set; }
        public static ArmyFactory ArmyFactory { get; set; }
        public static UnitFactory UnitFactory { get; set; }
        public static ComputerPlayerFactory ComputerPlayerFactory { get; set; }
        public static CampaignFactory CampaignFactory { get; set; }
        public static ScenarioFactory ScenarioFactory { get; set; }
        public static VictoryConditionFactory VictoryConditionFactory { get; set; }
        public static TurnFactory TurnFactory { get; set; }
        public static TileFactory TileFactory { get; set; }
        public static HexFactory HexFactory { get; set; }

        public static void InitializeGame()
        {
            Game.CurrentViewLevel = 2;
            Game.InGroundMode = true;
            Game.ShowTileInformation = true;
        }

        public static void SaveGame()
        {
            SaveCurrentTurn();
            SaveTilesWithoutUnits();
            SaveLandCombatUnits();
            SaveAirCombatUnits();
            SaveSeaCombatUnits();
            SaveLandTransportUnits();
            SaveSeaTransportUnits();
            SaveAirTransportUnits();
        }

        private static void SaveLandTransportUnits()
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            List<LandTransportUnit> landTransportUnits = new List<LandTransportUnit>();
            foreach (Tile tile in Game.BoardFactory.Tiles)
            {
                if (tile.GroundUnit is LandTransportUnit)
                {
                    landTransportUnits.Add((LandTransportUnit)tile.GroundUnit);
                }
            }
            IsolatedStorageFileStream landTransportUnitStream = isolatedStorageFile.OpenFile("landTransportUnits", FileMode.Create);
            XmlSerializer landTransportUnitSerializer = new XmlSerializer(typeof(List<LandTransportUnit>));
            XmlWriter landTransportUnitWriter = XmlWriter.Create(landTransportUnitStream);
            landTransportUnitSerializer.Serialize(landTransportUnitWriter, landTransportUnits);
            landTransportUnitStream.Close();
        }

        private static void SaveSeaTransportUnits()
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            List<SeaTransportUnit> seaTransportUnits = new List<SeaTransportUnit>();
            foreach (Tile tile in Game.BoardFactory.Tiles)
            {
                if (tile.GroundUnit is SeaTransportUnit)
                {
                    seaTransportUnits.Add((SeaTransportUnit)tile.GroundUnit);
                }
            }
            IsolatedStorageFileStream seaTransportUnitStream = isolatedStorageFile.OpenFile("seaTransportUnits", FileMode.Create);
            XmlSerializer seaTransportUnitSerializer = new XmlSerializer(typeof(List<SeaTransportUnit>));
            XmlWriter seaTransportUnitWriter = XmlWriter.Create(seaTransportUnitStream);
            seaTransportUnitSerializer.Serialize(seaTransportUnitWriter, seaTransportUnits);
            seaTransportUnitStream.Close();
        }

        private static void SaveAirTransportUnits()
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            List<AirTransportUnit> airTransportUnits = new List<AirTransportUnit>();
            foreach (Tile tile in Game.BoardFactory.Tiles)
            {
                if (tile.GroundUnit is AirTransportUnit)
                {
                    airTransportUnits.Add((AirTransportUnit)tile.AirUnit);
                }
            }
            IsolatedStorageFileStream airTransportUnitStream = isolatedStorageFile.OpenFile("airTransportUnits", FileMode.Create);
            XmlSerializer airTransportUnitSerializer = new XmlSerializer(typeof(List<AirTransportUnit>));
            XmlWriter airTransportUnitWriter = XmlWriter.Create(airTransportUnitStream);
            airTransportUnitSerializer.Serialize(airTransportUnitWriter, airTransportUnits);
            airTransportUnitStream.Close();
        }

        private static void SaveSeaCombatUnits()
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            List<SeaCombatUnit> seaCombatUnits = new List<SeaCombatUnit>();
            foreach (Tile tile in Game.BoardFactory.Tiles)
            {
                if (tile.GroundUnit is SeaCombatUnit)
                {
                    seaCombatUnits.Add((SeaCombatUnit)tile.GroundUnit);
                }
            }
            IsolatedStorageFileStream seaCombatUnitStream = isolatedStorageFile.OpenFile("seaCombatUnits", FileMode.Create);
            XmlSerializer seaCombatUnitSerializer = new XmlSerializer(typeof(List<SeaCombatUnit>));
            XmlWriter seaCombatUnitWriter = XmlWriter.Create(seaCombatUnitStream);
            seaCombatUnitSerializer.Serialize(seaCombatUnitWriter, seaCombatUnits);
            seaCombatUnitStream.Close();
        }

        private static void SaveAirCombatUnits()
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            List<AirCombatUnit> airCombatUnits = new List<AirCombatUnit>();
            foreach (Tile tile in Game.BoardFactory.Tiles)
            {
                if (tile.AirUnit is AirCombatUnit)
                {
                    airCombatUnits.Add((AirCombatUnit)tile.AirUnit);
                }
            }
            IsolatedStorageFileStream airCombatUnitStream = isolatedStorageFile.OpenFile("airCombatUnits", FileMode.Create);
            XmlSerializer airCombatUnitSerializer = new XmlSerializer(typeof(List<AirCombatUnit>));
            XmlWriter airCombatUnitWriter = XmlWriter.Create(airCombatUnitStream);
            airCombatUnitSerializer.Serialize(airCombatUnitWriter, airCombatUnits);
            airCombatUnitStream.Close();
        }

        private static void SaveLandCombatUnits()
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            List<LandCombatUnit> landCombatUnits = new List<LandCombatUnit>();
            foreach (Tile tile in Game.BoardFactory.Tiles)
            {
                if (tile.GroundUnit is LandCombatUnit)
                {
                    landCombatUnits.Add((LandCombatUnit)tile.GroundUnit);
                }
            }
            IsolatedStorageFileStream landCombatUnitStream = isolatedStorageFile.OpenFile("landCombatUnits", FileMode.Create);
            XmlSerializer landCombatUnitSerializer = new XmlSerializer(typeof(List<LandCombatUnit>));
            XmlWriter landCombatUnitWriter = XmlWriter.Create(landCombatUnitStream);
            landCombatUnitSerializer.Serialize(landCombatUnitWriter, landCombatUnits);
            landCombatUnitStream.Close();
        }

        private static void SaveTilesWithoutUnits()
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream tileStream = isolatedStorageFile.OpenFile("tiles", FileMode.Create);
            XmlSerializer tilesSerializer = new XmlSerializer(typeof(List<Tile>));
            XmlWriter tileWriter = XmlWriter.Create(tileStream);
            tilesSerializer.Serialize(tileWriter, Game.BoardFactory.Tiles);
            tileStream.Close();
        }

        private static void SaveCurrentTurn()
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream turnStream = isolatedStorageFile.OpenFile("turn", FileMode.Create);
            XmlSerializer turnSerializer = new XmlSerializer(typeof(List<Turn>));
            XmlWriter turnWriter = XmlWriter.Create(turnStream);
            turnSerializer.Serialize(turnWriter, Game.Turns);
            turnStream.Close();
        }

        public static void LoadGame()
        {
            LoadCurrentTurn();
            LoadTilesWithoutUnits();
            LoadLandCombatUnits();
            LoadAirCombatUnits();
            LoadSeaCombatUnits();
            LoadLandTransportUnits();
            LoadSeaTransportUnits();
            LoadAirTransportUnits();
            Game.BoardFactory.LoadBoard();
        }

        private static void LoadAirTransportUnits()
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream airTransportUnitStream = isolatedStorageFile.OpenFile("airTransportUnits", FileMode.Open);
            XmlSerializer airTransportUnitSerializer = new XmlSerializer(typeof(List<AirTransportUnit>));
            List<AirTransportUnit> airTransportUnits = (List<AirTransportUnit>)airTransportUnitSerializer.Deserialize(airTransportUnitStream);
            Tile currentTile = null;
            foreach (AirTransportUnit airTransportUnit in airTransportUnits)
            {
                currentTile = Game.TileFactory.Tiles.Where(t => t.TileId == airTransportUnit.CurrentTileId).FirstOrDefault();
                currentTile.AirUnit = airTransportUnit;
            }
        }

        private static void LoadSeaTransportUnits()
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream seaTransportUnitStream = isolatedStorageFile.OpenFile("seaTransportUnits", FileMode.Open);
            XmlSerializer seaTransportUnitSerializer = new XmlSerializer(typeof(List<SeaTransportUnit>));
            List<SeaTransportUnit> seaTransportUnits = (List<SeaTransportUnit>)seaTransportUnitSerializer.Deserialize(seaTransportUnitStream);
            Tile currentTile = null;
            foreach (SeaTransportUnit seaTransportUnit in seaTransportUnits)
            {
                currentTile = Game.TileFactory.Tiles.Where(t => t.TileId == seaTransportUnit.CurrentTileId).FirstOrDefault();
                currentTile.GroundUnit = seaTransportUnit;
            }

        }

        private static void LoadLandTransportUnits()
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream landTransportUnitStream = isolatedStorageFile.OpenFile("landTransportUnits", FileMode.Open);
            XmlSerializer landTransportUnitSerializer = new XmlSerializer(typeof(List<LandTransportUnit>));
            List<LandTransportUnit> landTransportUnits = (List<LandTransportUnit>)landTransportUnitSerializer.Deserialize(landTransportUnitStream);
            Tile currentTile = null;
            foreach (LandTransportUnit landTransportUnit in landTransportUnits)
            {
                currentTile = Game.TileFactory.Tiles.Where(t => t.TileId == landTransportUnit.CurrentTileId).FirstOrDefault();
                currentTile.GroundUnit = landTransportUnit;
            }
        }

        private static void LoadSeaCombatUnits()
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream seaCombatUnitStream = isolatedStorageFile.OpenFile("seaCombatUnits", FileMode.Open);
            XmlSerializer seaCombatUnitSerializer = new XmlSerializer(typeof(List<SeaCombatUnit>));
            List<SeaCombatUnit> seaCombatUnits = (List<SeaCombatUnit>)seaCombatUnitSerializer.Deserialize(seaCombatUnitStream);
            Tile currentTile = null;
            foreach (SeaCombatUnit seaCombatUnit in seaCombatUnits)
            {
                currentTile = Game.TileFactory.Tiles.Where(t => t.TileId == seaCombatUnit.CurrentTileId).FirstOrDefault();
                currentTile.GroundUnit = seaCombatUnit;
            }
        }

        private static void LoadAirCombatUnits()
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream airCombatUnitStream = isolatedStorageFile.OpenFile("airCombatUnits", FileMode.Open);
            XmlSerializer airCombatUnitSerializer = new XmlSerializer(typeof(List<AirCombatUnit>));
            List<AirCombatUnit> airCombatUnits = (List<AirCombatUnit>)airCombatUnitSerializer.Deserialize(airCombatUnitStream);
            Tile currentTile = null;
            foreach (AirCombatUnit airCombatUnit in airCombatUnits)
            {
                currentTile = Game.TileFactory.Tiles.Where(t => t.TileId == airCombatUnit.CurrentTileId).FirstOrDefault();
                currentTile.AirUnit = airCombatUnit;
            }
        }

        private static void LoadLandCombatUnits()
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream landCombatUnitStream = isolatedStorageFile.OpenFile("landCombatUnits", FileMode.Open);
            XmlSerializer landCombatUnitSerializer = new XmlSerializer(typeof(List<LandCombatUnit>));
            List<LandCombatUnit> landCombatUnits = (List<LandCombatUnit>)landCombatUnitSerializer.Deserialize(landCombatUnitStream);
            Tile currentTile = null;
            foreach (LandCombatUnit landCombatUnit in landCombatUnits)
            {
                currentTile = Game.TileFactory.Tiles.Where(t => t.TileId == landCombatUnit.CurrentTileId).FirstOrDefault();
                currentTile.GroundUnit = landCombatUnit;
            }
        }

        private static void LoadTilesWithoutUnits()
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream tileStream = isolatedStorageFile.OpenFile("tiles", FileMode.Open);
            XmlSerializer tilesSerializer = new XmlSerializer(typeof(List<Tile>));
            Game.TileFactory.Tiles = (List<Tile>)tilesSerializer.Deserialize(tileStream);
        }

        private static void LoadCurrentTurn()
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream turnStream = isolatedStorageFile.OpenFile("turn", FileMode.Open);
            XmlSerializer turnSerializer = new XmlSerializer(typeof(List<Turn>));
            Game.Turns = (List<Turn>)turnSerializer.Deserialize(turnStream);
        }

        public static void UpdatePrestigeAmount(IUnit winningUnit, Tile tile)
        {

            int prestigeAmount = 0;
            if (tile.VictoryIndicator)
            {
                prestigeAmount = Constants.PrestigeAmountForVictoryHex;
            }
            else if (tile.SupplyIndicator)
            {
                prestigeAmount = Constants.PrestigeAmountForSupplyHex;
            }


            if (winningUnit.Nation.SideEnum == SideEnum.Axis)
            {
                Game.CurrentTurn.CurrentAxisPrestige += prestigeAmount; 
            }
            else
            {
                Game.CurrentTurn.CurrentAlliedPrestige += prestigeAmount;
            }
        }
        public static void UpdatePrestigeAmount(IUnit winningUnit, IUnit destroyedUnit)
        {
            int prestigeAmount = (Int32)(destroyedUnit.Equipment.UnitCost * .5);

            if (winningUnit.Nation.SideEnum == SideEnum.Axis)
            {
                Game.CurrentTurn.CurrentAxisPrestige += prestigeAmount;
            }
            else
            {
                Game.CurrentTurn.CurrentAlliedPrestige += prestigeAmount;
            }
        }
        public static void GoToNextTurn()
        {
            int nextTurnId = Game.CurrentTurn.TurnId + 1;
            SetActiveTurn(nextTurnId);

        }
        public static void SetActiveTurn(int turnId)
        {
            foreach (Turn turn in Turns)
            {
                if (turn.TurnId == turnId)
                {
                    turn.ActiveTurn = true;

                }
                else
                {
                    turn.ActiveTurn = false;
                }
            }
        }

    }
}

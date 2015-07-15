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

namespace Tff.Panzer
{
    public static class Constants
    {
        public const int BaseHexHeight = 50;
        public const int BaseHexWidth = 60; 

        public const int BaseBoardRowPadding = 49;
        public const int BaseBoardRowOffset = -25;
        
        public const int BaseBoardColumnPadding = 44; 
        public const int BaseBoardColumnOffset = -22;

        public const int AirUnitBaseBoardRowOffset = -10;

        public const int TotalNumberOfGameTiles = 236;
        public const int TotalNumberOfUnits = 477;

        public const int PrestigeAmountForVictoryHex = 100;
        public const int PrestigeAmountForSupplyHex = 40;

        public const int CrosshairsXCoordinate = -240;
        public const int CrosshairsYCoordinate = 0;


        public const String DryTerrainImagePath = "/Tff.Panzer;component/Images/tacmap_dry.jpg";
        public const String FrozenTerrainImagePath = "/Tff.Panzer;component/Images/tacmap_frozen.png";
        public const String MuddyTerrainImagePath = "/Tff.Panzer;component/Images/tacmap_muddy.png";
        public const String EquipmentImagePath = "/Tff.Panzer;component/Images/tacicons.png";
        public const String StrengthImagePath = "/Tff.Panzer;component/Images/strength.png";
        public const String NationImagePath = "/Tff.Panzer;component/Images/flags.png";
        public const String StackedUnitImagePath = "/Tff.Panzer;component/Images/stackicn.png";
        public const String ExplodeImagePath = "/Tff.Panzer;component/Images/explode.png";
        public const String HexsidesImagePath = "/Tff.Panzer;component/Images/hexsides.png";

        public const String AirGroundButtonAirImagePath = "/Images/appbar.upload.rest.png";
        public const String AirGroundButtonGroundImagePath = "/Images/appbar.download.rest.png";

        public const String PropAirplaneSoundPath = "Sounds/air.wav";
        public const String JetAirplaneSoundPath = "Sounds/jet.wav";
        public const String BattleSoundPath = "Sounds/explode.wav";
        public const String WalkSoundPath = "Sounds/leg.wav";
        public const String NavalSoundPath = "Sounds/naval.wav";
        public const String TrackedSoundPath = "Sounds/tracked.wav";
        public const String WheeledSoundPath = "Sounds/wheeled.wav";

        public const String EquipmentDataPath = "/Tff.Panzer;component/LookupData/Equipment.xml";
        public const String EquipmentSubClassDataPath = "/Tff.Panzer;component/LookupData/EquipmentSubClass.xml";
        public const String EquipmentClassDataPath = "/Tff.Panzer;component/LookupData/EquipmentClass.xml";
        public const String EquipmentGroupDataPath = "/Tff.Panzer;component/LookupData/EquipmentGroup.xml";
        public const String EquipmentLossCalculationGroupDataPath = "/Tff.Panzer;component/LookupData/EquipmentLossCalculationGroup.xml";
        
        public const String TerrainDataPath = "/Tff.Panzer;component/LookupData/Terrain.xml";
        public const String TerrainTypeDataPath = "/Tff.Panzer;component/LookupData/TerrainType.xml";
        public const String TerrainGroupDataPath = "/Tff.Panzer;component/LookupData/TerrainGroup.xml";
        public const String TileNameDataPath = "/Tff.Panzer;component/LookupData/TileName.xml";
        public const String MovementTypeDataPath = "/Tff.Panzer;component/LookupData/MovementType.xml";
        public const String TargetTypeDataPath = "/Tff.Panzer;component/LookupData/TargetType.xml";
        public const String NationDataPath = "/Tff.Panzer;component/LookupData/Nation.xml";
        public const String SideDataPath = "/Tff.Panzer;component/LookupData/Side.xml";
        public const String WeatherDataPath = "/Tff.Panzer;component/LookupData/Weather.xml";
        public const String WeatherZoneDataPath = "/Tff.Panzer;component/LookupData/WeatherZone.xml";
        public const String WeatherProbabilityDataPath = "/Tff.Panzer;component/LookupData/WeatherProbability.xml";
        public const String TerrainConditionDataPath = "/Tff.Panzer;component/LookupData/TerrainCondition.xml";
        public const String MovementCostDataPath = "/Tff.Panzer;component/LookupData/MovementCost.xml";
        public const String MovementCostModifierDataPath = "/Tff.Panzer;component/LookupData/MovementCostModifier.xml";

        public const String CampaignDataPath = "/Tff.Panzer;component/LookupData/Campaign.xml";
        public const String Campaign_BriefingDataPath = "/Tff.Panzer;component/LookupData/Campaign_Briefing.xml";
        public const String Campaign_StepDataPath = "/Tff.Panzer;component/LookupData/Campaign_Step.xml";
        public const String Campaign_StepTypeDataPath = "/Tff.Panzer;component/LookupData/Campaign_StepType.xml";
        public const String Campaign_TreeDataPath = "/Tff.Panzer;component/LookupData/Campaign_Tree.xml";

        public const String ScenarioDataPath = "/Tff.Panzer;component/LookupData/Scenario.xml";
        public const String Scenario_TileDataPath = "/Tff.Panzer;component/LookupData/Scenario_Tile.xml";
        public const String Scenario_ClassPurchaseDataPath = "/Tff.Panzer;component/LookupData/Scenario_ClassPurchase.xml";
        public const String Scenario_NationDataPath = "/Tff.Panzer;component/LookupData/Scenario_Nation.xml";
        public const String Scenario_PrestigeAllotmentDataPath = "/Tff.Panzer;component/LookupData/Scenario_PrestigeAllotment.xml";
        public const String Scenario_SideDataPath = "/Tff.Panzer;component/LookupData/Scenario_Side.xml";
        public const String Scenario_UnitDataPath = "/Tff.Panzer;component/LookupData/Scenario_Unit.xml";

        public const String CampaignVictoryConditionDataPath = "/Tff.Panzer;component/LookupData/VictoryCondition_Campaign.xml";
        public const String StandAloneVictoryConditionDataPath = "/Tff.Panzer;component/LookupData/VictoryCondition_StandAlone.xml";
        public const String ObjectiveTypeDataPath = "/Tff.Panzer;component/LookupData/ObjectiveType.xml";

        public const String HelpDocumentsLocation = "http://www.tenfingersfree.com/panzer/IntroPage.htm";
        
    }
}

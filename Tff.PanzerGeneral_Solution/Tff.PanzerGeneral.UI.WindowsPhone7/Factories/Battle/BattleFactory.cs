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
using System.Collections.Generic;
using Tff.Panzer.Controls;
using System.Diagnostics;
using System.Linq;
using Tff.Panzer.Models;
using Tff.Panzer.Models.Geography;
using Tff.Panzer.Models.Army.Unit;
using Tff.Panzer.Models.Battle;

namespace Tff.Panzer.Factories.Battle
{
    public class BattleFactory
    {
        public BattleOutput CalculateBattle(BattleInput battleInput)
        {
            BattleOutput battleOutput = new BattleOutput();
            battleOutput.AggressorTile = battleInput.AggressorTile;
            battleOutput.AggressorUnit = battleInput.AggressorUnit;
            battleOutput.ProtectorTile = battleInput.ProtectorTile;
            battleOutput.ProtectorUnit = battleInput.ProtectorUnit;

            Volley volley = null;
            InitativeEnum initativeEnum = InitativeEnum.Simultanous;
            Int32 protectorUnitStartingStrength = battleInput.ProtectorUnit.CurrentStrength;

            int protectorBaseStrength = battleInput.ProtectorUnit.CurrentStrength;
            bool isSurprised = !battleInput.AggressorUnit.VisibleTiles.Contains(battleInput.ProtectorTile);

            IUnit supportingTileAttackerUnit = DetermineSupportingVolleyUnit(battleInput.AggressorUnit, 
                battleInput.ProtectorTile);
            if (supportingTileAttackerUnit != null)
            {
                Tile supportingTileAttackerTile = DetermineSupportingVolleyTile(battleInput.AggressorUnit,
                    battleInput.ProtectorTile);
                SetAttackPointsForVolley(supportingTileAttackerUnit);
                SetAttackPointsForVolley(battleInput.AggressorUnit);
                volley = CalculateVolley(supportingTileAttackerUnit, battleInput.AggressorUnit, 
                    supportingTileAttackerTile, battleInput.AggressorTile, battleInput.TerrainCondition);
                battleOutput.Vollies.Add(volley);

                SetAttackPointsForVolley(battleInput.ProtectorUnit);
                initativeEnum = CalculateInitiative(battleInput.AggressorUnit, battleInput.ProtectorUnit,
                    battleInput.ProtectorTile, isSurprised);
            }
            else
            {
                SetAttackPointsForVolley(battleInput.AggressorUnit);
                SetAttackPointsForVolley(battleInput.ProtectorUnit);
                initativeEnum = CalculateInitiative(battleInput.AggressorUnit, battleInput.ProtectorUnit, 
                    battleInput.ProtectorTile, isSurprised);
            }

            if (battleInput.AggressorUnit.EquipmentClassEnum == EquipmentClassEnum.Artillery ||
                battleInput.AggressorUnit.EquipmentClassEnum == EquipmentClassEnum.AntiAir)
            {
                volley = CalculateVolley(battleInput.AggressorUnit, battleInput.ProtectorUnit, 
                    battleInput.AggressorTile, battleInput.ProtectorTile, battleInput.TerrainCondition);
                battleOutput.Vollies.Add(volley);
            }
            else
            {
                switch (initativeEnum)
                {
                    case InitativeEnum.AttackerStrikesFirst:
                        volley = CalculateVolley(battleInput.AggressorUnit, battleInput.ProtectorUnit,
                            battleInput.AggressorTile, battleInput.ProtectorTile, battleInput.TerrainCondition);
                        battleOutput.Vollies.Add(volley);
                        volley = CalculateVolley(battleInput.ProtectorUnit, battleInput.AggressorUnit, 
                            battleInput.ProtectorTile, battleInput.AggressorTile, battleInput.TerrainCondition);
                        battleOutput.Vollies.Add(volley);
                        break;
                    case InitativeEnum.DefenderStrikesFirst:
                        volley = CalculateVolley(battleInput.ProtectorUnit, battleInput.AggressorUnit,
                            battleInput.ProtectorTile, battleInput.AggressorTile, battleInput.TerrainCondition);
                        battleOutput.Vollies.Add(volley);
                        volley = CalculateVolley(battleInput.AggressorUnit, battleInput.ProtectorUnit,
                            battleInput.AggressorTile, battleInput.ProtectorTile, battleInput.TerrainCondition);
                        battleOutput.Vollies.Add(volley);
                        break;
                    case InitativeEnum.Simultanous:
                        volley = CalculateVolley(battleInput.AggressorUnit, battleInput.ProtectorUnit,
                            battleInput.AggressorTile, battleInput.ProtectorTile, battleInput.TerrainCondition);
                        battleOutput.Vollies.Add(volley);
                        volley = CalculateVolley(battleInput.ProtectorUnit, battleInput.AggressorUnit, 
                            battleInput.ProtectorTile, battleInput.AggressorTile, battleInput.TerrainCondition);
                        battleOutput.Vollies.Add(volley);
                        break;
                }
            }

            battleOutput.BattleOutcomeEnum = DetermineBattleResult(battleInput.AggressorUnit,
                battleInput.ProtectorUnit, protectorUnitStartingStrength);
            return battleOutput;
        }
        private BattleOutcomeEnum DetermineBattleResult(IUnit aggressorUnit, IUnit protectorUnit, int protectorBaseStrength)
        {
            if (aggressorUnit.CurrentStrength <= 0 && protectorUnit.CurrentStrength <= 0)
            {
                return BattleOutcomeEnum.AggressorDestroyed_ProtectorDestroyed;
            }
            else if (aggressorUnit.CurrentStrength <= 0 && protectorUnit.CurrentStrength >= 0)
            {
                return BattleOutcomeEnum.AggressorDestroyed_ProtectorHolds;

            }
            else if (aggressorUnit.CurrentStrength >= 0 && protectorUnit.CurrentStrength <= 0)
            {
                return BattleOutcomeEnum.AggressorSurvives_ProtectorDestroyed;

            }
            else if (aggressorUnit.CurrentStrength >= 0 && protectorUnit.CurrentStrength >= 0)
            {
                if (DefenderRetreats(protectorUnit, protectorBaseStrength))
                {
                    return BattleOutcomeEnum.AggressorSurvives_ProtectorRetreats;
                }
                else
                {
                    return BattleOutcomeEnum.AggressorSurvives_ProtectorHolds;
                }
            }

            return BattleOutcomeEnum.AggressorSurvives_ProtectorHolds;
        }

        private Boolean DefenderRetreats(IUnit protectorUnit, int protectorBaseStrength)
        {
            if (protectorUnit is ILandUnit)
            {
                if ((protectorBaseStrength / 2) > protectorUnit.CurrentStrength)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        private Tile DetermineSupportingVolleyTile(IUnit aggressorUnit, Tile protectingTile)
        {
            List<Tile> tiles = null;
            TileFactory tileFactory = Game.TileFactory;

            if (aggressorUnit is IAirUnit)
            {
                tiles = tileFactory.GetAdjacentTiles(protectingTile, 1, true);
                tiles = tiles.Where(t => t.AirUnit != null).ToList();
                tiles = tiles.Where(t => t.AirUnit.EquipmentClassEnum == EquipmentClassEnum.Fighter).ToList();
                tiles = tiles.Where(t => t.AirUnit.SideEnum != aggressorUnit.SideEnum).ToList();
                if (tiles.Count > 0)
                {
                    return tiles[0];
                }
                tiles = tileFactory.GetAdjacentTiles(protectingTile, 1, true);
                tiles = tiles.Where(t => t.GroundUnit != null).ToList();
                tiles = tiles.Where(t => t.GroundUnit.EquipmentClassEnum == EquipmentClassEnum.AirDefense).ToList();
                tiles = tiles.Where(t => t.GroundUnit.SideEnum != aggressorUnit.SideEnum).ToList();
                if (tiles.Count > 0)
                {
                    return tiles[0]; ;
                }
            }
            else if (aggressorUnit is ILandUnit)
            {
                tiles = tileFactory.GetAdjacentTiles(protectingTile, 1, true);
                tiles = tiles.Where(t => t.GroundUnit != null).ToList();
                tiles = tiles.Where(t => t.GroundUnit.EquipmentClassEnum == EquipmentClassEnum.Artillery).ToList();
                tiles = tiles.Where(t => t.GroundUnit.SideEnum != aggressorUnit.SideEnum).ToList();
                if (tiles.Count > 0)
                {
                    return tiles[0];
                }
            }
            return null;
        }
        private IUnit DetermineSupportingVolleyUnit(IUnit aggressorUnit, Tile protectingTile)
        {
            List<Tile> tiles = null;
            TileFactory tileFactory = Game.TileFactory;

            if (aggressorUnit is IAirUnit)
            {
                tiles = tileFactory.GetAdjacentTiles(protectingTile, 1, true);
                tiles = tiles.Where(t => t.AirUnit != null).ToList();
                tiles = tiles.Where(t => t.AirUnit.EquipmentClassEnum == EquipmentClassEnum.Fighter).ToList();
                tiles = tiles.Where(t => t.AirUnit.SideEnum != aggressorUnit.SideEnum).ToList();
                if (tiles.Count > 0)
                {
                    return tiles[0].AirUnit;
                }
                tiles = tileFactory.GetAdjacentTiles(protectingTile, 1, true);
                tiles = tiles.Where(t => t.GroundUnit != null).ToList();
                tiles = tiles.Where(t => t.GroundUnit.EquipmentClassEnum == EquipmentClassEnum.AirDefense).ToList();
                tiles = tiles.Where(t => t.GroundUnit.SideEnum != aggressorUnit.SideEnum).ToList();
                if (tiles.Count > 0)
                {
                    return tiles[0].GroundUnit;
                }
            }
            else if (aggressorUnit is ILandUnit)
            {
                tiles = tileFactory.GetAdjacentTiles(protectingTile, 1, true);
                tiles = tiles.Where(t => t.GroundUnit != null).ToList();
                tiles = tiles.Where(t => t.GroundUnit.EquipmentClassEnum == EquipmentClassEnum.Artillery).ToList();
                tiles = tiles.Where(t => t.GroundUnit.SideEnum != aggressorUnit.SideEnum).ToList();
                if (tiles.Count > 0)
                {
                    return tiles[0].GroundUnit;
                }
            }
            return null;
        }

        private Volley CalculateVolley(IUnit attackingUnit, IUnit defendingUnit, 
            Tile attackingTile, Tile defendingTile, TerrainCondition terrainCondition)
        {
            Volley volley = new Volley();

            volley.AttackerUnit = attackingUnit;
            volley.AttackerTile = attackingTile;
            volley.DefenderUnit = defendingUnit;
            volley.DefenderTile = defendingTile;

            int attackerAttackGrade = 0;
            int defenderDefenseGrade = 0;
            int netAttackGrade = 0;

            int attackingUnitStartingStrength = attackingUnit.CurrentStrength;
            int defendingUnitStartingStrength = defendingUnit.CurrentStrength;

            attackerAttackGrade = CalculateAttackGrade(attackingUnit, defendingUnit, defendingTile);
            defenderDefenseGrade = CalculateDefenseGrade(attackingUnit, defendingUnit, 
                attackingTile, defendingTile, terrainCondition);
            netAttackGrade = attackerAttackGrade - defenderDefenseGrade;

            SetVolleyResults(attackingUnit, defendingUnit, netAttackGrade);

            int attackingUnitStrengthLoss = attackingUnitStartingStrength - attackingUnit.CurrentStrength;
            int defendingUnitStrengthLoss = defendingUnitStartingStrength - defendingUnit.CurrentStrength;

            if (attackingUnit is ICombatUnit)
            {
                ICombatUnit combatUnit = (ICombatUnit)attackingUnit;
                combatUnit.CurrentAmmo += -1;
            }
            if (defendingUnit is ICombatUnit)
            {
                ICombatUnit combatUnit = (ICombatUnit)defendingUnit;
                combatUnit.CurrentAmmo += -1;
            }

            if (attackingUnitStrengthLoss > 0 && defendingUnitStrengthLoss > 0)
            {
                volley.VollyOutcomeEnum= VolleyOutcomeEnum.AttackerHurt_DefenderHurt;
            }
            else if (attackingUnitStrengthLoss > 0 && defendingUnitStrengthLoss == 0)
            {
                volley.VollyOutcomeEnum = VolleyOutcomeEnum.AttackerHurt_DefenderUnhurt;
            }
            else if (attackingUnitStrengthLoss == 0 && defendingUnitStrengthLoss > 0)
            {
                volley.VollyOutcomeEnum = VolleyOutcomeEnum.AttackerUnhurt_DefenderHurt;
            }
            else
            {
                volley.VollyOutcomeEnum = VolleyOutcomeEnum.AttackerUnhurt_DefenderUnhurt;
            }

            return volley;
        }
        private void SetVolleyResults(IUnit attackingUnit, IUnit defendingUnit, Int32 netGrade)
        {

            Random random = new Random();
            int rollResult = 0;
            for (int i = 0; i < attackingUnit.CurrentAttackPoints; i++)
            {
                rollResult = random.Next(20);
                if (netGrade <= 4)
                {
                    rollResult += netGrade;
                }
                else
                {
                    rollResult += 4 + ((2 / 5) * (netGrade - 4));
                }

                if (attackingUnit.EquipmentLossCalculationGroupEnum == EquipmentLossCalculationGroupEnum.Standard)
                {
                    DetermineStandardEquipmentVolleyEffect(attackingUnit, defendingUnit, rollResult);
                }
                else
                {
                    DetermineSpecialEquipmentVolleyEffect(attackingUnit, defendingUnit, rollResult);
                }
            }
        }

        private static void SetAttackPointsForVolley(IUnit unit)
        {
            if (unit is LandCombatUnit)
            {
                LandCombatUnit combatUnit = (LandCombatUnit)unit;
                if (!combatUnit.IsSuppressed)
                {
                    combatUnit.CurrentAttackPoints = combatUnit.CurrentStrength;
                }
            }
            else if (unit is SeaCombatUnit)
            {
                SeaCombatUnit combatUnit = (SeaCombatUnit)unit;
                combatUnit.CurrentAttackPoints = combatUnit.CurrentStrength;
            }
            else if (unit is AirCombatUnit)
            {
                AirCombatUnit combatUnit = (AirCombatUnit)unit;
                combatUnit.CurrentAttackPoints = combatUnit.CurrentStrength;
            }
        }
        
        private static void DetermineSpecialEquipmentVolleyEffect(IUnit attackingUnit, IUnit defendingUnit, int rollResult)
        {
            if (rollResult >= 19)
            {
                defendingUnit.CurrentStrength += -1;
                if (attackingUnit.EquipmentClassEnum == EquipmentClassEnum.StrategicBomber)
                {
                    if (attackingUnit.SideEnum == SideEnum.Axis)
                    {
                        Game.CurrentTurn.CurrentAlliedPrestige += -25;
                    }
                    else
                    {
                        Game.CurrentTurn.CurrentAxisPrestige += -25;
                    }
                }
            }
            if (rollResult >= 11)
            {
                defendingUnit.CurrentAttackPoints += -1;
                if (defendingUnit is LandCombatUnit)
                {
                    LandCombatUnit landCombatUnit = (LandCombatUnit)defendingUnit;
                    landCombatUnit.CurrentEntrenchedLevel += -1;
                    if (attackingUnit.EquipmentClassEnum == EquipmentClassEnum.StrategicBomber)
                    {
                        //landCombatUnit.CurrentAmmo += -1;
                        if (defendingUnit is MotorizedLandCombatUnit)
                        {
                            MotorizedLandCombatUnit motorizedLandCombatUnit = (MotorizedLandCombatUnit)defendingUnit;
                            //motorizedLandCombatUnit.CurrentFuel += -1;
                        }
                    }
                }
                attackingUnit.CurrentExperience += 1;
                defendingUnit.CurrentExperience += 1;
            }

            if (attackingUnit is ICombatUnit)
            {
                ICombatUnit combatUnit = (ICombatUnit)attackingUnit;
                
            }

        }
        private static void DetermineStandardEquipmentVolleyEffect(IUnit attackingUnit, IUnit defendingUnit, int rollResult)
        {
            if (rollResult >= 13)
            {
                defendingUnit.CurrentStrength += -1;
            }
            if (rollResult >= 11)
            {
                defendingUnit.CurrentAttackPoints += -1;
                if (defendingUnit is LandCombatUnit)
                {
                    LandCombatUnit landCombatUnit = (LandCombatUnit)defendingUnit;
                    landCombatUnit.CurrentEntrenchedLevel += -1;
                }
            }
            attackingUnit.CurrentExperience += 1;
            defendingUnit.CurrentExperience += 1;
            if (attackingUnit is ICombatUnit)
            {
                ICombatUnit combatUnit = (ICombatUnit)attackingUnit;
                
            }
        }

        private Int32 CalculateAttackGrade(IUnit attackingUnit, IUnit defendingUnit, Tile defendingTile)
        {
            int attackerAttackGrade = 0;
            switch (defendingUnit.TargetTypeEnum)
            {
                case TargetTypeEnum.SoftGround:
                    attackerAttackGrade = attackingUnit.Equipment.SoftAttack;
                    break;
                case TargetTypeEnum.HardGround:
                    attackerAttackGrade = attackingUnit.Equipment.HardAttack;
                    break;
                case TargetTypeEnum.Air:
                    attackerAttackGrade = attackingUnit.Equipment.AirAttack;
                    break;
                case TargetTypeEnum.Sea:
                    attackerAttackGrade = attackingUnit.Equipment.NavalAttack;
                    break;
            }
            attackerAttackGrade += attackingUnit.CurrentNumberOfStars;
            if (defendingTile.Terrain.RiverInd == true)
            {
                attackerAttackGrade += 4;
            }
            if (attackingUnit is IMotorizedUnit)
            {
                IMotorizedUnit motorizedUnit = (IMotorizedUnit)attackingUnit;
                if (motorizedUnit.CurrentFuel == 0)
                {
                    attackerAttackGrade = attackerAttackGrade / 2;
                }
            }

            return attackerAttackGrade;
        }
        private Int32 CalculateDefenseGrade(IUnit attackingUnit, IUnit defendingUnit, 
            Tile attackingTile, Tile defendingTile, TerrainCondition terrainCondition)
        {
            int defenderDefenseGrade = 0;
            if (attackingUnit is IAirUnit)
            {
                defenderDefenseGrade = defendingUnit.Equipment.AirDefense;
            }
            else
            {
                defenderDefenseGrade = defendingUnit.Equipment.GroundDefense;
            }
            defenderDefenseGrade += defendingUnit.CurrentNumberOfStars;
            if (attackingTile.Terrain.RiverInd == true)
            {
                defenderDefenseGrade += 4;
            }
            if (CalculateRuggedDefense(attackingUnit, defendingUnit))
            {
                defenderDefenseGrade += 4;
            }
            defenderDefenseGrade = DetermineDefenseGradeEntrenchment(attackingUnit, defendingUnit, defenderDefenseGrade);
            defenderDefenseGrade = DetermineDefenseGradeOverrides(attackingUnit, defendingUnit, terrainCondition, defenderDefenseGrade);

            return defenderDefenseGrade;
        }

        private bool CalculateRuggedDefense(IUnit attackingUnit, IUnit defendingUnit)
        {
            if (defendingUnit is LandCombatUnit)
            {
                LandCombatUnit defendingLandCombatUnit = (LandCombatUnit)defendingUnit;
                Double experienceRatio = (defendingLandCombatUnit.CurrentExperience + 2) / (attackingUnit.CurrentExperience + 2);
                Double entrenchmentRatio = (defendingLandCombatUnit.Equipment.EquipmentSubClass.EquipmentClass.EntrenchmentRate + 1) / (attackingUnit.Equipment.EquipmentSubClass.EquipmentClass.EntrenchmentRate + 1);
                Double ruggedDefenseChance = defendingLandCombatUnit.CurrentEntrenchedLevel * experienceRatio * entrenchmentRatio * .05;
                Random random = new Random();
                int diceRoll = random.Next(100);
                if (ruggedDefenseChance > diceRoll)
                    return true;
                else
                    return false;
            }
            return false;
        }
        private int DetermineDefenseGradeEntrenchment(IUnit attackingUnit, IUnit defendingUnit, int defenderDefenseGrade)
        {
            if (!attackingUnit.Equipment.IgnoresEntrenchment)
            {
                if (defendingUnit is LandCombatUnit)
                {
                    LandCombatUnit defendingLandCombatUnit = (LandCombatUnit)defendingUnit;
                    if (attackingUnit.EquipmentClassEnum == EquipmentClassEnum.Infantry)
                    {
                        defenderDefenseGrade += (Int32)(defendingLandCombatUnit.CurrentEntrenchedLevel * .5);
                    }
                    else
                    {
                        defenderDefenseGrade += defendingLandCombatUnit.CurrentEntrenchedLevel;
                    }
                }
            }
            return defenderDefenseGrade;
        }
        private int DetermineDefenseGradeOverrides(IUnit attackingUnit, IUnit defendingUnit, 
            TerrainCondition terrainCondition, int defenderDefenseGrade)
        {
            if (attackingUnit is LandCombatUnit && defendingUnit is ISeaUnit)
            {
                defenderDefenseGrade += 8;
            }

            if (attackingUnit.EquipmentClassEnum == EquipmentClassEnum.Artillery && 
                terrainCondition.TerrainConditionEnum != TerrainConditionEnum.Dry)
            {
                defenderDefenseGrade += 3;
            }
            if (defendingUnit.EquipmentClassEnum == EquipmentClassEnum.Artillery)
            {
                defenderDefenseGrade += 3;
            }
            if (attackingUnit.EquipmentClassEnum == EquipmentClassEnum.Infantry && 
                defendingUnit.EquipmentSubClassEnum == EquipmentSubClassEnum.TowedLightAntiTank)
            {
                defenderDefenseGrade += 2;
            }
            if (attackingUnit.EquipmentClassEnum == EquipmentClassEnum.Infantry && 
                defendingUnit.EquipmentSubClassEnum == EquipmentSubClassEnum.TowedHeavyAntiTank)
            {
                defenderDefenseGrade += 2;
            }
            return defenderDefenseGrade;
        }

        private InitativeEnum CalculateInitiative(IUnit attackingUnit, IUnit defendingUnit, Tile battlefield, bool surprised)
        {
            TerrainType terrainType = battlefield.Terrain.TerrainType;
            int attackingInitiative = DetermineInitativeCaps(attackingUnit.Equipment.Initative, terrainType.InitiativeCap);
            int defendingInitiative = DetermineInitativeCaps(defendingUnit.Equipment.Initative, terrainType.InitiativeCap);
            int attackingExperienceBonus = CalculateExperienceBonusForInitiative(attackingUnit.CurrentNumberOfStars);
            int defendingExperienceBonus = CalculateExperienceBonusForInitiative(defendingUnit.CurrentNumberOfStars);
            attackingInitiative = attackingInitiative + attackingExperienceBonus;
            defendingInitiative = defendingInitiative + defendingExperienceBonus;

            attackingInitiative = DetermineAttackingInitativeOverrides(attackingUnit, defendingUnit, attackingInitiative, defendingInitiative);
            defendingInitiative = DetermineDefendingInitativeOverrides(attackingUnit, defendingUnit, attackingInitiative, defendingInitiative);

            if (surprised)
                attackingInitiative = 0;

            Random random = new Random();
            attackingInitiative = attackingInitiative + random.Next(3);
            defendingInitiative = defendingInitiative + random.Next(3);

            if (attackingInitiative > defendingInitiative)
                return InitativeEnum.AttackerStrikesFirst;
            else if (defendingInitiative > attackingInitiative)
                return InitativeEnum.DefenderStrikesFirst;
            else
                return InitativeEnum.Simultanous;
        }
        private Int32 DetermineAttackingInitativeOverrides(IUnit attackingUnit, IUnit defendingUnit, int attackingInitative, int defendingInitative)
        {
            if (attackingUnit.EquipmentClassEnum == EquipmentClassEnum.Submarine && defendingUnit.EquipmentClassEnum == EquipmentClassEnum.CapitalShip)
            {
                return 99;
            }
            if (attackingUnit.EquipmentClassEnum == EquipmentClassEnum.Submarine && defendingUnit.EquipmentClassEnum == EquipmentClassEnum.AircraftCarrier)
            {
                return 99;
            }
            if (attackingUnit.EquipmentClassEnum == EquipmentClassEnum.Submarine && defendingUnit.EquipmentClassEnum == EquipmentClassEnum.SeaTransport)
            {
                return 99;
            }
            if (attackingUnit.EquipmentSubClassEnum == EquipmentSubClassEnum.TankDestroyer && defendingUnit.EquipmentClassEnum == EquipmentClassEnum.Tank)
            {
                return 99;
            }
            if (attackingUnit.EquipmentClassEnum == EquipmentClassEnum.Fighter && defendingUnit.EquipmentClassEnum == EquipmentClassEnum.AirDefense)
            {
                return defendingInitative;
            }
            if (attackingUnit.EquipmentClassEnum == EquipmentClassEnum.TacticalBomber && defendingUnit.EquipmentClassEnum == EquipmentClassEnum.AirDefense)
            {
                return defendingInitative;
            }
            if (attackingUnit.EquipmentClassEnum == EquipmentClassEnum.StrategicBomber && defendingUnit.EquipmentClassEnum == EquipmentClassEnum.AirDefense)
            {
                return defendingInitative;
            }
            return attackingInitative;
        }
        private Int32 DetermineDefendingInitativeOverrides(IUnit attackingUnit, IUnit defendingUnit, int attackingInitative, int defendingInitative)
        {
            if (attackingUnit.EquipmentClassEnum == EquipmentClassEnum.Submarine && defendingUnit.EquipmentClassEnum == EquipmentClassEnum.CapitalShip)
            {
                return 0;
            }
            if (attackingUnit.EquipmentClassEnum == EquipmentClassEnum.Submarine && defendingUnit.EquipmentClassEnum == EquipmentClassEnum.AircraftCarrier)
            {
                return 0;
            }
            if (attackingUnit.EquipmentClassEnum == EquipmentClassEnum.Submarine && defendingUnit.EquipmentClassEnum == EquipmentClassEnum.SeaTransport)
            {
                return 0;
            }
            if (attackingUnit.EquipmentSubClassEnum == EquipmentSubClassEnum.TankDestroyer && defendingUnit.EquipmentClassEnum == EquipmentClassEnum.Tank)
            {
                return 0;
            }
            return defendingInitative;
        }
        private int DetermineInitativeCaps(int equipmentInitative, int terrainInitative)
        {
            int returnValue = 0;
            if (equipmentInitative < terrainInitative)
            {
                returnValue = equipmentInitative;
            }
            else
            {
                returnValue = terrainInitative;
            }
            return returnValue;
        }
        private Int32 CalculateExperienceBonusForInitiative(Int32 numberOfStars)
        {
            int returnValue = 0;
            switch (numberOfStars)
            {
                case 0:
                    returnValue = 0;
                    break;
                case 1:
                    returnValue = 1;
                    break;
                case 2:
                    returnValue = 1;
                    break;
                case 3:
                    returnValue = 2;
                    break;
                case 4:
                    returnValue = 2;
                    break;
                case 5:
                    returnValue = 3;
                    break;
                default:
                    returnValue = 3;
                    break;
            }
            return returnValue;
        }



    }
}

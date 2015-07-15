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
using System.Xml.Linq;
using System.Linq;
using System.Windows.Resources;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using Tff.Panzer.Models.Scenario;
using Tff.Panzer.Models;
using Tff.Panzer.Models.Army.Unit;


namespace Tff.Panzer.Factories.Army
{
    public class UnitFactory
    {
        public EquipmentFactory EquipmentFactory { get; private set; }

        public UnitFactory()
        {
            EquipmentFactory = new EquipmentFactory();
        }

        public IUnit CreateUnit(int unitId, ScenarioUnit scenarioUnit)
        {
            Equipment equipment = EquipmentFactory.GetEquipment(scenarioUnit.EquipmentId);
            switch(equipment.EquipmentGroupEnum)
            {
                case EquipmentGroupEnum.Land:
                    if (equipment.MovementType.IsMotorized)
                    {
                        MotorizedLandCombatUnit motorizedLandCombatUnit = CreateMotorizedLandCombatUnit(unitId, scenarioUnit);
                        if (scenarioUnit.TransportId != 0)
                        {
                            LandTransportUnit landTransportUnit = CreateLandTransport(unitId, scenarioUnit.TransportId, motorizedLandCombatUnit.Nation, motorizedLandCombatUnit.CoreIndicator,scenarioUnit.StartingScenarioTileId);
                            landTransportUnit.LandCombatUnit = motorizedLandCombatUnit;
                            motorizedLandCombatUnit.TransportUnit = landTransportUnit;
                        }
                        return motorizedLandCombatUnit;
                    }
                    else
                    {
                        LandCombatUnit landCombatUnit = CreateLandCombatUnit(unitId, scenarioUnit);
                        if (scenarioUnit.TransportId != 0)
                        {
                            LandTransportUnit landTransportUnit = CreateLandTransport(unitId, scenarioUnit.TransportId, landCombatUnit.Nation, landCombatUnit.CoreIndicator, scenarioUnit.StartingScenarioTileId);
                            landTransportUnit.LandCombatUnit = landCombatUnit;
                            landCombatUnit.TransportUnit = landTransportUnit;
                        }
                        return landCombatUnit;
                    }
                case EquipmentGroupEnum.Air:
                    return CreateAirCombatUnit(unitId, scenarioUnit);
                case EquipmentGroupEnum.Sea:
                    return CreateSeaCombatUnit(unitId, scenarioUnit);
            }
            return null;
        }

        public IUnit CreateDefaultUnit(int unitId, Int32 equipmentId, Int32 transportEquipmentId, Int32 nationId)
        {
            ScenarioUnit scenarioUnit = new ScenarioUnit();
            scenarioUnit.EquipmentId = equipmentId;
            scenarioUnit.TransportId = transportEquipmentId;
            scenarioUnit.NationId = nationId;
            scenarioUnit.Strength = 10;
            return CreateUnit(unitId, scenarioUnit);
        }

        public LandCombatUnit CreateLandCombatUnit(int unitId, ScenarioUnit scenarioUnit)
        {
            LandCombatUnit unit = new LandCombatUnit();
            unit.UnitId = unitId;
            unit.Equipment = EquipmentFactory.GetEquipment(scenarioUnit.EquipmentId);
            unit.UnitName = String.Format("{0} {1}", unitId, unit.Equipment.EquipmentDescription);
            unit.CoreIndicator = scenarioUnit.CordInd;
            unit.CurrentExperience = scenarioUnit.Experience;
            unit.CurrentStrength = scenarioUnit.Strength;
            unit.CurrentAttackPoints = scenarioUnit.Strength;
            unit.Nation = Game.NationFactory.GetNation(scenarioUnit.NationId);
            unit.CoreIndicator = scenarioUnit.CordInd;
            unit.CurrentAmmo = unit.Equipment.MaxAmmo;
            unit.CurrentEntrenchedLevel = 0;
            unit.CanMove = true;
            unit.CanAttack = true;
            unit.CurrentTileId = scenarioUnit.StartingScenarioTileId;
            return unit;

        }

        public MotorizedLandCombatUnit CreateMotorizedLandCombatUnit(int unitId, ScenarioUnit scenarioUnit)
        {
            MotorizedLandCombatUnit unit = new MotorizedLandCombatUnit();
            unit.UnitId = unitId;
            unit.Equipment = EquipmentFactory.GetEquipment(scenarioUnit.EquipmentId);
            unit.UnitName = String.Format("{0} {1}", unitId, unit.Equipment.EquipmentDescription);
            unit.CoreIndicator = scenarioUnit.CordInd;
            unit.CurrentExperience = scenarioUnit.Experience;
            unit.CurrentStrength = scenarioUnit.Strength;
            unit.CurrentAttackPoints = scenarioUnit.Strength;
            unit.Nation = Game.NationFactory.GetNation(scenarioUnit.NationId);
            unit.CoreIndicator = scenarioUnit.CordInd;
            unit.CurrentAmmo = unit.Equipment.MaxAmmo;
            unit.CurrentFuel = unit.Equipment.MaxFuel;
            unit.CurrentEntrenchedLevel = 0;
            unit.CanMove = true;
            unit.CanAttack = true;
            unit.CurrentTileId = scenarioUnit.StartingScenarioTileId;

            return unit;
        }

        public SeaCombatUnit CreateSeaCombatUnit(int unitId, ScenarioUnit scenarioUnit)
        {
            SeaCombatUnit unit = new SeaCombatUnit();
            unit.UnitId = unitId;
            unit.Equipment = EquipmentFactory.GetEquipment(scenarioUnit.EquipmentId);
            unit.UnitName = String.Format("{0} {1}", unitId, unit.Equipment.EquipmentDescription);
            unit.CoreIndicator = scenarioUnit.CordInd;
            unit.CurrentExperience = scenarioUnit.Experience;
            unit.CurrentStrength = scenarioUnit.Strength;
            unit.CurrentAttackPoints = scenarioUnit.Strength;
            unit.Nation = Game.NationFactory.GetNation(scenarioUnit.NationId);
            unit.CoreIndicator = scenarioUnit.CordInd;
            unit.CurrentAmmo = unit.Equipment.MaxAmmo;
            unit.CurrentFuel = unit.Equipment.MaxFuel;
            unit.CanMove = true;
            unit.CanAttack = true;
            unit.CurrentTileId = scenarioUnit.StartingScenarioTileId;

            return unit;
        }

        public AirCombatUnit CreateAirCombatUnit(int unitId, ScenarioUnit scenarioUnit)
        {
            AirCombatUnit unit = new AirCombatUnit();
            unit.UnitId = unitId;
            unit.Equipment = EquipmentFactory.GetEquipment(scenarioUnit.EquipmentId);
            unit.UnitName = String.Format("{0} {1}", unitId, unit.Equipment.EquipmentDescription);
            unit.CoreIndicator = scenarioUnit.CordInd;
            unit.CurrentExperience = scenarioUnit.Experience;
            unit.CurrentStrength = scenarioUnit.Strength;
            unit.CurrentAttackPoints = scenarioUnit.Strength;
            unit.Nation = Game.NationFactory.GetNation(scenarioUnit.NationId);
            unit.CoreIndicator = scenarioUnit.CordInd;
            unit.CurrentAmmo = unit.Equipment.MaxAmmo;
            unit.CurrentFuel = unit.Equipment.MaxFuel;
            unit.CanMove = true;
            unit.CanAttack = true;
            unit.CurrentTileId = scenarioUnit.StartingScenarioTileId;

            return unit;
        }

        public LandTransportUnit CreateLandTransport(int unitId, int equipmentId, Nation nation, bool coreInd, int startingTileId)
        {
            LandTransportUnit unit = new LandTransportUnit();
            unit.UnitId = unitId;
            unit.Equipment = EquipmentFactory.GetEquipment(equipmentId);
            unit.UnitName = String.Format("{0} {1}", unitId, unit.Equipment.EquipmentDescription);
            unit.CurrentStrength = 10;
            unit.CurrentExperience = 0;
            if (unit.Equipment.MaxFuel == 0)
            {
                unit.CurrentFuel = 99;
            }
            else
            {
                unit.CurrentFuel = unit.Equipment.MaxFuel;
            }
            unit.CoreIndicator = coreInd;
            unit.Nation = nation;
            unit.CanMove = true;
            unit.CurrentTileId = startingTileId;


            return unit;
        }

        public AirTransportUnit CreateAirTransportUnit(int unitId, Nation nation, int startingTileId)
        {
            int equipmentId = 0;
            AirTransportUnit unit = new AirTransportUnit();

            switch (nation.NationEnum)
            {
                case NationEnum.German:
                    equipmentId = 29;
                    break;
                case NationEnum.GreatBritain:
                    equipmentId = 178;
                    break;
                case NationEnum.Italy:
                    equipmentId = 309;
                    break;
                case NationEnum.UnitedStates:
                    equipmentId = 354;
                    break;
                default:
                    if (Game.NationFactory.SideFactory.GetSideForANation(nation.NationId).SideEnum == SideEnum.Axis)
                    {
                        equipmentId = 29;
                    }
                    else
                    {
                        equipmentId = 178;
                    }
                    break;
            }

            unit.UnitId = unitId;
            unit.Equipment = EquipmentFactory.GetEquipment(equipmentId);
            unit.UnitName = String.Format("{0} {1}", unitId, unit.Equipment.EquipmentDescription);
            unit.Nation = nation;
            unit.CurrentStrength = 10;
            unit.CurrentExperience = 0;
            unit.CurrentFuel = 99;
            unit.CanMove = true;
            unit.CurrentTileId = startingTileId;
            return unit;
        }

        public SeaTransportUnit CreateSeaTransportUnit(int unitId, Nation nation, int startingTileId)
        {
            SeaTransportUnit unit = new SeaTransportUnit();
            int equipmentId = 0;
            if (nation.SideEnum == SideEnum.Axis)
            {
                equipmentId = 299;
            }
            else
            {
                equipmentId = 291;
            }

            unit.UnitId = unitId;
            unit.Equipment = EquipmentFactory.GetEquipment(equipmentId);
            unit.UnitName = String.Format("{0} {1}", unitId, unit.Equipment.EquipmentDescription);
            unit.Nation = nation;
            unit.CurrentStrength = 10;
            unit.CurrentExperience = 0;
            unit.CurrentFuel = 99;
            unit.CanMove = true;
            unit.CurrentTileId = startingTileId;

            return unit;

            
        }

        public LandCombatUnit CreateDefaultAntiTankUnit(int unitId, Nation nation, int startingTileId)
        {
            LandCombatUnit unit = new LandCombatUnit();
            List<Equipment> equipments = EquipmentFactory.Equipments.Where(e => e.Nation == nation).ToList();
            Equipment equipment = equipments.Where(eq => eq.UnitCost == equipments.Min(e => e.UnitCost)).FirstOrDefault();

            unit.UnitId = unitId;
            unit.Equipment = equipment;
            unit.UnitName = String.Format("{0} {1}", unitId, unit.Equipment.EquipmentDescription);
            unit.Nation = nation;
            unit.CurrentStrength = 10;
            unit.CurrentExperience = 0;
            unit.CanMove = true;
            unit.CurrentTileId = startingTileId;

            if (unit.SideEnum == SideEnum.Axis)
            {
                Game.CurrentTurn.CurrentAxisPrestige = Game.CurrentTurn.CurrentAxisPrestige - unit.Equipment.UnitCost;
            }
            else
            {
                Game.CurrentTurn.CurrentAlliedPrestige = Game.CurrentTurn.CurrentAlliedPrestige - unit.Equipment.UnitCost;
            }

            return unit;

        }

        public void ResupplyUnit(IUnit unit)
        {
            if (unit is ICombatUnit)
            {
                ICombatUnit combatUnit = (ICombatUnit)unit;
                combatUnit.CurrentAmmo = unit.Equipment.MaxAmmo;
            }
            if (unit is IMotorizedUnit)
            {
                IMotorizedUnit motorizedUnit = (IMotorizedUnit)unit;
                motorizedUnit.CurrentFuel = unit.Equipment.MaxFuel;
            }
        }

        public void ReinforceUnit(IUnit unit, bool useEliteReplacements)
        {
            int numberOfStrengthDesiredToReinforce = 0;
            if (unit.CurrentStrength < 10)
            {
                numberOfStrengthDesiredToReinforce = 10 - unit.CurrentStrength;
            }
            else
            {
                numberOfStrengthDesiredToReinforce = 1;
            }

            int reinforceCost = unit.Equipment.UnitCost/10;
            if (useEliteReplacements)
            {
                reinforceCost = reinforceCost * 2;
            }

            int numberOfStrengthAvailableToReinforce = 0;
            if(unit.SideEnum == SideEnum.Axis)
            {
                numberOfStrengthAvailableToReinforce = Game.CurrentTurn.CurrentAxisPrestige/reinforceCost;
            }
            else
            {
                numberOfStrengthAvailableToReinforce = Game.CurrentTurn.CurrentAlliedPrestige/reinforceCost;
            }

            int numberOfStrengthToReinforce = 0;
            if (numberOfStrengthDesiredToReinforce >= numberOfStrengthAvailableToReinforce)
            {
                numberOfStrengthToReinforce = numberOfStrengthAvailableToReinforce;
            }
            else
            {
                numberOfStrengthToReinforce = numberOfStrengthDesiredToReinforce;
            }

            int totalCostToReinforce = numberOfStrengthToReinforce * reinforceCost;
            if (unit.SideEnum == SideEnum.Axis)
            {
                Game.CurrentTurn.CurrentAxisPrestige = Game.CurrentTurn.CurrentAxisPrestige - totalCostToReinforce;
            }
            else
            {
                Game.CurrentTurn.CurrentAlliedPrestige = Game.CurrentTurn.CurrentAlliedPrestige - totalCostToReinforce;
            }

            ResupplyUnit(unit);
            unit.CurrentStrength += numberOfStrengthToReinforce;
            if (useEliteReplacements == false)
            {
                int experienceUnit = unit.CurrentExperience / 10;
                int experienceCost = (Int32)numberOfStrengthToReinforce* experienceUnit;
                unit.CurrentExperience = unit.CurrentExperience - experienceCost;
            }

        }

    }
}

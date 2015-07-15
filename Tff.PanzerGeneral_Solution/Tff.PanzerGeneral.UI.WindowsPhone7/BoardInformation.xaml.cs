using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Tff.Panzer.Models.Army.Unit;
using Tff.Panzer.Models.Army;
using System.Windows.Media.Imaging;
using Tff.Panzer.Models;
using Tff.Panzer.Controls;
//TOM

namespace Tff.Panzer
{
    public partial class BoardInformation : PhoneApplicationPage
    {
        int currentGameViewLevel = Game.CurrentViewLevel;

        public BoardInformation()
        {
            InitializeComponent();
            currentGameViewLevel = Game.CurrentViewLevel;
            
            if (Game.CurrentUnit != null && Game.BoardFactory.ActiveTile != null)
            {
                DisplayUnitInformation(Game.CurrentUnit, Game.BoardFactory.ActiveTile);
            }
            if (Game.CurrentUnit == null && Game.BoardFactory.ActiveTile != null)
            {
                DisplayTileInformation(Game.BoardFactory.ActiveTile);
            }
        }

        private void DisplayTileInformation(Tile tile)
        {
            Game.CurrentViewLevel = 3;
            this.title.Text = tile.TileName.TileNameDescription;
            this.subtitle.Text = String.Format("{0} - {1}", tile.Terrain.TerrainTypeEnum, Game.BoardFactory.ActiveTile.Terrain.TerrainGroupEnum);
            AddImageToInformationScreen(null,tile);
            SetTextBlock("Victory Indicator", tile.VictoryIndicator.ToString());
            SetTextBlock("Supply Indicator", tile.SupplyIndicator.ToString());
            if (tile.Nation != null)
            {
                SetTextBlock("Nation", tile.Nation.NationEnum.ToString());
            }
            this.UpgradeButton.Visibility = System.Windows.Visibility.Collapsed;
            this.RetireButton.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void DisplayUnitInformation(IUnit unit, Tile tile)
        {
            Game.CurrentViewLevel = 3;
            this.title.Text = unit.UnitName;
            this.subtitle.Text = String.Format("{0} - {1} Target", unit.EquipmentSubClassEnum.ToString(), unit.TargetTypeEnum.ToString());
            AddImageToInformationScreen(unit, tile);
            if (unit is LandCombatUnit)
            {
                DisplayLandCombatUnitInformation();
            }
            if (unit is AirCombatUnit)
            {
                DisplayAirCombatUnitInformation();
            }
            if (unit is SeaCombatUnit)
            {
                DisplaySeaCombatUnitInformation();
            }
            if (unit is ITransportUnit)
            {
                DisplayTranportUnitInformation();
            }


            this.UpgradeButton.Visibility = System.Windows.Visibility.Collapsed;
            this.RetireButton.Visibility = System.Windows.Visibility.Collapsed;


        }
        
        private void DisplayLandCombatUnitInformation()
        {
            LandCombatUnit landCombatUnit = (LandCombatUnit)Game.CurrentUnit;
            SetTextBlock("Experience", landCombatUnit.CurrentExperience.ToString());
            SetTextBlock("Strength", landCombatUnit.CurrentStrength.ToString());
            SetTextBlock("Ammo", landCombatUnit.CurrentAmmo.ToString());
            if (landCombatUnit is IMotorizedUnit)
            {
                IMotorizedUnit motorizedUnit = (IMotorizedUnit)landCombatUnit;
                SetTextBlock("Fuel", motorizedUnit.CurrentFuel.ToString());
            }
            SetTextBlock("Movement Range", landCombatUnit.Equipment.BaseMovement.ToString());
            SetTextBlock("Entrenchment", landCombatUnit.CurrentEntrenchedLevel.ToString());
            if (landCombatUnit.TransportUnit != null)
            {
                SetTextBlock("Transport", landCombatUnit.TransportUnit.UnitName.ToString());
            }
            if(landCombatUnit.EquipmentClassEnum == EquipmentClassEnum.AirDefense ||
                landCombatUnit.EquipmentClassEnum == EquipmentClassEnum.Artillery)
            {
                SetTextBlock("Attack Range", landCombatUnit.Equipment.Range.ToString());
            }
            SetTextBlock("Hard Attack", landCombatUnit.Equipment.HardAttack.ToString());
            SetTextBlock("Soft Attack", landCombatUnit.Equipment.SoftAttack.ToString());
            SetTextBlock("Air Attack", landCombatUnit.Equipment.AirAttack.ToString());
            SetTextBlock("Sea Attack", landCombatUnit.Equipment.NavalAttack.ToString());
            SetTextBlock("Ground Defense", landCombatUnit.Equipment.GroundDefense.ToString());
            SetTextBlock("Air Defense", landCombatUnit.Equipment.AirDefense.ToString());
            SetTextBlock("Sea Defense", landCombatUnit.Equipment.SeaDefense.ToString());
            SetTextBlock("Can Move", landCombatUnit.CanMove.ToString());
            SetTextBlock("Can Attack", landCombatUnit.CanAttack.ToString());
        }
        private void DisplaySeaCombatUnitInformation()
        {
            SeaCombatUnit seaCombatUnit = (SeaCombatUnit)Game.CurrentUnit;
            SetTextBlock("Experience", seaCombatUnit.CurrentExperience.ToString());
            SetTextBlock("Strength", seaCombatUnit.CurrentStrength.ToString());
            SetTextBlock("Ammo", seaCombatUnit.CurrentAmmo.ToString());
            SetTextBlock("Fuel", seaCombatUnit.CurrentFuel.ToString());
            SetTextBlock("Movement Range", seaCombatUnit.Equipment.BaseMovement.ToString());
            SetTextBlock("Attack Range", seaCombatUnit.Equipment.AttackRange.ToString());
            SetTextBlock("Hard Attack", seaCombatUnit.Equipment.HardAttack.ToString());
            SetTextBlock("Soft Attack", seaCombatUnit.Equipment.SoftAttack.ToString());
            SetTextBlock("Air Attack", seaCombatUnit.Equipment.AirAttack.ToString());
            SetTextBlock("Sea Attack", seaCombatUnit.Equipment.NavalAttack.ToString());
            SetTextBlock("Ground Defense", seaCombatUnit.Equipment.GroundDefense.ToString());
            SetTextBlock("Air Defense", seaCombatUnit.Equipment.AirDefense.ToString());
            SetTextBlock("Sea Defense", seaCombatUnit.Equipment.SeaDefense.ToString());
            SetTextBlock("Can Move", seaCombatUnit.CanMove.ToString());
            SetTextBlock("Can Attack", seaCombatUnit.CanAttack.ToString());
        }
        private void DisplayAirCombatUnitInformation()
        {
            AirCombatUnit airCombatUnit = (AirCombatUnit)Game.CurrentUnit;
            SetTextBlock("Experience", airCombatUnit.CurrentExperience.ToString());
            SetTextBlock("Strength", airCombatUnit.CurrentStrength.ToString());
            SetTextBlock("Ammo", airCombatUnit.CurrentAmmo.ToString());
            SetTextBlock("Fuel", airCombatUnit.CurrentFuel.ToString());
            SetTextBlock("Movement Range", airCombatUnit.Equipment.BaseMovement.ToString());
            SetTextBlock("Hard Attack", airCombatUnit.Equipment.HardAttack.ToString());
            SetTextBlock("Soft Attack", airCombatUnit.Equipment.SoftAttack.ToString());
            SetTextBlock("Air Attack", airCombatUnit.Equipment.AirAttack.ToString());
            SetTextBlock("Sea Attack", airCombatUnit.Equipment.NavalAttack.ToString());
            SetTextBlock("Ground Defense", airCombatUnit.Equipment.GroundDefense.ToString());
            SetTextBlock("Air Defense", airCombatUnit.Equipment.AirDefense.ToString());
            SetTextBlock("Sea Defense", airCombatUnit.Equipment.SeaDefense.ToString());
            SetTextBlock("Can Move", airCombatUnit.CanMove.ToString());
            SetTextBlock("Can Attack", airCombatUnit.CanAttack.ToString());
        }
        private void DisplayTranportUnitInformation()
        {
            ITransportUnit transportUnit = (ITransportUnit)Game.CurrentUnit;
            SetTextBlock("Transporting Unit", transportUnit.LandCombatUnit.UnitName);
            SetTextBlock("Strength", transportUnit.CurrentStrength.ToString());
            if (transportUnit is IMotorizedUnit)
            {
                IMotorizedUnit motorizedUnit = (IMotorizedUnit)transportUnit;
                SetTextBlock("Fuel", motorizedUnit.CurrentFuel.ToString());
            }
            SetTextBlock("Movement Range", transportUnit.Equipment.BaseMovement.ToString());
            SetTextBlock("Ground Defense", transportUnit.Equipment.GroundDefense.ToString());
            SetTextBlock("Air Defense", transportUnit.Equipment.AirDefense.ToString());
            SetTextBlock("Sea Defense", transportUnit.Equipment.SeaDefense.ToString());
            SetTextBlock("Can Move", transportUnit.CanMove.ToString());
        }

        private void SetTextBlock(string name, string value)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = String.Format("{0} : {1}",name,value);
            this.detailsStackPanel.Children.Add(textBlock);
        }
        private void AddImageToInformationScreen(IUnit unit, Tile tile)
        {
            Hex hex = Game.HexFactory.GetHex(tile);
            if (unit != null)
            {
                Game.HexFactory.AddUnit(hex, unit);
            }
            this.image.Children.Add(hex);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Game.CurrentViewLevel = currentGameViewLevel;
            this.NavigationService.GoBack(); 
        }

        private void UpgradeButton_Click(object sender, RoutedEventArgs e)
        {
            Game.UnitUpgrade = true;
            this.NavigationService.Navigate(new Uri(@"/PurchaseUnit.xaml", UriKind.Relative));
        }

        private void RetireButton_Click(object sender, RoutedEventArgs e)
        {

            if (Game.InCampaign)
            {
                Game.CampaignArmy.Remove(Game.CurrentUnit);
            }
            this.NavigationService.GoBack();
        }
    }
}
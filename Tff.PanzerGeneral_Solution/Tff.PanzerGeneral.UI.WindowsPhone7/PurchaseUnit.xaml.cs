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
using Tff.Panzer.Models.Army;
using Tff.Panzer.Models;
using Tff.Panzer.Models.Geography;
using Tff.Panzer.Models.Army.Unit;
using Tff.Panzer.Controls;

namespace Tff.Panzer
{
    public partial class PurchaseUnit : PhoneApplicationPage
    {
        IUnit unitToBePurchased = null;
        LandTransportUnit transportUnitToBePurchased = null;
        public Boolean ResetListBox { get; set; }

        public PurchaseUnit()
        {
            InitializeComponent();

            if (Game.UnitUpgrade == true)
            {
                this.transportButton.Visibility = System.Windows.Visibility.Collapsed;
                this.airButton.Visibility = System.Windows.Visibility.Collapsed;
                this.landButton.Visibility = System.Windows.Visibility.Collapsed;
                EquipmentClassEnum equipmentClassEnum = (EquipmentClassEnum)Game.CurrentUnit.EquipmentClassEnum;
                LoadCombatEquipmentBasedOnEquipmentClass(equipmentClassEnum);
            }
            else
            {
                if (Game.BoardFactory.ActiveTile != null)
                {
                    this.airButton.Visibility = System.Windows.Visibility.Collapsed;
                    this.landButton.Visibility = System.Windows.Visibility.Collapsed;
                    this.transportButton.Visibility = System.Windows.Visibility.Collapsed;

                    TerrainTypeEnum terrainTypeEnum = Game.BoardFactory.ActiveTile.Terrain.TerrainTypeEnum;
                    if (terrainTypeEnum == TerrainTypeEnum.Airfield)
                    {
                        ResetPageForAirCombatUnit();
                    }
                    else if (terrainTypeEnum == TerrainTypeEnum.City)
                    {
                        ResetPageForLandCombatUnit();
                    }
                    else if (terrainTypeEnum == TerrainTypeEnum.Port)
                    {
                        ResetPageForLandCombatUnit();
                    }
                }
                else
                {
                    this.landButton.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    this.landButton.Visibility = System.Windows.Visibility.Visible;
                    this.transportButton.Visibility = System.Windows.Visibility.Collapsed;
                    this.airButton.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    this.airButton.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void LandButton_Click(object sender, RoutedEventArgs e)
        {
            ResetListBox = true;
            ResetPageForLandCombatUnit();
            LoadEquipmentClassListBox(EquipmentGroupEnum.Land);
        }
        private void AirButton_Click(object sender, RoutedEventArgs e)
        {
            ResetPageForAirCombatUnit();
            LoadEquipmentClassListBox(EquipmentGroupEnum.Air);

        }

        private void ResetPageForLandCombatUnit()
        {
            equipmentClassListBox.Items.Clear();
            this.combatEquipmentListBox.Items.Clear();
            this.transportEquipmentListBox.Items.Clear();
            this.combatUnitImage.Children.Clear();
            this.transportUnitImage.Children.Clear();
            this.combatUnitStackPanel.Children.Clear();
            this.transportUnitStackPanel.Children.Clear();

            this.landButton.Visibility = System.Windows.Visibility.Visible;                
            equipmentClassListBox.Visibility = System.Windows.Visibility.Visible;
            combatEquipmentListBox.Visibility = System.Windows.Visibility.Collapsed;
            transportEquipmentListBox.Visibility = System.Windows.Visibility.Collapsed;
            combatUnitScrollViewer.Visibility = System.Windows.Visibility.Collapsed;
            transportUnitScrollViewer.Visibility = System.Windows.Visibility.Collapsed;
            this.purchaseCombatUnitButton.Visibility = System.Windows.Visibility.Collapsed;
            this.purchaseTransportUnitButton.Visibility = System.Windows.Visibility.Collapsed;
            this.transportButton.Visibility = System.Windows.Visibility.Collapsed;

            ResetListBox = false;
        }
        private void ResetPageForAirCombatUnit()
        {
            equipmentClassListBox.Items.Clear();
            combatEquipmentListBox.Items.Clear();
            transportEquipmentListBox.Items.Clear();
            this.combatUnitImage.Children.Clear();
            this.combatUnitStackPanel.Children.Clear();

            this.airButton.Visibility = System.Windows.Visibility.Visible;
            equipmentClassListBox.Visibility = System.Windows.Visibility.Visible;
            combatEquipmentListBox.Visibility = System.Windows.Visibility.Visible;
            combatUnitScrollViewer.Visibility = System.Windows.Visibility.Collapsed;
            this.purchaseCombatUnitButton.Visibility = System.Windows.Visibility.Collapsed;


        }

        private void LoadEquipmentClassListBox(EquipmentGroupEnum equipmentGroupEnum)
        {
            List<EquipmentClass> equipmentClasses = Game.UnitFactory.EquipmentFactory.EquipmentSubClassFactory.EquipmentClassFactory.EquipmentClasses;
            equipmentClasses = equipmentClasses.Where(ec => ec.EquipmentGroupEnum == equipmentGroupEnum).ToList();
            foreach (EquipmentClass equipmentClass in equipmentClasses)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = equipmentClass.ToString();
                textBlock.Tag = equipmentClass.EquipmentClassEnum;
                this.equipmentClassListBox.Items.Add(textBlock);
            }
        }
        private void EquipmentClassListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ResetListBox)
            {
                TextBlock textBlock = (TextBlock)this.equipmentClassListBox.SelectedItem;
                EquipmentClassEnum equipmentClassEnum = (EquipmentClassEnum)textBlock.Tag;
                LoadCombatEquipmentBasedOnEquipmentClass(equipmentClassEnum);
            }
        }
        private void LoadCombatEquipmentBasedOnEquipmentClass( EquipmentClassEnum equipmentClassEnum)
        {
            TextBlock textBlock = null;
            List<Equipment> equipments = Game.UnitFactory.EquipmentFactory.Equipments;
            equipments = equipments.Where(eq => eq.EquipmentClassEnum == equipmentClassEnum).ToList();
            equipments = equipments.Where(eq => eq.Nation.NationEnum == NationEnum.German).ToList();
            equipments = equipments.Where(eq => eq.EndService >= Game.CurrentTurn.TurnDate).ToList();
            equipments = equipments.Where(eq => eq.StartService <= Game.CurrentTurn.TurnDate).ToList();
            equipments = equipments.Where(eq => eq.UnitCost <= Game.CurrentTurn.CurrentAxisPrestige).ToList();
            foreach (Equipment equipment in equipments)
            {
                textBlock = new TextBlock();
                textBlock.Text = equipment.ToString();
                textBlock.Tag = equipment.EquipmentId;
                this.combatEquipmentListBox.Items.Add(textBlock);
            }
            equipmentClassListBox.Visibility = System.Windows.Visibility.Collapsed;
            combatEquipmentListBox.Visibility = System.Windows.Visibility.Visible;
        }
        private void CombatEquipmentListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.combatEquipmentListBox.Visibility = System.Windows.Visibility.Collapsed;
            TextBlock textBlock = (TextBlock)this.combatEquipmentListBox.SelectedItem;
            if (textBlock != null)
            {
                int equipmentId = (Int32)textBlock.Tag;
                int unitId = 0;
                if(Game.InCampaign)
                {
                    unitId = Game.CampaignArmy.Select(u => u.UnitId).Max();
                    unitId += 1;
                }
                unitToBePurchased = Game.UnitFactory.CreateDefaultUnit(unitId, equipmentId, 0, NationEnum.German.GetHashCode());
                DisplayCombatUnitInformation(unitToBePurchased);
            }
        }
        private void DisplayCombatUnitInformation(IUnit unit)
        {
            AddCombatUnitImageToInformationScreen(unit);
            combatUnitScrollViewer.Visibility = System.Windows.Visibility.Visible;
            if (unit is LandCombatUnit)
            {
                LandCombatUnit landCombatUnit = (LandCombatUnit)unit;
                DisplayLandCombatUnitInformation(landCombatUnit);
                if (landCombatUnit is IMotorizedUnit == false && landCombatUnit.TransportUnit == null)
                {
                    transportEquipmentListBox.Visibility = System.Windows.Visibility.Visible;
                }
                this.purchaseCombatUnitButton.Visibility = System.Windows.Visibility.Visible;
            }
            if (unit is AirCombatUnit)
            {
                AirCombatUnit airCombatUnit = (AirCombatUnit)unit;
                DisplayAirCombatUnitInformation(airCombatUnit);
                this.purchaseCombatUnitButton.Visibility = System.Windows.Visibility.Visible;
            }
        }
        private void AddCombatUnitImageToInformationScreen(IUnit unit)
        {
            Tile tile = Game.BoardFactory.ActiveTile;
            if (tile == null)
            {
                tile = Game.TileFactory.CreateTile(TerrainTypeEnum.Clear);
            }
            Hex hex = Game.HexFactory.GetHex(tile);
            Game.HexFactory.AddUnit(hex, unit);
            this.combatUnitImage.Children.Add(hex);
        }
        private void DisplayLandCombatUnitInformation(LandCombatUnit landCombatUnit)
        {
            int totalCost = landCombatUnit.Equipment.UnitCost;
            SetCombatUnitTextBlock("Ammo", landCombatUnit.Equipment.MaxAmmo.ToString());
            if (landCombatUnit is IMotorizedUnit)
            {
                IMotorizedUnit motorizedUnit = (IMotorizedUnit)landCombatUnit;
                SetCombatUnitTextBlock("Fuel", motorizedUnit.Equipment.MaxFuel.ToString());
            }
            SetCombatUnitTextBlock("Movement Range", landCombatUnit.Equipment.BaseMovement.ToString());
            if (landCombatUnit.EquipmentClassEnum == EquipmentClassEnum.AirDefense ||
                landCombatUnit.EquipmentClassEnum == EquipmentClassEnum.Artillery)
            {
                SetCombatUnitTextBlock("Attack Range", landCombatUnit.Equipment.Range.ToString());
            }
            SetCombatUnitTextBlock("Hard Attack", landCombatUnit.Equipment.HardAttack.ToString());
            SetCombatUnitTextBlock("Soft Attack", landCombatUnit.Equipment.SoftAttack.ToString());
            SetCombatUnitTextBlock("Air Attack", landCombatUnit.Equipment.AirAttack.ToString());
            SetCombatUnitTextBlock("Sea Attack", landCombatUnit.Equipment.NavalAttack.ToString());
            SetCombatUnitTextBlock("Ground Defense", landCombatUnit.Equipment.GroundDefense.ToString());
            SetCombatUnitTextBlock("Air Defense", landCombatUnit.Equipment.AirDefense.ToString());
            SetCombatUnitTextBlock("Sea Defense", landCombatUnit.Equipment.SeaDefense.ToString());
            if (landCombatUnit.TransportUnit != null)
            {
                SetCombatUnitTextBlock("Transport", landCombatUnit.TransportUnit.UnitName.ToString());
                totalCost += landCombatUnit.TransportUnit.Equipment.UnitCost;
            }
            else
            {
                if (landCombatUnit is IMotorizedUnit == false)
                {
                    SetCombatUnitTextBlock("Transport", "NONE");
                    this.transportButton.Visibility = System.Windows.Visibility.Visible;
                }
            }
            SetCombatUnitTextBlock("COST", totalCost.ToString());
        }
        private void DisplayAirCombatUnitInformation(AirCombatUnit airCombatUnit)
        {
            SetCombatUnitTextBlock("Ammo", airCombatUnit.Equipment.MaxAmmo.ToString());
            SetCombatUnitTextBlock("Fuel", airCombatUnit.Equipment.MaxFuel.ToString());
            SetCombatUnitTextBlock("Movement Range", airCombatUnit.Equipment.BaseMovement.ToString());
            SetCombatUnitTextBlock("Hard Attack", airCombatUnit.Equipment.HardAttack.ToString());
            SetCombatUnitTextBlock("Soft Attack", airCombatUnit.Equipment.SoftAttack.ToString());
            SetCombatUnitTextBlock("Air Attack", airCombatUnit.Equipment.AirAttack.ToString());
            SetCombatUnitTextBlock("Sea Attack", airCombatUnit.Equipment.NavalAttack.ToString());
            SetCombatUnitTextBlock("Ground Defense", airCombatUnit.Equipment.GroundDefense.ToString());
            SetCombatUnitTextBlock("Air Defense", airCombatUnit.Equipment.AirDefense.ToString());
            SetCombatUnitTextBlock("Sea Defense", airCombatUnit.Equipment.SeaDefense.ToString());
            SetCombatUnitTextBlock("COST", airCombatUnit.Equipment.UnitCost.ToString());
        }
        private void SetCombatUnitTextBlock(string name, string value)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = String.Format("{0} : {1}", name, value);
            this.combatUnitStackPanel.Children.Add(textBlock);
        }

        private void TransportButton_Click(object sender, RoutedEventArgs e)
        {
            transportEquipmentListBox.Items.Clear();
            LoadTransportEquipment();
        }
        private void LoadTransportEquipment()
        {
            TextBlock textBlock = null;
            this.transportEquipmentListBox.Visibility = System.Windows.Visibility.Visible;
            List<Equipment> equipments = Game.UnitFactory.EquipmentFactory.Equipments;
            equipments = equipments.Where(eq => eq.EquipmentClassEnum == EquipmentClassEnum.GroundTransport).ToList();
            equipments = equipments.Where(eq => eq.Nation.NationEnum == NationEnum.German).ToList();
            equipments = equipments.Where(eq => eq.EndService >= Game.CurrentTurn.TurnDate).ToList();
            equipments = equipments.Where(eq => eq.StartService <= Game.CurrentTurn.TurnDate).ToList();
            equipments = equipments.Where(eq => eq.UnitCost <= Game.CurrentTurn.CurrentAxisPrestige).ToList();
            foreach (Equipment equipment in equipments)
            {
                textBlock = new TextBlock();
                textBlock.Text = equipment.ToString();
                textBlock.Tag = equipment.EquipmentId;
                this.transportEquipmentListBox.Items.Add(textBlock);
            }
        }
        private void TransportEquipmentListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBlock textBlock = (TextBlock)this.transportEquipmentListBox.SelectedItem;
            if (textBlock != null)
            {
                int unitId = 0;
                if (Game.InCampaign)
                {
                    unitId = Game.CampaignArmy.Count;
                }
                int equipmentId = (Int32)textBlock.Tag;
                LandTransportUnit landTransportUnit = Game.UnitFactory.CreateLandTransport(unitId, equipmentId, unitToBePurchased.Nation, unitToBePurchased.CoreIndicator, Game.BoardFactory.ActiveTile.TileId);
                transportUnitToBePurchased = landTransportUnit;
                DisplayTransportUnitInformation(landTransportUnit);
            }
        }
        private void DisplayTransportUnitInformation(LandTransportUnit landTransportUnit)
        {
            AddTransportUnitImageToInformationScreen(landTransportUnit);
            DisplayLandTransportUnitInformation(landTransportUnit);
            transportUnitScrollViewer.Visibility = System.Windows.Visibility.Visible;
            this.purchaseTransportUnitButton.Visibility = System.Windows.Visibility.Visible;
            transportEquipmentListBox.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void AddTransportUnitImageToInformationScreen(IUnit unit)
        {
            Tile tile = Game.BoardFactory.ActiveTile;
            if (tile == null)
            {
                tile = Game.TileFactory.CreateTile(TerrainTypeEnum.Clear);
            }
            Hex hex = Game.HexFactory.GetHex(tile);
            Game.HexFactory.AddUnit(hex, unit);
            this.transportUnitImage.Children.Add(hex);
        }
        private void DisplayLandTransportUnitInformation(LandTransportUnit landTransportUnit)
        {
            IMotorizedUnit motorizedUnit = (IMotorizedUnit)landTransportUnit;
            SetTransportUnitTextBlock("Fuel", motorizedUnit.Equipment.MaxFuel.ToString());
            SetTransportUnitTextBlock("Movement Range", landTransportUnit.Equipment.BaseMovement.ToString());
            SetTransportUnitTextBlock("Ground Defense", landTransportUnit.Equipment.GroundDefense.ToString());
            SetTransportUnitTextBlock("Air Defense", landTransportUnit.Equipment.AirDefense.ToString());
            SetTransportUnitTextBlock("Sea Defense", landTransportUnit.Equipment.SeaDefense.ToString());
            SetTransportUnitTextBlock("COST", landTransportUnit.Equipment.UnitCost.ToString());
        }
        private void SetTransportUnitTextBlock(string name, string value)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = String.Format("{0} : {1}", name, value);
            this.transportUnitStackPanel.Children.Add(textBlock);
        }
        private void PurchaseTransportUnitButton_Click(object sender, RoutedEventArgs e)
        {
            LandCombatUnit landCombatUnit = (LandCombatUnit)unitToBePurchased;
            landCombatUnit.TransportUnit = transportUnitToBePurchased;
            ResetPageForLandCombatUnit();
            DisplayCombatUnitInformation(landCombatUnit);
        }

        private void PurchaseCombatUnitButton_Click(object sender, RoutedEventArgs e)
        {
            if (Game.UnitUpgrade)
            {
                AdjustNewUnitData();
            }
            AdjustGamePrestige();
            NavigateToNewPage();
        }

        private void AdjustNewUnitData()
        {
            unitToBePurchased.CurrentExperience = Game.CurrentUnit.CurrentExperience;
            unitToBePurchased.CurrentStrength = Game.CurrentUnit.CurrentStrength;
        }
        private void NavigateToNewPage()
        {
            GameBoard gameBoard = (GameBoard)Game.CurrentBoard.Parent;
            if (Game.InCampaign)
            {
                if (Game.UnitUpgrade)
                {
                    Game.CampaignArmy.Remove(Game.CurrentUnit);
                }
                Game.CampaignArmy.Add(unitToBePurchased);
                if (gameBoard == null)
                {
                    this.NavigationService.Navigate(new Uri(@"/ArmySetup.xaml", UriKind.Relative));
                }
                else
                {
                    gameBoard.NavigationService.GoBack();
                    Game.BoardFactory.AddNewUnitToActiveTile(unitToBePurchased, Game.BoardFactory.ActiveTile);
                }
            }
            else
            {
                gameBoard.NavigationService.GoBack();
                Game.BoardFactory.AddNewUnitToActiveTile(unitToBePurchased, Game.BoardFactory.ActiveTile);
            }
            
        }
        private void AdjustGamePrestige()
        {
            int totalCost = unitToBePurchased.Equipment.UnitCost;
            if (unitToBePurchased is LandCombatUnit)
            {
                LandCombatUnit landCombatUnit = (LandCombatUnit)unitToBePurchased;
                if (landCombatUnit.TransportUnit != null)
                {
                    totalCost += landCombatUnit.TransportUnit.Equipment.UnitCost;
                }
            }
            Game.CurrentTurn.CurrentAxisPrestige -= totalCost;
        }
        
    }
}
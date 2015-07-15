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
using Tff.Panzer.Models.Geography;
using Tff.Panzer.Models.Army;

namespace Tff.Panzer
{
    public partial class ArmySetup : PhoneApplicationPage
    {
        public ArmySetup()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            LoadArmyList();
            base.OnNavigatedTo(e);
        }

        private void LoadArmyList()
        {
            EquipmentSubClassEnum currentEquipmentSubClassEnum;
            EquipmentSubClassEnum unitEquipmentSubClassEnum;
            this.armyListListBox.Items.Clear();
            List<IUnit> sortedUnits = Game.CampaignArmy.OrderBy(u => u.EquipmentSubClassEnum).ToList();
            currentEquipmentSubClassEnum = sortedUnits.Select(u => u.EquipmentSubClassEnum).FirstOrDefault();
            TextBox headerTextBlock = new TextBox();
            headerTextBlock.Text = currentEquipmentSubClassEnum.ToString();
            this.armyListListBox.Items.Add(headerTextBlock);

            foreach (IUnit unit in sortedUnits)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = unit.ToString();
                textBlock.Tag = unit.UnitId;
                unitEquipmentSubClassEnum = unit.EquipmentSubClassEnum;
                if (currentEquipmentSubClassEnum != unitEquipmentSubClassEnum)
                {
                    headerTextBlock = new TextBox();
                    headerTextBlock.Text = unitEquipmentSubClassEnum.ToString();
                    this.armyListListBox.Items.Add(headerTextBlock);
                    currentEquipmentSubClassEnum = unitEquipmentSubClassEnum;
                }
                this.armyListListBox.Items.Add(textBlock);
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Game.SetActiveTurn(0);
            Game.CurrentViewLevel = 2;
            this.NavigationService.Navigate(new Uri(@"/TurnSetup.xaml", UriKind.Relative));
        }

        private void armyListListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBlock textBlock = (TextBlock)this.armyListListBox.SelectedItem;
            if (textBlock != null)
            {
                int unitId = (Int32)textBlock.Tag;
                Game.CurrentUnit = Game.CampaignArmy.Where(u => u.UnitId == unitId).FirstOrDefault();
                Game.BoardFactory.ActiveTile = Game.TileFactory.CreateTile(TerrainTypeEnum.Clear);
                this.NavigationService.Navigate(new Uri(@"/BoardInformation.xaml", UriKind.Relative));
            }
        }

        private void PurchaseUnitButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"/PurchaseUnit.xaml", UriKind.Relative));
        }
    }
}
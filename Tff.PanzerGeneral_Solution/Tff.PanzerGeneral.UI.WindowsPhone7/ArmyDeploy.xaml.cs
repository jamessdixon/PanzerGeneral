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
using Tff.Panzer.Models.Army.Unit;

namespace Tff.Panzer
{
    public partial class ArmyDeploy : PhoneApplicationPage
    {
        public ArmyDeploy()
        {
            InitializeComponent();
            LoadArmyList();
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

        private void armyListListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBlock textBlock = (TextBlock)this.armyListListBox.SelectedItem;
            if(textBlock !=null)
            {
                int unitId = (Int32)textBlock.Tag;
                Game.CurrentUnit = Game.CampaignArmy.Where(u => u.UnitId == unitId).FirstOrDefault();
                if (Game.CurrentUnit is AirCombatUnit)
                {
                    if (Game.BoardFactory.ActiveTile.AirUnit == null)
                    {
                        Game.BoardFactory.ActiveTile.AirUnit = Game.CurrentUnit as IAirUnit;
                    }
                }
                else
                {
                    if (Game.BoardFactory.ActiveTile.GroundUnit == null)
                    {
                        Game.BoardFactory.ActiveTile.GroundUnit = Game.CurrentUnit as IGroundUnit;
                    }
                }
                this.NavigationService.GoBack();
            }

        }
    }
}
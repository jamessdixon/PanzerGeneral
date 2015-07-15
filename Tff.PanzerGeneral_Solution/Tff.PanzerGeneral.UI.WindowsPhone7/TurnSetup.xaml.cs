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
using Tff.Panzer.Models.Scenario;
using Tff.Panzer.Factories.Scenario;
using Tff.Panzer.Models;
using Tff.Panzer.Models.Army.Unit;

namespace Tff.Panzer
{
    public partial class TurnSetup : PhoneApplicationPage
    {
        public TurnSetup()
        {
            InitializeComponent();
            LoadTurnInformation();
            UpdateGameBoardTerrain();
            SetUpUnitsForNextTurn();

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            this.StatusTitle.Visibility = System.Windows.Visibility.Collapsed;
        }

        private static void SetUpUnitsForNextTurn()
        {
            Game.BoardFactory.ActiveTile = null;
            Game.BoardFactory.ActiveUnit = null;
            List<Tile> tiles = Game.BoardFactory.Tiles.Where(t => t.GroundUnit != null).ToList();
            foreach (Tile tile in tiles)
            {
                tile.GroundUnit.CanMove = true;
                if (tile.GroundUnit is LandCombatUnit)
                {
                    ((LandCombatUnit)tile.GroundUnit).CanAttack = true;
                }
                if (tile.GroundUnit.SideEnum == SideEnum.Allies)
                {
                    if (tile.GroundUnit is LandTransportUnit)
                    {
                        LandTransportUnit landTransportUnit = (LandTransportUnit)tile.GroundUnit;
                        tile.GroundUnit = landTransportUnit.LandCombatUnit;
                    }
                }
            }
            tiles = Game.BoardFactory.Tiles.Where(t => t.AirUnit != null).ToList();
            foreach (Tile tile in tiles)
            {
                tile.AirUnit.CanMove = true;
                if (tile.AirUnit is LandCombatUnit)
                {
                    ((AirCombatUnit)tile.AirUnit).CanAttack = true;
                }
            }




        }

        private void UpdateGameBoardTerrain()
        {
            Game.BoardFactory.LoadBoard();
        }

        private void LoadTurnInformation()
        {
            AdjustPrestige();
            UpdateUI();
        }

        private void UpdateUI()
        {
            this.TurnTitle.Text = String.Format("Turn: {0}", Game.CurrentTurn.TurnNumber);
            this.CurrentWeatherTitle.Text = String.Format("Current Weather: {0}", Game.CurrentTurn.CurrentWeather.WeatherDescription);
            this.CurrentGroundConditionTitle.Text = String.Format("Current Conditions: {0}", Game.CurrentTurn.CurrentTerrainCondition.TerrainConditionDescription);
            this.ForecastWeatherTitle.Text = String.Format("Forecast Weather: {0}", Game.CurrentTurn.ForcastedWeather.WeatherDescription);
            this.ForecastGroundConditionTitle.Text = String.Format("Forecast Conditions: {0}", Game.CurrentTurn.ForcastedTerrainCondition.TerrainConditionDescription);

        }

        private static void AdjustPrestige()
        {
            if (Game.CurrentTurn.TurnId > 0)
            {

                Turn priorTurn = Game.Turns[Game.CurrentTurn.TurnId - 1];
                ScenarioPrestigeAllotment scenarioPrestigeAllotment =
                    Game.ScenarioFactory.ScenarioPrestigeAllotmentFactory.GetScenarioPrestigeAllotment(Game.CurrentScenarioId, Game.CurrentTurn.TurnId);
                Game.CurrentTurn.CurrentAxisPrestige += scenarioPrestigeAllotment.AxisPrestige + priorTurn.CurrentAxisPrestige;
                Game.CurrentTurn.CurrentAlliedPrestige += scenarioPrestigeAllotment.AlliedPrestige + priorTurn.CurrentAlliedPrestige;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.StatusTitle.Visibility = System.Windows.Visibility.Visible;
            this.OkButton.IsEnabled = false;
            this.NavigationService.Navigate(new Uri(@"/GameBoard.xaml", UriKind.Relative));
        }
    }
}
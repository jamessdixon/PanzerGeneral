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
using Tff.Panzer.Factories.Scenario;
using Tff.Panzer.Models.Scenario;
using Tff.Panzer.Models;

namespace Tff.Panzer
{
    public partial class Briefing : PhoneApplicationPage
    {
        public Briefing()
        {
            InitializeComponent();
            this.ScenarioBriefingText.Text = Game.ScenarioFactory.GetScenarioInfo(Game.CurrentScenarioId).ScenarioDescription;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.StatusTitle.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.OkButton.IsEnabled = false;
            this.StatusTitle.Visibility = System.Windows.Visibility.Visible;
            SetUpAndNavigateToTurnSetup();

        }

        private void SetUpAndNavigateToTurnSetup()
        {
            Game.SetActiveTurn(0);
            ScenarioSide scenarioSide = Game.ScenarioFactory.ScenarioSideFactory.GetScenarioSide(Game.CurrentScenarioId, SideEnum.Axis);
            Game.CurrentTurn.CurrentAxisPrestige += scenarioSide.Prestige;
            scenarioSide = Game.ScenarioFactory.ScenarioSideFactory.GetScenarioSide(Game.CurrentScenarioId, SideEnum.Allies);
            Game.CurrentTurn.CurrentAlliedPrestige = scenarioSide.Prestige;

            if (Game.InCampaign == false)
            {
                this.NavigationService.Navigate(new Uri(@"/TurnSetup.xaml", UriKind.Relative));
            }
            else
            {
                if (Game.CurrentScenarioId > 0)
                {
                    this.NavigationService.Navigate(new Uri(@"/ArmySetup.xaml", UriKind.Relative));
                }
                else
                {
                    this.NavigationService.Navigate(new Uri(@"/TurnSetup.xaml", UriKind.Relative));
                }
            }
        }
    }
}
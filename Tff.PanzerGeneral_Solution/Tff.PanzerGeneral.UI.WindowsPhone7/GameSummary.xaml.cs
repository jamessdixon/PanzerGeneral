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
using Tff.Panzer.Controls;
using Tff.Panzer.Factories.Scenario;
using Tff.Panzer.Models.Scenario;
using Tff.Panzer.Models.Campaign;
using Tff.Panzer.Factories.Campaign;
using Tff.Panzer.Models;
using Tff.Panzer.Models.Army.Unit;

namespace Tff.Panzer
{
    public partial class GameSummary : PhoneApplicationPage
    {
        public GameSummary()
        {
            InitializeComponent();
            DisplayResults();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Game.InCampaign)
            {
                NavigateToNextScenarioInCampaign();
            }
            else
            {
                this.NavigationService.Navigate(new Uri(@"/MainPage.xaml", UriKind.Relative));
            }
        }
        private void DisplayResults()
        {
            if (Game.CurrentCampaignId >=0)
            {
                CalculateAndDisplayCampaignWinner();
                UpdateCampaignArmy();
            }
            else 
            {
                DisplayScenarioWinner();
            }
        }

        private void DisplayScenarioWinner()
        {
            int scenarioId = Game.CurrentScenarioId;
            int axisTiles = Game.BoardFactory.GetNumberOfVictoryTiles(SideEnum.Axis);
            int alliedTiles = Game.BoardFactory.GetNumberOfVictoryTiles(SideEnum.Allies);
            
            SideEnum sideEnum = Game.VictoryConditionFactory.CalculateWinningSide(scenarioId, axisTiles, alliedTiles);

            ScenaioResultTitle.Text = String.Format("{0} Victory", sideEnum);
            ScenarioBriefing.Text = String.Empty;
        }

        private void CalculateAndDisplayCampaignWinner()
        {
            CampaignStepTypeEnum campaignStepTypeEnum = CalculateCampaignStepType();
            //TODO: This sets the campaign to Major Victory.  USe For Testing
            //campaignStepTypeEnum = CampaignStepTypeEnum.MajorVictory;

            CampaignTree campaignTree = CalculateCampignTree(campaignStepTypeEnum);
            DisplayCampaignWinner(campaignTree);
            UpdateCampaignScore(campaignTree);
            Game.CurrentCampaignStep = campaignTree.NextCampaignStep;
        }

        private void UpdateCampaignArmy()
        {
            Game.CampaignArmy.Clear();

            List<Tile> groundTiles = Game.BoardFactory.Tiles.Where(t => t.GroundUnit != null).ToList();
            groundTiles = groundTiles.Where(t => t.GroundUnit.SideEnum == SideEnum.Axis).ToList();
            groundTiles = groundTiles.Where(t => t.GroundUnit.CoreIndicator == true).ToList();
            foreach (Tile tile in groundTiles)
            {
                Game.CampaignArmy.Add(tile.GroundUnit);
            }
            
            
            List<Tile> airTiles = Game.BoardFactory.Tiles.Where(t => t.AirUnit != null).ToList();
            airTiles = airTiles.Where(t => t.AirUnit.SideEnum == SideEnum.Axis).ToList();
            airTiles = airTiles.Where(t => t.AirUnit.CoreIndicator == true).ToList();
            foreach (Tile tile in airTiles)
            {
                Game.CampaignArmy.Add(tile.AirUnit);
            }

        }

        private CampaignStepTypeEnum CalculateCampaignStepType()
        {
            int scenarioId = Game.CurrentScenarioId;
            int turnId = Game.CurrentTurn.TurnId;
            int axisTiles = Game.BoardFactory.GetNumberOfVictoryTiles(SideEnum.Axis);
            int alliedTiles = Game.BoardFactory.GetNumberOfVictoryTiles(SideEnum.Allies);
            return Game.VictoryConditionFactory.CalculateCampaignStepResult(scenarioId, turnId, axisTiles, alliedTiles);
        }

        private CampaignTree CalculateCampignTree(CampaignStepTypeEnum campaignStepTypeEnum)
        {
            return Game.CampaignFactory.CampaignTreeFactory.GetCampaignTree(Game.CurrentCampaignStep, campaignStepTypeEnum);
        }

        private void DisplayCampaignWinner(CampaignTree campaignTree)
        {
            int campaignBriefingId = campaignTree.CampaignBriefing.CampaignBriefingId;
            CampaignBriefing campaignBriefing = Game.CampaignFactory.CampaignBriefingFactory.GetCampaignBriefing(campaignBriefingId);
            ScenaioResultTitle.Text = String.Format("{0}", campaignTree.CampaignStepType.CampaignStepTypeEnum);
            ScenarioBriefing.Text = campaignBriefing.CampaignBriefingDescription;
        }

        private void UpdateCampaignScore(CampaignTree campaignTree)
        {
            Game.CurrentTurn.CurrentAxisPrestige += campaignTree.Prestige;
        }

        private void NavigateToNextScenarioInCampaign()
        {
            if (Game.CurrentCampaignStep == null)
            {
                 this.NavigationService.Navigate(new Uri(@"/MainPage.xaml", UriKind.Relative));
            }
            else
            {
                Int32 scenarioId = Game.CurrentCampaignStep.ScenarioId;
                Game.CurrentScenarioId = scenarioId;
                Game.BoardFactory.BoardLoaded += new EventHandler<EventArgs>(BoardFactory_BoardLoaded);
                Game.BoardFactory.PopulateBoard(scenarioId);
                Game.CurrentBoard = Game.BoardFactory.Board;
                int carryoverPrestige = Game.CurrentTurn.CurrentAxisPrestige;
                Game.Turns = Game.TurnFactory.GetTurnsForAScenario(scenarioId);
                Game.SetActiveTurn(0);
                Game.CurrentTurn.CurrentAxisPrestige += carryoverPrestige;
            }
        }

        void BoardFactory_BoardLoaded(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"/Briefing.xaml", UriKind.Relative));
        }
    }
}
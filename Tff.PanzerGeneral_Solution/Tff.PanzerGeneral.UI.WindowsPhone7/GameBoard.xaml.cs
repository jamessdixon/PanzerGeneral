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
using Tff.Panzer.Models.Army;
using Tff.Panzer.Models.Geography;
using Microsoft.Phone.Shell;
using Tff.Panzer.Factories;

namespace Tff.Panzer
{
    public partial class GameBoard : PhoneApplicationPage
    {
        public Boolean PurchasingUnit { get; set; }
        public GameBoard() 
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.Content = Game.CurrentBoard;
            PurchasingUnit = false;
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (!PurchasingUnit)
            {
                this.Content = null;
            }
        }

        public void doneButton_Click(object sender, EventArgs e)
        {
            Game.ComputerPlayerFactory.CalculateTurn();
            this.Content = null;
            DetermineTurnResult();
        }

        public void airSurfaceButton_Click(object sender, EventArgs e)
        {
            IApplicationBarIconButton airSurfaceButton = sender as IApplicationBarIconButton;

            if (Game.InGroundMode)
            {
                airSurfaceButton.IconUri = new Uri(Constants.AirGroundButtonAirImagePath, UriKind.Relative);
                Game.InGroundMode = false;
            }
            else
            {
                airSurfaceButton.IconUri = new Uri(Constants.AirGroundButtonGroundImagePath, UriKind.Relative);
                Game.InGroundMode = true;
            }
            Game.BoardFactory.ToggleAirAndGroundUnits();
        }

        private void infoButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"/ScenarioInformation.xaml", UriKind.Relative));
        }

        public void NavigateToBoardInformationPage_Click(object sender, EventArgs e)
        {
            Game.BoardFactory.NavigateToBoardInformationPage();
        }

        public void MountDismountMenuItem_Click(object sender, EventArgs e)
        {
            Game.BoardFactory.ToggleMountAndDismount();
        }

        public void EmbarkMenuItem_Click(object sender, EventArgs e)
        {
            Game.BoardFactory.ToggleEmbarkAndDisembark();

        }

        public void SupplyMenuButton_Click(object sender, EventArgs e)
        {
            Game.BoardFactory.ResupplyActiveUnit();
        }

        public void EliteReplacementsMenuItem_Click(object sender, EventArgs e)
        {
            Game.BoardFactory.ReinforceActiveUnit(true);
        }

        public void RegularReplacementsMenuItem_Click(object sender, EventArgs e)
        {
            Game.BoardFactory.ReinforceActiveUnit(false);
        }

        public void UpgradeMenuItem_Click(object sender, EventArgs e)
        {
            Game.UnitUpgrade = true;
            Game.BoardFactory.ActiveTile = Game.BoardFactory.ActiveTile;
            Game.CurrentUnit = Game.BoardFactory.ActiveUnit;
            this.NavigationService.Navigate(new Uri(@"/PurchaseUnit.xaml", UriKind.Relative));
            
        }

        public void DisbandMenuItem_Click(object sender, EventArgs e)
        {
            Game.BoardFactory.RemoveUnitFromGame(Game.BoardFactory.ActiveUnit, Game.BoardFactory.ActiveTile);
        }

        public void PurchaseMenuItem_Click(object sender, EventArgs e)
        {
            PurchasingUnit = true;
            this.NavigationService.Navigate(new Uri(@"/PurchaseUnit.xaml", UriKind.Relative));
        }
        
        public void DetermineTurnResult()
        {
            int nextTurnNumber = Game.CurrentTurn.TurnNumber + 1;
            int maxTurnNumber = Game.Turns.Count;
            bool allTilesCapturedByOneSide = Game.BoardFactory.AllTilesCapturedByOneSide();
            bool anyTurnsRemaining = nextTurnNumber < maxTurnNumber;

            //TODO: This ends the scenario immediately.  Use For testing.
            //allTilesCapturedByOneSide = true;

            if (allTilesCapturedByOneSide)
            {
                this.NavigationService.Navigate(new Uri(@"/GameSummary.xaml", UriKind.Relative));
            }
            else
            {
                if (anyTurnsRemaining)
                {
                    Game.GoToNextTurn();
                    this.NavigationService.Navigate(new Uri(@"/TurnSetup.xaml", UriKind.Relative));
                }
                else
                {
                    this.NavigationService.Navigate(new Uri(@"/GameSummary.xaml", UriKind.Relative));
                }
            }
        }

    }
}
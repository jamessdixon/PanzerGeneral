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
using Tff.Panzer.Models;
using Microsoft.Phone.Shell;

namespace Tff.Panzer
{
    public partial class ScenarioInformation : PhoneApplicationPage
    {
        public ScenarioInformation()
        {
            InitializeComponent();
            LoadScoreboard();
        }

        private void LoadScoreboard()
        {
            this.scoreboardStackPanel.Children.Clear();
            SetScoreboardTextBlock("Axis Prestige",Game.CurrentTurn.CurrentAxisPrestige.ToString());
            SetScoreboardTextBlock("Allied Prestige", Game.CurrentTurn.CurrentAlliedPrestige.ToString());
            SetScoreboardTextBlock(String.Empty,String.Empty);
            SetScoreboardTextBlock("Axis Victory Points", Game.BoardFactory.GetNumberOfVictoryTiles(SideEnum.Axis).ToString());
            SetScoreboardTextBlock("Allied Victory Points", Game.BoardFactory.GetNumberOfVictoryTiles(SideEnum.Allies).ToString());
            SetScoreboardTextBlock(String.Empty, String.Empty);
            SetScoreboardTextBlock("Current Turn Number", Game.CurrentTurn.TurnNumber.ToString());
            SetScoreboardTextBlock("Turns Remaining", (Game.Turns.Count - Game.CurrentTurn.TurnNumber).ToString());
            SetScoreboardTextBlock(String.Empty, String.Empty);
            SetScoreboardTextBlock("Current Terrain Condition", Game.CurrentTurn.CurrentTerrainCondition.TerrainConditionDescription);
            SetScoreboardTextBlock("Current Weather", Game.CurrentTurn.CurrentWeather.WeatherDescription);
            SetScoreboardTextBlock("Forcast Terrain Condition", Game.CurrentTurn.ForcastedTerrainCondition.TerrainConditionDescription);
            SetScoreboardTextBlock("Forcast Weather", Game.CurrentTurn.ForcastedWeather.WeatherDescription);
        }

        private void SetScoreboardTextBlock(string name, string value)
        {
            TextBlock textBlock = new TextBlock();
            if (String.IsNullOrEmpty(name) && String.IsNullOrEmpty(value))
            {
                textBlock.Text = "----------";
            }
            else
            {
                textBlock.Text = String.Format("{0} : {1}", name, value);
            }
            this.scoreboardStackPanel.Children.Add(textBlock);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"/GameInformation.xaml", UriKind.Relative));
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            foreach (ApplicationBarIconButton button in this.ApplicationBar.Buttons)
            {
                button.IsEnabled = false;
            }

            Game.LoadGame();
            LoadScoreboard();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            foreach (ApplicationBarIconButton button in this.ApplicationBar.Buttons)
            {
                button.IsEnabled = false;
            }
            Game.SaveGame();
            
        }
    }
}
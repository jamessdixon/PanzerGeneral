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
using System.Diagnostics;

using Tff.Panzer.Models;
using Tff.Panzer.Models.Army;
using Tff.Panzer.Models.Army.Unit;
using Tff.Panzer.Models.Geography;
using Tff.Panzer.Models.Scenario;
using Tff.Panzer.Models.Campaign;

using Tff.Panzer.Factories;
using Tff.Panzer.Factories.Army;
using Tff.Panzer.Factories.Campaign;
using Tff.Panzer.Factories.Scenario;
using Tff.Panzer.Factories.Geography;
using Tff.Panzer.Factories.Movement;
using Tff.Panzer.Factories.Battle;
using Tff.Panzer.Factories.VictoryCondition;

using System.Threading;
using System.ComponentModel;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Net.NetworkInformation;

namespace Tff.Panzer
{
    public partial class MainPage : PhoneApplicationPage
    {
        public Boolean RequestingUserInput { get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            LoadGameData();
            Game.InitializeGame();
            LoadScenarioList();
            foreach (ApplicationBarIconButton button in this.ApplicationBar.Buttons)
            {
                button.IsEnabled = true;
            }
            ScenarioTitle.Text = "Scenario";
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {

            base.OnNavigatingFrom(e);
        }

        private void LoadGameData()
        {
            Game.ImageFactory = new ImageFactory();
            Game.SoundFactory = new SoundFactory();
            Game.TerrainFactory = new TerrainFactory();
            Game.StrengthFactory = new StrengthFactory();
            Game.NationFactory = new NationFactory();
            Game.WeatherFactory = new WeatherFactory();
            Game.HexOutline = new SolidColorBrush(Colors.Gray);
            Game.BattleFactory = new BattleFactory();
            Game.MovementFactory = new MovementFactory();
            Game.ArmyFactory = new ArmyFactory();
            Game.UnitFactory = new UnitFactory();
            Game.ScenarioFactory = new ScenarioFactory();
            Game.CampaignFactory = new CampaignFactory();
            Game.VictoryConditionFactory = new VictoryConditionFactory(); 
            Game.ComputerPlayerFactory = new ComputerPlayerFactory();
            Game.CampaignArmy = new List<IUnit>();
            Game.TileFactory = new TileFactory();
            Game.HexFactory = new HexFactory();
            Game.BoardFactory = new BoardFactory();
            Game.MenuFactory = new MenuFactory();
            Game.TurnFactory = new TurnFactory();
            
        }

        private void LoadScenarioList()
        {
            RequestingUserInput = false;
            this.ScenarioListBox.Items.Clear();
            Game.ScenarioFactory.ScenariosLoaded += new EventHandler<EventArgs>(ScenarioFactory_ScenariosLoaded);

            if (DeviceNetworkInformation.IsNetworkAvailable)
            {
                Game.ScenarioFactory.PopulateScenariosFromWebService();
            }
            else
            {
                Game.ScenarioFactory.PopulateScenariosLocally();
            }

            this.ScenarioListBox.IsEnabled = true;
            RequestingUserInput = true;
        }

        void ScenarioFactory_ScenariosLoaded(object sender, EventArgs e)
        {
            foreach (ScenarioInfo scenarioInfo in Game.ScenarioFactory.Scenarios)
            {
                TextBlock textBlock = GetScenarioTextBlock(scenarioInfo);
                this.ScenarioListBox.Items.Add(textBlock);
            }
        }

        private TextBlock GetScenarioTextBlock(ScenarioInfo scenarioInfo)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.FontSize = 30;
            textBlock.Width = 450;
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Text = scenarioInfo.ToString();
            textBlock.Tag = scenarioInfo.ScenarioId;
            return textBlock;
        }

        private void LoadScenarioInformation(Int32 scenarioId)
        {
            ScenarioInfo scenarioInfo = Game.ScenarioFactory.Scenarios.Where(s => s.ScenarioId == scenarioId).FirstOrDefault();
            TextBlock textBlock = GetScenarioTextBlock(scenarioInfo);
            textBlock.Text += " LOADING...";
            textBlock.Foreground = new SolidColorBrush(Colors.Red);
            this.ScenarioListBox.Items.Add(textBlock);
        }

        private void UpdateScenarioInformation()
        {
            TextBlock textBlock = (TextBlock)this.ScenarioListBox.SelectedItem;
            textBlock.Text += " LOADING...";
            textBlock.Foreground = new SolidColorBrush(Colors.Red);
            this.ScenarioListBox.IsEnabled = false;

        }

        private void ScenarioListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RequestingUserInput)
            {
                RequestingUserInput = false;
                UpdateScenarioInformation();
                Game.CurrentCampaignId = -1;
                foreach (ApplicationBarIconButton button in this.ApplicationBar.Buttons)
                {
                    button.IsEnabled = false;
                }
                TextBlock textBlock = (TextBlock)this.ScenarioListBox.SelectedItem;
                int scenarioId = (Int32)textBlock.Tag;
                SetupScenario(scenarioId);
            }
        }

        private void SetupScenario(int scenarioId)
        {
            Game.CurrentScenarioId = scenarioId;
            Game.Turns = Game.TurnFactory.GetTurnsForAScenario(scenarioId);
            Game.Turns[0].ActiveTurn = true;
            Game.BoardFactory.BoardLoaded += new EventHandler<EventArgs>(BoardFactory_BoardLoaded);
            Game.BoardFactory.PopulateBoard(scenarioId);
            Game.CurrentBoard = Game.BoardFactory.Board;
        }

        void BoardFactory_BoardLoaded(object sender, EventArgs e)
        {
            Game.BoardFactory.BoardLoaded -= new EventHandler<EventArgs>(BoardFactory_BoardLoaded);
            this.NavigationService.Navigate(new Uri(@"/Briefing.xaml", UriKind.Relative));
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            try
            {
                Game.LoadGame();
                this.NavigationService.Navigate(new Uri(@"/GameBoard.xaml", UriKind.Relative));
            }
            catch (Exception exception)
            {
                ScenarioTitle.Text = "NO GAME FOUND";
            }
        }

        private void gameInformationButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"/GameInformation.xaml", UriKind.Relative));

        }
    }
}
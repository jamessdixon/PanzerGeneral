using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Shell;

namespace Tff.Panzer.Factories
{
    public class MenuFactory
    {
        private Boolean _showBoardInformation = true;
        private Boolean _showMount = false;
        private Boolean _showEmbark = false;
        private Boolean _showSupply = false;
        private Boolean _showEliteReplacements = false;
        private Boolean _showRegularReplacements = false;
        private Boolean _showUpgrade = false;
        private Boolean _showDisband = false;
        private Boolean _showPurchase = false;

        public Boolean ShowBoardInformation { get { return _showBoardInformation; } set { _showBoardInformation = value; CreateMenuItems(); } }
        public Boolean ShowMount { get { return _showMount; } set { _showMount = value; CreateMenuItems(); } }
        public Boolean ShowEmbark { get { return _showEmbark; } set { _showEmbark = value; CreateMenuItems(); } }
        public Boolean ShowSupply { get { return _showSupply; } set { _showSupply = value; CreateMenuItems(); } }
        public Boolean ShowEliteReplacements { get { return _showEliteReplacements; } set { _showEliteReplacements = value; CreateMenuItems(); } }
        public Boolean ShowRegularReplacements { get { return _showRegularReplacements; } set { _showRegularReplacements = value; CreateMenuItems(); } }
        public Boolean ShowUpgrade { get { return _showUpgrade; } set { _showUpgrade = value; CreateMenuItems(); } }
        public Boolean ShowDisband { get { return _showDisband; } set { _showDisband = value; CreateMenuItems(); } }
        public Boolean ShowPurchase { get { return _showPurchase; } set { _showPurchase = value; CreateMenuItems(); } }




        public void ActivateAllMenuItems()
        {
            _showBoardInformation = true;
            _showMount = true;
            _showEmbark = true;
            _showSupply = true;
            _showEliteReplacements = true;
            _showRegularReplacements = true;
            _showUpgrade = true;
            _showDisband = true;
            _showPurchase = true;
            CreateMenuItems();
        }

        public void DeactivateAllMenuItems()
        {
            _showBoardInformation = false;
            _showMount = false;
            _showEmbark = false;
            _showSupply = false;
            _showEliteReplacements = false;
            _showRegularReplacements = false;
            _showUpgrade = false;
            _showDisband = false;
            _showPurchase = false;
            CreateMenuItems();
        }

        private void CreateMenuItems()
        {
            GameBoard gameBoard = (GameBoard)Game.CurrentBoard.Parent;
            gameBoard.ApplicationBar.MenuItems.Clear();
            ApplicationBarMenuItem menuItem = null;

            if (ShowBoardInformation)
            {
                menuItem = new ApplicationBarMenuItem();
                menuItem.Text = "Info";
                menuItem.Click += new EventHandler(gameBoard.NavigateToBoardInformationPage_Click);
                gameBoard.ApplicationBar.MenuItems.Add(menuItem);
            }

            if (ShowMount)
            {
                menuItem = new ApplicationBarMenuItem();
                menuItem.Text = "Mount/Dismount";
                menuItem.Click += new EventHandler(gameBoard.MountDismountMenuItem_Click);
                gameBoard.ApplicationBar.MenuItems.Add(menuItem);
            }
            if (ShowEmbark)
            {
                menuItem = new ApplicationBarMenuItem();
                menuItem.Text = "Embark/Disembark";
                menuItem.Click += new EventHandler(gameBoard.EmbarkMenuItem_Click);
                gameBoard.ApplicationBar.MenuItems.Add(menuItem);
            }
            if (ShowSupply)
            {
                menuItem = new ApplicationBarMenuItem();
                menuItem.Text = "Supply";
                menuItem.Click += new EventHandler(gameBoard.SupplyMenuButton_Click);
                gameBoard.ApplicationBar.MenuItems.Add(menuItem);
            }
            if (ShowEliteReplacements)
            {
                menuItem = new ApplicationBarMenuItem();
                menuItem.Text = "Elite Replacements";
                menuItem.Click += new EventHandler(gameBoard.EliteReplacementsMenuItem_Click);
                gameBoard.ApplicationBar.MenuItems.Add(menuItem);
            }
            if (ShowRegularReplacements)
            {
                menuItem = new ApplicationBarMenuItem();
                menuItem.Text = "Regular Replacements";
                menuItem.Click += new EventHandler(gameBoard.RegularReplacementsMenuItem_Click);
                gameBoard.ApplicationBar.MenuItems.Add(menuItem);
            }
            if (ShowUpgrade)
            {
                menuItem = new ApplicationBarMenuItem();
                menuItem.Text = "Upgrade";
                menuItem.Click += new EventHandler(gameBoard.UpgradeMenuItem_Click);
                gameBoard.ApplicationBar.MenuItems.Add(menuItem);
            }
            if (ShowDisband)
            {
                menuItem = new ApplicationBarMenuItem();
                menuItem.Text = "Disband";
                menuItem.Click += new EventHandler(gameBoard.DisbandMenuItem_Click);
                gameBoard.ApplicationBar.MenuItems.Add(menuItem);
            }
            if (ShowPurchase)
            {
                menuItem = new ApplicationBarMenuItem();
                menuItem.Text = "Purchase";
                menuItem.Click += new EventHandler(gameBoard.PurchaseMenuItem_Click);
                gameBoard.ApplicationBar.MenuItems.Add(menuItem);
            }
        }


    }
}

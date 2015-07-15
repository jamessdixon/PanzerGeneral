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

namespace Tff.Panzer
{
    public partial class GameInformation : PhoneApplicationPage
    {
        public GameInformation()
        {
            InitializeComponent();
            this.HelpBrowser.Loaded += new RoutedEventHandler(HelpBrowser_Loaded);
        }

        void HelpBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            this.HelpBrowser.Navigate(new Uri(Constants.HelpDocumentsLocation, UriKind.Absolute));
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
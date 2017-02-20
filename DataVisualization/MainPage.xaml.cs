using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DataVisualization
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            MyFrame.Navigate(typeof(Pages.Home));
            TitleTextBlock.Text = "Home";
            Home.IsSelected = true;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Home.IsSelected)
            {
                MyFrame.Navigate(typeof(Pages.Home));
                TitleTextBlock.Text = "Home";
            }
            else if (Market.IsSelected)
            {
                MyFrame.Navigate(typeof(Pages.Market));
                TitleTextBlock.Text = "Market";
            }
            else if (Report.IsSelected)
            {
                MyFrame.Navigate(typeof(Pages.Report));
                TitleTextBlock.Text = "Report";
            }
            else if (Settings.IsSelected)
            {
                MyFrame.Navigate(typeof(Pages.Settings));
                TitleTextBlock.Text = "Settings";
            }
        }

        private void ListBox_SelectionChanged2(object sender, SelectionChangedEventArgs e)
        {
            if (Exit.IsSelected)
                Application.Current.Exit();
        }
    }
}

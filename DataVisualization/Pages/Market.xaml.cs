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
using LiveCharts;
using LiveCharts.Uwp;

// Pour plus d'informations sur le modèle d'élément Page vierge, voir la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace DataVisualization.Pages
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class Market : Page
    {
        public Market()
        {
            this.InitializeComponent();
        }

        private void EMA_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MACD_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SMA_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            var item = (ComboBoxItem)combo.SelectedItem;

            if (item.Content.ToString() == "Green")
            {
                Red.Visibility = Visibility.Collapsed;
                Green.Visibility = Visibility.Visible;
                Blue.Visibility = Visibility.Collapsed;
            }
            else if (item.Content.ToString() == "Red")
            {
                Red.Visibility = Visibility.Visible;
                Green.Visibility = Visibility.Collapsed;
                Blue.Visibility = Visibility.Collapsed;
            }
            else if (item.Content.ToString() == "Blue")
            {
                Red.Visibility = Visibility.Collapsed;
                Green.Visibility = Visibility.Collapsed;
                Blue.Visibility = Visibility.Visible;
            }
        }
    }
}

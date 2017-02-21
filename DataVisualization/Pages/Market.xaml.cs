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
            Darky.Visibility = Visibility.Collapsed;
            Bat.Visibility = Visibility.Collapsed;
            Super.Visibility = Visibility.Visible;
            MyComboBox.
        }

        private void EMA_Click(object sender, RoutedEventArgs e)
        {
            Text_block_Tool.Text = "EMA";
        }

        private void MACD_Click(object sender, RoutedEventArgs e)
        {
            Text_block_Tool.Text = "MACD";
        }

        private void SMA_Click(object sender, RoutedEventArgs e)
        {
            Text_block_Tool.Text = "SMA";
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            var item = (ComboBoxItem)combo.SelectedItem;

            if (Darky == null || Bat == null || Super == null)
                return;

            if (item.Content.ToString() == "bat")
            {
                Darky.Visibility = Visibility.Collapsed;
                Bat.Visibility = Visibility.Visible;
                Super.Visibility = Visibility.Collapsed;
                Text_bloc_symbol.Text = "bat";
            }
            else if (item.Content.ToString() == "darky")
            {
                Darky.Visibility = Visibility.Visible;
                Bat.Visibility = Visibility.Collapsed;
                Super.Visibility = Visibility.Collapsed;
                Text_bloc_symbol.Text = "darky";
            }
            else if (item.Content.ToString() == "super")
            {
                Darky.Visibility = Visibility.Collapsed;
                Bat.Visibility = Visibility.Collapsed;
                Super.Visibility = Visibility.Visible;
                Text_bloc_symbol.Text = "super";
            }
        }
    }
}

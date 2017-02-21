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
using DataVisualization.Jobs.ActiveSymbols;
using Windows.UI.Popups;

// Pour plus d'informations sur le modèle d'élément Page vierge, voir la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace DataVisualization.Pages
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    
    public sealed partial class Market : Page
    {
        protected List<ActiveSymbol> ActiveSymbols { get; set; }

        public Market()
        {
            this.InitializeComponent();

            Init_first_view();
            

            var cboxitem = new ComboBoxItem();

            cboxitem.Content = "Item3";

            Combo_box_symbols.Items.Add(cboxitem);
        }

        private void Init_first_view()
        {
            Darky.Visibility = Visibility.Collapsed;
            Bat.Visibility = Visibility.Collapsed;
            Super.Visibility = Visibility.Visible;

            this.ActiveSymbols = new List<ActiveSymbol>();



            ActiveSymbol actv_symbol = new ActiveSymbol(1, "EURUSD", "Instrument, which price is based on quotations of Euro to American Dollar on the interbank market.");
            this.ActiveSymbols.Add(actv_symbol);


            Text_block_Tool.Text = "SMA";
            Text_block_Separator.Text = "/";
            Text_bloc_symbol.Text = this.ActiveSymbols.FirstOrDefault().Name;
     
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

        private void Combo_box_symbols_SelectionChanged(object sender, SelectionChangedEventArgs e)
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


        /*
        private async void EMA_Click(object sender, RoutedEventArgs e)
        {
            Text_block_Tool.Text = "EMA";

            MessageDialog showDialog = new MessageDialog("Hi Welcome to Windows 10");
            showDialog.Commands.Add(new UICommand("Yes")
            {
                Id = 0
            });
            showDialog.Commands.Add(new UICommand("No")
            {
                Id = 1
            });
            showDialog.DefaultCommandIndex = 0;
            showDialog.CancelCommandIndex = 1;
            var result = await showDialog.ShowAsync();
            if ((int)result.Id == 0)
            {
                //do your task  
            }
            else
            {
                //skip your task  
            }

        }*/
    }
}

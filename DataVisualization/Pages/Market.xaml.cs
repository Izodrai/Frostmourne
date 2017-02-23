﻿using System;
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
            

            
        }

        private void Init_first_view()
        {
            this.ActiveSymbols = new List<ActiveSymbol>();
            
            ActiveSymbol actv_symbol = new ActiveSymbol(1, "EURUSD", "Instrument, which price is based on quotations of Euro to American Dollar on the interbank market.");
            this.ActiveSymbols.Add(actv_symbol);
            actv_symbol = new ActiveSymbol(1, "EURGBP", "Instrument, which price is based on quotations of Euro to British Pound on the interbank market.");
            this.ActiveSymbols.Add(actv_symbol);
            actv_symbol = new ActiveSymbol(1, "GBPUSD", "Instrument, which price is based on quotations of British Pound to American Dollar on the interbank market.");
            this.ActiveSymbols.Add(actv_symbol);


            Text_block_Tool.Text = "SMA";
            Text_block_Separator.Text = "/";
            Text_bloc_symbol.Text = this.ActiveSymbols.FirstOrDefault().Name;

            foreach (ActiveSymbol element in this.ActiveSymbols)
            {
                var cboxitem = new ComboBoxItem();
                cboxitem.Content = element.Name;
                Combo_box_symbols.Items.Add(cboxitem);
            }
            Combo_box_symbols.SelectedIndex = 0;
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

            if (this.ActiveSymbols == null)
                return;
            
            if (item.Content.ToString() != null && item.Content.ToString() != "")
            {
                foreach (ActiveSymbol element in this.ActiveSymbols)
                {
                    if (element.Name != item.Content.ToString())
                        continue;

                    Text_bloc_symbol.Text = element.Name;
                    break;
                }
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

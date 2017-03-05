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
using DataVisualization.Jobs.Symbols;
using Windows.UI.Popups;
using DataVisualization.Dbs;
using DataVisualization.Errors;

// Pour plus d'informations sur le modèle d'élément Page vierge, voir la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace DataVisualization.Pages
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    
    public sealed partial class Market : Page
    {
        protected List<Jobs.Symbols.Symbol> Symbols { get; set; }

        protected Mysql MyDB_Connector { get; set; }
        protected Error err { get; set; }

        public Market()
        {
            this.InitializeComponent();

            this.err = new Error(false, "");

            err = Init_first_view();
            if (err.IsAnError)
            {
                Text_bloc_process_state.Text = err.MessageError;
            }
            else
            {
                Text_bloc_process_state.Text = err.MessageError;
            }

            


        }

        private Error Init_first_view()
        {

            this.Symbols = new List<Jobs.Symbols.Symbol>();
            
            
            this.MyDB_Connector = new Mysql("localhost", "market", "market_user", "azerty");

            if (this.MyDB_Connector.Connect().IsAnError)
            {
                return new Error(true, "We have a problem with the mysql connexion");
            }

            List<Jobs.Symbols.Symbol> ss = new List<Jobs.Symbols.Symbol>();

            /*
            err = this.MyDB_Connector.Load_symbols(ref ss);
            if (err.IsAnError)
            {
                return err;
            }
            this.Symbols = ss;
            */


            

            Jobs.Symbols.Symbol actv_symbol = new Jobs.Symbols.Symbol(1, "EURUSD", "Instrument, which price is based on quotations of Euro to American Dollar on the interbank market.");
            this.Symbols.Add(actv_symbol);
            /*actv_symbol = new Jobs.Symbols.Symbol(1, "EURGBP", "Instrument, which price is based on quotations of Euro to British Pound on the interbank market.");
            this.Symbols.Add(actv_symbol);
            actv_symbol = new Jobs.Symbols.Symbol(1, "GBPUSD", "Instrument, which price is based on quotations of British Pound to American Dollar on the interbank market.");
            this.Symbols.Add(actv_symbol);*/


            Text_block_Tool.Text = "SMA";
            Text_block_Separator.Text = "/";
            Text_bloc_symbol.Text = this.Symbols.FirstOrDefault().Name;

            foreach (Jobs.Symbols.Symbol element in this.Symbols)
            {
                var cboxitem = new ComboBoxItem();
                cboxitem.Content = element.Name;
                Combo_box_symbols.Items.Add(cboxitem);
            }
            Combo_box_symbols.SelectedIndex = 0;
            

            return new Error(false, "first informations loaded !");
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

            if (this.Symbols == null)
                return;
            
            if (item.Content.ToString() != null && item.Content.ToString() != "")
            {
                foreach (Jobs.Symbols.Symbol element in this.Symbols)
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

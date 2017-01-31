using DataManager.Views;
using System.Windows;
using System;
using System.Windows.Controls;

namespace DataManager
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ana_global_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new Page1();
            MessageBox.Show("ana_global_Click");
        }

        private void ana_sma_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new Page2();
            MessageBox.Show("ana_sma_Click");
        }

        private void ana_ema_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ana_ema_Click");
        }

        private void ana_macd_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ana_macd_Click");
        }

        private void gen_exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Fermeture du programme");
            Application.Current.Shutdown();
        }

        private void gen_configuration_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("gen_configuration_Click");
        }

        private void func_activation_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("func_activation_Click");
        }

        private void func_eve_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("func_eve_Click");
        }

        private void func_stop_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("func_stop_Click");
        }


        /*
        private void button_Click(object sender, RoutedEventArgs e)
        {
            graph_sma g_sma = new graph_sma();
            
        }
        */
    }
}

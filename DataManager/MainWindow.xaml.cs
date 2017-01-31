using System.Windows;
using System;
using System.Windows.Controls;
using DataManager.Views.Analyzes;

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
            frame.Content = new Ana_global();
        }

        private void ana_sma_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new Ana_sma();
        }

        private void ana_ema_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new Ana_ema();
        }

        private void ana_macd_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new Ana_macd();
        }

        private void gen_exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Fermeture du programme");
            Application.Current.Shutdown();
        }

        private void gen_informations_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("gen_informations_Click");
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
    }
}

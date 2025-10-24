using System.Windows;
using System.Windows.Controls;

namespace WpfApp6
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnProducts_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.Navigate(new ProductsPage());
        }

        private void BtnStatistics_Click(object sender, RoutedEventArgs e)
        {
           // MainFrame.Navigate(new StatisticsPage());
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.Navigate(new SearchPage());
        }
    }
}
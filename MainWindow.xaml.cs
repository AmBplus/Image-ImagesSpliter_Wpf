using System;
using System.Windows;

namespace WpfCustomerService
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnHome_OnClick(object sender, RoutedEventArgs e)
        {
            HomePanel.Visibility = Visibility.Collapsed;
        }


        private void BtnImages_OnClickges_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnImage_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
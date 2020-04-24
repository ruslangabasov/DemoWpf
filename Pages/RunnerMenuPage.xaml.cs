using System;
using System.Windows;
using System.Windows.Controls;

namespace DMgit.Pages
{
    /// <summary> 
    /// Логика взаимодействия для RunnerMenuPage.xaml
    /// </summary>
    public partial class RunnerMenuPage : Page
    {
        public RunnerMenuPage()
        {
            InitializeComponent();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/MainPage.xaml", UriKind.Relative));
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            popupContacts.IsOpen = true;
        }
        private void LblClose_Click(object sender, RoutedEventArgs e)
        {
            popupContacts.IsOpen = false;
        }
        private void popupContacts_Closed(object sender, System.EventArgs e)
        {

        }
    }
}

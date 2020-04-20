using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DMgit.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdministratorMenuPage.xaml
    /// </summary>
    public partial class AdministratorMenuPage : Page
    {
        public AdministratorMenuPage()
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
    }
}

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
    /// Логика взаимодействия для ThankSponsorPage.xaml
    /// </summary>
    public partial class ThankSponsorPage : Page
    {
        public ThankSponsorPage()
        {
            InitializeComponent();
        }
        public ThankSponsorPage(string runnerName, string charityName, string summa)
        {
            InitializeComponent();
            tbSumma.Text = summa;
            tbRunnerName.Text = runnerName;
            tbCharityName.Text = charityName;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/MainPage.xaml", UriKind.Relative));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using DMgit.Model;

namespace DMgit.Pages
{
    /// <summary>
    /// Логика взаимодействия для SponsorRunnerPage.xaml
    /// </summary>
    public partial class SponsorRunnerPage : Page
    {
        int total = 0;
        int RegId = -1;
        //List<RunnerList> runnerLists = new List<RunnerList>();
        //string connectionString = ConfigurationManager.ConnectionStrings
        //    ["SqlCon"].ConnectionString;
        Charity charity;
        public SponsorRunnerPage()
        {
            InitializeComponent();
            tbSumma.Text = total.ToString();
            LoadDataAboutRunners();
        }


        void LoadDataAboutRunners()
        {

            using (Marathon2020Entities1 db = new Marathon2020Entities1())
            {
                var runnerList = (from runner in db.Runner
                                  join user in db.User on runner.Email equals user.Email
                                  join country in db.Country on runner.CountryCode equals country.CountryCode
                                  select new
                                  {
                                      IdRunner = runner.RunnerId,
                                      RunnerName = user.FirstName + " " + user.LastName + " <" + runner.RunnerId + "> " + country.CountryCode
                                  }).ToList();
                cmbRunner.ItemsSource = runnerList.ToList();

            }

        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnPlusSumClick(object sender, RoutedEventArgs e)
        {
            if (tbSumma.Text == "") return;
            total += 10;
            tbSumma.Text = total.ToString();
        }

        private void BtnMinusSumClick(object sender, RoutedEventArgs e)
        {
            if (tbSumma.Text == "") return;

            if (10 > total)
            {
                MessageBox.Show("Error, you can't minus so many money",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            total -= 10;
            tbSumma.Text = total.ToString();

        }

        private void TbSumma_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblSumma.Content = tbSumma.Text;
            if (tbSumma.Text != "")
                total = Convert.ToInt32(tbSumma.Text);
            else
                total = 0;
        }

        private void CmbRunner_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbRunner.SelectedIndex != -1)
            {
                GetRunnerCharityOrganization(
                    Convert.ToInt32(cmbRunner.SelectedValue));
            }
        }


        string CheckInputData()
        {
            string cardNumber = @"[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}";
            string kart = @"[1-9]{1}[0-9]{2}";
            string errorMessage = "";
            if (tbSponsorName.Text == "")
                errorMessage += "Поле: Ваше имя - пустое\n";
            if (cmbRunner.SelectedIndex == -1)
                errorMessage += "Не выбран бегун\n";
            if (tbKartOwner.Text == "")
                errorMessage += "Поле: Владелец карты - пустое\n";
            if (total == 0)
                errorMessage += "Сумма: 0\n";
            if (!Regex.IsMatch(tbKartNumber.Text, cardNumber))
                errorMessage += "Неправильный номер карты\n";

            string[] words = tbKartValid.Text.Split(' ');
            int month, year;
            if ((words.Length == 2) && (int.TryParse(words[0], out month))
                && (int.TryParse(words[1], out year)))
            {
                if ((month >= 1) && (month < 13))
                {
                    DateTime dateFuture = new System.DateTime(year, month, 30, 0, 0, 0);
                    DateTime nowDateTime = DateTime.Now;
                    TimeSpan diff1 = dateFuture.Subtract(nowDateTime);
                    if (diff1.TotalDays < 0)
                        errorMessage += "Поле: Срок действия - некорректное\n";
                }
                else
                    errorMessage += "Поле: Срок действия - некорректное\n";
            }
            else
                errorMessage += "Поле: Срок действия - некорректное\n";
            if (!Regex.IsMatch(tbCVC.Text, kart))
                errorMessage += "Поле: Срок CVC-код - пустое\n";

            return errorMessage;
        }
        void InsertData()
        {
            //string cardNumber = @"[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}";
            //if ((!Regex.IsMatch(tbKartNumber.Text, cardNumber)) ||
            //    (tbSponsorName.Text == "") ||
            //    (tbKartOwner.Text == "") ||
            //    (tbSumma.Text == "") ||
            //     (cmbRunner.SelectedIndex == -1) ||
            //     (tbKartValid.Text =="")||
            //     (tbCVC.Text ==""))
            string er = CheckInputData();
            if (er != "")
            {
                MessageBox.Show(er, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            using (Marathon2020Entities1 db = new Marathon2020Entities1())
            {
                if (RegId == -1) return;
                Sponsorship sponsorship = new Sponsorship
                {
                    SponsorName = tbSponsorName.Text,
                    RegistrationId = RegId,
                    Amount = Convert.ToDecimal(tbSumma.Text)
                };
                db.Sponsorship.Add(sponsorship);
                db.SaveChanges();
                MessageBox.Show("Спасибо, Ваши деньги" +
                   " успешно переведены на счёт бегуна",
                   "Информация",
                   MessageBoxButton.OK,
                   MessageBoxImage.Information);
            }



            //    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            //{
            //    sqlConnection.Open();
            //    if (RegId == -1) return;
            //    SqlCommand insertData = new SqlCommand("INSERT INTO Sponsorship" +
            //        " VALUES(@SponsorName,@RegistrationId,@Amount)", sqlConnection);
            //    insertData.Parameters.AddWithValue("@SponsorName", tbSponsorName.Text);
            //    insertData.Parameters.AddWithValue("@RegistrationId", RegId);
            //    insertData.Parameters.AddWithValue("@Amount", Convert.ToDouble(tbSumma.Text));
            //    insertData.ExecuteNonQuery();
            //    MessageBox.Show("Спасибо, Ваши деньги" +
            //        " успешно переведены на счёт бегуна",
            //        "Информация",
            //        MessageBoxButton.OK,
            //        MessageBoxImage.Information);
            //    //
            //    //PAge Thank you Gonna be here

            //}
        }

        void GetRunnerCharityOrganization(int idRunner)
        {
            //    lblCharityName.Content = "";
            //    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            //    {
            //        sqlConnection.Open();


            //        SqlCommand sqlGetId = new SqlCommand("SELECT " +
            //            "[Registration].CharityId  " +
            //            "FROM Registration WHERE RunnerID= " + idRunner, sqlConnection);
            //        int charityId = Convert.ToInt32(sqlGetId.ExecuteScalar());

            //        SqlCommand sqlRegId = new SqlCommand("SELECT " +
            //           "[Registration].RegistrationId  " +
            //           "FROM Registration WHERE RunnerID= " + idRunner,
            //           sqlConnection);
            //        RegId = -1;
            //        if (sqlRegId.ExecuteScalar() != DBNull.Value)
            //            RegId = Convert.ToInt32(sqlRegId.ExecuteScalar());


            //        SqlCommand sqlGetCharity = new SqlCommand("SELECT " +
            //            " *  " +
            //            "FROM Charity WHERE CharityID= " + charityId, sqlConnection);


            //        SqlDataReader reader = sqlGetCharity.ExecuteReader();
            //        charity = null;
            //        if (reader.HasRows)
            //        {
            //            while (reader.Read())
            //            {
            //                int id = reader.GetInt32(0);
            //                string name = reader.GetString(1);
            //                string description = reader.GetString(2);
            //                string logo = reader.GetString(3);
            //                charity = new Charity()
            //                {
            //                    IdCharity = id,
            //                    CharityName = name,
            //                    CharityLogo = logo,
            //                    CharityDescription = description
            //                };
            //                lblCharityName.Content = name;
            //            }
            //        }
            //        reader.Close();
            //    }
            charity = null;
            lblCharityName.Content = "";
            RegId = -1;
            using (Marathon2020Entities1 db = new Marathon2020Entities1())
            {
                var charityId = db.Registration.FirstOrDefault(p => p.RunnerId == idRunner);
                if (charityId != null)
                {

                    RegId = charityId.RegistrationId;
                    charity = db.Charity.Find(charityId.CharityId);
                    lblCharityName.Content = charity.CharityName;
                }
            }

        }

        private void btnPopup_Click(object sender, RoutedEventArgs e)
        {
            if (!(charity is null))
            {
                lbCharityName.Content = charity.CharityName;
                tbCharityDescription.Text = charity.CharityDescription;
                imgCharityLogo.Source = new BitmapImage(
                    new Uri(AppDomain.CurrentDomain.BaseDirectory
                    + "Images/" + charity.CharityLogo, UriKind.Absolute));
                pLink.IsOpen = true;
            }


        }

        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            InsertData();
            NavigationService.Navigate(new Pages.ThankSponsorPage(cmbRunner.Text, charity.CharityName, "$" + Convert.ToDouble(tbSumma.Text)));
        }

        private void tbKartValid_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[0|1]{1}[0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

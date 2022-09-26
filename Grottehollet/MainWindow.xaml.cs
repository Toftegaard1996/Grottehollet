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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using Grottehollet.Class;

namespace Grottehollet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DBConnection DB = new DBConnection();
        Member member;
        int ButtonPressed = 0;
        
        public MainWindow()
        {
            InitializeComponent();
            member = new Member(DB);

        }
        private void NameLoginBox_GotFocus(object sender, RoutedEventArgs e)
        {
            NameLoginBox.Text = null;
            //DB.cnn.Open();
        }

        private void CodeLoginBox_GotFocus(object sender, RoutedEventArgs e)
        {
            CodeLoginBox.Text = null;
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            DB.cnn.Open();
            DB.cmd = new SqlCommand("SELECT * FROM Medlemmer WHERE Medlemskode=@Medlemskode", DB.cnn);
            DB.cmd.Parameters.AddWithValue("Medlemskode", CodeLoginBox.Text);
            DB.reader = DB.cmd.ExecuteReader();
            
            if (DB.reader.Read())
            {
                if (CodeLoginBox.Text == DB.reader["Medlemskode"].ToString() && NameLoginBox.Text == DB.reader["Navn"].ToString())
                {
                    member.SeeProfile(CodeLoginBox.Text);
                    if (member.Admin == "True")
                    {
                        MakeViewRequest.Content = "View requests";
                        MakeOrView1.Content = "View Request";
                        MakeOrView2.Content = "View Request";
                        MakeOrView3.Content = "View Request";
                        MakeOrView4.Content = "View Request";
                    }
                    Grottehollet.SelectedIndex = 2;
                }
            }
            else { MessageBox.Show("Koden findes ikke"); }
            DB.cnn.Close();
        }
        #region knaps
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Grottehollet.SelectedIndex = 1;
        }

        private void ReturnToLogin_Click(object sender, RoutedEventArgs e)
        {
            Grottehollet.SelectedIndex = 0;
        }

        private void NameRegisterBox_GotFocus(object sender, RoutedEventArgs e)
        {
            NameRegisterBox.Text = null;
        }

        private void AdressRegisterBox_GotFocus(object sender, RoutedEventArgs e)
        {
            AdressRegisterBox.Text = null;
        }

        private void CityRegisterBox_GotFocus(object sender, RoutedEventArgs e)
        {
            CityRegisterBox.Text = null;
        }

        private void RegisterConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameRegisterBox.Text != "" && AdressRegisterBox.Text != "" && CityRegisterBox.Text != "")
            {
                member.CreateMember(NameRegisterBox.Text, AdressRegisterBox.Text, CityRegisterBox.Text);
                Grottehollet.SelectedIndex = 2;
            }
            else MessageBox.Show("Udfyld venligst alle felter");
        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            Grottehollet.SelectedIndex = 2;
        }

        private void MakeRequestButton_Click(object sender, RoutedEventArgs e)
        {
            if (member.Admin == "True")
            {
                Grottehollet.SelectedIndex = 6;
                MakeViewRequest.Content = "View requests";
                AdminRequest.ItemsSource = member.ViewRequests();
            }
            else 
            {
                Grottehollet.SelectedIndex = 4;
            }

        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            Grottehollet.SelectedIndex = 5;
            PFWelcome.Content = "Welcome " + member.Name;
            ProfileName.Text = member.Name;
            if (member.Nickname != "")
            {
                ProfileNickname.Text = member.Nickname;
            }
            if (member.Adress != "")
            {
                AdressAdress.Text = member.Adress;
            }
            if (member.City != "")
            {
                AdressCity.Text = member.City;
            }
            if (member.Number != "")
            {
                ProfileNumber.Text = member.City;
            }
        }

        private void ViewStockButton_Click(object sender, RoutedEventArgs e)
        {
            Grottehollet.SelectedIndex = 3;
            StockList.ItemsSource = DB.SeeBoardGameList();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Grottehollet.SelectedIndex = 0;
            NameLoginBox.Text = "Your name";
            CodeLoginBox.Text = "Code";
        }

        private void HideShowCode_Click(object sender, RoutedEventArgs e)
        {
            
            if (ButtonPressed == 0)
            {
                BitmapImage Image = new BitmapImage(new Uri("pack://application:,,,/Grottehollet;component/Resources/Show.png"));
                ShowImg.Source = Image;
                ProfileCode.Content = member.Code;
                ButtonPressed += 1;
            }
            else if (ButtonPressed == 1)
            {
                BitmapImage Image = new BitmapImage(new Uri("pack://application:,,,/Grottehollet;component/Resources/Hide.png"));
                ShowImg.Source = Image;
                ProfileCode.Content = "********";
                ButtonPressed = 0;
            }
            
            
        }
        private void SaveInfo_Click(object sender, RoutedEventArgs e)
        {
            member.UpdateProfile(CodeLoginBox.Text, ProfileNickname.Text, ProfileNumber.Text, ECName.Text, ECTlf.Text, AdressAdress.Text, AdressCity.Text);
        }

        private void MakeBorrowingRequest_Click(object sender, RoutedEventArgs e)
        {
            string Forborrowing = ((Button)sender).DataContext.ToString();
            ViewYourRequest.ItemsSource = member.RequestForBorrowing(Forborrowing);
            //Remove the button when it's pressed

        }
        private void RemoveFromBorrowingRequest_Click(object sender, RoutedEventArgs e)
        {
            BorrowRequest brrrr = ((Button)sender).DataContext as BorrowRequest;
            member.borrowRequests.Remove(brrrr);
        }

        private void PlaceRequest_Click(object sender, RoutedEventArgs e)
        {
            List<string> RequestBorrowing = new List<string>();
            if (RequestBorrowing != null)
            {
                foreach (var item in ViewYourRequest.Items)
                {
                    RequestBorrowing.Add(item.ToString());
                }
                member.PlaceRequest(RequestBorrowing);
                RequestPlacedFeedback.Visibility = Visibility.Visible;
                RequestBorrowing.Clear();
                ViewYourRequest.ItemsSource = null;

            }
        }

        #endregion
        private void AcceptRequestButton_Click(object sender, RoutedEventArgs e)
        {
            BorrowRequest brrrr = ((Button)sender).DataContext as BorrowRequest;
            string AcceptBorrowing = ((Button)sender).DataContext.ToString();
            member.ConfirmRequest(AcceptBorrowing);
            member.borrowRequests.Remove(brrrr);
        }

        private void RejectRequestButton_Click(object sender, RoutedEventArgs e)
        {
            BorrowRequest brrrr = ((Button)sender).DataContext as BorrowRequest;
            string RejectBorrowing = ((Button)sender).DataContext.ToString();
            member.RejectRequest(RejectBorrowing);
            member.borrowRequests.Remove(brrrr);
        }
    }
}

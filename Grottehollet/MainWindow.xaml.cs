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
                if (CodeLoginBox.Text == DB.reader["Medlemskode"].ToString())
                {
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
            Grottehollet.SelectedIndex = 3;
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            Grottehollet.SelectedIndex = 4;
        }

        private void ViewStockButton_Click(object sender, RoutedEventArgs e)
        {
            Grottehollet.SelectedIndex = 5;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Grottehollet.SelectedIndex = 0;
        }

        private void HideCode_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShowCode_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

    }
}

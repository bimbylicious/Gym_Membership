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

namespace Gym_Membership
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string loggedInStaffID = "";
        public static string loggedInStaffRole = "";
        DataClasses1DataContext _gmDC = null;
        string userName = "";
        bool loginFlag = false;
        public static bool isAdmin = false;
        public MainWindow()
        {
            InitializeComponent();
            _gmDC= new DataClasses1DataContext(Properties.Settings.Default.GymMembershipConnectionString);
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            loginFlag = false;
            DateTime cDT = DateTime.Now;

            if (username.Text.Length > 0 && password.Text.Length > 0)
            {
                var loginQuery = from s in _gmDC.Staffs
                                 where
                                    s.Staff_Username == username.Text
                                 select s;

                if (loginQuery.Count() == 1)
                {
                    foreach (var login in loginQuery)
                    {
                        if (login.Staff_Password == password.Text)
                        {
                            loginFlag = true;
                            userName = login.Staff_Username;
                            loggedInStaffID = login.Staff_ID; // Save Staff_ID
                            loggedInStaffRole = login.Staff_Role; // Save Staff_Role
                            isAdmin = login.Staff_Role.ToLower() == "admin";
                            _gmDC.SubmitChanges();
                        }
                    }
                }


                if (loginFlag)
                {
                    MessageBox.Show($"Login success! Welcome back {userName}!");
                    Menu w1 = new Menu(null);
                    w1.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Username and/or password is incorrect");
                }
            }
            else
            {
                MessageBox.Show("Please input username and/or password");
            }
        }
        private void username_GotFocus(object sender, RoutedEventArgs e)
        {
            if (username.Text == "Username")
            {
                username.Text = "";
                username.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        private void username_LostFocus(object sender, RoutedEventArgs e)
        {
            if (username.Text == "")
            {
                username.Text = "Username";
                username.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }
        private void password_GotFocus(object sender, RoutedEventArgs e)
        {
            if (password.Text == "Password")
            {
                password.Text = "";
                password.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (password.Text == "")
            {
                password.Text = "Password";
                password.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void username_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                password.Focus();
            }
        }

        private void password_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                loginFlag = false;
                DateTime cDT = DateTime.Now;

                if (username.Text.Length > 0 && password.Text.Length > 0)
                {
                    var loginQuery = from s in _gmDC.Staffs
                                     where
                                        s.Staff_Username == username.Text
                                     select s;

                    if (loginQuery.Count() == 1)
                    {
                        foreach (var login in loginQuery)
                        {
                            if (login.Staff_Password == password.Text)
                            {
                                loginFlag = true;
                                userName = login.Staff_Username;
                                _gmDC.SubmitChanges();
                            }
                        }
                    }


                    if (loginFlag)
                    {
                        MessageBox.Show($"Login success! Welcome back {userName}!");
                        Menu w1 = new Menu(null);
                        w1.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Username and/or password is incorrect");
                    }
                }
                else
                {
                    MessageBox.Show("Please input username and/or password");
                }
            }
        }
    }
}


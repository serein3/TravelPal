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
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserManager userManager;
        private TravelManager? travelManager;
        public MainWindow()
        {
            InitializeComponent();
            tbUsername.Focus();
            this.userManager = new();
        }

        public MainWindow(UserManager userManager, TravelManager travelManager)
        {
            InitializeComponent();
            tbUsername.Focus();
            this.userManager = userManager;
            this.travelManager = travelManager;

        }

        // Resets all window fields
        private void ResetLoginUI()
        {
            tbUsername.Clear();
            pbPassword.Clear();
            tbUsername.Focus();
        }

            // Checks whether it is possible to sign in user, checks if user is admin, and creates new instance of travelswindow, passing all the necessary information
            private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            bool isUserFound = userManager.SignInUser(tbUsername.Text, pbPassword.Password);
            bool isUserAdmin = userManager.CheckIfAdmin();

            // MAYBE REFRACTOR THIS INTO METHOD LATER
            if (travelManager == null)
            {
                travelManager = new(userManager);
            }

            if (isUserFound)
            {
                ResetLoginUI();

                TravelsWindow travelsWindow = new(userManager, travelManager, isUserAdmin);
                travelsWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid Username or Password", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Creates new instance of register window, passing all necessary information
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new(userManager);
            registerWindow.ShowDialog();
        }
    }
}

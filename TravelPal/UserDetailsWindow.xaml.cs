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
using TravelPal.Enums;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for UserDetailsWindow.xaml
    /// </summary>
    public partial class UserDetailsWindow : Window
    {
        private UserManager userManager;
        private TravelsWindow travelsWindow;
        public UserDetailsWindow(UserManager userManager, TravelsWindow travelsWindow)
        {
            InitializeComponent();
            this.userManager = userManager;
            this.travelsWindow = travelsWindow;
            cbDetailsCountry.ItemsSource = Enum.GetValues(typeof(Countries)); // TODO Display descriptions instead of names with underscores
            SetUserDetails();
        }

        private void SetUserDetails()
        {
            tbDetailsUsername.Text = userManager.SignedInUser.Username;
            pbDetailsPassword.Password = userManager.SignedInUser.Password;
            cbDetailsCountry.SelectedIndex = (int)userManager.SignedInUser.Location;
        }

        private void btnUserDetailsCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnUserDetailsSave_Click(object sender, RoutedEventArgs e)
        {
            bool isValidUsernameLength = userManager.ValidateUsernameLength(tbDetailsUsername.Text);
            bool isValidPasswordLength = userManager.ValidatePasswordLength(pbDetailsPassword.Password);

            if (!string.IsNullOrEmpty(tbDetailsUsername.Text) && !string.IsNullOrEmpty(pbDetailsPassword.Password) && !string.IsNullOrEmpty(pbDetailsConfirmPassword.Password) && cbDetailsCountry != null)
            {
                if (isValidUsernameLength)
                {
                    if (isValidPasswordLength)
                    {
                        if (pbDetailsPassword.Password == pbDetailsConfirmPassword.Password)
                        {
                            User user = new(tbDetailsUsername.Text, pbDetailsPassword.Password, (Countries)cbDetailsCountry.SelectedItem);

                            if (userManager.UpdateUser(user))
                            {
                                MessageBox.Show("Changes saved!");
                                travelsWindow.UpdateWelcomeMessage();
                                Close();

                            }
                            else
                            {
                                MessageBox.Show("Username not available!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Passwords must match!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Password must be at least 5 characters!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Username must be at least 3 characters!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("All fields must be filled!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void pbDetailsPassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            pbDetailsPassword.Clear();
        }
    }
}

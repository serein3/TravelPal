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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private UserManager userManager;
        public RegisterWindow(UserManager userManager)
        {
            InitializeComponent();
            this.userManager = userManager;
            cbRegisterCountry.ItemsSource = Enum.GetValues(typeof(Countries));
            tbRegisterUsername.Focus();
        }

        // Checks if user can be added, sends appropriate message to user depending on outcome and closes this window
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                User newUser = new(tbRegisterUsername.Text, pbRegisterPassword.Password, (Countries)cbRegisterCountry.SelectedItem);

                if (userManager.AddUser(newUser))
                {
                    MessageBox.Show("Registration successful!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else
                {
                    MessageBox.Show("User already exists!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        // Checks if all necessary fields are filled, displays appropriate warning message otherwise
        private bool ValidateInput()
        {
            if (!string.IsNullOrEmpty(tbRegisterUsername.Text) && !string.IsNullOrEmpty(pbRegisterPassword.Password) && !string.IsNullOrEmpty(pbRegisterConfirmPassword.Password) && cbRegisterCountry.SelectedItem != null)
            {
                bool isValidUsernameLength = userManager.ValidateUsernameLength(tbRegisterUsername.Text);
                bool isValidPasswordLength = userManager.ValidatePasswordLength(pbRegisterPassword.Password);


                if (isValidUsernameLength)
                {
                    if (isValidPasswordLength)
                    {
                        if (pbRegisterPassword.Password == pbRegisterConfirmPassword.Password)
                        {
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Passwords must match!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return false;
                        }
                    }
                    else
                    {

                        MessageBox.Show("Password must be at least 5 characters!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Username must be at least 3 characters!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            else
            {

                MessageBox.Show("All fields must be filled!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}

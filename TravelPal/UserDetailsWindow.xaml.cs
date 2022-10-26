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

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for UserDetailsWindow.xaml
    /// </summary>
    public partial class UserDetailsWindow : Window
    {
        private UserManager userManager;
        public UserDetailsWindow(UserManager userManager)
        {
            InitializeComponent();
            this.userManager = userManager;
            cbDetailsCountry.ItemsSource = Enum.GetValues(typeof(Countries));
            SetUserDetails();
        }

        private void SetUserDetails()
        {
            tbDetailsUsername.Text = userManager.SignedInUser.Username;
            tbDetailsPassword.Password = userManager.SignedInUser.Password;
            tbDetailsConfirmPassword.Password = userManager.SignedInUser.Password;
            cbDetailsCountry.SelectedIndex = (int)userManager.SignedInUser.Location; ;
        }
    }
}

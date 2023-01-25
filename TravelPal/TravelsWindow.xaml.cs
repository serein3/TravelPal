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
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for TravelsWindow.xaml
    /// </summary>
    public partial class TravelsWindow : Window
    {
        private UserManager userManager;
        private TravelManager travelManager;
        private User? signedInUser;
        private bool isAdmin;
        public TravelsWindow(UserManager userManager, TravelManager travelManager, bool isAdmin)
        {
            InitializeComponent();
            this.userManager = userManager;
            this.travelManager = travelManager;
            signedInUser = userManager.SignedInUser as User;
            this.isAdmin = isAdmin;
            UpdateWelcomeMessage();
            UpdateTravelsList();
            UpdateAdminUI();
                
        }

        // Updates the UI based on user type (user or admin)
        private void UpdateAdminUI()
        {
            if (isAdmin)
            {
                btnTravelAdd.IsEnabled = false;
                btnTravelDetails.IsEnabled = false;
                btnUser.IsEnabled = false;
                btnInfo.IsEnabled = false;
            }
        }

        // Updates welcome message based on signed in user's current username
        public void UpdateWelcomeMessage()
        {
            txtWelcome.Text = $"Welcome {userManager.SignedInUser.Username}!";
        }

        // Updates travels list based on user type (user or admin)
        private void UpdateTravelsList()
        {
            if (isAdmin && travelManager.Travels.Count() != 0)
            {
                foreach (Travel travel in travelManager.Travels)
                {
                    ListViewItem listViewItem = new();
                    listViewItem.Tag = travel;
                    listViewItem.Content = travel.GetInfo();
                    lvTravels.Items.Add(listViewItem);
                }
            }
            else if (!isAdmin && signedInUser.Travels.Count() != 0)
            {
                foreach (Travel travel in signedInUser.Travels)
                {
                    ListViewItem listViewItem = new();
                    listViewItem.Tag = travel;
                    listViewItem.Content = travel.GetInfo();
                    lvTravels.Items.Add(listViewItem);
                }
            }
        }

        // Creates new instance of MainWindow, passing all necessary information and closes this window
        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new(userManager, travelManager);
            mainWindow.Show();
            this.Close();
        }

        // Creates a new instance of UserDetailsWindow, passing necessary information
        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            UserDetailsWindow userDetailsWindow = new(userManager, this);
            userDetailsWindow.ShowDialog();
        }

        // Displays MessageBox with some info
        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("" + "Use the buttons below in order to add, edit or view your existing travels. \n\nWhen adding a new travel, do not hesitate to use the packing list feature to keep track of all your necessities!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Based on user type, performs the appropriate remove action for a selected travel. In case of no selection, displays MessageBox to user informing what is wrong
        private void btnTravelRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lvTravels.SelectedItem != null)
            {
                if (isAdmin)
                {
                    ListViewItem selectedItem = lvTravels.SelectedItem as ListViewItem;
                    travelManager.AdminRemoveTravel(selectedItem.Tag as Travel);
                    lvTravels.Items.RemoveAt(lvTravels.SelectedIndex);
                }
                else
                {
                    ListViewItem selectedItem = lvTravels.SelectedItem as ListViewItem;
                    travelManager.RemoveTravel(selectedItem.Tag as Travel);
                    lvTravels.Items.RemoveAt(lvTravels.SelectedIndex);
                }
            }
            else
            {
                MessageBox.Show("Selection required!", "warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Creates a new instance of AddTravelWindow, passing all necessary information
        private void btnTravelAdd_Click(object sender, RoutedEventArgs e)
        {
            AddTravelWindow addTravelWindow = new(userManager, travelManager);
            addTravelWindow.ShowDialog();
        }

        // Creates a new instance of TravelDetailsWindow displaying details about selected travel, passing all necessary information. In case of no selection, displays MessageBox to user informing what is wrong
        private void btnTravelDetails_Click(object sender, RoutedEventArgs e)
        {
            TravelDetailsWindow travelDetailsWindow;

            if (lvTravels.SelectedItem != null)
            {
                ListViewItem selectedItem = lvTravels.SelectedItem as ListViewItem;
                travelDetailsWindow = new(selectedItem.Tag as Travel, travelManager);
                travelDetailsWindow.ShowDialog();

            }
            else
            {
                MessageBox.Show("Selection required!", "warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Event that triggers every time window gets activated, making sure travel list is updated after any possible changes
        private void Window_Activated(object sender, EventArgs e)
        {
            lvTravels.Items.Clear();
            UpdateTravelsList();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
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
    }
}

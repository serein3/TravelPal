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
        private User signedInUser;
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

        public void UpdateWelcomeMessage()
        {
            txtWelcome.Text = $"Welcome {userManager.SignedInUser.Username}!";
        }

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
            else if (signedInUser.Travels.Count() != 0)
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

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new(userManager, travelManager);
            mainWindow.Show();
            this.Close();
        }

        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            UserDetailsWindow userDetailsWindow = new(userManager, this);
            userDetailsWindow.ShowDialog();
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("TravelPal is a company for travelling or something... This app allows you to easily track your travels!!!\n\nYou can see your travels in the list below." +
                " In order to add, remove, or update your travels, use the buttons you silly goose.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnTravelRemove_Click(object sender, RoutedEventArgs e)
        {
            // REFRACTORRRRRRRR AAA
            if (isAdmin)
            {
                if (lvTravels.SelectedItem != null)
                {
                    ListViewItem selectedItem = lvTravels.SelectedItem as ListViewItem;
                    travelManager.RemoveTravel(selectedItem.Tag as Travel);
                    lvTravels.Items.RemoveAt(lvTravels.SelectedIndex);
                }
                else
                {
                    MessageBox.Show("Selection required!", "warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                if (lvTravels.SelectedItem != null)
                {
                    ListViewItem selectedItem = lvTravels.SelectedItem as ListViewItem;
                    travelManager.RemoveTravel(selectedItem.Tag as Travel);
                    lvTravels.Items.RemoveAt(lvTravels.SelectedIndex);
                }
                else
                {
                    MessageBox.Show("Selection required!", "warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void btnTravelAdd_Click(object sender, RoutedEventArgs e)
        {
            AddTravelWindow addTravelWindow = new(userManager, travelManager);
            addTravelWindow.ShowDialog();
        }

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
    }
}

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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private UserManager userManager;
        private TravelManager travelManager;
        public AdminWindow(UserManager userManager, TravelManager travelManager)
        {
            InitializeComponent();
            this.userManager = userManager;
            this.travelManager = travelManager;
            UpdateTravelsList();

        }

        private void UpdateTravelsList()
        {
            
            if (travelManager.Travels.Count() != 0)
            {
                foreach (Travel travel in travelManager.Travels)
                {
                    ListViewItem listViewItem = new();
                    listViewItem.Tag = travel;
                    listViewItem.Content = travel.GetInfo();
                    lvTravels.Items.Add(listViewItem);
                }
            }
        }

        private void btnTravelRemove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

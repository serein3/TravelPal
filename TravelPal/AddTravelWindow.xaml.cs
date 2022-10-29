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
    /// Interaction logic for AddTravelWindow.xaml
    /// </summary>
    public partial class AddTravelWindow : Window
    {
        private UserManager userManager;
        private TravelManager travelManager;
        private bool isTripTypeFilled = false; // MAYBE REFRACTOR THIS INTO ONE METHOD INSTEAD OF HAVING AN ADDITIONAL BOOL just fix this shit in general
        public AddTravelWindow(UserManager userManager, TravelManager travelManager)
        {
            InitializeComponent();
            this.userManager = userManager;
            this.travelManager = travelManager;
            cbDetailsCountry.ItemsSource = Enum.GetValues(typeof(Countries));
            cbTravelType.ItemsSource = travelManager.TravelTypes;
            cbTripTypeOrAllInclusive.ItemsSource = Enum.GetValues(typeof(TripTypes));
        }

        // RETURN STRING WITH TRAVEL TYPE INSTEAD AND ADD ANOTHER METHOD MODIFYTRAVELTYPEUI
        private void DetermineTravelType()
        {
            if (cbTravelType.SelectedItem != null)
            {
                if (cbTravelType.SelectedItem == "Trip")
                {
                    xbAllInclusive.Visibility = Visibility.Hidden;
                    txtTripType.Visibility = Visibility.Visible;
                    cbTripTypeOrAllInclusive.Visibility = Visibility.Visible;
                    cbTripTypeOrAllInclusive.ItemsSource = Enum.GetValues(typeof(TripTypes));
                }
                else if (cbTravelType.SelectedItem == "Vacation")
                {
                    txtTripType.Visibility = Visibility.Hidden;
                    cbTripTypeOrAllInclusive.Visibility = Visibility.Hidden;
                    xbAllInclusive.Visibility = Visibility.Visible;
                    isTripTypeFilled = true;
                }
            }
        }
        private bool CheckIfAllFieldsAreFilled()
        {
            if (dpStartingDate.SelectedDate != null && dpEndingDate.SelectedDate != null && !string.IsNullOrEmpty(tbDestination.Text) && !string.IsNullOrEmpty(tbTravelers.Text) && cbTravelType.SelectedItem != null)
            {
                if (isTripTypeFilled)
                {
                    if (int.TryParse(tbTravelers.Text, out int result))
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Please input a valid amount of travellers!", "warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                }
            }
            MessageBox.Show("All fields must be filled!", "warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        private void CheckIfTripTypeIsFilled()
        {
            if (cbTripTypeOrAllInclusive.SelectedItem != null)
            {
                isTripTypeFilled = true;
            }
        }

        private void btnAddTravel_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfAllFieldsAreFilled())
            {
                 
                MessageBox.Show("Added!");
            }
        }

        private void cbTravelType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbTravelType.SelectedItem != null)
            {
                DetermineTravelType();
                CheckIfTripTypeIsFilled();
            }
        }

        private void cbTripTypeOrAllInclusive_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckIfTripTypeIsFilled();
        }
    }
}

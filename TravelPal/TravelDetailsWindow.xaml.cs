using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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
using TravelPal.Interfaces;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for TravelDetailsWindow.xaml
    /// </summary>
    public partial class TravelDetailsWindow : Window
    {
        private Travel travel;
        private TravelManager travelManager;
        public TravelDetailsWindow(Travel travel, TravelManager travelManager)
        {
            InitializeComponent();
            this.travel = travel;
            this.travelManager = travelManager;
            cbDetailsCountry.ItemsSource = Enum.GetValues(typeof(Countries));
            cbTravelType.ItemsSource = travelManager.TravelTypes;
            cbTripTypeOrAllInclusive.ItemsSource = Enum.GetValues(typeof(TripTypes));
            UpdateUI();
            PopulatePackingListView();
        }

        // Populates the packing list listview with selected travel's packing list items
        private void PopulatePackingListView()
        {
            foreach (IPackingListItem item in travel.PackingList)
            {
                ListViewItem listViewItem = new();
                listViewItem.Content = item.GetInfo();
                listViewItem.Tag = item;
                lvDetailsPackingList.Items.Add(listViewItem);
            }
        }

        // Locks all fields and sets them according to selected travel's information
        private void UpdateUI()
        {
            dpStartingDate.IsEnabled = false;
            dpEndDate.IsEnabled = false;
            tbDestination.IsEnabled = false;
            cbDetailsCountry.IsEnabled = false;
            tbTravelers.IsEnabled = false;
            cbTravelType.IsEnabled = false;
            cbTripTypeOrAllInclusive.IsEnabled = false;
            xbAllInclusive.IsEnabled = false;

            dpStartingDate.Text = travel.StartDate.ToString();
            dpEndDate.Text = travel.EndDate.ToString();
            tbTravelLength.Text = travel.TravelDays.ToString();
            tbDestination.Text = travel.Destination.ToString();
            tbTravelers.Text = travel.Travellers.ToString();
            cbDetailsCountry.SelectedItem = travel.Country;

            DetermineTripType();

        }

        // Determines whether the selected travel is a trip or a vacation, selects the correct one in the ComboBox and updates the additional fields accordingly
        private void DetermineTripType()
        {
            if (travel is Trip)
            {
                Trip trip = travel as Trip;

                cbTravelType.SelectedItem = "Trip";
                txtTripType.Visibility = Visibility.Visible;
                cbTripTypeOrAllInclusive.Visibility = Visibility.Visible;
                cbTripTypeOrAllInclusive.ItemsSource = Enum.GetValues(typeof(TripTypes));
                cbTripTypeOrAllInclusive.SelectedItem = trip.TripType;
            }
            else if (travel is Vacation)
            {
                Vacation vacation = travel as Vacation;

                cbTravelType.SelectedItem = "Vacation";
                xbAllInclusive.Visibility= Visibility.Visible;

                if (vacation.AllInclusive)
                {
                    xbAllInclusive.IsChecked = true;
                }
            }
        }
    }
}

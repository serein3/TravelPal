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
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for TravelDetailsWindow.xaml
    /// </summary>
    public partial class TravelDetailsWindow : Window
    {
        private Travel travel;
        private List<string> travelTypes = new List<string> { "Trip", "Vacation" };
        public TravelDetailsWindow(Travel travel)
        {
            InitializeComponent();
            this.travel = travel;
            cbDetailsCountry.ItemsSource = Enum.GetValues(typeof(Countries));
            cbTravelType.ItemsSource = travelTypes;
            cbTripTypeOrAllInclusive.ItemsSource = Enum.GetValues(typeof(TripTypes));
            UpdateUI();
        }

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
            tbDestination.Text = travel.Destination.ToString();
            tbTravelers.Text = travel.Travellers.ToString();
            cbDetailsCountry.SelectedItem = travel.Country;

            DetermineTripType();

        }

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

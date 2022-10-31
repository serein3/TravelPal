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
using TravelPal.Interfaces;
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
        private User signedInUser;
        private ListViewItem listViewItem;
        private ListViewItem packingListViewItem = new();
        private List<IPackingListItem> packingList = new();
        public AddTravelWindow(UserManager userManager, TravelManager travelManager)
        {
            InitializeComponent();
            this.userManager = userManager;
            this.travelManager = travelManager;
            this.signedInUser = userManager.SignedInUser as User;

            cbDetailsCountry.ItemsSource = Enum.GetValues(typeof(Countries));
            cbTravelType.ItemsSource = travelManager.TravelTypes;
            cbTripTypeOrAllInclusive.ItemsSource = Enum.GetValues(typeof(TripTypes));


            // ADD A CHECK LATER FOR GOING BACK IN TIME WITH THE BOOKING
            dpStartingDate.DisplayDateStart = DateTime.Now;
            dpEndingDate.DisplayDateStart = DateTime.Now.AddDays(1);

        }


        private void ClearPackingListUI()
        {
            tbItemName.Clear();
            tbQuantity.Clear();
            xbDocument.IsChecked = false;
            xbRequired.IsChecked = false;
        }
        private bool CheckIfRequiredIsChecked()
        {
            if (xbRequired.IsChecked == true)
            {
                return true;
            }
            return false;
        }

        private bool DetermineDocumentRequired()
        {
            string userLocation = signedInUser.Location.ToString();
            string travelLocation = cbDetailsCountry.SelectedItem.ToString();

            if (Enum.TryParse<EuropeanCountries>(userLocation, out EuropeanCountries idk1) && !Enum.TryParse<EuropeanCountries>(travelLocation, out EuropeanCountries idk2))
            {
                return true;
            }
            else if (Enum.TryParse<EuropeanCountries>(userLocation, out EuropeanCountries idk3) && Enum.TryParse<EuropeanCountries>(travelLocation, out EuropeanCountries idk4))
            {
                return false;
            }
            return true;
        }

        private string DetermineTravelType()
        {
            if (cbTravelType.SelectedItem != null)
            {
                if (cbTravelType.SelectedItem == "Trip")
                {
                    return "Trip";
                }
                else if (cbTravelType.SelectedItem == "Vacation")
                {
                    return "Vacation";
                }
            }
            return null;
        }

        private void ModifyTravelTypeUI(string travelType)
        {
            if (travelType == "Trip")
            {
                xbAllInclusive.Visibility = Visibility.Hidden;
                txtTripType.Visibility = Visibility.Visible;
                cbTripTypeOrAllInclusive.Visibility = Visibility.Visible;
                cbTripTypeOrAllInclusive.ItemsSource = Enum.GetValues(typeof(TripTypes));
            }
            else if (travelType == "Vacation")
            {
                txtTripType.Visibility = Visibility.Hidden;
                cbTripTypeOrAllInclusive.Visibility = Visibility.Hidden;
                xbAllInclusive.Visibility = Visibility.Visible;
            }
        }
        private bool CheckIfAllFieldsAreFilled()
        {
            if (dpStartingDate.SelectedDate != null && dpEndingDate.SelectedDate != null && !string.IsNullOrEmpty(tbDestination.Text) && !string.IsNullOrEmpty(tbTravelers.Text) && cbTravelType.SelectedItem != null && cbDetailsCountry.SelectedItem != null)
            {
                    if (int.TryParse(tbTravelers.Text, out int result))
                    {
                        if (DetermineTravelType() == "Trip")
                        {
                            if (cbTripTypeOrAllInclusive.SelectedItem != null)
                            {
                                return true;
                            }
                        }
                        else if (DetermineTravelType() == "Vacation")
                        {
                            return true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please input a valid amount of travellers!", "warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
            }
            MessageBox.Show("All fields must be filled!", "warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        private void btnAddTravel_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfAllFieldsAreFilled())
            {
                if (DetermineTravelType() == "Trip")
                {
                    Trip newTrip = new(tbDestination.Text, (Countries)cbDetailsCountry.SelectedItem, Convert.ToInt32(tbTravelers.Text), (DateTime)dpStartingDate.SelectedDate, (DateTime)dpEndingDate.SelectedDate, (TripTypes)cbTripTypeOrAllInclusive.SelectedItem);
                    // TODO POSSIBLY REFRACTOR THIS SHIT INTO ITS OWN ADDPACKINGLISTITEM METHOD OR SOMETHING ALSO BELOW
                    signedInUser.Travels.Add(newTrip);
                    travelManager.Travels.Add(newTrip);
                    newTrip.PackingList.AddRange(packingList);
                }
                else if (DetermineTravelType() == "Vacation")
                {
                    Vacation newVacation;

                    if (xbAllInclusive.IsChecked == true)
                    {
                        newVacation = new(tbDestination.Text, (Countries)cbDetailsCountry.SelectedItem, Convert.ToInt32(tbTravelers.Text), (DateTime)dpStartingDate.SelectedDate, (DateTime)dpEndingDate.SelectedDate, true);

                    }
                    else
                    {
                        newVacation = new(tbDestination.Text, (Countries)cbDetailsCountry.SelectedItem, Convert.ToInt32(tbTravelers.Text), (DateTime)dpStartingDate.SelectedDate, (DateTime)dpEndingDate.SelectedDate, false);
                    }
                    signedInUser.Travels.Add(newVacation);
                    travelManager.Travels.Add(newVacation);
                    newVacation.PackingList.AddRange(packingList);
                }

                MessageBox.Show("Added!");
                this.Close();
            }
        }

        private void cbTravelType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModifyTravelTypeUI(DetermineTravelType());
        }

        private void cbDetailsCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (DetermineDocumentRequired())
            {

                if (lvPackingList.Items.Count > 0)
                {
                    lvPackingList.Items.Remove(packingListViewItem);
                    packingList.Remove(packingListViewItem.Tag as IPackingListItem);
                }

                TravelDocument newTravelDocument = new("Passport", true);
                packingListViewItem.Content = newTravelDocument.GetInfo();
                packingListViewItem.Tag = newTravelDocument;
                lvPackingList.Items.Add(packingListViewItem);
                packingList.Add(newTravelDocument);
            }
            else
            {
                if (lvPackingList.Items.Count > 0)
                {
                    lvPackingList.Items.Remove(packingListViewItem);
                    packingList.Remove(packingListViewItem.Tag as IPackingListItem);
                }

                TravelDocument newTravelDocument = new("Passport", false);
                packingListViewItem.Content = newTravelDocument.GetInfo();
                packingListViewItem.Tag = newTravelDocument;
                lvPackingList.Items.Add(packingListViewItem);
                packingList.Add(newTravelDocument);
            }
        }

        private void btnPackingListRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lvPackingList.SelectedItem != null)
            {
                ListViewItem item = lvPackingList.SelectedItem as ListViewItem;

                signedInUser.Travels.Remove(item.Tag as Travel);
                lvPackingList.Items.Remove(item);
            }
            else
            {
                MessageBox.Show("Selection required!", "warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void xbDocument_Checked(object sender, RoutedEventArgs e)
        {
            xbRequired.Visibility = Visibility.Visible;
            tbQuantity.Visibility = Visibility.Hidden;
            txtQuantity.Visibility = Visibility.Hidden;
        }

        private void xbDocument_Unchecked(object sender, RoutedEventArgs e)
        {
            xbRequired.IsChecked = false;
            xbRequired.Visibility = Visibility.Hidden;
            tbQuantity.Visibility = Visibility.Visible;
            txtQuantity.Visibility = Visibility.Visible;
        }

        private void btnPackingListAdd_Click(object sender, RoutedEventArgs e)
        {
            bool isRequired = CheckIfRequiredIsChecked();
            ListViewItem newPackingListItem = new();

            if (!String.IsNullOrEmpty(tbItemName.Text))
            {

                if (xbDocument.IsChecked == true)
                {
                    TravelDocument newDocument = new(tbItemName.Text, isRequired);
                    newPackingListItem.Tag = newDocument;
                    newPackingListItem.Content = newDocument.GetInfo();
                    lvPackingList.Items.Add(newPackingListItem);
                    packingList.Add(newDocument);
                }
                else
                {
                    if (!String.IsNullOrEmpty(tbQuantity.Text) && int.TryParse(tbQuantity.Text, out int result))
                    {
                        OtherItem newOtherItem = new(tbItemName.Text, Convert.ToInt32(tbQuantity.Text));
                        newPackingListItem.Tag = newOtherItem;
                        newPackingListItem.Content = newOtherItem.GetInfo();
                        lvPackingList.Items.Add(newPackingListItem);
                        packingList.Add(newOtherItem);

                    }
                    else
                    {
                        MessageBox.Show("Please input a valid quantity!", "warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("All fields must be filled!", "warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            ClearPackingListUI();
        }
    }
}

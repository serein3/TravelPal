using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Enums;
using TravelPal.Interfaces;
using TravelPal.Models;

namespace TravelPal.Managers
{
    public class TravelManager
    {
        public List<Travel> Travels { get; set; } = new();
        public List<string> TravelTypes { get; set; } = new() { "Trip", "Vacation" };

        private UserManager userManager;
        private User signedInUser;
        private List<User> users = new();
        public TravelManager(UserManager userManager)
        {
            this.userManager = userManager;
            signedInUser = userManager.SignedInUser as User;
            AddDefaultTravels();
        }


        // Adds all default user travels to this travels list (Gandalf in this case)
        private void AddDefaultTravels()
        {
            foreach (IUser user in userManager.Users)
            {
                if (user is User)
                {
                    users.Add(user as User);
                }
            }

            foreach (User user in users)
            {
                Travels.AddRange(user.Travels);
            }
        }

        // Adds travel to the general travels list as well as signed in user's travels list
        public void AddTravel(Travel travel)
        {
            Travels.Add(travel);
            signedInUser.Travels.Add(travel);
        }

        // Removes travel from the general travels list as well as the signed in user's travels list
        public void RemoveTravel(Travel travel)
        {
            {
                Travels.Remove(travel);
                signedInUser.Travels.Remove(travel);
            }
        }

        // Removes travel from the general travels list as well as the travel owner's travels list
        public void AdminRemoveTravel(Travel travel)
        {
            Travels.Remove(travel);
            travel.Owner.Travels.Remove(travel);
        }
    }
}

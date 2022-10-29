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


            // Add all existing user travels to this travels list
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

        public void AddTravel(Travel travel)
        {
            Travels.Add(travel);
            signedInUser.Travels.Add(travel);
        }

        public void RemoveTravel(Travel travel)
        {
            {
                Travels.Remove(travel);
                signedInUser.Travels.Remove(travel);
            }
        }

        public void AdminRemoveTravel(Travel travel)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Enums;
using TravelPal.Models;

namespace TravelPal.Managers
{
    public class TravelManager
    {
        public List<Travel> Travels { get; set; } = new();

        private UserManager userManager;
        private User signedInUser;
        public TravelManager(UserManager userManager)
        {
            this.userManager = userManager;
            signedInUser = userManager.SignedInUser as User;
        }

        public void AddTravel(Travel travel)
        {
            Travels.Add(travel);
            signedInUser.Travels.Add(travel);
        }

        public void RemoveTravel(Travel travel)
        {
            Travels.Remove(travel);
            signedInUser.Travels.Remove(travel);
        }
    }
}

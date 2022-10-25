using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Interfaces;
using TravelPal.Models;

namespace TravelPal.Managers
{
    public class UserManager
    {
        public List<User> Users { get; set; } = new();
        public Iuser SignedInUser { get; set; }

        public bool AddUser(IUser user)
        {
            return false;
        }

        public bool UpdateUsername(IUser user, string password)
        {
            return false;
        }

        private bool ValidateUsername(string username)
        {
            return false;
        }

        public bool SignInUser(string username, string password)
        {
            return false;
        }
    }
}

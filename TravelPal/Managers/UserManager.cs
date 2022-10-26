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
    public class UserManager
    {
        public List<IUser> Users { get; set; } = new();
        public IUser SignedInUser { get; set; }

        public UserManager()
        {
            Admin admin = new("admin", "password", Countries.Japan);
            Users.Add(admin);

            User gandalf = new("Gandalf", "password", Countries.Korea);
            Users.Add(gandalf);
        }

        public bool AddUser(IUser newUser)
        {
            if (ValidateUsername(newUser.Username))
            {
                Users.Add(newUser);
                return true;
            }
            return false;
        }

        public bool UpdateUsername(IUser user, string password)
        {
            return false;
        }

        private bool ValidateUsername(string username)
        {
            foreach (IUser user in Users)
            {
                if (user.Username == username)
                {
                    return false;
                }
            }
            return true;
        }

        public bool SignInUser(string username, string password)
        {

            foreach (IUser user in Users)
            {
                if (user.Username == username && user.Password == password)
                {
                    SignedInUser = user;
                    return true;
                }
            }
            return false;
        }

        public bool CheckIfAdmin()
        {
            if (SignedInUser is Admin)
            {
                return true;
            }
            return false;
        }
    }
}

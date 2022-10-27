using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            GenerateDefaultUsers();
        }

        private void GenerateDefaultUsers()
        {
            Admin admin = new("admin", "password", Countries.Japan);
            Users.Add(admin);

            User gandalf = new("Gandalf", "password", Countries.Korea);
            Users.Add(gandalf);
            Vacation vacation1 = new("Frankfurt", Countries.Germany, 2, new DateTime(2022, 10, 30), new DateTime(2022, 11, 07), true);
            gandalf.Travels.Add(vacation1);
            Trip trip1 = new("Tokyo", Countries.Japan, 1, new DateTime(2023, 01, 23), new DateTime(2023, 01, 25), TripTypes.Work);
            gandalf.Travels.Add(trip1);
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

        public bool UpdateUser(IUser userToUpdate)
        {
            if (ValidateUsername(userToUpdate.Username) || userToUpdate.Username == SignedInUser.Username)
            {
                SignedInUser.Username = userToUpdate.Username;
                SignedInUser.Password = userToUpdate.Password;
                SignedInUser.Location = userToUpdate.Location;
                return true;
            }
            return false;
        }

        public bool UpdateUsername(IUser user, string password)
        {
            return false;
        }
        
        public bool ValidateUsernameLength(string username)
        {
            if (username.Length < 3)
            {
                return false;
            }
            return true;
        }

        public bool ValidatePasswordLength(string password)
        {
            if (password.Length < 5)
            {
                return false;
            }
            return true;
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

using PurposeColor.interfaces;
using PurposeColor.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PurposeColor.Database
{
    public class ApplicationSettings
    {
        public SQLite.Net.SQLiteConnection Connection { get; set; }

        public ApplicationSettings()
        {
            try
            {
                Connection = DependencyService.Get<IDBConnection>().GetConnection();
                
                var users = (from t in Connection.Table<User>() select t).ToList();
                if (users == null || users.Count < 1)
                {
                    Connection.CreateTable<User>();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("AplicationSettings :: " + ex.Message);
            }
        }

        public List<User> GetAllUsers()
        {
            try
            {
                return (from t in Connection.Table<User>() select t).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetAllUsers :: " + ex.Message);
                return null;
            }
        }

        public User GetUserWithId(int id)
        {
            try
            {
                return Connection.Table<User>().FirstOrDefault(t => t.ID == id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetUserWithId :: " + ex.Message);
                return null;
            }
        }

        public User GetUserWithUserName(string userName)
        {
            try
            {
                return Connection.Table<User>().FirstOrDefault(t => t.UserName == userName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetUserWithId :: " + ex.Message);
                return null;
            }
        }

        public void DeleteAllUsers()
        {
            try
            {
                Connection.DeleteAll<User>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DeleteAllUsers :: " + ex.Message);
            }
        }

        public void DeleteUserWithId(int id)
        {
            try
            {
                Connection.Delete<User>(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DeleteUserWithId :: " + ex.Message);
            }
        }

        public bool SaveUser(User user)
        {
            bool isUserAdded = false;

            var newUser = new User();
            try
            {
                newUser.UserName = user.UserName;
                newUser.DisplayName = user.DisplayName;
                newUser.AuthenticationToken = user.AuthenticationToken;
                newUser.Password = user.Password;
                //newUser.PreferredGEMS = user.PreferredGEMS;
                newUser.Mobile = user.Mobile;
                newUser.Gender = user.Gender;
                newUser.Age = user.Age;
                newUser.Country = user.Country;

                if (user.ID == 0)
                {
                    Connection.Insert(newUser);
                }
                else
                {
                    newUser.ID = user.ID;
                    Connection.Update(newUser);
                }

                isUserAdded = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SaveUser :: " + ex.Message);
                isUserAdded = false;
                return isUserAdded;
            }

            return isUserAdded;
        }
    }
}

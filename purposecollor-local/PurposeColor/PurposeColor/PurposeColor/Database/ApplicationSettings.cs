using PurposeColor.CustomControls;
using PurposeColor.interfaces;
using PurposeColor.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

                var userTableInfo = Connection.GetTableInfo("User");

                if (userTableInfo == null || userTableInfo.Count < 1)
                {
                    Connection.CreateTable<User>();
                }

                var emationsInfo = Connection.GetTableInfo("Emotion");
                if (emationsInfo == null || emationsInfo.Count < 1)
                {
                    Connection.CreateTable<Emotion>();
                }

                var userEventInfo = Connection.GetTableInfo("UserEvent");
                if (userEventInfo == null || userEventInfo.Count < 1)
                {
                    Connection.CreateTable<UserEvent>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("AplicationSettings :: " + ex.Message);
            }
        }

        #region USER TABLE

        public User GetUser()
        {
            return Connection.Table<User>().FirstOrDefault();
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
                newUser.UserId = user.UserId;

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

        #endregion

        #region EMOTIONS TABLE
        public List<CustomListViewItem> GetAllEmotions()
        {
            try
            {
                List<Emotion> localEmotions = new List<Emotion>();
                localEmotions = (from t in Connection.Table<Emotion>() select t).ToList();

                List<CustomListViewItem> localEmotionListToDisplay = null;

                if (localEmotions != null && localEmotions.Count > 0)
                {
                    localEmotionListToDisplay = new List<CustomListViewItem>();
                    foreach (var item in localEmotions)
                    {
                        localEmotionListToDisplay.Add(new CustomListViewItem { EmotionID = item.EmotionId.ToString(), Name = item.EmpotionName, SliderValue = item.EmotionValue  });
                    }
                    
                }
                return localEmotionListToDisplay;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetAllEmotions :: " + ex.Message);
                return null;
            }
        }

        public bool SaveEmotions(List<CustomListViewItem> emotionList)
        {
            if (emotionList == null || emotionList.Count < 1)
            {
                return false;
            }

            try
            {
                List<Emotion> emotions = new List<Emotion>();
                foreach (CustomListViewItem item in emotionList)
                {
                    emotions.Add(new Emotion { EmpotionName = item.Name, EmotionValue = item.SliderValue, EmotionId = item.SliderValue });
                }
                Connection.InsertAll(emotions);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SaveEmotion :: " + ex.Message);
            }

            return false;
        }

        public void DeleteAllEmotions()
        {
            try
            {
                Connection.DeleteAll<Emotion>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DeleteAllEmotions :: " + ex.Message);
            }
        }

        #endregion

        #region EVENTS TABLE

        public bool SaveEvents(List<CustomListViewItem> eventsList)
        {
            if (eventsList == null || eventsList.Count < 1)
            {
                return false;
            }

            try
            {
                List<UserEvent> events = new List<UserEvent>();
                foreach (CustomListViewItem item in eventsList)
                {
                    events.Add(new UserEvent { EventId = item.SliderValue, EventName = item.Name });
                }
                Connection.InsertAll(events);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SaveEvents :: " + ex.Message);
                return false;
            }
        }

        public List<CustomListViewItem> GetAllEvents()
        {
            try
            {
                List<UserEvent> localEvents = new List<UserEvent>();
                localEvents = (from t in Connection.Table<UserEvent>() select t).ToList();
                List<CustomListViewItem> localEventListToDisplay = null;
                if (localEvents != null && localEvents.Count > 0)
                {
                    localEventListToDisplay = new List<CustomListViewItem>();
                    foreach (var item in localEvents)
                    {
                        localEventListToDisplay.Add(new CustomListViewItem { Name = item.EventName, EventID = item.EventId.ToString()});
                    }
                }
                return localEventListToDisplay;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetAllEvents :: " + ex.Message);
                return null;
            }
        }


        public void DeleteAllEvents()
        {
            try
            {
                Connection.DeleteAll<UserEvent>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DeleteAllEvents :: " + ex.Message);
            }
        }

        #endregion

    }
}

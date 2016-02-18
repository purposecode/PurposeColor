using PurposeColor.CustomControls;
using PurposeColor.interfaces;
using PurposeColor.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                var appSettingsTableInfo = Connection.GetTableInfo("GlobalSettings");
                if (appSettingsTableInfo == null ||appSettingsTableInfo.Count < 1)
                {
                    Connection.CreateTable<GlobalSettings>();
                }

				var gemsEventTitleInfo = Connection.GetTableInfo("EventTitle");
				if (gemsEventTitleInfo == null || gemsEventTitleInfo.Count < 1)
				{
					Connection.CreateTable<EventTitle>();
				}

				var gemsEventDateAndTime = Connection.GetTableInfo("EventDatetime");
				if (gemsEventDateAndTime == null || gemsEventDateAndTime.Count < 1)
				{
					Connection.CreateTable<EventDatetime>();
				}

				var gemsEventDetails = Connection.GetTableInfo("EventDetail");
				if (gemsEventDetails == null || gemsEventDetails.Count < 1)
				{
					Connection.CreateTable<EventDetail>();
				}

				var gemsEventMedia = Connection.GetTableInfo("EventMedia");
				if (gemsEventMedia == null || gemsEventMedia.Count < 1)
				{
					Connection.CreateTable<EventMedia>();
				}


				var gemsEmotions = Connection.GetTableInfo("GemsEmotionsDetailsDB");
				if (gemsEmotions == null || gemsEmotions.Count < 1)
				{
					Connection.CreateTable<GemsEmotionsDetailsDB>();
				}

				var gemsActionTitleInfo = Connection.GetTableInfo("ActionTitle");
				if (gemsActionTitleInfo == null || gemsActionTitleInfo.Count < 1)
				{
					Connection.CreateTable<ActionTitle>();
				}

				var gemsActionDateAndTime = Connection.GetTableInfo("ActionDatetime");
				if (gemsActionDateAndTime == null || gemsActionDateAndTime.Count < 1)
				{
					Connection.CreateTable<ActionDatetime>();
				}

				var gemsActionDetails = Connection.GetTableInfo("ActionDetail");
				if (gemsActionDetails == null || gemsActionDetails.Count < 1)
				{
					Connection.CreateTable<ActionDetail>();
				}

				var gemsActionMedia = Connection.GetTableInfo("ActionMedia");
				if (gemsActionMedia == null || gemsActionMedia.Count < 1)
				{
					Connection.CreateTable<ActionMedia>();
				}

				var gemsGoalsDetailsDB = Connection.GetTableInfo("GemsGoalsDetailsDB");
				if (gemsGoalsDetailsDB == null || gemsGoalsDetailsDB.Count < 1)
				{
					Connection.CreateTable<GemsGoalsDetailsDB>();
				}


                var pendingGoals = Connection.GetTableInfo("PendingGoalsDetailsDB");
                if (pendingGoals == null || pendingGoals.Count < 1)
                {
                    Connection.CreateTable<PendingGoalsDetailsDB>();
                }

                var pendingActionTitle = Connection.GetTableInfo("PendingActionTitle");
                if (pendingActionTitle == null || pendingActionTitle.Count < 1)
                {
                    Connection.CreateTable<PendingActionTitle>();
                }

                var completedGoals = Connection.GetTableInfo("CompletedGoalsDetailsDB");
                if (completedGoals == null || completedGoals.Count < 1)
                {
                    Connection.CreateTable<CompletedGoalsDetailsDB>();
                }

                var completedActionTitle = Connection.GetTableInfo("CompletedActionTitle");
                if (completedActionTitle == null || completedActionTitle.Count < 1)
                {
                    Connection.CreateTable<CompletedActionTitle>();
                }

				var communityGemsDetailsDB = Connection.GetTableInfo("CommunityGemsDetailsDB");
				if (communityGemsDetailsDB == null || communityGemsDetailsDB.Count < 1)
				{
					Connection.CreateTable<CommunityGemsDetailsDB>();
				}

				var gemMedia = Connection.GetTableInfo("GemMedia");
				if (gemMedia == null || gemMedia.Count < 1)
				{
					Connection.CreateTable<GemMedia>();
				}

				var eventITbl = Connection.GetTableInfo("EventWithImage");
				if (eventITbl == null) {
					Connection.CreateTable<EventWithImage>();
				}

				var actionITable = Connection.GetTableInfo("ActionWithImage");
				if (actionITable == null) {
					Connection.CreateTable<ActionWithImage>();
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

        public async Task<User> GetUserWithUserName(string userName)
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

        public async Task<bool> SaveUser(User user)
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
                newUser.UserType = user.UserType;
				newUser.AllowCommunitySharing = user.AllowCommunitySharing;

                if (Connection.Table<User>().FirstOrDefault(t => t.UserName == user.UserName) == null)
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

        #region APP SETTINGS TABLE

        public GlobalSettings GetAppGlobalSettings()
        {
            return Connection.Table<GlobalSettings>().FirstOrDefault();
        }

        public async Task<bool> SaveAppGlobalSettings(GlobalSettings settings)
        {
            bool isAppGlobalSettingsAdded = false;
            try
            {
                var newSettings = new GlobalSettings();
                newSettings.IsLoggedIn = settings.IsLoggedIn;
                newSettings.ShowRegistrationScreen = settings.ShowRegistrationScreen;
                newSettings.IsFirstLogin = settings.IsFirstLogin;

                Connection.DeleteAll<GlobalSettings>();
                Connection.Insert(newSettings);

                isAppGlobalSettingsAdded = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SaveAppGlobalSettings :: " + ex.Message);
                isAppGlobalSettingsAdded = false;
            }

            return isAppGlobalSettingsAdded;
        }
        
        #endregion

		#region GEMS


		public void DeleteAllGemsActions()
		{
			try
			{
				Connection.DeleteAll<ActionMedia>();
				Connection.DeleteAll<ActionDetail>();
				Connection.DeleteAll<ActionTitle>();
				Connection.DeleteAll<ActionDatetime>();
				Connection.DeleteAll<GemsGoalsDetailsDB>();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("DeleteAllEvents :: " + ex.Message);
			}
		}


		public GemsGoalsObject GetGemsGoalsObject()
		{
            try
            {
                GemsGoalsObject masterObject = new GemsGoalsObject();


                List<ActionTitle> listActionTitle = new List<ActionTitle>();
                List<ActionDetail> listActionDetails = new List<ActionDetail>();
                List<ActionMedia> listActionMedia = new List<ActionMedia>();
                List<ActionDatetime> listActionDatetime = new List<ActionDatetime>();
                List<GemsGoalsDetailsDB> listGoals = new List<GemsGoalsDetailsDB>();
                List<GemsGoalsDetails> gemsGoals = new List<GemsGoalsDetails>();

                listActionTitle = (from t in Connection.Table<ActionTitle>() select t).ToList();
                listActionDetails = (from t in Connection.Table<ActionDetail>() select t).ToList();
                listActionMedia = (from t in Connection.Table<ActionMedia>() select t).ToList();
                listActionDatetime = (from t in Connection.Table<ActionDatetime>() select t).ToList();
                listGoals = (from t in Connection.Table<GemsGoalsDetailsDB>() select t).ToList();

                masterObject.resultarray = new List<GemsGoalsDetails>();
                foreach (var item in listGoals)
                {
                    GemsGoalsDetails resultArray = new GemsGoalsDetails();

                    resultArray.user_id = item.user_id;
                    resultArray.goal_id = item.goal_id;
                    resultArray.goal_title = item.goal_title;

                    resultArray.action_title = new List<ActionTitle>();
                    resultArray.action_details = new List<ActionDetail>();
                    resultArray.action_datetime = new List<ActionDatetime>();
                    resultArray.action_media = new List<ActionMedia>();

                    // Title
                    foreach (var titeObject in listActionTitle)
                    {
                        if (titeObject.goal_id == item.goal_id)
                            resultArray.action_title.Add(titeObject);
                    }

                    // Details
                    foreach (var detailsObject in listActionDetails)
                    {
                        if (detailsObject.goal_id == item.goal_id)
                            resultArray.action_details.Add(detailsObject);
                    }

                    // date and time
                    foreach (var dateObject in listActionDatetime)
                    {
                        if (dateObject.goal_id == item.goal_id)
                            resultArray.action_datetime.Add(dateObject);
                    }

                    // media
                    foreach (var mediaObject in listActionMedia)
                    {
                        if (mediaObject.goal_id == item.goal_id)
                            resultArray.action_media.Add(mediaObject);
                    }

                    masterObject.resultarray.Add(resultArray);
                }


                masterObject.code = (listGoals != null && listGoals.Count > 0) ? listGoals[0].code : "";
                masterObject.noimageurl = (listGoals != null && listGoals.Count > 0) ? listGoals[0].noimageurl : "";
                masterObject.mediapath = (listGoals != null && listGoals.Count > 0) ? listGoals[0].mediapath : "";
                masterObject.mediathumbpath = (listGoals != null && listGoals.Count > 0) ? listGoals[0].mediathumbpath : "";
                return masterObject;
            }
            catch (Exception)
            {
                
                throw;
            }
		
		}


		public bool SaveAllGoalsGems(  GemsGoalsObject gemsGoals )
		{
			try
			{
				if (gemsGoals == null || gemsGoals.resultarray == null)
				{
					return false;
				}

				List<ActionTitle> listActionTitle = new List<ActionTitle>();
				List<ActionDetail> listActionDetails = new List<ActionDetail>();
				List<ActionMedia> listActionMedia = new List<ActionMedia>();
				List<ActionDatetime> listActionDatetime = new List<ActionDatetime>();
				List<GemsGoalsDetailsDB> listGoals = new List<GemsGoalsDetailsDB>();
				foreach (var item in gemsGoals.resultarray )
				{
					listActionTitle.Clear();
					listActionDetails.Clear();
					listActionMedia.Clear();
					listActionDatetime.Clear();
					listGoals.Clear();

					// Emotion details
					GemsGoalsDetailsDB emotion = new GemsGoalsDetailsDB(){ code = gemsGoals.code, goal_id = item.goal_id, goal_title = item.goal_title,
						mediapath = gemsGoals.mediapath, mediathumbpath = gemsGoals.mediathumbpath, noimageurl = gemsGoals.noimageurl, user_id = item.user_id};
					listGoals.Add(emotion);
					Connection.InsertAll(listGoals);

					if( item.action_title != null && item.action_title.Count > 0 )
					{
						// event title
						foreach (var actionTitleItem in item.action_title)
						{
							actionTitleItem.goal_id= item.goal_id;
							listActionTitle.Add(actionTitleItem);

						}
						Connection.InsertAll(listActionTitle);
					}


					if( item.action_details != null && item.action_details.Count > 0 )
					{
						// event details
						foreach (var actionDetailItem in item.action_details)
						{
							actionDetailItem.goal_id = item.goal_id;
							listActionDetails.Add(actionDetailItem);
						}
						Connection.InsertAll(listActionDetails);
					}

					if( item.action_media != null && item.action_media.Count > 0 )
					{
						// event media
						foreach (var actionMediaItem in item.action_media)
						{
							actionMediaItem.goal_id = item.goal_id;
							listActionMedia.Add(actionMediaItem);
						}
						Connection.InsertAll(listActionMedia);
					}


					if( item.action_datetime != null && item.action_datetime.Count > 0 )
					{
						// event date and time
						foreach (var actionDateItem in item.action_datetime)
						{
							actionDateItem.goal_id = item.goal_id;
							listActionDatetime.Add(actionDateItem);
						}
						Connection.InsertAll(listActionDatetime);
					}


				}

				listGoals.Clear();


				listGoals = null;
				listActionDatetime = null;
				listActionDetails = null;
				listActionMedia = null;
				listActionTitle = null;
				return true;
			}
			catch (Exception ex)
			{

				throw;
			}

		}


		public void DeleteAllGemsEvents()
		{
			try
			{
				Connection.DeleteAll<EventMedia>();
				Connection.DeleteAll<EventDetail>();
				Connection.DeleteAll<EventTitle>();
				Connection.DeleteAll<EventDatetime>();
				Connection.DeleteAll<GemsEmotionsDetailsDB>();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("DeleteAllEvents :: " + ex.Message);
			}
		}


		public GemsEmotionsObject GetGemsEmotionsObject()
		{
            try
            {
                GemsEmotionsObject masterObject = new GemsEmotionsObject();

                List<EventTitle> listEventTitle = new List<EventTitle>();
                List<EventDetail> listEventDetails = new List<EventDetail>();
                List<EventMedia> listEventMedia = new List<EventMedia>();
                List<EventDatetime> listEventDatetime = new List<EventDatetime>();
                List<GemsEmotionsDetailsDB> listEmotions = new List<GemsEmotionsDetailsDB>();
                List<GemsEmotionsDetails> gemsEmotions = new List<GemsEmotionsDetails>();

                listEventTitle = (from t in Connection.Table<EventTitle>() select t).ToList();
                listEventDetails = (from t in Connection.Table<EventDetail>() select t).ToList();
                listEventMedia = (from t in Connection.Table<EventMedia>() select t).ToList();
                listEventDatetime = (from t in Connection.Table<EventDatetime>() select t).ToList();
                listEmotions = (from t in Connection.Table<GemsEmotionsDetailsDB>() select t).ToList();

                masterObject.resultarray = new List<GemsEmotionsDetails>();
                foreach (var item in listEmotions)
                {
                    GemsEmotionsDetails resultArray = new GemsEmotionsDetails();

                    resultArray.user_id = item.user_id;
                    resultArray.emotion_id = item.emotion_id;
                    resultArray.emotion_title = item.emotion_title;

                    resultArray.event_title = new List<EventTitle>();
                    resultArray.event_details = new List<EventDetail>();
                    resultArray.event_datetime = new List<EventDatetime>();
                    resultArray.event_media = new List<EventMedia>();

                    // Title
                    foreach (var titeObject in listEventTitle)
                    {
                        if (titeObject.emotion_id == item.emotion_id)
                            resultArray.event_title.Add(titeObject);
                    }

                    // Details
                    foreach (var detailsObject in listEventDetails)
                    {
                        if (detailsObject.emotion_id == item.emotion_id)
                            resultArray.event_details.Add(detailsObject);
                    }

                    // date and time
                    foreach (var dateObject in listEventDatetime)
                    {
                        if (dateObject.emotion_id == item.emotion_id)
                            resultArray.event_datetime.Add(dateObject);
                    }

                    // media
                    foreach (var mediaObject in listEventMedia)
                    {
                        if (mediaObject.emotion_id == item.emotion_id)
                            resultArray.event_media.Add(mediaObject);
                    }

                    masterObject.resultarray.Add(resultArray);
                }


                masterObject.code = (listEmotions != null && listEmotions.Count > 0) ? listEmotions[0].code : "";
                masterObject.noimageurl = (listEmotions != null && listEmotions.Count > 0) ? listEmotions[0].noimageurl : "";
                masterObject.mediapath = (listEmotions != null && listEmotions.Count > 0) ? listEmotions[0].mediapath : "";
                masterObject.mediathumbpath = (listEmotions != null && listEmotions.Count > 0) ? listEmotions[0].mediathumbpath : "";
                return masterObject;
            }
            catch (Exception)
            {
                
                return null;
            }

		}


        public void DeleteAllPendingGoals()
        {
            try
            {
                Connection.DeleteAll<PendingGoalsDetailsDB>();
                Connection.DeleteAll<PendingActionTitle>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DeleteAllEvents :: " + ex.Message);
            }
        }


        public void DeleteAllCompletedGoals()
        {
            try
            {
                Connection.DeleteAll<CompletedActionTitle>();
                Connection.DeleteAll<CompletedGoalsDetailsDB>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DeleteAllEvents :: " + ex.Message);
            }
        }

        public PendingGoalsObject GetPendingGoalsObject()
        {
            try
            {
                PendingGoalsObject masterObject = new PendingGoalsObject();

                List<PendingActionTitle> listPendingActionTitle = new List<PendingActionTitle>();
                List<PendingGoalsDetailsDB> listPendingDetailsDB = new List<PendingGoalsDetailsDB>();

                listPendingActionTitle = (from t in Connection.Table<PendingActionTitle>() select t).ToList();
                listPendingDetailsDB = (from t in Connection.Table<PendingGoalsDetailsDB>() select t).ToList();

                masterObject.resultarray = new List<PendingGoalsDetails>();

                foreach (var item in listPendingDetailsDB)
                {
                    PendingGoalsDetails resultArray = new PendingGoalsDetails();
                    resultArray.pending_action_title = new List<PendingActionTitle>();

                    resultArray.user_id = item.user_id;
                    resultArray.goal_id = item.goal_id;
                    resultArray.goal_title = item.goal_title;
                    resultArray.goal_details = item.goal_details;
                    resultArray.goal_media = item.goal_media;
                    masterObject.code = item.code;
                    masterObject.mediapath = item.mediapath;
                    masterObject.mediathumbpath = item.mediathumbpath;
                    masterObject.noimageurl = item.noimageurl;

                    resultArray.pending_action_title = new List<PendingActionTitle>();

                    // Title
                    foreach (var titeObject in listPendingActionTitle)
                    {
                        if (titeObject.goal_id == item.goal_id)
                            resultArray.pending_action_title.Add(titeObject);
                    }

                    masterObject.resultarray.Add(resultArray);
                }


                masterObject.code = (listPendingDetailsDB != null && listPendingDetailsDB.Count > 0) ? listPendingDetailsDB[0].code : "";
                masterObject.noimageurl = (listPendingDetailsDB != null && listPendingDetailsDB.Count > 0) ? listPendingDetailsDB[0].noimageurl : "";
                masterObject.mediapath = (listPendingDetailsDB != null && listPendingDetailsDB.Count > 0) ? listPendingDetailsDB[0].mediapath : "";
                masterObject.mediathumbpath = (listPendingDetailsDB != null && listPendingDetailsDB.Count > 0) ? listPendingDetailsDB[0].mediathumbpath : "";
                return masterObject;
            }
            catch (Exception)
            {

                return null;
            }

        }


        public GemsGoalsObject GetCompletedGoalsObject()
        {
            try
            {
                GemsGoalsObject masterObject = new GemsGoalsObject();

                List<CompletedActionTitle> listCompletedActionTitle = new List<CompletedActionTitle>();
                List<CompletedGoalsDetailsDB> listCompletedDetailsDB = new List<CompletedGoalsDetailsDB>();

                listCompletedActionTitle = (from t in Connection.Table<CompletedActionTitle>() select t).ToList();
                listCompletedDetailsDB = (from t in Connection.Table<CompletedGoalsDetailsDB>() select t).ToList();

                masterObject.resultarray = new List<GemsGoalsDetails>();

                foreach (var item in listCompletedDetailsDB)
                {
                    GemsGoalsDetails resultArray = new GemsGoalsDetails();
                    resultArray.action_title = new List<ActionTitle>();

                    resultArray.user_id = item.user_id;
                    resultArray.goal_id = item.goal_id;
                    resultArray.goal_title = item.goal_title;
                    resultArray.goal_details = item.goal_details;
                    resultArray.goal_media = item.goal_media;
                    masterObject.code = item.code;
                    masterObject.mediapath = item.mediapath;
                    masterObject.mediathumbpath = item.mediathumbpath;
                    masterObject.noimageurl = item.noimageurl;


                    // Title
                    foreach (var titeObject in listCompletedActionTitle)
                    {
                        ActionTitle title = new ActionTitle() { action_title = titeObject.action_title, goal_id = titeObject.goal_id, goalaction_id = titeObject.goalaction_id };
                        if (titeObject.goal_id == item.goal_id)
                            resultArray.action_title.Add(title);
                    }

                    masterObject.resultarray.Add(resultArray);
                }


                masterObject.code = (listCompletedDetailsDB != null && listCompletedDetailsDB.Count > 0) ? listCompletedDetailsDB[0].code : "";
                masterObject.noimageurl = (listCompletedDetailsDB != null && listCompletedDetailsDB.Count > 0) ? listCompletedDetailsDB[0].noimageurl : "";
                masterObject.mediapath = (listCompletedDetailsDB != null && listCompletedDetailsDB.Count > 0) ? listCompletedDetailsDB[0].mediapath : "";
                masterObject.mediathumbpath = (listCompletedDetailsDB != null && listCompletedDetailsDB.Count > 0) ? listCompletedDetailsDB[0].mediathumbpath : "";
                return masterObject;
            }
            catch (Exception)
            {

                return null;
            }

        }

        public bool SavePendingGoalsDetails(PendingGoalsObject pendingGoals)
        {
            try
            {
                if (pendingGoals == null || pendingGoals.resultarray == null  )
                {
                    return false;
                }

                List<PendingGoalsDetailsDB> listPendingGoalsDetails = new List<PendingGoalsDetailsDB>();
                List<PendingActionTitle> listPendingActionTitle = new List<PendingActionTitle>();

                foreach (var item in pendingGoals.resultarray)
                {

                    // Emotion details
                    PendingGoalsDetailsDB goal = new PendingGoalsDetailsDB()
                    {
                        goal_details = item.goal_details,
                        goal_id = item.goal_id,
                        goal_media = item.goal_media,
                        user_id = item.user_id,
                        code = pendingGoals.code,
                        mediapath = pendingGoals.mediapath,
                        mediathumbpath = pendingGoals.mediathumbpath,
                        noimageurl = pendingGoals.noimageurl
                    };
                    listPendingGoalsDetails.Add(goal);

                    foreach (var itemTitle in item.pending_action_title)
                    {
                        listPendingActionTitle.Add(itemTitle);
                    }
                    
                }

                Connection.InsertAll(listPendingGoalsDetails);
                Connection.InsertAll(listPendingActionTitle);

                listPendingActionTitle.Clear();
                listPendingGoalsDetails.Clear();

                listPendingActionTitle = null;
                listPendingGoalsDetails = null;

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }


        public bool SaveCompletedGoalsDetails(GemsGoalsObject completedGoals)
        {
            try
            {
                if (completedGoals == null || completedGoals.resultarray == null)
                {
                    return false;
                }

                List<CompletedGoalsDetailsDB> listCompletedGoalsDetails = new List<CompletedGoalsDetailsDB>();
                List<CompletedActionTitle> listCompletedActionTitle = new List<CompletedActionTitle>();

                foreach (var item in completedGoals.resultarray)
                {

                    // goals details
                    CompletedGoalsDetailsDB goal = new CompletedGoalsDetailsDB()
                    {
                        goal_details = item.goal_details,
                        goal_id = item.goal_id,
                        goal_media = item.goal_media,
                        user_id = item.user_id,
                        code = completedGoals.code,
                        mediapath = completedGoals.mediapath,
                        mediathumbpath = completedGoals.mediathumbpath,
                        noimageurl = completedGoals.noimageurl
                    };
                    listCompletedGoalsDetails.Add(goal);

                    foreach (var itemTitle in item.action_title)
                    {
                        CompletedActionTitle title = new CompletedActionTitle() { goal_id = itemTitle.goal_id,  goalaction_id = itemTitle.goalaction_id, action_title = itemTitle.action_title };
                        listCompletedActionTitle.Add(title);
                    }

                }

                Connection.InsertAll(listCompletedGoalsDetails);
                Connection.InsertAll(listCompletedActionTitle);

                listCompletedActionTitle.Clear();
                listCompletedGoalsDetails.Clear();

                listCompletedActionTitle = null;
                listCompletedGoalsDetails = null;

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }



		public bool SaveAllEmotionGems(  GemsEmotionsObject gemsEmotions )
		{
			try
			{
				if (gemsEmotions == null || gemsEmotions.resultarray == null)
				{
					return false;
				}

				List<EventTitle> listEventTitle = new List<EventTitle>();
				List<EventDetail> listEventDetails = new List<EventDetail>();
				List<EventMedia> listEventMedia = new List<EventMedia>();
				List<EventDatetime> listEventDatetime = new List<EventDatetime>();
				List<GemsEmotionsDetailsDB> listEmotions = new List<GemsEmotionsDetailsDB>();
				foreach (var item in gemsEmotions.resultarray )
				{
					listEventTitle.Clear();
					listEventDetails.Clear();
					listEventMedia.Clear();
					listEventDatetime.Clear();
					listEmotions.Clear();

					// Emotion details
					GemsEmotionsDetailsDB emotion = new GemsEmotionsDetailsDB(){ code = gemsEmotions.code, emotion_id = item.emotion_id, emotion_title = item.emotion_title,
						mediapath = gemsEmotions.mediapath, mediathumbpath = gemsEmotions.mediathumbpath, noimageurl = gemsEmotions.noimageurl, user_id = item.user_id};
					listEmotions.Add(emotion);
					Connection.InsertAll(listEmotions);

					// event title
					foreach (var eventTitleItem in item.event_title)
					{
						eventTitleItem.emotion_id = item.emotion_id;
						listEventTitle.Add(eventTitleItem);

					}
					Connection.InsertAll(listEventTitle);

					// event details
					foreach (var eventDetailItem in item.event_details)
					{
						eventDetailItem.emotion_id = item.emotion_id;
						listEventDetails.Add(eventDetailItem);
					}
					Connection.InsertAll(listEventDetails);

					// event media
					foreach (var eventMediaItem in item.event_media)
					{
						eventMediaItem.emotion_id = item.emotion_id;
						listEventMedia.Add(eventMediaItem);
					}
					Connection.InsertAll(listEventMedia);


					// event date and time
					foreach (var eventDateItem in item.event_datetime)
					{
						eventDateItem.emotion_id = item.emotion_id;
						listEventDatetime.Add(eventDateItem);
					}
					Connection.InsertAll(listEventDatetime);

				}

				listEmotions.Clear();


				listEmotions = null;
				listEventDatetime = null;
				listEventDetails = null;
				listEventMedia = null;
				listEventTitle = null;
				return true;
			}
			catch (Exception ex)
			{

				return false;
			}

		}
		#endregion



		public void DeleteCommunityGems()
		{
			try
			{
				Connection.DeleteAll<GemMedia>();
				Connection.DeleteAll<CommunityGemsDetailsDB>();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("DeleteAllEvents :: " + ex.Message);
			}
		}


		public CommunityGemsObject GetCommunityGemsObject()
		{
			try
			{
				CommunityGemsObject masterObject = new CommunityGemsObject();

				List<CommunityGemsDetailsDB> listCommunityDetails = new List<CommunityGemsDetailsDB>();
				List<GemMedia> listGemMedia = new List<GemMedia>();

				listCommunityDetails = (from t in Connection.Table<CommunityGemsDetailsDB>() select t).ToList();
				listGemMedia = (from t in Connection.Table<GemMedia>() select t).ToList();

				masterObject.resultarray = new List<CommunityGemsDetails>();

				foreach (var item in listCommunityDetails)
				{
					CommunityGemsDetails resultArray = new CommunityGemsDetails();
					resultArray.gem_media = new List<GemMedia>();

					resultArray.user_id = item.user_id;
					resultArray.like_status = item.like_status;
					resultArray.likecount = item.likecount;
					resultArray.gem_id = item.gem_id;
					resultArray.gem_title = item.gem_title;
					resultArray.gem_details = item.gem_details;
					resultArray.gem_datetime = item.gem_datetime;
					resultArray.share_status = item.share_status;
					resultArray.firstname = item.firstname;
					resultArray.profileimg = item.profileimg;
					resultArray.gem_type = item.gem_type;


					// Title
					foreach (var mediaObject in listGemMedia)
					{
						if (mediaObject.gem_id == item.gem_id)
							resultArray.gem_media.Add(mediaObject);
					}

					masterObject.resultarray.Add(resultArray);
				}

				return masterObject;
			}
			catch (Exception)
			{

				return null;
			}

		}


		public bool SaveCommunityGemsDetails( CommunityGemsObject gemsObject )
		{
			try
			{
				if (gemsObject == null || gemsObject.resultarray == null)
				{
					return false;
				}

				List<CommunityGemsDetailsDB> listCommunityGemsDetails = new List<CommunityGemsDetailsDB>();
				List<GemMedia> listGemMedia = new List<GemMedia>();

				foreach (var item in gemsObject.resultarray)
				{


					if( item != null )
					{
						CommunityGemsDetailsDB detail = new CommunityGemsDetailsDB();
						detail.firstname = item.firstname;
						detail.gem_datetime = item.gem_datetime;
						detail.gem_details = item.gem_details;
						detail.gem_id =  item.gem_id;
						detail.gem_title = item.gem_title;
						detail.likecount = item.likecount;
						detail.like_status = item.like_status;
						detail.profileimg = item.profileimg;
						detail.share_status = item.share_status;
						detail.user_id = item.user_id;
						detail.gem_type = item.gem_type;
						listCommunityGemsDetails.Add( detail );
					}


					foreach (var gemMedia in item.gem_media)
					{
						GemMedia media = new GemMedia();
						media = gemMedia;
						listGemMedia.Add(media);
					}

				}

				Connection.InsertAll(listCommunityGemsDetails);
				Connection.InsertAll(listGemMedia);

				listGemMedia.Clear();
				listCommunityGemsDetails.Clear();

				listGemMedia = null;
				listCommunityGemsDetails = null;

				return true;
			}
			catch (Exception ex)
			{

				return false;
			}
		}

		#region EventsWithImage

		//ActionWithImage
		public async Task<bool> SaveEventsWithImage(List<EventWithImage> eventsWithImg)
		{
			try {
				if (eventsWithImg == null) {
					return false;
				}

				Connection.DeleteAll<EventWithImage>();
				Connection.InsertAll(eventsWithImg);
				return true;
			} catch (Exception ex) {
				var test = ex.Message;
			}
			return false;
		}

		public async Task<List<EventWithImage>> GetAllEventWithImage()
		{
			try {
				List<EventWithImage> eventsWithI = new List<EventWithImage> ();
				eventsWithI = (from c in Connection.Table<EventWithImage> ()
					select c).ToList ();
				return eventsWithI;
			} catch (Exception ex) {
				var test = ex.Message;
			}
			return null;
		}


		#endregion

		#region ActionWithImage

		//List<ActionWithImage>
		public async Task<bool> SaveActionsWithImage(List<ActionWithImage> actionWithImage)
		{
			try {
				if (actionWithImage == null) {
					return false;
				} else {
					Connection.DeleteAll<ActionWithImage>();
					Connection.InsertAll(actionWithImage);
					return true;
				}
			} catch (Exception ex) {
				var test = ex.Message;
			}
			return false;
		}

		public async Task<List<ActionWithImage>> GetAllActionWithImage()
		{
			try {
				List<ActionWithImage> actionWithImage = new List<ActionWithImage>();
				actionWithImage = (from tbl in Connection.Table<ActionWithImage>() select tbl).ToList();
				return actionWithImage ;
			} catch (Exception ex) {
				var test = ex.Message;
			}
			return null;
		}

		#endregion

    }
}

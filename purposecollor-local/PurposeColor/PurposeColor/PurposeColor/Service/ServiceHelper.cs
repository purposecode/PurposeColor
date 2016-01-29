using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using PurposeColor.Model;
using PurposeColor.CustomControls;
using System.Net.Http;
using Plugin.Connectivity;

namespace PurposeColor.Service
{
    public class ServiceHelper
    {
        public static async Task<List<CustomListViewItem>> GetEmotions(int sliderValue)
        {

            if (!CrossConnectivity.Current.IsConnected)
            {
                return null;
            }


            var client = new System.Net.Http.HttpClient();

            client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);

            string uriString = "api.php?action=emotions&user_id=2&emotion_value=" + sliderValue.ToString();

            var response = await client.GetAsync(uriString);

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                var earthquakesJson = response.Content.ReadAsStringAsync().Result;

                var rootobject = JsonConvert.DeserializeObject<Emotions>(earthquakesJson);

                List<CustomListViewItem> emotionsList = new List<CustomListViewItem>();

                if (rootobject != null && rootobject.emotion_title != null)
                {
                    foreach (var item in rootobject.emotion_title)
                    {
                        CustomListViewItem listItem = new CustomListViewItem();
                        listItem.Name = item;
                        listItem.SliderValue = sliderValue;
                        emotionsList.Add(listItem);
                    }
                    client.Dispose();
                }
                return emotionsList;
            }

            List<CustomListViewItem> noEmotionsList = new List<CustomListViewItem>();
            noEmotionsList.Add(new CustomListViewItem { Name = "No emotions Found" });
            return null;

        }

        public static async Task<List<CustomListViewItem>> GetAllEmotions(int userID)
        {
            try
            {
				
                if (!CrossConnectivity.Current.IsConnected)
                {
                    return null;
                }


                var client = new System.Net.Http.HttpClient();

                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);

                string uriString = "api.php?action=getallemotions&user_id=" + userID.ToString();

                var response = await client.GetAsync(uriString);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var allEmotionsJson = response.Content.ReadAsStringAsync().Result;

                    var rootobject = JsonConvert.DeserializeObject<EmotionsCollections>(allEmotionsJson);

                    List<CustomListViewItem> emotionsList = new List<CustomListViewItem>();

                    if (rootobject != null && rootobject.resultarray != null)
                    {
                        foreach (var item in rootobject.resultarray)
                        {
                            CustomListViewItem listItem = new CustomListViewItem();
                            listItem.Name = item.emotion_title;
                            listItem.EmotionID = item.emotion_id;
                            listItem.SliderValue = Convert.ToInt32(item.emotion_value);
                            emotionsList.Add(listItem);
                        }
                        client.Dispose();
                    }
                    return emotionsList;
                }

                List<CustomListViewItem> noEmotionsList = new List<CustomListViewItem>();
                noEmotionsList.Add(new CustomListViewItem { Name = "No emotions Found" });
                return null;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }


        }

        public static async Task<List<CustomListViewItem>> GetGoalCategory(int userID)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    return null;
                }


                var client = new System.Net.Http.HttpClient();

                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);

                string uriString = "api.php?action=getallcategory";

                var response = await client.GetAsync(uriString);

                var allEmotionsJson = response.Content.ReadAsStringAsync().Result;

                var rootobject = JsonConvert.DeserializeObject<EmotionsCollections>(allEmotionsJson);
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static async Task<List<CustomListViewItem>> GetAllGoals(int userID)
        {
            List<CustomListViewItem> goalsList = null;
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    return null;
                }

                var client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);
                string uriString = "api.php?action=getallgoals&user_id=" + userID.ToString();
                var response = await client.GetAsync(uriString);
                var allGoalsJson = response.Content.ReadAsStringAsync().Result;
                goalsList = new List<CustomListViewItem>();

                var rootobject = JsonConvert.DeserializeObject<AllGoals>(allGoalsJson);
                if (rootobject != null && rootobject.resultarray != null)
                {
                    foreach (var item in rootobject.resultarray)
                    {
                        CustomListViewItem listItem = new CustomListViewItem();
                        listItem.Name = item.goal_title;
                        listItem.EventID = item.goal_id;
                        goalsList.Add(listItem);
                    }
                    client.Dispose();
                }

                return goalsList;
            }
            catch (Exception ex)
            {
                var test = ex.Message;
                return goalsList;
            }
        }

        public static async Task<bool> GetCurrentAddressToList( double lattitude, double longitude )
        {
            try
            {
              if (!CrossConnectivity.Current.IsConnected)
                {
                    return false;
                }

                var client = new System.Net.Http.HttpClient();
                //User user = App.Settings.GetUser();

                ///////// for testing

                User user = new User { UserId = 2, UserName = "sam" };

                if (user == null)
                {
                    // show alert
                    return false;
                }
                else
                {
                    client.BaseAddress = new Uri("http://maps.googleapis.com/maps/api/geocode/");
                    string lat = lattitude.ToString();
                    string lon = longitude.ToString();
                    string uriString = "json?latlng=" + lat + "," + lon + "&sensor=true";
                    var response = await client.GetAsync(uriString);
                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        var eventsJson = response.Content.ReadAsStringAsync().Result;

                        var rootobject = JsonConvert.DeserializeObject<AddressBase>(eventsJson);

                        if( rootobject != null  && rootobject.results != null )
                        {
                            foreach (var item in rootobject.results )
                            {
                                CustomListViewItem listItem = new CustomListViewItem();
                                listItem.Name = item.formatted_address;
                                App.nearByLocationsSource.Add(listItem);
                            }
                            client.Dispose();
                        }

                       return true;
                    }
                    return true;
                }

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
            return true;
        }

        public static async Task<bool> GetNearByLocations( double lattitude, double longitude )
        {
            try
            {
              if (!CrossConnectivity.Current.IsConnected)
                {
                    return false;
                }

                var client = new System.Net.Http.HttpClient();
                //User user = App.Settings.GetUser();

                ///////// for testing

                User user = new User { UserId = 2, UserName = "sam" };

                if (user == null)
                {
                    // show alert
                    return false;
                }
                else
                {
                    client.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/place/nearbysearch/");
                    string lat = lattitude.ToString();
                    string lon = longitude.ToString();
                    string uriString = "json?location=" + lat + "," + lon + "&radius=500&key=AIzaSyAuqCJwc2K4wQeUvTcywvoR9WcsRI5AIn4";
                    var response = await client.GetAsync(uriString);
                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        var eventsJson = response.Content.ReadAsStringAsync().Result;

                        var rootobject = JsonConvert.DeserializeObject<LocationMasterObject>(eventsJson);

                        if( rootobject != null  && rootobject.results != null )
                        {
                            foreach (var item in rootobject.results)
                            {
                                CustomListViewItem listItem = new CustomListViewItem();
                                listItem.Name = item.name;
                                App.nearByLocationsSource.Add( listItem );
                            }
                            client.Dispose();
                        }

                       return true;
                    }
                    return true;
                }

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
            return true;
        }

        public static async Task<List<CustomListViewItem>> GetAllEvents()
        {
            try
            {

                if (!CrossConnectivity.Current.IsConnected)
                {
                    return null;
                }

                var client = new System.Net.Http.HttpClient();
                //User user = App.Settings.GetUser();

                ///////// for testing

                User user = new User { UserId = 2, UserName = "sam" };

                if (user == null)
                {
                    // show alert
                    return null;
                }
                else
                {
                    client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);
                    string uriString = "api.php?action=getallevents&user_id=" + user.UserId;
                    var response = await client.GetAsync(uriString);
                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        var eventsJson = response.Content.ReadAsStringAsync().Result;

                        var rootobject = JsonConvert.DeserializeObject<AllEvents>(eventsJson);

                        List<CustomListViewItem> eventsList = new List<CustomListViewItem>();

                        if (rootobject != null && rootobject.resultarray != null)
                        {
                            foreach (var item in rootobject.resultarray)
                            {
                                CustomListViewItem listItem = new CustomListViewItem();
                                listItem.Name = item.event_title;
                                listItem.EventID = item.event_id;
                                eventsList.Add(listItem);
                            }
                            client.Dispose();
                        }
                        return eventsList;
                    }
                }// else ie, user != null

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
            return null;
        }

        public static async Task<List<CustomListViewItem>> GetAllSpportingActions()
        {
            List<CustomListViewItem> actionsList = null;
            try
            {
                //////// for testing

                User user = new User { UserId = 2, UserName = "sam" };

                if (user == null)
                {
                    // show alert
                    return null;
                }

                if (!CrossConnectivity.Current.IsConnected)
                {
                    return null;
                }
                var client = new System.Net.Http.HttpClient();

                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);

                string uriString = "api.php?action=getallactions&user_id=" + user.UserId.ToString();

                var response = await client.GetAsync(uriString);

                var actionsJson = response.Content.ReadAsStringAsync().Result;

                actionsList = new List<CustomListViewItem>();

                var rootobject = JsonConvert.DeserializeObject<AllSupportingActions>(actionsJson);
                if (rootobject != null && rootobject.resultarray != null)
                {
                    foreach (var item in rootobject.resultarray)
                    {
                        CustomListViewItem listItem = new CustomListViewItem();
                        listItem.Name = item.action_title;
                        listItem.EventID = item.goalaction_id;
                        actionsList.Add(listItem);
                    }
                    client.Dispose();
                }
                return actionsList;
            }
            catch (Exception ex)
            {
                var test = ex.Message;
                return actionsList;
            }
        }

        public static async Task<bool> PostMedia(MediaPost mediaFile)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return false;
            }

            string url = "http://purposecodes.com/pc/api.php?action=eventinsert";
            string result = String.Empty;

            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                       {
                             new KeyValuePair<string, string>("event_details", mediaFile.event_details),
                             new KeyValuePair<string, string>("event_title", mediaFile.event_title),
                              new KeyValuePair<string, string>("user_id",  mediaFile.user_id.ToString()),
                              new KeyValuePair<string, string>("event_image",  mediaFile.event_image )
                        });



                using (var response = await client.PostAsync(url, content))
                {
                    using (var responseContent = response.Content)
                    {


                    }
                }
            }
            return true;
        }

        public static async Task<bool> AddEmotion(string emotionID, string title, string userID)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return false;
            }

            try
            {
                string url = "http://purposecodes.com/pc/api.php?action=newemotion";
                string result = String.Empty;

                using (var client = new HttpClient())
                {
                    var content = new FormUrlEncodedContent(new[]
                           {
                                 new KeyValuePair<string, string>("emotion_value", emotionID),
                                 new KeyValuePair<string, string>("user_id", userID),
                                  new KeyValuePair<string, string>("emotion_title",  title )
                            });



                    using (var response = await client.PostAsync(url, content))
                    {
                        using (var responseContent = response.Content)
                        {


                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static async Task<bool> AddEvent(EventDetails eventDetails)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return false;
            }

            try
            {
                string result = String.Empty;
                var client = new HttpClient();
                client.Timeout = new TimeSpan(0, 15, 0);
                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

                var url = "api.php?action=eventinsert";

                MultipartFormDataContent content = new MultipartFormDataContent();

                for (int index = 0; index < App.MediaArray.Count; index++)
                {
                    int imgIndex = index + 1;
                    MediaItem media = App.MediaArray[index];
                    content.Add(new StringContent(media.MediaString, Encoding.UTF8), "event_media" + imgIndex.ToString());
                    content.Add(new StringContent(App.ExtentionArray[index], Encoding.UTF8), "file_type" + imgIndex.ToString());
                }
                content.Add(new StringContent(App.MediaArray.Count.ToString(), Encoding.UTF8), "media_count");
                // content.Add(new StringContent(eventDetails.emotion_value, Encoding.UTF8), "emotion_value");

                for (int index = 0; index < App.ContactsArray.Count; index++)
                {
                    int contactsindex = index + 1;
                    content.Add(new StringContent(App.ContactsArray[index], Encoding.UTF8), "contact_name" + contactsindex.ToString());
                }
                content.Add(new StringContent(App.ContactsArray.Count.ToString(), Encoding.UTF8), "contact_count");

                if (eventDetails.event_title != null)
                    content.Add(new StringContent(eventDetails.event_title, Encoding.UTF8), "event_title");
                if (eventDetails.user_id != null)
                    content.Add(new StringContent(eventDetails.user_id, Encoding.UTF8), "user_id");
                if (eventDetails.event_details != null)
                    content.Add(new StringContent(eventDetails.event_details, Encoding.UTF8), "event_details");
                if (!string.IsNullOrEmpty(eventDetails.location_latitude))
                    content.Add(new StringContent(eventDetails.location_latitude, Encoding.UTF8), "location_latitude");
                if (!string.IsNullOrEmpty(eventDetails.location_longitude))
                    content.Add(new StringContent(eventDetails.location_longitude, Encoding.UTF8), "location_longitude");
                if (!string.IsNullOrEmpty(eventDetails.location_address))
                    content.Add(new StringContent(eventDetails.location_address, Encoding.UTF8), "location_address");
                if (!string.IsNullOrEmpty(eventDetails.event_id))
                    content.Add(new StringContent(eventDetails.event_id, Encoding.UTF8), "event_id");


                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var eventsJson = response.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<ResultJSon>(eventsJson);
                    if (rootobject.code == "200")
                    {
                        client.Dispose();
                        App.ExtentionArray.Clear();
                        App.MediaArray.Clear();
                        return true;
                    }

                }
                else
                {
                    client.Dispose();
                    return false;
                }

                client.Dispose();
                return false;
            }
            catch (Exception ex)
            {
                var test = ex.Message;
                return false;
            }
        }

        public static async Task<bool> AddGoal(GoalDetails goalDetails)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return false;
            }

            try
            {
                string result = String.Empty;
                var client = new HttpClient();
                client.Timeout = new TimeSpan(0, 15, 0);
                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

                var url = "api.php?action=goalinsert";

                MultipartFormDataContent content = new MultipartFormDataContent();
                if (App.MediaArray != null && App.MediaArray.Count > 0)
                {
                    for (int index = 0; index < App.MediaArray.Count; index++)
                    {
                        int imgIndex = index + 1;
                        MediaItem media = App.MediaArray[index];
                        content.Add(new StringContent(media.MediaString, Encoding.UTF8), "goal_media" + imgIndex.ToString());
                        content.Add(new StringContent(App.ExtentionArray[index], Encoding.UTF8), "file_type" + imgIndex.ToString());
                    }
                    content.Add(new StringContent(App.MediaArray.Count.ToString(), Encoding.UTF8), "media_count");
                }

                if (App.ContactsArray != null && App.ContactsArray.Count > 0)
                {
                    for (int index = 0; index < App.ContactsArray.Count; index++)
                    {
                        int contactsindex = index + 1;
                        content.Add(new StringContent(App.ContactsArray[index], Encoding.UTF8), "contact_name" + contactsindex.ToString());
                    }
                    content.Add(new StringContent(App.ContactsArray.Count.ToString(), Encoding.UTF8), "contact_count");
                }
                if (!string.IsNullOrEmpty(goalDetails.start_date))
                {
                    content.Add(new StringContent(goalDetails.start_date, Encoding.UTF8), "start_date");
                    content.Add(new StringContent(goalDetails.end_date, Encoding.UTF8), "end_date");
                }
                
                if (!string.IsNullOrEmpty(goalDetails.location_latitude))
                    content.Add(new StringContent(goalDetails.location_latitude, Encoding.UTF8), "location_latitude");
                if (!string.IsNullOrEmpty(goalDetails.location_longitude))
                    content.Add(new StringContent(goalDetails.location_longitude, Encoding.UTF8), "location_longitude");
                if (!string.IsNullOrEmpty(goalDetails.location_address))
                    content.Add(new StringContent(goalDetails.location_address, Encoding.UTF8), "location_address");

                if (goalDetails.goal_title != null && goalDetails.goal_details != null && goalDetails.user_id != null)
                {
                    content.Add(new StringContent(goalDetails.goal_title, Encoding.UTF8), "goal_title");
                    content.Add(new StringContent(goalDetails.goal_details, Encoding.UTF8), "goal_details");
                    content.Add(new StringContent(goalDetails.user_id, Encoding.UTF8), "user_id");
                }

                if (!string.IsNullOrEmpty(goalDetails.goal_id))
                {
                    content.Add(new StringContent(goalDetails.goal_id, Encoding.UTF8), "goal_id");
                }

                content.Add(new StringContent("1", Encoding.UTF8), "actionstatus_id");//  to be confirmed - status id of new goal // for testing only // test
                content.Add(new StringContent("1", Encoding.UTF8), "category_id"); // category_id = 1 for testing only // test

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var eventsJson = response.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<ResultJSon>(eventsJson);
                    if (rootobject.code == "200")
                    {
                        client.Dispose();
                        App.ExtentionArray.Clear();
                        App.MediaArray.Clear();

                        return true;
                    }
                }

                client.Dispose();
                return false;
            }
            catch (Exception ex)
            {
                var test = ex.Message;
                return false;
            }
        }

        public static async Task<bool> AddAction(ActionModel actionDetails)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return false;
            }

            try
            {
                string result = String.Empty;
                var client = new HttpClient();
                client.Timeout = new TimeSpan(0, 15, 0);
                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

                var url = "api.php?action=actioninsert";

                MultipartFormDataContent content = new MultipartFormDataContent();
                if (App.MediaArray != null && App.MediaArray.Count > 0)
                {
                    for (int index = 0; index < App.MediaArray.Count; index++)
                    {
                        int imgIndex = index + 1;
                        MediaItem media = App.MediaArray[index];
                        content.Add(new StringContent(media.MediaString, Encoding.UTF8), "action_media" + imgIndex.ToString());
                        content.Add(new StringContent(App.ExtentionArray[index], Encoding.UTF8), "file_type" + imgIndex.ToString());
                    }
                    content.Add(new StringContent(App.MediaArray.Count.ToString(), Encoding.UTF8), "media_count");
                }

                if (App.ContactsArray != null && App.ContactsArray.Count > 0)
                {
                    for (int index = 0; index < App.ContactsArray.Count; index++)
                    {
                        int contactsindex = index + 1;
                        content.Add(new StringContent(App.ContactsArray[index], Encoding.UTF8), "contact_name" + contactsindex.ToString());
                    }
                    content.Add(new StringContent(App.ContactsArray.Count.ToString(), Encoding.UTF8), "contact_count");
                }

                if (!string.IsNullOrEmpty(actionDetails.start_date))
                    content.Add(new StringContent(actionDetails.start_date, Encoding.UTF8), "action_startdate");
                if (!string.IsNullOrEmpty(actionDetails.end_date))
                    content.Add(new StringContent(actionDetails.end_date, Encoding.UTF8), "action_enddate");
                if (!string.IsNullOrEmpty(actionDetails.start_time))
                    content.Add(new StringContent(actionDetails.start_time, Encoding.UTF8), "action_starttime");
                if (!string.IsNullOrEmpty(actionDetails.end_time))
                    content.Add(new StringContent(actionDetails.end_time, Encoding.UTF8), "action_endtime");

                if (!string.IsNullOrEmpty(actionDetails.location_latitude))
                    content.Add(new StringContent(actionDetails.location_latitude, Encoding.UTF8), "location_latitude");
                if (!string.IsNullOrEmpty(actionDetails.location_longitude))
                    content.Add(new StringContent(actionDetails.location_longitude, Encoding.UTF8), "location_longitude");
                if (!string.IsNullOrEmpty(actionDetails.location_address))
                    content.Add(new StringContent(actionDetails.location_address, Encoding.UTF8), "location_address");

                if (!string.IsNullOrEmpty(actionDetails.action_repeat))
                    content.Add(new StringContent(actionDetails.action_repeat, Encoding.UTF8), "action_repeat");
                if (!string.IsNullOrEmpty(actionDetails.action_alert))
                    content.Add(new StringContent(actionDetails.action_alert, Encoding.UTF8), "action_alert");

                if (actionDetails.action_title != null && actionDetails.action_details != null && actionDetails.user_id != null)
                {
                    content.Add(new StringContent(actionDetails.action_title, Encoding.UTF8), "action_title");
                    content.Add(new StringContent(actionDetails.action_details, Encoding.UTF8), "action_details");
                    content.Add(new StringContent(actionDetails.user_id, Encoding.UTF8), "user_id");
                }

                if (!string.IsNullOrEmpty(actionDetails.action_id))
                {
                    content.Add(new StringContent(actionDetails.action_id, Encoding.UTF8), "goalaction_id");
                }
                content.Add(new StringContent("1", Encoding.UTF8), "actionstatus_id");//  to be confirmed - status id of new goal // for testing only // test
                content.Add(new StringContent("1", Encoding.UTF8), "category_id"); // category_id = 1 for testing only // test

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var eventsJson = response.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<ResultJSon>(eventsJson);
                    if (rootobject.code == "200")
                    {
                        client.Dispose();
                        App.ExtentionArray.Clear();
                        App.MediaArray.Clear();
                        return true;
                    }
                }
                else
                {
                    client.Dispose();
                    return false;
                }

                client.Dispose();
                return false;
            }
            catch (Exception ex)
            {
                var test = ex.Message;
                return false;
            }
        }

        public static async Task<bool> SaveEmotionAndEvent(string emotionID, string eventID, string userID)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return false;
            }

            try
            {
                string url = "http://purposecodes.com/pc/api.php?action=saveuseremotion";
                string result = String.Empty;

                using (var client = new HttpClient())
                {
                    var content = new FormUrlEncodedContent(new[]
                           {
                                 new KeyValuePair<string, string>("emotion_id", emotionID),
                                 new KeyValuePair<string, string>("user_id", userID),
                                  new KeyValuePair<string, string>("event_id",  eventID )
                            });

                    HttpResponseMessage response = await client.PostAsync(url, content);
                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        var eventsJson = response.Content.ReadAsStringAsync().Result;
                        var rootobject = JsonConvert.DeserializeObject<SaveEmotionsResult>(eventsJson);
                        if (rootobject != null)
                        {
                            if (rootobject.resultarray != null)
                            {
                                App.newEmotionId = rootobject.resultarray;
                            }
                            if (rootobject.code == "200")
                            {
                                client.Dispose();
                                return true;
                            }
                        }
                    }
                    else
                    {
                        client.Dispose();
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static async Task<bool> SaveGoalsAndActions(string supportValue, string relatedEmotionId,string goalId, List<CustomListViewItem> actions)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return false;
            }

            try
            {
                User user = new User { UserId = 2 }; // for testing only // test remove after testing

                if (App.Settings.GetUser() != null)
                {
                    user = App.Settings.GetUser();
                }

                if (user == null)
                {
                    return false;
                }
                var client = new HttpClient();
                client.Timeout = new TimeSpan(0, 15, 0);
                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

                var url = "api.php?action=saveusergoal";

                MultipartFormDataContent content = new MultipartFormDataContent();


                if (actions != null && actions.Count > 0)
                {
                    for (int index = 0; index < actions.Count; index++)
                    {
                        int contactsindex = index + 1;
                        content.Add(new StringContent(actions[index].EventID, Encoding.UTF8), "goalaction_id" + contactsindex.ToString());
                    }
                    content.Add(new StringContent(actions.Count.ToString(), Encoding.UTF8), "action_count");
                }

                if (!string.IsNullOrEmpty(relatedEmotionId))
                {
                    content.Add(new StringContent(relatedEmotionId, Encoding.UTF8), "useremotion_id");
                }
                content.Add(new StringContent(supportValue.ToString(), Encoding.UTF8), "emotion_value");
                content.Add(new StringContent(goalId.ToString(), Encoding.UTF8), "goal_id");
                content.Add(new StringContent(user.UserId.ToString(), Encoding.UTF8), "user_id");
                

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var eventsJson = response.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<ResultJSon>(eventsJson);
                    if (rootobject.code == "200")
                    {
                        client.Dispose();
                        
                        return true;
                    }
                }
                

                client.Dispose();
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<GemsEmotionsObject> GetAllSupportingEmotions()
        {
            try
            {
                User user = new User { UserId = 2, UserName = "sam" }; // for testing only // test

                if (user == null)
                {
                    return null;
                }

                if (!CrossConnectivity.Current.IsConnected)
                {
                    return null;
                }

                var client = new System.Net.Http.HttpClient();

                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);

                string uriString = "api.php?action=getallsupportingemotion&user_id=" + user.UserId.ToString();

                var response = await client.GetAsync(uriString);

                if( response != null && response.Content != null )
                {
                    var actionsJson = response.Content.ReadAsStringAsync().Result;


                    var rootobject = JsonConvert.DeserializeObject<GemsEmotionsObject>(actionsJson);
                    if (rootobject != null && rootobject.resultarray != null)
                    {
                        client.Dispose();
                        return rootobject; 
                    }
                    client.Dispose();
                    return null;

                }
                else
                {
                    client.Dispose();
                    return null;
                }

             
            }
            catch (Exception)
            {
				return null;
            }
        }

        public static async Task<GemsGoalsObject> GetAllSupportingGoals()
        {
            try
            {
                User user = new User { UserId = 2, UserName = "sam" };// for testing only // testing

                if (user == null)
                {
                    return null;
                }

                if (!CrossConnectivity.Current.IsConnected)
                {
                    return null;
                }

                var client = new System.Net.Http.HttpClient();

                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);

                string uriString = "api.php?action=getallsupportinggoal&user_id=" + user.UserId.ToString();

                var response = await client.GetAsync(uriString);

                if (response != null && response.Content != null)
                {
                    var actionsJson = response.Content.ReadAsStringAsync().Result;


                    var rootobject = JsonConvert.DeserializeObject<GemsGoalsObject>(actionsJson);
                    if (rootobject != null && rootobject.resultarray != null)
                    {
                        client.Dispose();
                        return rootobject;
                    }
                    client.Dispose();
                    return null;

                }
                else
                {
                    client.Dispose();
                    return null;
                }
            }
            catch (Exception ex)
            {
				return null;
            }
        }

        public static async Task<string> RegisterUser(string name, string email, string password)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return "500";
            }

            try
            {
                string result = String.Empty;
                var client = new HttpClient();
                client.Timeout = new TimeSpan(0, 15, 0);
                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

                var url = "api.php?action=register";

                MultipartFormDataContent content = new MultipartFormDataContent();


                if (!string.IsNullOrEmpty(name))
                {
                    content.Add(new StringContent(name, Encoding.UTF8), "firstname");
                }

                if (!string.IsNullOrEmpty(email))
                {
                    content.Add(new StringContent(email, Encoding.UTF8), "email");
                }

                if (!string.IsNullOrEmpty(password))
                {
                    content.Add(new StringContent(password, Encoding.UTF8), "password");
                }

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response != null && response.Content != null)
                {
                    var eventsJson = response.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<ResultJSon>(eventsJson);

                    if (rootobject.code != null)
                    {
                        client.Dispose();
                        return rootobject.code.ToString();
                    }
                }
                else
                {
                    client.Dispose();
                    return "500";
                }

                client.Dispose();
                return "500";
            }
            catch (Exception ex)
            {
                var test = ex.Message;
                return "500";
            }
        }

        public static async Task<UserDetailsOnLogin> Login(string email, string password)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return null;
            }

            try
            {
                string result = String.Empty;
                var client = new HttpClient();
                client.Timeout = new TimeSpan(0, 15, 0);
                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

                var url = "api.php?action=login";

                MultipartFormDataContent content = new MultipartFormDataContent();
                
                if (!string.IsNullOrEmpty(email))
                {
                    content.Add(new StringContent(email, Encoding.UTF8), "email");
                }

                if (!string.IsNullOrEmpty(password))
                {
                    content.Add(new StringContent(password, Encoding.UTF8), "password");
                }

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response != null && response.Content != null)
                {

                    var eventsJson = response.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<UserDetailsOnLogin>(eventsJson);
                    if (rootobject != null)
                    {
                        return rootobject;
                    }
                }
                client.Dispose();
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }

            return null;
        }

        public static async Task<string> LogOut(string userId)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return "404";
            }

            try
            {
                string result = String.Empty;
                var client = new HttpClient();
                client.Timeout = new TimeSpan(0, 15, 0);
                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

                var url = "api.php?action=logout";

                MultipartFormDataContent content = new MultipartFormDataContent();

                if (!string.IsNullOrEmpty(userId))
                {
                    content.Add(new StringContent(userId, Encoding.UTF8), "user_id");
                }

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response != null && response.Content != null)
                {

                    var eventsJson = response.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<UserDetailsOnLogin>(eventsJson);
                    if (rootobject != null && rootobject.code != null)
                    {
                        client.Dispose();
                        return rootobject.code;
                    }
                }
                client.Dispose();
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }

            return "404";
        }

        public static async Task<string> ResetPassword(string email)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return "404";
            }

            try
            {
                string result = String.Empty;
                var client = new HttpClient();
                client.Timeout = new TimeSpan(0, 15, 0);
                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

                var url = "api.php?action=forgotpassword";

                MultipartFormDataContent content = new MultipartFormDataContent();

                if (!string.IsNullOrEmpty(email))
                {
                    content.Add(new StringContent(email, Encoding.UTF8), "email");
                }

                HttpResponseMessage response = await client.PostAsync(url, content);


                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var eventsJson = response.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<ResultJSon>(eventsJson);
                    if (rootobject.code != null)
                    {
                        client.Dispose();
                        return rootobject.code;
                    }
                }
                client.Dispose();
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }

            return "404";
        }

        public static async Task<string> UpdatePassword(string userId, string oldPassword, string newPassword, string confirmPassword)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return "400";
            }

            try
            {
                string result = String.Empty;
                var client = new HttpClient();
                client.Timeout = new TimeSpan(0, 15, 0);
                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

                var url = "api.php?action=changepassword";

                MultipartFormDataContent content = new MultipartFormDataContent();

                if (!string.IsNullOrEmpty(oldPassword))
                {
                    content.Add(new StringContent(oldPassword, Encoding.UTF8), "oldpass");
                }

                if (!string.IsNullOrEmpty(newPassword))
                {
                    content.Add(new StringContent(newPassword, Encoding.UTF8), "newpass");
                }

                if (!string.IsNullOrEmpty(confirmPassword))
                {
                    content.Add(new StringContent(confirmPassword, Encoding.UTF8), "cpass");
                }

                if (!string.IsNullOrEmpty(userId))
                {
                    content.Add(new StringContent(userId, Encoding.UTF8), "user_id");
                }

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var eventsJson = response.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<ResultJSon>(eventsJson);
                    if (rootobject.code != null)
                    {
                        client.Dispose();
                        return rootobject.code;
                    }
                }
                client.Dispose();
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }

            return "400";
        }

        public static async Task<AddCommentsResponse> AddComment(string userId, string commentTxt, string shareComment, string goalId, string eventId, string actionId)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return null;
            }

            try
            {
                string result = String.Empty;
                var client = new HttpClient();
                client.Timeout = new TimeSpan(0, 15, 0);
                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

                var url = "api.php?action=addcomments";

                MultipartFormDataContent content = new MultipartFormDataContent();

                if (!string.IsNullOrEmpty(goalId))
                {
                    content.Add(new StringContent(goalId, Encoding.UTF8), "goal_id");
                }
                if (!string.IsNullOrEmpty(eventId))
                {
                    content.Add(new StringContent(eventId, Encoding.UTF8), "event_id");
                }
                if (!string.IsNullOrEmpty(actionId))
                {
                    content.Add(new StringContent(actionId, Encoding.UTF8), "goalaction_id");
                }

                if (!string.IsNullOrEmpty(commentTxt))
                {
                    content.Add(new StringContent(commentTxt, Encoding.UTF8), "comment_txt");
                }

                if (!string.IsNullOrEmpty(shareComment))
                {
                    content.Add(new StringContent(shareComment, Encoding.UTF8), "share_comment");
                }

                if (!string.IsNullOrEmpty(userId))
                {
                    content.Add(new StringContent(userId, Encoding.UTF8), "user_id");
                }

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var eventsJson = response.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<AddCommentsResponse>(eventsJson);
                    if (rootobject != null)
                    {
                        client.Dispose();
                        return rootobject;
                    }
                }
                client.Dispose();
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }

            return null;
        }

        public static async Task<string> ShareToCommunity(string goalId, string eventId, string actionId)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return "404";
            }

            try
            {
                string result = String.Empty;
                var client = new HttpClient();
                client.Timeout = new TimeSpan(0, 15, 0);
                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

                var url = "api.php?action=sharetocommunity";

                MultipartFormDataContent content = new MultipartFormDataContent();

                if (!string.IsNullOrEmpty(goalId))
                {
                    content.Add(new StringContent(goalId, Encoding.UTF8), "goal_id");
                }

                if (!string.IsNullOrEmpty(actionId))
                {
                    content.Add(new StringContent(actionId, Encoding.UTF8), "goalaction_id");
                }

                if (!string.IsNullOrEmpty(eventId))
                {
                    content.Add(new StringContent(eventId, Encoding.UTF8), "event_id");
                }

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var eventsJson = response.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<ResultJSon>(eventsJson);
                    if (rootobject.code != null)
                    {
                        client.Dispose();
                        return rootobject.code;
                    }
                }
                client.Dispose();
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }

            return "400";
        }

        public static async Task<List<Comment>> GetComments(string gemId, GemType gemType, bool isCommunityGem = false)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return null;
            }

            try
            {
                string result = String.Empty;
                var client = new HttpClient();
                client.Timeout = new TimeSpan(0, 15, 0);
                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

                var url = "api.php?action=getcomments";

                MultipartFormDataContent content = new MultipartFormDataContent();


                switch (gemType)
                {
                    case GemType.Goal:
                        content.Add(new StringContent(gemId, Encoding.UTF8), "goal_id");
                        break;
                    case GemType.Event:
                        content.Add(new StringContent(gemId, Encoding.UTF8), "event_id");
                        break;
                    case GemType.Action:
                        content.Add(new StringContent(gemId, Encoding.UTF8), "goalaction_id");
                        break;
                    case GemType.Emotion:
                        break;
                    default:
                        break;
                }

                if (isCommunityGem)
                {
                    content.Add(new StringContent("1", Encoding.UTF8), "share_comment");
                }
                else
                {
                    content.Add(new StringContent("0", Encoding.UTF8), "share_comment");
                }

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var eventsJson = response.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<GetCommentsResult>(eventsJson);
                    if (rootobject != null && rootobject.resultarray != null)
                    {
                        client.Dispose();
                        return rootobject.resultarray;
                    }
                }
                client.Dispose();
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }

            return null;
        }

		public static async Task<string> RemoveComment(string commentId)
		{
			if (!CrossConnectivity.Current.IsConnected)
			{
				return "404";
			}

            try
			{
                var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Add("Post", "application/json");
                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);

                string uriString = "api.php?action=removecomment&commentId=" + commentId;
                var response = await client.GetAsync(uriString);
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var responseJson = response.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<ReoveCommentResponse>(responseJson);
                    if (rootobject != null && rootobject.code != null)
                    {
                        return rootobject.code;
                    }
                }
			}
			catch (Exception ex)
			{
				var test = ex.Message;
			}

			return "404";
		}

        public static async Task<string> AddToFavorite(string userId, string gemId, GemType gemType)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return "404";
            }

            try
            {
                string result = String.Empty;
                var client = new HttpClient();
                client.Timeout = new TimeSpan(0, 15, 0);
                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

                var url = "api.php?action=addtofavorite";

                MultipartFormDataContent content = new MultipartFormDataContent();

                content.Add(new StringContent(userId, Encoding.UTF8), "user_id");

                switch (gemType)
                {
                    case GemType.Goal:
                        content.Add(new StringContent(gemId, Encoding.UTF8), "goal_id");
                        break;
                    case GemType.Event:
                        content.Add(new StringContent(gemId, Encoding.UTF8), "event_id");
                        break;
                    case GemType.Action:
                        content.Add(new StringContent(gemId, Encoding.UTF8), "goalaction_id");
                        break;
                    case GemType.Emotion:
                        break;
                    default:
                        break;
                }

                HttpResponseMessage response = null;
                try
                {
                    response = await client.PostAsync(url, content);
                }
                catch (Exception ex)
                {
                    var test = ex.Message;
                }

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var eventsJson = response.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<ResultJSon>(eventsJson);
                    if (rootobject.code != null)
                    {
                        client.Dispose();
                        return rootobject.code;
                    }
                }

                client.Dispose();
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }

            return "404";
        }

		public static async Task<PendingGoalsObject> GetAllPendingGoalsAndActions()
		{
			try
			{
				User user = new User { UserId = 2, UserName = "sam" };

				if (user == null)
				{
					return null;
				}

				if (!CrossConnectivity.Current.IsConnected)
				{
					return null;
				}

				var client = new System.Net.Http.HttpClient();

				client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);

				string uriString = "api.php?action=getallmypendinggoal&user_id=" + user.UserId.ToString();

				var response = await client.GetAsync(uriString);

				if( response != null && response.Content != null )
				{
					var actionsJson = response.Content.ReadAsStringAsync().Result;


					var rootobject = JsonConvert.DeserializeObject<PendingGoalsObject>(actionsJson);
					if (rootobject != null && rootobject.resultarray != null)
					{
						client.Dispose();
						return rootobject; 
					}
					client.Dispose();
					return null;

				}
				else
				{
					client.Dispose();
					return null;
				}


			}
			catch (Exception)
			{

				throw;
			}
		}

		public static async Task<GemsGoalsObject> GetAllMyGoals()
		{
			try
			{
				User user = new User { UserId = 2, UserName = "sam" };

				if (user == null)
				{
					return null;
				}

				if (!CrossConnectivity.Current.IsConnected)
				{
					return null;
				}

				var client = new System.Net.Http.HttpClient();

				client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);

				string uriString = "api.php?action=getallmygoals&user_id=" + user.UserId.ToString();

				var response = await client.GetAsync(uriString);

				if( response != null && response.Content != null )
				{
					var actionsJson = response.Content.ReadAsStringAsync().Result;


					var rootobject = JsonConvert.DeserializeObject<GemsGoalsObject>(actionsJson);
					if (rootobject != null && rootobject.resultarray != null)
					{
						client.Dispose();
						return rootobject; 
					}
					client.Dispose();
					return null;

				}
				else
				{
					client.Dispose();
					return null;
				}


			}
			catch (Exception)
			{

				throw;
			}
		}

		public static async Task<bool> ChangePendingActionStatus(string savedGoalID )
		{
			if (!CrossConnectivity.Current.IsConnected)
			{
				return false;
			}

			try
			{
				string url = "http://purposecodes.com/pc/api.php?action=changeactionstatus";
				string result = String.Empty;

				using (var client = new HttpClient())
				{
					var content = new FormUrlEncodedContent(new[]
						{
							new KeyValuePair<string, string>("savedgoal_id", savedGoalID)
						});

					HttpResponseMessage response = await client.PostAsync(url, content);
					if (response != null && response.StatusCode == HttpStatusCode.OK)
					{
						var eventsJson = response.Content.ReadAsStringAsync().Result;
						var rootobject = JsonConvert.DeserializeObject<ChangePendingGoalReturn>(eventsJson);
						if (rootobject != null)
						{

							if (rootobject.code == "200")
							{
								client.Dispose();
								return true;
							}
						}
					}
					else
					{
						client.Dispose();
						return false;
					}
				}
				return true;
			}
			catch (Exception ex)
			{

				return false;
			}
		}


        public static async Task<SelectedGoal> GetSelectedGoalDetails(  string selectedGoalID )
        {
            try
            {
                User user = new User { UserId = 2, UserName = "sam" };

                if (user == null)
                {
                    return null;
                }

                if (!CrossConnectivity.Current.IsConnected)
                {
                    return null;
                }

                var client = new System.Net.Http.HttpClient();

                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);

                string uriString = "api.php?action=getgoal&goal_id=" + selectedGoalID;

                var response = await client.GetAsync(uriString);

                if (response != null && response.Content != null)
                {
                    var actionsJson = response.Content.ReadAsStringAsync().Result;


                    var rootobject = JsonConvert.DeserializeObject<SelectedGoal>(actionsJson);
                    if (rootobject != null && rootobject.resultarray != null)
                    {
                        client.Dispose();
                        return rootobject;
                    }
                    client.Dispose();
                    return null;

                }
                else
                {
                    client.Dispose();
                    return null;
                }


            }
            catch (Exception)
            {

                throw;
            }
        }


        public static async Task<CommunityGemsObject>GetCommunityGemsDetails()
        {
            try
            {
                User user = new User { UserId = 2, UserName = "sam" };

                if (user == null)
                {
                    return null;
                }

                if (!CrossConnectivity.Current.IsConnected)
                {
                    return null;
                }

                var client = new System.Net.Http.HttpClient();

                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);

                string uriString = "api.php?action=getallcommunitygems";

                var response = await client.GetAsync(uriString);

                if (response != null && response.Content != null)
                {
                    var actionsJson = response.Content.ReadAsStringAsync().Result;


                    var rootobject = JsonConvert.DeserializeObject<CommunityGemsObject>(actionsJson);
                    if (rootobject != null && rootobject.resultarray != null)
                    {
                        client.Dispose();
                        return rootobject;
                    }
                    client.Dispose();
                    return null;

                }
                else
                {
                    client.Dispose();
                    return null;
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        public static async Task<string> DeleteGem(string gemId, GemType gemtype)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    return "404";
                }

                var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Add("Post", "application/json");
                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);
                string gemIdString = string.Empty;

                switch (gemtype)
                {
                    case GemType.Goal:
                        gemIdString = "&goal_id=";
                        break;
                    case GemType.Event:
                        gemIdString = "&event_id=";
                        break;
                    case GemType.Action:
                        gemIdString = "&goalaction_id=";
                        break;
                    case GemType.Emotion:
                        gemIdString = "&emotion_id=";
                        break;
                    default:
                        break;
                }

                string uriString = "api.php?action=deletegem&" + gemIdString + gemId;

                var response = await client.GetAsync(uriString);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var responseJson = response.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<ReoveCommentResponse>(responseJson);
                    if (rootobject != null && rootobject.code != null)
                    {
                        return rootobject.code;
                    }
                }

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }

            return "404";
        }

        public static async Task<string> DeleteMediaFromGem(string gemId, GemType gemtype, string mediaName)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    return "404";
                }

                var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Add("Post", "application/json");
                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);
                string gemIdString = string.Empty;

                switch (gemtype)
                {
                    case GemType.Goal:
                        gemIdString = "&goal_id=";
                        break;
                    case GemType.Event:
                        gemIdString = "&event_id=";
                        break;
                    case GemType.Action:
                        gemIdString = "&goalaction_id=";
                        break;
                    case GemType.Emotion:
                        gemIdString = "&emotion_id=";
                        break;
                    default:
                        break;
                }

                string uriString = "api.php?action=deletemedia&"+ gemIdString + gemId+"&media_file=" + mediaName ;

                var response = await client.GetAsync(uriString);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var responseJson = response.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<ReoveCommentResponse>(responseJson);
                    if (rootobject != null && rootobject.code != null)
                    {
                        return rootobject.code;
                    }
                }

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }

            return "404";
        }


		public static async Task<string> LikeGem(string gemId )
		{
			try
			{

				User user = App.Settings.GetUser();

				if( user == null )
					user = new User(){ UserId = 2 };

				if (!CrossConnectivity.Current.IsConnected)
				{
					return "404";
				}

				string result = String.Empty;
				var client = new HttpClient();
				client.Timeout = new TimeSpan(0, 15, 0);
				client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);
				client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

				var url = "api.php?action=like";//&goal_id=" + gemId + "&user_id=" + user.UserId;

				MultipartFormDataContent content = new MultipartFormDataContent();

				if (!string.IsNullOrEmpty(gemId))
				{
					content.Add(new StringContent(gemId, Encoding.UTF8), "goal_id");
				}

				content.Add(new StringContent(user.UserId.ToString(), Encoding.UTF8), "user_id");
				HttpResponseMessage response = await client.PostAsync(url, content);


				/* var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Add("Post", "application/json");
                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);

                string uriString = "api.php?action=like&goal_id=" + gemId + "&user_id=" + user.UserId;

				var response = await client.PostAsync(uriString);*/

				if (response != null && response.StatusCode == HttpStatusCode.OK)
				{
					var responseJson = response.Content.ReadAsStringAsync().Result;
					var rootobject = JsonConvert.DeserializeObject<ReoveCommentResponse>(responseJson);
					if (rootobject != null && rootobject.code != null)
					{
						return rootobject.code;
					}
				}

			}
			catch (Exception ex)
			{
				var test = ex.Message;
			}

			return "404";
		}


        public static async Task<ShareStatusAndCommentsCount> GetShareStatusAndCommentsCount(string gemId, GemType gemtype, string userId)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    return null;
                }

                var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Add("Post", "application/json");
                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);
                string gemIdString = string.Empty;

                switch (gemtype)
                {
                    case GemType.Goal:
                        gemIdString = "&goal_id=";
                        break;
                    case GemType.Event:
                        gemIdString = "&event_id=";
                        break;
                    case GemType.Action:
                        gemIdString = "&goalaction_id=";
                        break;
                    case GemType.Emotion:
                        gemIdString = "&emotion_id=";
                        break;
                    default:
                        break;
                }

                string uriString = "api.php?action=sharestatus&user_id=" +userId+ gemIdString + gemId;

                var response = await client.GetAsync(uriString);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var responseJson = response.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<ShareStatusResult>(responseJson);
                    if (rootobject != null && rootobject.resultarray != null)
                    {
                        return rootobject.resultarray;
                    }
                }

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }

            return null;
        }
    }
}

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

                // date for testing only // test
                content.Add(new StringContent(DateTime.Now.ToString("yyyy/MM/dd"), Encoding.UTF8), "start_date");
                content.Add(new StringContent(DateTime.Now.ToString("yyyy/MM/dd"), Encoding.UTF8), "end_date");

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
            User user = App.Settings.GetUser();
            user = new User { UserId = 2 }; // for testing only // test
            if (user == null)
            {
                return false;
            }

            try
            {
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

                string uriString = "api.php?action=getallsupportingemotion&user_id=" + user.UserId.ToString();

                var response = await client.GetAsync(uriString);

                if( response != null && response.Content != null )
                {
                    var actionsJson = response.Content.ReadAsStringAsync().Result;


                    var rootobject = JsonConvert.DeserializeObject<GemsEmotionsObject>(actionsJson);
                    if (rootobject != null && rootobject.resultarray != null)
                    {
                        foreach (var item in rootobject.resultarray)
                        {
                            CustomListViewItem listItem = new CustomListViewItem();
                        }
                    }
                    return rootobject; ;
                }
                else
                {
                    return null;
                }

             
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}

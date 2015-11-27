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
using Connectivity.Plugin;

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

            client.BaseAddress = new Uri( Constants.SERVICE_BASE_URL );

            string uriString = "api.php?action=emotions&user_id=2&emotion_value=" + sliderValue.ToString();

            var response = await client.GetAsync(uriString);

            if( response != null && response.StatusCode == HttpStatusCode.OK )
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

            List<CustomListViewItem>  noEmotionsList = new List<CustomListViewItem>();
            noEmotionsList.Add(new CustomListViewItem { Name = "No emotions Found" });
            return null;

        }




        public static async Task<List<CustomListViewItem>> GetAllEmotions(int userID )
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
                throw;
            }

        }



        public static async Task<List<CustomListViewItem>> GetAlGoals(int userID)
        {
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

                var rootobject = JsonConvert.DeserializeObject<Goals>(allGoalsJson);

                if( rootobject != null && rootobject.goal_title != null )
                {
                    List<CustomListViewItem> goalsList = new List<CustomListViewItem>();
                    foreach (var item in rootobject.goal_title)
                    {
                        CustomListViewItem listItem = new CustomListViewItem();
                        listItem.Name = item;
                        goalsList.Add(listItem);
                    }
                    return goalsList;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }


        }


        public static async Task<List<CustomListViewItem>> GetAllEvents()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return null;
            }

            var client = new System.Net.Http.HttpClient();
            User user = App.Settings.GetUser();

            ///////// for testing

            user = new User { UserId = 2, UserName="sam" };

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

            return null;
        }




        public static async Task<bool> PostMedia(MediaPost mediaFile)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return false;
            }

            string url = "http://purposecodes.com/pc/api.php?action=eventinsert";// Constants.SERVICE_BASE_URL;
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



        public static async Task<bool> AddEmotion( string emotionID, string title, string userID)
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
                
                throw;
            }
        }


        public static async Task<bool> AddEvent(  EventDetails eventDetails )
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

                var url = "api.php?action=eventinsertarray";

                MultipartFormDataContent content = new MultipartFormDataContent();

                for (int index = 0; index < App.MediaArray.Count; index++)
                {
                    int imgIndex = index + 1;
                    content.Add(new StringContent(App.MediaArray[index], Encoding.UTF8), "event_media" + imgIndex.ToString());
                    content.Add(new StringContent(App.ExtentionArray[index], Encoding.UTF8), "file_type" + imgIndex.ToString());
                }


                content.Add(new StringContent(App.MediaArray.Count.ToString(), Encoding.UTF8), "event_image_count");
                content.Add(new StringContent(eventDetails.event_title, Encoding.UTF8), "event_title");
                content.Add(new StringContent(eventDetails.user_id, Encoding.UTF8), "user_id");
                content.Add(new StringContent(eventDetails.event_details, Encoding.UTF8), "event_details");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var eventsJson = response.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<TestJSon>(eventsJson);
                   if( rootobject.code == "200" )
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
                return false;
            }
        }



        public static async Task<bool> AddGoal(EventDetails eventDetails)
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

                var url = "api.php?action=eventinsertarray";

                MultipartFormDataContent content = new MultipartFormDataContent();

                for (int index = 0; index < App.MediaArray.Count; index++)
                {
                    int imgIndex = index + 1;
                    content.Add(new StringContent(App.MediaArray[index], Encoding.UTF8), "event_media" + imgIndex.ToString());
                    content.Add(new StringContent(App.ExtentionArray[index], Encoding.UTF8), "file_type" + imgIndex.ToString());
                }


                content.Add(new StringContent(App.MediaArray.Count.ToString(), Encoding.UTF8), "event_image_count");
                content.Add(new StringContent(eventDetails.event_title, Encoding.UTF8), "event_title");
                content.Add(new StringContent(eventDetails.user_id, Encoding.UTF8), "user_id");
                content.Add(new StringContent(eventDetails.event_details, Encoding.UTF8), "event_details");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var eventsJson = response.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<TestJSon>(eventsJson);
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

                throw;
            }
        }
       
       
    }
}

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

namespace PurposeColor.Service
{
    public class ServiceHelper
    {

        public static async Task<List<CustomListViewItem>> GetEmotions(int sliderValue)
        {

            var client = new System.Net.Http.HttpClient();

            client.BaseAddress = new Uri( Constants.SERVICE_BASE_URL );

            string uriString = "api.php?action=emotions&emotion_value=" + sliderValue.ToString();

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
                        listItem.ID = sliderValue;
                        emotionsList.Add(listItem);
                    }
                }
                return emotionsList;
            }

            List<CustomListViewItem>  noEmotionsList = new List<CustomListViewItem>();
            noEmotionsList.Add(new CustomListViewItem { Name = "No emotions Found" });
            return null;

        }

        public static async Task<List<CustomListViewItem>> GetAllEvents()
        {
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

                    var rootobject = JsonConvert.DeserializeObject<Events>(eventsJson);

                    List<CustomListViewItem> eventsList = new List<CustomListViewItem>();

                    if (rootobject != null && rootobject.event_title != null)
                    {
                        foreach (var item in rootobject.event_title)
                        {
                            CustomListViewItem listItem = new CustomListViewItem();
                            listItem.Name = item;
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

       
    }
}

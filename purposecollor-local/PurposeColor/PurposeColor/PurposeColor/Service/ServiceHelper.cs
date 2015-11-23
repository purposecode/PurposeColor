using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using PurposeColor.Model;
using PurposeColor.CustomControls;

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
                        emotionsList.Add(listItem);
                    }
                }
                return emotionsList;
            }

            List<CustomListViewItem>  noEmotionsList = new List<CustomListViewItem>();
            noEmotionsList.Add(new CustomListViewItem { Name = "No emotions Found" });
            return null;

        }

       
    }
}

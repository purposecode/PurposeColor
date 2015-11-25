using System;
using PurposeColor.Droid;
using Xamarin.Geolocation;
using Android.Widget;
using System.Threading;
using System.Threading.Tasks;
using Android.Locations;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidLocationImpl))]
namespace PurposeColor.Droid
{
	public class AndroidLocationImpl : ILocation
	{
		public async Task<string> GetLocation( double lat, double lon )
		{
			var geo = new Geocoder (MainActivity.GetMainActivity());
			var addresses = await geo.GetFromLocationAsync (lat, lon, 1);
			if (addresses != null) 
			{
				foreach (var item in addresses)
				{
					string address =  "@" + item.Thoroughfare + " " + item.SubLocality + "  " + item.SubAdminArea + "  " + item.AdminArea +  "  " + item.CountryName;
					return address;
				}
			}
			return null;
		}


	}
}


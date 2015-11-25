using System;
using System.Threading.Tasks;

namespace PurposeColor
{
	public interface ILocation
	{
		Task<string> GetLocation( double lat, double lon );
	}
}


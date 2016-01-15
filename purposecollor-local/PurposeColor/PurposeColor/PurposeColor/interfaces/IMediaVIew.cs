using System;
using Media.Plugin.Abstractions;
using System.Threading.Tasks;

namespace PurposeColor
{
	public interface IMediaVIew
	{
		void ShowImage (string imageURL);

		/// <summary>
		///  Does nothing
		/// </summary>
		/// <param name="file">The file image</param>
		/// <returns>True if rotation occured, else false</returns>
		Task<bool> FixOrientationAsync(MediaFile file);
	}
}


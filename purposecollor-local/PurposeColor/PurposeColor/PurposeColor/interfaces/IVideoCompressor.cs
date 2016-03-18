using System;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;

namespace PurposeColor
{
	public interface IVideoCompressor
	{
		MemoryStream CompressVideo( string sourceFilePath, string destinationFilePath, bool deleteSourceFile );
		MemoryStream CreateVideoThumbnail ( string inputVideoPath, string outputImagePath );
		Size GetCameraSize ();
	}
}


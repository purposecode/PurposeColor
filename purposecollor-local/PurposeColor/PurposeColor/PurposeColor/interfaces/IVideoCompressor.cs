using System;
using System.Threading.Tasks;
using System.IO;

namespace PurposeColor
{
	public interface IVideoCompressor
	{
		MemoryStream CompressVideo( string sourceFilePath, string destinationFilePath, bool deleteSourceFile );
		MemoryStream CreateVideoThumbnail ( string inputVideoPath, string outputImagePath );
	}
}


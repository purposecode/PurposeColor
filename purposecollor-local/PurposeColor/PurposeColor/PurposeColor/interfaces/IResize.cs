using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.interfaces
{
    public interface IResize
    {
		byte[] Resize(byte[] imageData, float width, float height, string path);

        MemoryStream CompessImage(int ratio, MemoryStream ms);
    }
}

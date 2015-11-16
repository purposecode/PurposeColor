using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.interfaces
{
    public interface IResize
    {
        void Resize(byte[] imageData, float width, float height);
    }
}

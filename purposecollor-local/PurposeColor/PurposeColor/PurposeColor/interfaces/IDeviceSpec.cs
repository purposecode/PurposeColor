using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cross
{
    public interface IDeviceSpec
    {
         double ScreenWidth { get;  }
         double ScreenHeight { get;  }
         double ScreenDensity { get;  }
         void ExitApp();
    }
}

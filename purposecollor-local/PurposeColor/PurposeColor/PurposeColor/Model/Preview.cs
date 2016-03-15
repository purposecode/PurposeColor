using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.Model
{
    public class PreviewItem
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Path { get; set; }
    }

    public class MediaItem
    {
        public string Name { get; set; }
        public string MediaString { get; set; }
		public string MediaThumbString { get; set; }
		public Constants.MediaType MediaType{ get; set; } 
    }
}

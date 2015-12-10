using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.Model
{
    public class Locationarray
    {
        public string location_name { get; set; }
        public string location_address { get; set; }
        public double location_latitude { get; set; }
        public double location_longitude { get; set; }
    }

    public class Location
    {
        public List<Locationarray> locationarray { get; set; }
    }
}

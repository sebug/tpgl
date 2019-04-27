using System;
using System.Collections.Generic;

namespace tpgl.Models
{
    public class StopsResponse
    {
        public StopsResponse()
        {
        }

        public string Timestamp { get; set; }
        public List<Stop> Stops { get; set; }
    }
}

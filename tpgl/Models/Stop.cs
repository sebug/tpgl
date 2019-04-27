using System;
using System.Collections.Generic;

namespace tpgl.Models
{
    public class Stop
    {
        public Stop()
        {
        }

        public string StopCode { get; set; }
        public string StopName { get; set; }
        public List<Connection> Connections { get; set; }
    }
}

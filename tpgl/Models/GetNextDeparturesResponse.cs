using System;
using System.Collections.Generic;

namespace tpgl.Models
{
    public class GetNextDeparturesResponse
    {
        public GetNextDeparturesResponse()
        {
        }

        public string TimeStamp { get; set; }
        public Stop Stop { get; set; }
        public List<Departure> Departures { get; set; }

    }
}

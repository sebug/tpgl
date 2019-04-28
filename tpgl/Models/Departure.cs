using System;
namespace tpgl.Models
{
    public class Departure
    {
        public Departure()
        {
        }

        public long? DepartureCode { get; set; }
        public string TimeStamp { get; set; }
        public long? WaitingTimeMillis { get; set; }
        public string WaitingTime { get; set; }
        public string Reliability { get; set; }
        public string Characteristics { get; set; }
        public string VehiculeType { get; set; }
        public long? VehiculeNo { get; set; }
        public Line Line { get; set; }
    }
}

using System;
using System.Text;

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

        public string ShortLineDisplay
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (this.Line != null)
                {
                    sb.Append(this.Line.LineCode + " - ");
                    sb.Append(this.Line.DestinationName + " ");
                }
                sb.Append(this.WaitingTime);
                return sb.ToString();
            }
        }
    }
}

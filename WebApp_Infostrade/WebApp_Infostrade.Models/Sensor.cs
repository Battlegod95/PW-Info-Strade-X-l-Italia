using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp_Infostrade.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Id_gateway { get; set; }
		public int Id_incrocio { get; set; }
		public int Crossroad_id { get; set; }
		public int Intersection { get; set; }
		public int Location { get; set; }
		public bool Healthy { get; set; }
		public TimeSpan Time { get; set; }
		public int Temperature { get; set; }
		public int Humidity { get; set; }
		public int Pressure { get; set; }
		public int Coordinate_x { get; set; }
		public int Coordinate_y { get; set; }
	}
}

using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeMarchi.InfostradexItalia.Models
{
	[Table("SensorTable")]
	public class Sensor
	{
		[Key]
		public int Id { get; set; }
		public string Description { get; set; }
		//public DateTime Date { get; set; }
		public string Id_gateway { get; set; }
		public int Id_incrocio { get; set; }
		public string Crossroad_id { get; set; }
		public string Intersection { get; set; }
		public string Location { get; set; }
		public int Healthy { get; set; }
		//public TimeSpan Time { get; set; }
		public float Temperature { get; set; }
		public float Humidity { get; set; }
		public float Pressure { get; set; }
		public float Coordinate_x { get; set; }
		public float Coordinate_y { get; set; }
	}
}

/*
 [Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](25) NOT NULL,
	[Id_gateway] [nvarchar](25) NOT NULL,
	[Id_incrocio] [int] NOT NULL,
	[Crossroad_id] [nchar](25) NOT NULL,
	[Intersection] [nchar](25) NOT NULL,
	[Location] [nchar](25) NOT NULL,
	[Healthy] [bit] NOT NULL,
	[Date] [date] NOT NULL,
	[Time] [time](4) NOT NULL,
	[Temperature] [float] NULL,
	[Humidity] [float] NULL,
	[Pressure] [float] NULL,
	[Coordinate_x] [float] NOT NULL,
	[Coordinate_y] [float] NOT NULL,
 */

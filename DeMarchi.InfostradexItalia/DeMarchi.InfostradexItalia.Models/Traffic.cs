using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeMarchi.InfostradexItalia.Models
{
	[Table("TrafficTable")]
	public class Traffic
    {
		[Key]
        public int Id { get; set; }
		public string Description { get; set; }
		public string Id_gateway { get; set; }
		public int Id_incrocio { get; set; }
		public Byte Id_semaforo { get; set; }
		public Byte Id_strada { get; set; }
		public Byte Stato_semaforo { get; set; }
		public Byte Fascia_oraria { get; set; }
		public DateTime Date { get; set; }
		public string Tipologia_veicolo { get; set; }
		public int Conteggio { get; set; }
	}
}
/*
 [Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](25) NOT NULL,
	[Id_gateway] [nvarchar](25) NOT NULL,
	[Id_incrocio] [int] NOT NULL,
	[Id_semaforo] [tinyint] NOT NULL,
	[Id_strada] [tinyint] NOT NULL,
	[Stato_semaforo] [tinyint] NOT NULL,
	[Fascia_oraria] [tinyint] NOT NULL,
	[Data] [date] NOT NULL,
	[Tipologia_veicolo] [varchar](25) NOT NULL,
	[Conteggio] [int] NOT NULL,
 */

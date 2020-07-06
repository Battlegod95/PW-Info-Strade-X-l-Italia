using System;
using System.Collections.Generic;
using System.Text;

namespace PWInfoStradeFunctions
{
    public class SQLJsonObject
    {
        public int Id_temporizzazione { get; set; }
        public int Id_incrocio { get; set; }
        public bool Id_semaforo { get; set; }
        public int Fascia_oraria { get; set; }
        public int Valore_tempo { get; set; }
    }
}

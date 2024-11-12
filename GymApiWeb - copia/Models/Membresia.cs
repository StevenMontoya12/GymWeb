using System;
using System.Collections.Generic;

namespace gymapiweb.Models
{
    public class Membresia
    {
        public Guid MembresiaID { get; set; } // Llave primaria
        public string TipoMembresia { get; set; } = string.Empty;
        public decimal Costo { get; set; }
        public int DuracionMeses { get; set; }
        public string Descripcion { get; set; } = string.Empty;

        // Relación con Pago
        public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    }
}

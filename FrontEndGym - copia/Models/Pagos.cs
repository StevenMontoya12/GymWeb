using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorFrontend.Models
{
    public class Pago
    {
        [Key]
        public Guid PagoID { get; set; }

        [Required]
        public Guid ClienteID { get; set; }

        [Required]
        public Guid MembresiaID { get; set; }

        public Guid? EmpleadoID { get; set; } // Opcional

        [Required]
        public decimal MontoPagado { get; set; }

        [Required]
        public DateTime FechaPago { get; set; }
    }
}

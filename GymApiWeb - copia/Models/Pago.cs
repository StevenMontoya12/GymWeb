using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gymapiweb.Models
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
        [Column(TypeName = "decimal(10, 2)")]
        public decimal MontoPagado { get; set; }

        [Required]
        public DateTime FechaPago { get; set; }

        // Propiedades de navegación, marcadas como opcionales
        [ForeignKey("ClienteID")]
        public Cliente? Cliente { get; set; }

        [ForeignKey("MembresiaID")]
        public Membresia? Membresia { get; set; }

        [ForeignKey("EmpleadoID")]
        public Empleado? Empleado { get; set; }
    }
}

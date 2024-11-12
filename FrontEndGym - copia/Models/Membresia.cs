// Models/Membresia.cs
namespace BlazorFrontend.Models
{
    public class Membresia
    {
        public Guid MembresiaID { get; set; } // Llave primaria
        public string TipoMembresia { get; set; } = string.Empty;
        public decimal Costo { get; set; }
        public int DuracionMeses { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }
}

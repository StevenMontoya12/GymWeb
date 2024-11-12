namespace gymapiweb.Models
{
    public class EntrenadorDto
    {
        public Guid EntrenadorID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Sexo { get; set; }
        public string Especialidad { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public DateTime FechaContratacion { get; set; }
        public string? FotoBase64 { get; set; } // Cadena en Base64 para la imagen
    }
}

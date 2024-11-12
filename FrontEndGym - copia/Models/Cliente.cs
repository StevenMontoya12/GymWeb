namespace gymapiweb.Models
{
    public class Cliente
    {
        public Guid ClienteID { get; set; }  // Asegúrate de que esto sea opcional en el POST (se genera en el servidor)
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Sexo { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        
    }
}

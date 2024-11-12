namespace gymapiweb.Models
{
    public class Entrenador
    {
        public Guid EntrenadorID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Sexo { get; set; }
        public string Especialidad { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public DateTime FechaContratacion { get; set; }
        public byte[] Foto { get; set; } // Campo para almacenar la imagen en bytes
    }
}

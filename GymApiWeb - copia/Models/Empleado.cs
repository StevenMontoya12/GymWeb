namespace gymapiweb.Models
{
    public class Empleado
    {
        public Guid EmpleadoID { get; set; }
        public required string? Nombre { get; set; }
        public required string? Apellido { get; set; }
        public required string? Sexo { get; set; }
        public required string? Telefono { get; set; }
        public required string? CorreoElectronico { get; set; }
        public required string? Direccion { get; set; }
        public required DateTime? FechaContratacion { get; set; }
        public required string? Rol { get; set; }
        public required string? Usuario { get; set; }
        public required string? Password { get; set; }
    }
}

namespace gymapiweb.Models
{
    public class Cliente
    {
    public Guid ClienteID { get; set; }
    public required string? Nombre { get; set; }
    public required string? Apellido { get; set; }
    public required string? Sexo { get; set; }
    public DateTime? FechaNacimiento { get; set; } // Agregar propiedad de fecha de nacimiento
    public required string? Telefono { get; set; }
    public required string? CorreoElectronico { get; set; }
    public required string? Direccion { get; set; }
    public required string? Password { get; set; }

        
        

    }
}

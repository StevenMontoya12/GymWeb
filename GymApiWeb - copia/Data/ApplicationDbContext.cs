using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using gymapiweb.Models;
using System.Threading.Tasks;

namespace gymapiweb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Membresia> Membresias { get; set; }
        public DbSet<Entrenador> Entrenadores { get; set; }

        // Definir la tabla Clientes como DbSet
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empleado> Empleados { get; set; } // Asegúrate de agregar esta línea para Empleados

        // Método para iniciar sesión de Empleado
        public async Task<Empleado> LoginEmpleadoAsync(string correo, string password)
        {
            var empleado = await Empleados
                .FromSqlRaw("EXEC sp_LoginEmpleado @Correo, @Password", 
                            new SqlParameter("@Correo", correo), // Cambié de Usuario a Correo
                            new SqlParameter("@Password", password))
                .FirstOrDefaultAsync();

            return empleado;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pago>()
                .Property(p => p.MontoPagado)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Membresia>()
                .Property(m => m.Costo)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }

  // Método para registrar un nuevo cliente usando un procedimiento almacenado
        public async Task RegistrarClienteAsync(Cliente nuevoCliente)
        {
            // Parámetros del procedimiento almacenado
            var parameters = new[]
            {
                new SqlParameter("@Nombre", nuevoCliente.Nombre),
                new SqlParameter("@Apellido", nuevoCliente.Apellido),
                new SqlParameter("@Sexo", nuevoCliente.Sexo),
                new SqlParameter("@FechaNacimiento", nuevoCliente.FechaNacimiento ?? (object)DBNull.Value),
                new SqlParameter("@Telefono", nuevoCliente.Telefono),
                new SqlParameter("@CorreoElectronico", nuevoCliente.CorreoElectronico),
                new SqlParameter("@Direccion", nuevoCliente.Direccion),
                new SqlParameter("@Password", nuevoCliente.Password)
            };

            // Ejecutar el procedimiento almacenado
            await Database.ExecuteSqlRawAsync("EXEC sp_RegistrarCliente @Nombre, @Apellido, @Sexo, @FechaNacimiento, @Telefono, @CorreoElectronico, @Direccion, @Password", parameters);
        }



        // Método para iniciar sesión de Cliente
        public async Task<bool> LoginClienteAsync(string correoElectronico, string password)
        {
            // Ejecuta el procedimiento almacenado para iniciar sesión.
            var result = await Database.ExecuteSqlRawAsync(
                "EXEC sp_LoginCliente @CorreoElectronico, @Password", 
                new SqlParameter("@CorreoElectronico", correoElectronico), 
                new SqlParameter("@Password", password)
            );

            // Retorna true si se afectó al menos una fila, lo que indica un inicio de sesión exitoso.
            return result > 0;
        }
    }
}

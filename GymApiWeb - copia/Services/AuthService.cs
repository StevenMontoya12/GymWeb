using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using gymapiweb.Data;
using gymapiweb.Models;

namespace gymapiweb.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _jwtSecret;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _jwtSecret = configuration["Jwt:SecretKey"];
            _jwtIssuer = configuration["Jwt:Issuer"];
            _jwtAudience = configuration["Jwt:Audience"];
        }

        public async Task<Empleado?> LoginEmpleadoAsync(string usuario, string password)
        {
            try
            {
                var empleado = _context.Empleados
                    .FromSqlRaw("EXEC sp_LoginEmpleado @Usuario, @Password",
                                new SqlParameter("@Usuario", usuario),
                                new SqlParameter("@Password", password))
                    .AsEnumerable()  // Realiza el resto de la operación en memoria
                    .FirstOrDefault();

                return empleado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al iniciar sesión como empleado: {ex.Message}");
                return null;
            }
        }

        public async Task<Cliente?> LoginClienteAsync(string correoElectronico, string password)
        {
            try
            {
                var cliente = _context.Clientes
                    .FromSqlRaw("EXEC sp_LoginCliente @CorreoElectronico, @Password",
                                new SqlParameter("@CorreoElectronico", correoElectronico),
                                new SqlParameter("@Password", password))
                    .AsEnumerable()  // Realiza el resto de la operación en memoria
                    .FirstOrDefault();

                return cliente;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al iniciar sesión como cliente: {ex.Message}");
                return null;
            }
        }

        public string GenerateJwtToken(string email, string role, Guid userId)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(ClaimTypes.Role, role),
                new Claim("UserID", userId.ToString()),  // Asegúrate de que el nombre de la clave sea "UserID"
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

namespace BlazorFrontend.Models
{
    public class LoginResponse
    {
        public string Token { get; set; } // Esta propiedad almacenará el JWT
        public string Role { get; set; }  // Asegúrate de que también incluya el rol del usuario, si el backend lo devuelve
    }
}

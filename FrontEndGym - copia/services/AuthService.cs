using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace BlazorFrontend.Services
{
    public class AuthService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;

        public AuthService(ILocalStorageService localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        public event Action OnChange;
        public bool IsCliente { get; private set; }
        public bool IsEmpleado { get; private set; }
        public bool IsAuthenticated { get; private set; }

        public async Task LoginAsync(string jwtToken)
        {
            await _localStorage.SetItemAsync("jwtToken", jwtToken);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", jwtToken);

            await SetUserRoleAndClienteID(jwtToken);
            IsAuthenticated = true;
            NotifyStateChanged();
        }

        public async Task LogoutAsync()
        {
            IsCliente = false;
            IsEmpleado = false;
            IsAuthenticated = false;

            await _localStorage.RemoveItemAsync("jwtToken");
            await _localStorage.RemoveItemAsync("ClienteID");
            _httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyStateChanged();
        }

        public async Task InitializeAuthStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("jwtToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                await SetUserRoleAndClienteID(token);
                IsAuthenticated = true;
                NotifyStateChanged();
            }
        }

        public async Task<Guid> GetClienteIDAsync()
        {
            var clienteId = await _localStorage.GetItemAsync<Guid?>("ClienteID");
            return clienteId ?? Guid.Empty;
        }

        private async Task SetUserRoleAndClienteID(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtTokenObj = tokenHandler.ReadJwtToken(jwtToken);

            var role = jwtTokenObj.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var userIdClaim = jwtTokenObj.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;

            IsCliente = role == "Cliente";
            IsEmpleado = role == "Empleado";

            // Si el usuario es un cliente, almacena el UserID en el almacenamiento local
            if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var userId))
            {
                await _localStorage.SetItemAsync("ClienteID", userId);
            }
        }


        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}

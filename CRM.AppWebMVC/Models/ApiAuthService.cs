using System.Text;
using System.Text.Json;

namespace CRM.AppWebMVC.Models
{
    public class ApiAuthService
    {
        private readonly HttpClient _httpClient;

        public ApiAuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TokenResponse?> LoginAsync(string name, string password)
        {
            // Crear el cuerpo del request
            var loginData = new
            {
                Name = name,
                Password = password
            };

            var content = new StringContent(
                JsonSerializer.Serialize(loginData),
                Encoding.UTF8,
                "application/json"
            );

            // Llamar a la API
            var response = await _httpClient.PostAsync("https://api.example.com/auth/login", content);

            if (!response.IsSuccessStatusCode)
            {
                return null; // Login fallido
            }

            // Leer y deserializar el token de la respuesta
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TokenResponse>(responseString);
        }
    }
}

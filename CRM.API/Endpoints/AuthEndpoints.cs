using CRM.API.Models.DAL;
using CRM.API.Models.EN;  // Asegúrate de que el espacio de nombres es correcto
using CRM.DTOs.UsersDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CRM.API.Endpoints
{
    public static class AuthEndpoints
    {
        public static void AddAuthEndpoints(this WebApplication app)
        {
            app.MapPost("/api/auth/login", async ([FromBody] LoginDTO loginDTO, IConfiguration configuration, UsersDAL usersDAL) =>
            {
                // Lógica de validación de credenciales (reemplazar con lógica real usando UsersDAL)
                var user = await usersDAL.ObtenerUsuarioPorDUIyPassword(loginDTO.Name, loginDTO.Password);

                if (user == null)
                {
                    return Results.Json(new { message = "Credenciales inválidas" }, statusCode: StatusCodes.Status401Unauthorized);
                }

                // Crear las reclamaciones (claims) del token
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) // Usando el ID del usuario como claim
                };

                // Leer la clave secreta desde el archivo de configuración
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? "TuLlaveSecretaSuperSegura123"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Crear el token
                var token = new JwtSecurityToken(
                    issuer: null,
                    audience: null,
                    claims: claims,
                    expires: DateTime.Now.AddHours(24),
                    signingCredentials: creds);

                // Devolver el token al cliente
                return Results.Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            })
            .WithTags("Authentication")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        }
    }
}

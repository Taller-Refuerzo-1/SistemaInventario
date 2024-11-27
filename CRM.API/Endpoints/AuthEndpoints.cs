using CRM.API.Models.DAL;
using CRM.API.Models.EN;
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
            app.MapPost("/api/auth/login", async (
                [FromBody] LoginDTO loginDTO,
                IConfiguration configuration,
                UsersDAL usersDAL) =>
            {
                // Validar credenciales
                var user = await usersDAL.ObtenerUsuarioPorDUIyPassword(loginDTO.Name, loginDTO.Password);

                if (user == null)
                {
                    return Results.Problem(
                        detail: "Credenciales inválidas",
                        statusCode: StatusCodes.Status401Unauthorized
                    );
                }

                // Configurar los claims para el token
                var claims = new[]
{
    new Claim(ClaimTypes.Name, user.Name),
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(JwtRegisteredClaimNames.Email, user.Email),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
};


                // Leer la clave secreta desde la configuración
                var secretKey = configuration["Jwt:Key"];
                if (string.IsNullOrEmpty(secretKey))
                {
                    return Results.Problem(
                        detail: "La clave secreta para generar el token no está configurada",
                        statusCode: StatusCodes.Status500InternalServerError
                    );
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Crear el token JWT
                var token = new JwtSecurityToken(
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(24), // Token expira en 24 horas
                    signingCredentials: creds
                );

                // Responder con el token y su expiración
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

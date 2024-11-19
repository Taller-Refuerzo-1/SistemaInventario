using CRM.API.Endpoints;
using CRM.API.Models.DAL;
using CRM.API.Models.EN;
using CRM.DTOs.UsersDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Servicios necesarios
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Configuración de seguridad para JWT en Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Por favor ingresa el token JWT",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configuración de base de datos
builder.Services.AddDbContext<CRMContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Conn"))
);

builder.Services.AddScoped<CustomerDAL>();
builder.Services.AddScoped<UsersDAL>();
builder.Services.AddScoped<ProvidersDAL>();

// Configuración de autenticación y autorización
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
.AddJwtBearer("JwtBearer", options =>
{
    var secretKey = builder.Configuration["Jwt:Key"];
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key  // La clave para verificar la firma del token
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Autenticación
app.UseAuthorization();  // Autorización

// Endpoints
app.AddCustomerEndpoints();
app.AddUsersEndpoints();
app.AddProviderEndpoints();
app.AddAuthEndpoints();  // Llamada para agregar los endpoints de autenticación

app.Run();

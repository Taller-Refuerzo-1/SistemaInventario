using CRM.API.Endpoints;
using CRM.API.Models.DAL;
using CRM.API.Models.EN;
using CRM.DTOs.UsersDTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuración del entorno
var configuration = builder.Configuration;

// --- Configuración de servicios ---

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingresa el token JWT en el formato: Bearer {token}",
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

// Configuración de la base de datos
builder.Services.AddDbContext<CRMContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("Conn"))
);

// Registrar servicios DAL
builder.Services.AddScoped<CustomerDAL>();
builder.Services.AddScoped<UsersDAL>();
builder.Services.AddScoped<ProvidersDAL>();
builder.Services.AddScoped<SucursalDAL>();
builder.Services.AddScoped<CompanyDAL>();
builder.Services.AddScoped<CategoryDAL>();
builder.Services.AddScoped<BodegaDAL>();
builder.Services.AddScoped<ProductDAL>();

// Configuración de autenticación y JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = configuration["Jwt:Audience"],

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        };

        // Eventos para depuración
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validated successfully.");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// --- Configuración de middlewares ---

// Middleware para el entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "JWT Auth API V1");
    });
}
else
{
    // Configuración para entornos de producción (si aplica)
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "JWT Auth API V1");
    });
}

//app.UseHttpsRedirection();
app.UseAuthentication(); // Debe estar antes de UseAuthorization
app.UseAuthorization();

// --- Registro de endpoints ---

app.AddCustomerEndpoints();
app.AddUsersEndpoints();
app.AddProviderEndpoints();
app.AddSucursalEndpoint();
app.AddAuthEndpoints();
app.AddCompanyEndpoints();
app.AddCategoryEndpoints();
app.AddBodegaEndpoints();
app.AddProductEndpoints();

app.Run();

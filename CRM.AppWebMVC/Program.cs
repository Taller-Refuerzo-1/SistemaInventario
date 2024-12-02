using CRM.AppWebMVC.Controllers;
using CRM.AppWebMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args); // Crea un constructor de aplicaciones web

// Agrega servicios al contenedor de dependencias.
builder.Services.AddControllersWithViews(); // Agrega servicios para controladores y vistas

builder.Services.AddHttpClient<ApiAuthService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7007/"); // Cambia esta URL a la de tu API
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

// Configura y agrega un cliente HTTP con nombre "CRMAPI"
builder.Services.AddHttpClient("CRMAPI", c =>
{
    // Configura la dirección base del cliente HTTP desde la configuración
    c.BaseAddress = new Uri(builder.Configuration["UrlsAPI:CRM"]); 
    // Puedes configurar otras opciones del HttpClient aquí según sea necesario
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

var app = builder.Build(); // Crea una instancia de la aplicación web

// Configura el pipeline de solicitudes HTTP.
if (!app.Environment.IsDevelopment())
{
    // Maneja excepciones en caso de errores y redirige a la acción "Error" en el controlador "Home"
    app.UseExceptionHandler("/Home/Error"); 
    // El valor HSTS predeterminado es de 30 días. Puedes cambiarlo para escenarios de producción.
    app.UseHsts();
}



app.UseHttpsRedirection(); // Redirige las solicitudes HTTP a HTTPS
app.UseStaticFiles(); // Habilita el uso de archivos estáticos como CSS, JavaScript, imágenes, etc.

app.UseRouting(); // Configura el enrutamiento de solicitudes

app.UseAuthorization(); // Habilita la autorización para proteger rutas y acciones de controladores

// Mapea la ruta predeterminada de controlador y acción
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); // Inicia la aplicación web

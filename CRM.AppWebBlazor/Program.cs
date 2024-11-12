using CRM.AppWebBlazor.Data; // Importa el espacio de nombres donde se encuentra CustomerService

var builder = WebApplication.CreateBuilder(args);

// Agrega servicios al contenedor de dependencias.
builder.Services.AddRazorPages(); // Agrega soporte para páginas Razor
builder.Services.AddServerSideBlazor(); // Agrega soporte para Blazor en el lado del servidor

// Registra CustomerService como un servicio Singleton (una instancia única para toda la aplicación)
builder.Services.AddSingleton<CustomerService>(); 

// Configura y agrega un cliente HTTP con nombre "CRMAPI"
builder.Services.AddHttpClient("CRMAPI", c =>
{
    // Configura la dirección base del cliente HTTP desde la configuración
    c.BaseAddress = new Uri(builder.Configuration["UrlsAPI:CRM"]); 
    // Puedes configurar otras opciones del HttpClient aquí según sea necesario
});

var app = builder.Build(); // Crea una instancia de la aplicación web

// Configura el pipeline de solicitudes HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); // Maneja excepciones en caso de errores
    // El valor HSTS predeterminado es de 30 días. Puedes cambiarlo para escenarios de producción.
    app.UseHsts();
}

//app.UseHttpsRedirection(); // Redirige las solicitudes HTTP a HTTPS

app.UseStaticFiles(); // Habilita el uso de archivos estáticos como CSS, JavaScript, imágenes, etc.

app.UseRouting(); // Configura el enrutamiento de solicitudes

app.MapBlazorHub(); // Mapea BlazorHub para habilitar la funcionalidad de Blazor en tiempo real
app.MapFallbackToPage("/_Host"); // Mapea la página _Host para servir aplicaciones Blazor

app.Run(); // Inicia la aplicación web

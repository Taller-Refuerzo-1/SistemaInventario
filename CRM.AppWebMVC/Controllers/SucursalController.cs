using CRM.DTOs.SucursalDTOs;
using Microsoft.AspNetCore.Mvc;

namespace CRM.AppWebMVC.Controllers
{
    public class SucursalController : Controller
    {
        // Para Hacer Solicitudes Al Servidor:
        private readonly HttpClient _HttpClient;
         
        public SucursalController(IHttpClientFactory httpClientFactory)
        {
            _HttpClient = httpClientFactory.CreateClient("CRMAPI");
        }


        // OBTIENE TODOS LOS REGISTROS DE LA DB:
        public async Task<IActionResult> Registrados()
        {
            // Solicitud "GET" al Endpoint:
            HttpResponseMessage JSON_Obtenidos = await _HttpClient.GetAsync("/api/Sucursal");

            // OBJETO:
            Registrados_Sucursal Lista_Sucursales = new Registrados_Sucursal();

            // True=200-299
            if (JSON_Obtenidos.IsSuccessStatusCode)
            {
                // Deserializamos el Json:
                Lista_Sucursales = await JSON_Obtenidos.Content.ReadFromJsonAsync<Registrados_Sucursal>();
            }

            return View(Lista_Sucursales);
        }


        // OBTIENE UN REGISTRO CON EL MISMO ID:
        public async Task<IActionResult> Detalle(int id)
        {
            // Solicitud "GET" al Endpoint:
            HttpResponseMessage JSON_Obtenido = await _HttpClient.GetAsync("/api/Sucursal/" + id);

            // OBJETO:
            Obtener_PorID Objeto_Obtenido = new Obtener_PorID();

            // Codigo Status:
            if (JSON_Obtenido.IsSuccessStatusCode)
            {
                Objeto_Obtenido = await JSON_Obtenido.Content.ReadFromJsonAsync<Obtener_PorID>();
            }

            return View(Objeto_Obtenido);
        }


        // NOS MANDA A LA VISTA:
        public ActionResult Crear()
        {
            return View();
        }

        // RECIBE UN OBJETO Y LO GUARDA EN LA DB:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Crear_Sucursal crear_Sucursal)
        {
            // Solicitud "POST" al Endpoint:
            HttpResponseMessage Respuesta = await _HttpClient.PostAsJsonAsync("/api/Sucursal", crear_Sucursal);

            // Codigo Status:
            if (Respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction("Registrados", "Sucursal");
            }

            return View();
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO MANDA A VISTA
        public async Task<IActionResult> Editar(int id)
        {
            // Solicitud "GET" al Endpoint:
            HttpResponseMessage JSON_Obtenido = await _HttpClient.GetAsync("/api/Sucursal/" + id);

            // OBJETO:
            Obtener_PorID Objeto_Obtenido = new Obtener_PorID();

            // Codigo Status:
            if (JSON_Obtenido.IsSuccessStatusCode)
            {
                Objeto_Obtenido = await JSON_Obtenido.Content.ReadFromJsonAsync<Obtener_PorID>();
            }

            Editar_Sucursal Objeto_Editar = new Editar_Sucursal
            {
                Id = Objeto_Obtenido.Id,
                Nombre = Objeto_Obtenido.Nombre,
                Direccion = Objeto_Obtenido.Direccion,
                Telefono = Objeto_Obtenido.Telefono,
                Empleados = Objeto_Obtenido.Empleados
            };

            return View(Objeto_Editar);
        }

        // RECIBE EL OBJETO MODIFICADO Y LO MODIFICA EN DB:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Editar_Sucursal editar_Sucursal)
        {
            // Solicitud "PUT" al Endpoint:
            HttpResponseMessage Respuesta = await _HttpClient.PutAsJsonAsync("/api/Sucursal", editar_Sucursal);

            // Codigo Status:
            if (Respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction("Registrados", "Sucursal");
            }

            return View();
        }




        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO MANDA A VISTA
        public async Task<IActionResult> Eliminar(int id)
        {
            // Solicitud "GET" al Endpoint:
            HttpResponseMessage JSON_Obtenido = await _HttpClient.GetAsync("/api/Sucursal/" + id);

            // OBJETO:
            Obtener_PorID Objeto_Obtenido = new Obtener_PorID();

            // Codigo Status:
            if (JSON_Obtenido.IsSuccessStatusCode)
            {
                Objeto_Obtenido = await JSON_Obtenido.Content.ReadFromJsonAsync<Obtener_PorID>();
            }

            return View(Objeto_Obtenido);
        }

        // RECIBE EL OBJETO MODIFICADO Y LO MODIFICA EN DB:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(Obtener_PorID obtener_PorID)
        {
            // Solicitud "PUT" al Endpoint:
            HttpResponseMessage Respuesta = await _HttpClient.DeleteAsync("/api/Sucursal/" + obtener_PorID.Id);

            // Codigo Status:
            if (Respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction("Registrados", "Sucursal");
            }

            return View();
        }
    }
}

using CRM.DTOs.ProviderDTOs;
using Microsoft.AspNetCore.Mvc;

namespace CRM.AppWebMVC.Controllers
{
    public class ProvidersController : Controller
    {
        private readonly HttpClient _httpClientCRMAPI;

        // Constructor que recibe una instancia de IHttpClientFactory para crear el cliente HTTP
        public ProvidersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientCRMAPI = httpClientFactory.CreateClient("CRMAPI");
        }

        // Método para mostrar la lista de clientes
        public async Task<IActionResult> Index(SearchQueryProviderDTO searchQueryProviderDTO, int CountRow = 0)
        {
            // Configuración de valores por defecto para la búsqueda
            if (searchQueryProviderDTO.SendRowCount == 0)
                searchQueryProviderDTO.SendRowCount = 2;
            if (searchQueryProviderDTO.Take == 0)
                searchQueryProviderDTO.Take = 10;

            var result = new SearchResultProviderDTO();

            // Realizar una solicitud HTTP POST para buscar clientes en el servicio web
            var response = await _httpClientCRMAPI.PostAsJsonAsync("Provider/search", searchQueryProviderDTO);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<SearchResultProviderDTO>();

            result = result != null ? result : new SearchResultProviderDTO();

            // Configuración de valores para la vista
            if (result.CountRow == 0 && searchQueryProviderDTO.SendRowCount == 1)
                result.CountRow = CountRow;

            ViewBag.CountRow = result.CountRow;
            searchQueryProviderDTO.SendRowCount = 0;
            ViewBag.SearchQuery = searchQueryProviderDTO;

            return View(result);
        }

        // Método para mostrar los detalles de un cliente
        public async Task<IActionResult> Details(int id)
        {
            var result = new GetIdResultProviderDTO();

            // Realizar una solicitud HTTP GET para obtener los detalles del cliente por ID
            var response = await _httpClientCRMAPI.GetAsync("Provider/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultProviderDTO>();

            return View(result ?? new GetIdResultProviderDTO());
        }

        // Método para mostrar el formulario de creación de un cliente
        public ActionResult Create()
        {
            return View();
        }

        // Método para procesar la creación de un cliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProviderDTO createProviderDTO)
        {
            try
            {
                // Realizar una solicitud HTTP POST para crear un nuevo cliente
                var response = await _httpClientCRMAPI.PostAsJsonAsync("Provider", createProviderDTO);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = "Error al intentar guardar el registro";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // Método para mostrar el formulario de edición de un cliente
        public async Task<IActionResult> Edit(int id)
        {
            var result = new GetIdResultProviderDTO();
            var response = await _httpClientCRMAPI.GetAsync("Provider/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultProviderDTO>();

            return View(new EditProviderDTO(result ?? new GetIdResultProviderDTO()));
        }

        // Método para procesar la edición de un cliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditProviderDTO editProviderDTO)
        {
            try
            {
                // Realizar una solicitud HTTP PUT para editar el cliente
                var response = await _httpClientCRMAPI.PutAsJsonAsync("Provider", editProviderDTO);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = "Error al intentar editar el registro";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // Método para mostrar la página de confirmación de eliminación de un cliente
        public async Task<IActionResult> Delete(int id)
        {
            var result = new GetIdResultProviderDTO();
            var response = await _httpClientCRMAPI.GetAsync("Provider/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultProviderDTO>();

            return View(result ?? new GetIdResultProviderDTO());
        }

        // Método para procesar la eliminación de un cliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, GetIdResultProviderDTO getIdResultProviderDTO)
        {
            try
            {
                // Realizar una solicitud HTTP DELETE para eliminar el cliente por ID
                var response = await _httpClientCRMAPI.DeleteAsync("Provider/" + id);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = "Error al intentar eliminar el registro";
                return View(getIdResultProviderDTO);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(getIdResultProviderDTO);
            }
        }
    }
}

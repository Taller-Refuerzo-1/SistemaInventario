using CRM.DTOs.BodegaDTOs;
using Microsoft.AspNetCore.Mvc;

namespace CRM.AppWebMVC.Controllers
{
    public class BodegaController : Controller
    {
        private readonly HttpClient _httpClientCRMAPI;

        // Constructor que recibe una instancia de IHttpClientFactory para crear el cliente HTTP
        public BodegaController(IHttpClientFactory httpClientFactory)
        {
            _httpClientCRMAPI = httpClientFactory.CreateClient("CRMAPI");
        }
        // Método para mostrar la lista de bodegas
        public async Task<IActionResult> Index(SearchQueryBodegaDTO searchQueryBodegaDTO, int CountRow = 0)
        {
            // Configuración de valores por defecto para la búsqueda
            if (searchQueryBodegaDTO.SendRowCount == 0)
                searchQueryBodegaDTO.SendRowCount = 2;
            if (searchQueryBodegaDTO.Take == 0)
                searchQueryBodegaDTO.Take = 10;

            var result = new SearchResultBodegaDTO();

            // Realizar una solicitud HTTP POST para buscar bodegas en el servicio web
            var response = await _httpClientCRMAPI.PostAsJsonAsync("bodega/search", searchQueryBodegaDTO);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<SearchResultBodegaDTO>();

            result = result != null ? result : new SearchResultBodegaDTO();

            // Configuración de valores para la vista
            if (result.CountRow == 0 && searchQueryBodegaDTO.SendRowCount == 1)
                result.CountRow = CountRow;

            ViewBag.CountRow = result.CountRow;
            searchQueryBodegaDTO.SendRowCount = 0;
            ViewBag.SearchQuery = searchQueryBodegaDTO;

            return View(result);
        }

        // Método para mostrar los detalles de una bodega
        public async Task<IActionResult> Details(int id)
        {
            var result = new GetIdResultBodegaDTO();

            // Realizar una solicitud HTTP GET para obtener los detalles de la bodega por ID
            var response = await _httpClientCRMAPI.GetAsync("bodega/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultBodegaDTO>();

            return View(result ?? new GetIdResultBodegaDTO());
        }

        // Método para mostrar el formulario de creación de una bodega
        public ActionResult Create()
        {
            return View();
        }

        // Método para procesar la creación de una bodega
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBodegaDTO createBodegaDTO)
        {
            try
            {
                // Realizar una solicitud HTTP POST para crear una nueva bodega
                var response = await _httpClientCRMAPI.PostAsJsonAsync("bodega", createBodegaDTO);

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

        // Método para mostrar el formulario de edición de una bodega
        public async Task<IActionResult> Edit(int id)
        {
            var result = new GetIdResultBodegaDTO();
            var response = await _httpClientCRMAPI.GetAsync("bodega/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultBodegaDTO>();

            return View(new EditBodegaDTO(result ?? new GetIdResultBodegaDTO()));
        }

        // Método para procesar la edición de una bodega
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditBodegaDTO editBodegaDTO)
        {
            try
            {
                // Realizar una solicitud HTTP PUT para editar la bodega
                var response = await _httpClientCRMAPI.PutAsJsonAsync("bodega", editBodegaDTO);

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

        // Método para mostrar la página de confirmación de eliminación de una bodega
        public async Task<IActionResult> Delete(int id)
        {
            var result = new GetIdResultBodegaDTO();
            var response = await _httpClientCRMAPI.GetAsync("bodega/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultBodegaDTO>();

            return View(result ?? new GetIdResultBodegaDTO());
        }

        // Método para procesar la eliminación de una bodega
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, GetIdResultBodegaDTO getIdResultBodegaDTO)
        {
            try
            {
                // Realizar una solicitud HTTP DELETE para eliminar la bodega por ID
                var response = await _httpClientCRMAPI.DeleteAsync("bodega/" + id);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = "Error al intentar eliminar el registro";
                return View(getIdResultBodegaDTO);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(getIdResultBodegaDTO);
            }
        }
    }
}

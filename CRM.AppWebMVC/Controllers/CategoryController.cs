using CRM.DTOs.CategoryDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.AppWebMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly HttpClient _httpClientCRMAPI;

        // Constructor que recibe una instancia de IHttpClientFactory para crear el cliente HTTP
        public CategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientCRMAPI = httpClientFactory.CreateClient("CRMAPI");
        }

        // Método para mostrar la lista de compañías
        public async Task<IActionResult> Index(SearchQueryCategoryDTOs searchQueryCategoryDTO, int CountRow = 0)
        {
            // Configuración de valores por defecto para la búsqueda
            if (searchQueryCategoryDTO.SendRowCount == 0)
                searchQueryCategoryDTO.SendRowCount = 2;
            if (searchQueryCategoryDTO.Take == 0)
                searchQueryCategoryDTO.Take = 10;

            var result = new SearchResultCategoryDTO();

            // Realizar una solicitud HTTP POST para buscar compañías en el servicio web
            var response = await _httpClientCRMAPI.PostAsJsonAsync("category/search", searchQueryCategoryDTO);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<SearchResultCategoryDTO>();

            result = result != null ? result : new SearchResultCategoryDTO();

            // Configuración de valores para la vista
            if (result.CountRow == 0 && searchQueryCategoryDTO.SendRowCount == 1)
                result.CountRow = CountRow;

            ViewBag.CountRow = result.CountRow;
            searchQueryCategoryDTO.SendRowCount = 0;
            ViewBag.SearchQuery = searchQueryCategoryDTO;

            return View(result);
        }

        // Método para mostrar los detalles de una compañía
        public async Task<IActionResult> Details(int id)
        {
            var result = new GetIdResultCategoryDTO();

            // Realizar una solicitud HTTP GET para obtener los detalles de la compañía por ID 
            var response = await _httpClientCRMAPI.GetAsync("category/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultCategoryDTO>();

            return View(result ?? new GetIdResultCategoryDTO());
        }

        // Método para mostrar el formulario de creación de una compañía
        public ActionResult Create()
        {
            return View();
        }

        // Método para procesar la creación de una compañía
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryDTO createCategoryDTO)
        {
            try
            {
                // Realizar una solicitud HTTP POST para crear una nueva compañía
                var response = await _httpClientCRMAPI.PostAsJsonAsync("category", createCategoryDTO);

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

        // Método para mostrar el formulario de edición de una compañía
        public async Task<IActionResult> Edit(int id)
        {
            var result = new GetIdResultCategoryDTO();
            var response = await _httpClientCRMAPI.GetAsync("category/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultCategoryDTO>();

            return View(new EditCategoryDTO(result ?? new GetIdResultCategoryDTO()));
        }

        // Método para procesar la edición de una compañía
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCategoryDTO editCategoryDTO)
        {
            try
            {
                // Realizar una solicitud HTTP PUT para editar la compañía
                var response = await _httpClientCRMAPI.PutAsJsonAsync("category", editCategoryDTO);

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

        // Método para mostrar la página de confirmación de eliminación de una compañía
        public async Task<IActionResult> Delete(int id)
        {
            var result = new GetIdResultCategoryDTO();
            var response = await _httpClientCRMAPI.GetAsync("category/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultCategoryDTO>();

            return View(result ?? new GetIdResultCategoryDTO());
        }

        // Método para procesar la eliminación de una compañía
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, GetIdResultCategoryDTO getIdResultCategryDTO, object getIdResultCategoryDTO)
        {
            try
            {
                // Realizar una solicitud HTTP DELETE para eliminar la compañía por ID
                var response = await _httpClientCRMAPI.DeleteAsync("category/" + id);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = "Error al intentar eliminar el registro";
                return View(getIdResultCategoryDTO);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(getIdResultCategoryDTO);
            }
        }
    }
}

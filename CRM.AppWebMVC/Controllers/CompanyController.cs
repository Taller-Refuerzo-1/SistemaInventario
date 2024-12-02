using CRM.DTOs.CompanyDTOs;
using Microsoft.AspNetCore.Mvc;

namespace CRM.AppWebMVC.Controllers
{
    public class CompanyController : Controller
    {
        private readonly HttpClient _httpClientCRMAPI;

        // Constructor que recibe una instancia de IHttpClientFactory para crear el cliente HTTP
        public CompanyController(IHttpClientFactory httpClientFactory)
        {
            _httpClientCRMAPI = httpClientFactory.CreateClient("CRMAPI");
        }

        // Método para mostrar la lista de compañías
        public async Task<IActionResult> Index(SearchQueryCompanyDTO searchQueryCompanyDTO, int CountRow = 0)
        {
            // Configuración de valores por defecto para la búsqueda
            if (searchQueryCompanyDTO.SendRowCount == 0)
                searchQueryCompanyDTO.SendRowCount = 2;
            if (searchQueryCompanyDTO.Take == 0)
                searchQueryCompanyDTO.Take = 10;

            var result = new SearchResultCompanyDTO();

            // Realizar una solicitud HTTP POST para buscar compañías en el servicio web
            var response = await _httpClientCRMAPI.PostAsJsonAsync("company/search", searchQueryCompanyDTO);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<SearchResultCompanyDTO>();

            result = result != null ? result : new SearchResultCompanyDTO();

            // Configuración de valores para la vista
            if (result.CountRow == 0 && searchQueryCompanyDTO.SendRowCount == 1)
                result.CountRow = CountRow;

            ViewBag.CountRow = result.CountRow;
            searchQueryCompanyDTO.SendRowCount = 0;
            ViewBag.SearchQuery = searchQueryCompanyDTO;

            return View(result);
        }

        // Método para mostrar los detalles de una compañía
        public async Task<IActionResult> Details(int id)
        {
            var result = new GetIdResultCompanyDTO();

            // Realizar una solicitud HTTP GET para obtener los detalles de la compañía por ID
            var response = await _httpClientCRMAPI.GetAsync("company/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultCompanyDTO>();

            return View(result ?? new GetIdResultCompanyDTO());
        }

        // Método para mostrar el formulario de creación de una compañía
        public ActionResult Create()
        {
            return View();
        }

        // Método para procesar la creación de una compañía
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCompanyDTO createCompanyDTO)
        {
            try
            {
                // Realizar una solicitud HTTP POST para crear una nueva compañía
                var response = await _httpClientCRMAPI.PostAsJsonAsync("company", createCompanyDTO);

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
            var result = new GetIdResultCompanyDTO();
            var response = await _httpClientCRMAPI.GetAsync("company/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultCompanyDTO>();

            return View(new EditCompanyDTO(result ?? new GetIdResultCompanyDTO()));
        }

        // Método para procesar la edición de una compañía
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCompanyDTO editCompanyDTO)
        {
            try
            {
                // Realizar una solicitud HTTP PUT para editar la compañía
                var response = await _httpClientCRMAPI.PutAsJsonAsync("company", editCompanyDTO);

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
            var result = new GetIdResultCompanyDTO();
            var response = await _httpClientCRMAPI.GetAsync("company/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultCompanyDTO>();

            return View(result ?? new GetIdResultCompanyDTO());
        }

        // Método para procesar la eliminación de una compañía
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, GetIdResultCompanyDTO getIdResultCompanyDTO)
        {
            try
            {
                // Realizar una solicitud HTTP DELETE para eliminar la compañía por ID
                var response = await _httpClientCRMAPI.DeleteAsync("company/" + id);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = "Error al intentar eliminar el registro";
                return View(getIdResultCompanyDTO);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(getIdResultCompanyDTO);
            }
        }
    }
}


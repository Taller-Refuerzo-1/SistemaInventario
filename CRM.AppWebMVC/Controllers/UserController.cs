using CRM.DTOs.CustomerDTOs;
using CRM.DTOs.UsersDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.AppWebMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClientCRMAPI;
        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientCRMAPI = httpClientFactory.CreateClient("CRMAPI");
        }
        // GET: UserController
        public async Task<IActionResult> Index(SearchQueryUsersDTO searchQueryUsers, int CountRow = 0)
        {
            // Configuración de valores por defecto para la búsqueda
            if (searchQueryUsers.SendRowCount == 0)
                searchQueryUsers.SendRowCount = 2;
            if (searchQueryUsers.Take == 0)
                searchQueryUsers.Take = 10;

            var result = new SearchResultUsersDTO();

            // Realizar una solicitud HTTP POST para buscar clientes en el servicio web
            var response = await _httpClientCRMAPI.PostAsJsonAsync("User/search", searchQueryUsers);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<SearchResultUsersDTO>();

            result = result != null ? result : new SearchResultUsersDTO();

            // Configuración de valores para la vista
            if (result.CountRow == 0 && searchQueryUsers.SendRowCount == 1)
                result.CountRow = CountRow;

            ViewBag.CountRow = result.CountRow;
            searchQueryUsers.SendRowCount = 0;
            ViewBag.SearchQuery = searchQueryUsers;

            return View(result);
        }

        // GET: UserController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var result = new GetIdResultUsersDTO();

            // Realizar una solicitud HTTP GET para obtener los detalles del cliente por ID
            var response = await _httpClientCRMAPI.GetAsync("User/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultUsersDTO>();

            return View(result ?? new GetIdResultUsersDTO());
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUsersDTO createUsersDTO)
        {
            try
            {
                // Realizar una solicitud HTTP POST para crear un nuevo cliente
                var response = await _httpClientCRMAPI.PostAsJsonAsync("User", createUsersDTO);

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

        // GET: UserController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = new GetIdResultUsersDTO();
            var response = await _httpClientCRMAPI.GetAsync("User/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultUsersDTO>();

            return View(new EditUsersDTO(result ?? new GetIdResultUsersDTO()));
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditUsersDTO editUsers)
        {
            try
            {
                // Realizar una solicitud HTTP PUT para editar el cliente
                var response = await _httpClientCRMAPI.PutAsJsonAsync("User", editUsers);

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

        // GET: UserController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var result = new GetIdResultUsersDTO();
            var response = await _httpClientCRMAPI.GetAsync("User/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultUsersDTO>();

            return View(result ?? new GetIdResultUsersDTO());
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, GetIdResultCustomerDTO getIdResultCustomerDTO)
        {
            try
            {
                // Realizar una solicitud HTTP DELETE para eliminar el cliente por ID
                var response = await _httpClientCRMAPI.DeleteAsync("User/" + id);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = "Error al intentar eliminar el registro";
                return View(getIdResultCustomerDTO);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(getIdResultCustomerDTO);
            }
        }
    }
}

using CRM.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Mvc;

namespace CRM.AppWebMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClientCRMAPI;

        // Constructor que recibe una instancia de IHttpClientFactory para crear el cliente HTTP
        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientCRMAPI = httpClientFactory.CreateClient("CRMAPI");
        }

        // Método para mostrar la lista de productos
        public async Task<IActionResult> Index(SearchQueryProductDTO searchQueryProductDTO, int CountRow = 0)
        {
            if (searchQueryProductDTO.SendRowCount == 0)
                searchQueryProductDTO.SendRowCount = 2;
            if (searchQueryProductDTO.Take == 0)
                searchQueryProductDTO.Take = 10;

            var result = new SearchResultProductDTO();



            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<SearchResultProductDTO>();

            result = result ?? new SearchResultProductDTO();

            if (result.CountRow == 0 && searchQueryProductDTO.SendRowCount == 1)
                result.CountRow = CountRow;

            ViewBag.CountRow = result.CountRow;
            searchQueryProductDTO.SendRowCount = 0;
            ViewBag.SearchQuery = searchQueryProductDTO;

            return View(result);
        }

        // Método para mostrar los detalles de un producto
        public async Task<IActionResult> Details(int id)
        {
            var result = new GetIdResultProductDTO();

            var response = await _httpClientCRMAPI.GetAsync("product/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultProductDTO>();

            return View(result ?? new GetIdResultProductDTO());
        }

        // Método para mostrar el formulario de creación de un producto
        public ActionResult Create()
        {
            return View();
        }

        // Método para procesar la creación de un producto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductDTO createProductDTO)
        {
            try
            {
                var response = await _httpClientCRMAPI.PostAsJsonAsync("product", createProductDTO);

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

        // Método para mostrar el formulario de edición de un producto
        public async Task<IActionResult> Edit(int id)
        {
            var result = new GetIdResultProductDTO();
            var response = await _httpClientCRMAPI.GetAsync("product/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultProductDTO>();

            return View(new EditProductDTO(result ?? new GetIdResultProductDTO()));
        }

        // Método para procesar la edición de un producto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditProductDTO editProductDTO)
        {
            try
            {
                var response = await _httpClientCRMAPI.PutAsJsonAsync("product", editProductDTO);

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

        // Método para mostrar la página de confirmación de eliminación de un producto
        public async Task<IActionResult> Delete(int id)
        {
            var result = new GetIdResultProductDTO();
            var response = await _httpClientCRMAPI.GetAsync("product/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultProductDTO>();

            return View(result ?? new GetIdResultProductDTO());
        }

        // Método para procesar la eliminación de un producto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, GetIdResultProductDTO getIdResultProductDTO)
        {
            try
            {
                var response = await _httpClientCRMAPI.DeleteAsync("product/" + id);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = "Error al intentar eliminar el registro";
                return View(getIdResultProductDTO);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(getIdResultProductDTO);
            }
        }
    }
}
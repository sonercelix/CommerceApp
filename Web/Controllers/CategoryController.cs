using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Web.Helper;
using Web.Models;

namespace Web.Controllers
{
    [Authorize(Roles = "CategoryManagement")]
    public class CategoryController : Controller
    {

        private ApiHelper apiHelper = new ApiHelper();

        public async Task<IActionResult> Index()
        {
            IEnumerable<CategoryModel> categories = await GetCategories();
            return View(categories);
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<CategoryModel> categories = await GetCategories();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = apiHelper.Initial();
                string token = HttpContext.Request.Cookies["token"];
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var postTask = await client.PostAsJsonAsync<CategoryModel>("Category", categoryModel);
                if (postTask.IsSuccessStatusCode)
                {
                    var readTask = await postTask.Content.ReadAsStringAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, string.Format("StatusCode: {0} Reason: {1}", postTask.StatusCode, postTask.ReasonPhrase));
                }
            }
            else
            {
                IEnumerable<CategoryModel> categories = await GetCategories();
                ViewBag.Categories = categories;
            }
            return View(categoryModel);
        }

        public async Task<IEnumerable<CategoryModel>> GetCategories()
        {
            HttpClient client = apiHelper.Initial();
            string token = HttpContext.Request.Cookies["token"];
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            IEnumerable<CategoryModel> categories = new List<CategoryModel>();
            var responseTask = await client.GetAsync("Category");
            if (responseTask.IsSuccessStatusCode)
            {
                var readTask = await responseTask.Content.ReadAsStringAsync(); // .ReadAsAsync<IList<CategoryModel>>();
                categories = JsonSerializer.Deserialize<IEnumerable<CategoryModel>>(readTask, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });
            }
            else
            {
                ModelState.AddModelError(string.Empty, string.Format("StatusCode: {0} Reason: {1}", responseTask.StatusCode, responseTask.ReasonPhrase));
                categories = Enumerable.Empty<CategoryModel>();
            }
            return categories;
        }

        public async Task<IActionResult> Edit(int id)
        {
            IEnumerable<CategoryModel> categories = await GetCategories();
            ViewBag.Categories = categories;

            HttpClient client = apiHelper.Initial();
            string token = HttpContext.Request.Cookies["token"];
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseTask = await client.GetAsync("Category/" + id.ToString());
            if (responseTask.IsSuccessStatusCode)
            {
                var readTask = await responseTask.Content.ReadAsStringAsync(); // .ReadAsAsync<IList<CategoryModel>>();
                CategoryModel category = JsonSerializer.Deserialize<CategoryModel>(readTask, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });
                return View(category);
            }
            else
            {
                ModelState.AddModelError(string.Empty, string.Format("StatusCode: {0} Reason: {1}", responseTask.StatusCode, responseTask.ReasonPhrase));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = apiHelper.Initial();
                string token = HttpContext.Request.Cookies["token"];
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var responseTask = await client.PutAsJsonAsync<CategoryModel>("Category", categoryModel);
                if (responseTask.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, string.Format("StatusCode: {0} Reason: {1}", responseTask.StatusCode, responseTask.ReasonPhrase));
                }
            }

            IEnumerable<CategoryModel> categories = await GetCategories();
            ViewBag.Categories = categories;
            return View(categoryModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            HttpClient client = apiHelper.Initial();
            string token = HttpContext.Request.Cookies["token"];
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseTask = await client.DeleteAsync("Category/" + id.ToString());
            if (responseTask.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, string.Format("StatusCode: {0} Reason: {1}", responseTask.StatusCode, responseTask.ReasonPhrase));
            }
            return RedirectToAction("Index");
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Web.Helper;
using Web.Models;

namespace Web.Controllers
{
    [Authorize(Roles = "product_view")]
    public class ProductController : Controller
    {
        private ApiHelper apiHelper = new ApiHelper();

        public async Task<IActionResult> Index()
        {
            IEnumerable<ProductModel> productList = await GetProducts();
            return View(productList);
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<CategoryModel> categories = await GetCategories();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductModel productModel, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    productModel.ImageUrl = file.FileName;
                }

                HttpClient client = apiHelper.Initial();
                string token = HttpContext.Request.Cookies["token"];
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var postTask = await client.PostAsJsonAsync<ProductModel>("Product", productModel);
                if (postTask.IsSuccessStatusCode)
                {
                    var readTask = await postTask.Content.ReadAsStringAsync();
                    return RedirectToAction("Index");
                }
                else //web api sent error response
                {
                    ModelState.AddModelError(string.Empty, string.Format("StatusCode: {0} Reason: {1}", postTask.StatusCode, postTask.ReasonPhrase));
                }
            }

            IEnumerable<CategoryModel> categories = await GetCategories();
            ViewBag.Categories = categories;
            return View(productModel);
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

        private async Task<IEnumerable<ProductModel>> GetProducts()
        {
            IEnumerable<ProductModel> productList = new List<ProductModel>();

            HttpClient client = apiHelper.Initial();
            string token = HttpContext.Request.Cookies["token"];
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseTask = await client.GetAsync("Product");
            if (responseTask.IsSuccessStatusCode)
            {
                var readTask = await responseTask.Content.ReadAsStringAsync();
                productList = JsonSerializer.Deserialize<IEnumerable<ProductModel>>(readTask, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });
            }
            else
            {
                ModelState.AddModelError(string.Empty, string.Format("StatusCode: {0} Reason: {1}", responseTask.StatusCode, responseTask.ReasonPhrase));
                productList = Enumerable.Empty<ProductModel>();
            }
            return productList;
        }

        public async Task<IActionResult> Delete(int id)
        {
            HttpClient client = apiHelper.Initial();
            string token = HttpContext.Request.Cookies["token"];
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseTask = await client.DeleteAsync("Product/" + id.ToString());
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

        public async Task<IActionResult> Detail(int id)
        {
            HttpClient client = apiHelper.Initial();
            string token = HttpContext.Request.Cookies["token"];
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseTask = await client.GetAsync("Product/" + id.ToString());
            if (responseTask.IsSuccessStatusCode)
            {
                var readTask = await responseTask.Content.ReadAsStringAsync();
                ProductModel product = JsonSerializer.Deserialize<ProductModel>(readTask, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });
                return View(product);
            }
            else
            {
                ModelState.AddModelError(string.Empty, string.Format("StatusCode: {0} Reason: {1}", responseTask.StatusCode, responseTask.ReasonPhrase));
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            IEnumerable<CategoryModel> categories = await GetCategories();
            ViewBag.Categories = categories;

            HttpClient client = apiHelper.Initial();
            string token = HttpContext.Request.Cookies["token"];
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseTask = await client.GetAsync("Product/" + id.ToString());
            if (responseTask.IsSuccessStatusCode)
            {
                var readTask = await responseTask.Content.ReadAsStringAsync();
                ProductModel product = JsonSerializer.Deserialize<ProductModel>(readTask, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });
                return View(product);
            }
            else
            {
                ModelState.AddModelError(string.Empty, string.Format("StatusCode: {0} Reason: {1}", responseTask.StatusCode, responseTask.ReasonPhrase));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductModel productModel, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    productModel.ImageUrl = file.FileName;
                }

                HttpClient client = apiHelper.Initial();
                string token = HttpContext.Request.Cookies["token"];
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var responseTask = await client.PutAsJsonAsync<ProductModel>("Product", productModel);
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
            return View(productModel);
        }
    }
}
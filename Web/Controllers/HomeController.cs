using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.Helper;
using Web.Models;

namespace Web.Controllers
{
    [Authorize(Roles = "Normal,product_view,CategoryManagement")]
    public class HomeController : Controller
    {
        private ApiHelper apiHelper = new ApiHelper();

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            string token = string.Empty;
            string message = string.Empty;
            ActionResult result;
            HttpClient client = apiHelper.Initial();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string serailizeddto = JsonConvert.SerializeObject(loginModel);
            var inputMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                Content = new StringContent(serailizeddto, Encoding.UTF8, "application/json")
            };
            HttpResponseMessage responseMessage = await client.PostAsync("/api/authenticate/login", inputMessage.Content);
            var responseJson = await responseMessage.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseJson);

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                token = jObject.GetValue("token").ToString();
                message = jObject.GetValue("message").ToString();
                string roles = jObject.GetValue("roles").ToString();
                string[] rolList = roles.Split(",");

                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, loginModel.UserName, ClaimValueTypes.String));
                claims.Add(new Claim("Token", token, ClaimValueTypes.String));
                foreach (var item in rolList)
                {
                    claims.Add(new Claim(ClaimTypes.Role, item, ClaimValueTypes.String));
                }
                var userIdentity = new ClaimsIdentity("UserIdentity");
                userIdentity.AddClaims(claims);
                var userPrincipal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                  userPrincipal,
                  new AuthenticationProperties
                  {
                      ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                      IsPersistent = false,
                      AllowRefresh = false
                  });

                CookieOptions cookie = new CookieOptions();
                cookie.Expires = DateTime.Now.AddYears(10);
                Response.Cookies.Append("token", token, cookie);

                result = RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = jObject.GetValue("message").ToString();
                result = View(loginModel);
            }

            return result;
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
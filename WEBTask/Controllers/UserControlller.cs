using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using WEBTask.Models;
using WEBTask.Models.DTO;

namespace WEBTask.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("https://localhost:5001/api/Users/register", model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }

            ModelState.AddModelError(string.Empty, "Registration failed.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("https://localhost:5001/api/Users/login", model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Task");
            }

            ModelState.AddModelError(string.Empty, "Login failed.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync("https://localhost:5001/api/Users/logout", null);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Logout failed");
            return RedirectToAction("Index", "Home");
        }
    }

}

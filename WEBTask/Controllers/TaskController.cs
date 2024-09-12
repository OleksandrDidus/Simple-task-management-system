using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WEBTask.Models;
using System.Net.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WEBTask.Models.DTO;
using System.Reflection;
namespace WEBTask.Controllers
{
    public class TaskController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TaskController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync("https://localhost:5001/api/Tasks");

            if (response.IsSuccessStatusCode)
            {
                var tasks = await response.Content.ReadFromJsonAsync<List<TaskDto>>();
                return View(tasks);
            }

            ModelState.AddModelError(string.Empty, "Failed to load tasks.");
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskDto model)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("https://localhost:5001/api/Tasks/addtask", model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to create task.");
            return View(model);
        }
        public async Task<IActionResult> Update(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync($"https://localhost:5001/api/Tasks/{id}");

            if (response.IsSuccessStatusCode)
            {
                var tasks = await response.Content.ReadFromJsonAsync<TaskDto>();
                return View(tasks);
            }

            ModelState.AddModelError(string.Empty, "Failed to load tasks.");
            return RedirectToAction("Login", "User");
        }
        [HttpPost]
        public async Task<IActionResult> Update(Guid id, CreateTaskDto model)
        {

            var client = _httpClientFactory.CreateClient();
            var response = await client.PutAsJsonAsync($"https://localhost:5001/api/Tasks/{id}", model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to update task.");
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:5001/api/Tasks/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to delete task.");
            return RedirectToAction("Index");
        }
    }


}

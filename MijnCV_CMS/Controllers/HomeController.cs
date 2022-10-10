using Microsoft.AspNetCore.Mvc;
using MijnCV_CMS.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace MijnCV_CMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {

            List<Section> sections = new List<Section>();
            List<Page> pages = new List<Page>();
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("https://localhost:7059/api/Sections");
                string apiResponse = await response.Content.ReadAsStringAsync();
                sections = JsonConvert.DeserializeObject<List<Section>>(apiResponse);

                response = await httpClient.GetAsync("https://localhost:7059/api/Pages");
                apiResponse = await response.Content.ReadAsStringAsync();
                pages = JsonConvert.DeserializeObject<List<Page>>(apiResponse);
            }
            catch (Exception)
            {
                return RedirectToAction("Offline", "Error");
            }
            ViewData["Sections"] = sections;
            ViewData["Pages"] = pages;
            return View();
        }

        public async Task<IActionResult> Add()
        {
            List<Page> pages = new List<Page>();
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("https://localhost:7059/api/Pages");
                string apiResponse = await response.Content.ReadAsStringAsync();
                pages = JsonConvert.DeserializeObject<List<Page>>(apiResponse);
            }
            catch (Exception)
            {
                return RedirectToAction("Offline", "Error");
            }
            ViewData["Pages"] = pages;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([Bind("CV,Title,Paragraph,Image,Layout,Position,PageID")] Section section)
        {
            try
            {
                var httpClient = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(section), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://localhost:7059/api/Sections", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string Id)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.DeleteAsync("https://localhost:7059/api/Sections/" + Id);
                string apiResponse = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
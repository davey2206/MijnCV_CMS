using Microsoft.AspNetCore.Mvc;
using MijnCV_CMS.Models;
using Newtonsoft.Json;
using System.Text;

namespace MijnCV_CMS.Controllers
{
    public class PageController : Controller
    {
        public async Task<IActionResult> Index()
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

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([Bind("Name, UserID")] Page page)
        {

            try
            {
                var httpClient = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(page), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://localhost:7059/api/Pages", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Index", "Page");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string Id)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.DeleteAsync("https://localhost:7059/api/Pages/" + Id);
                string apiResponse = await response.Content.ReadAsStringAsync(); ;
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Index", "Page");
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MijnCV_CMS.Models;
using Newtonsoft.Json;
using System.Text;

namespace MijnCV_CMS.Controllers
{
    public class PageController : Controller
    {
        [Authorize]
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

        [Authorize]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
        public async Task<IActionResult> EditAsync(string id)
        {
            Page page = new Page();
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("https://localhost:7059/api/Pages/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                page = JsonConvert.DeserializeObject<Page>(apiResponse);
            }
            catch (Exception)
            {
                return RedirectToAction("Offline", "Error");
            }
            ViewBag.Page = page;
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditAsync([Bind("Id,Name,UserID")] Page page)
        {
            try
            {
                var httpClient = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(page), Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync("https://localhost:7059/api/Pages/" + page.Id, content);
                string apiResponse = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Index", "Page");
        }
    }
}
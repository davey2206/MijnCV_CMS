using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MijnCV_CMS.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;

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
                string name = User.Identity.Name;
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("https://localhost:7236/api/Users");
                string apiResponse = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<List<User>>(apiResponse);
                string cvName = user.Where(u => u.Email == name).First().CV;

                response = await httpClient.GetAsync("https://localhost:7059/api/Pages/CV/" + cvName);
                apiResponse = await response.Content.ReadAsStringAsync();
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
        public async Task<IActionResult> AddAsync()
        {
            string cvName;
            try
            {
                string name = User.Identity.Name;
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("https://localhost:7236/api/Users");
                string apiResponse = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<List<User>>(apiResponse);
                cvName = user.Where(u => u.Email == name).First().CV;
            }
            catch (Exception)
            {
                return RedirectToAction("Offline", "Error");
            }

            ViewData["CV"] = cvName;
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddAsync([Bind("Name, cv")] Page page)
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
        public async Task<IActionResult> EditAsync([Bind("Id,Name,CV")] Page page)
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
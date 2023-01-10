using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MijnCV_CMS.Models;
using Newtonsoft.Json;

namespace MijnCV_CMS.Controllers
{
    public class StatsController : Controller
    {
        [Authorize]
        public async Task<IActionResult> IndexAsync()
        {
            Statistics statistic = new Statistics();
            try
            {
                string name = User.Identity.Name;
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("https://localhost:7236/api/Users");
                string apiResponse = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<List<User>>(apiResponse);
                string cvName = user.Where(u => u.Email == name).First().CV;

                response = await httpClient.GetAsync("https://localhost:7236/api/Statistics/" + cvName);
                apiResponse = await response.Content.ReadAsStringAsync();
                statistic = JsonConvert.DeserializeObject<Statistics>(apiResponse);
            }
            catch (Exception)
            {
                return RedirectToAction("Offline", "Error");
            }

            //add new viewbag when adding new Statistic
            ViewBag.Views = statistic.Views;
            return View();
        }
    }
}

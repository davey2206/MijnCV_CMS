using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MijnCV_CMS.Models;
using Newtonsoft.Json;
using NuGet.Protocol;
using static System.Collections.Specialized.BitVector32;
using System.Text;

namespace MijnCV_CMS.Controllers
{
    public class AccountController : Controller
    {
        [Authorize]
        public async Task<IActionResult> IndexAsync()
        {
            var user = User.Identity.Name;

            List<User> users = new List<User>();
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("https://localhost:7236/api/Users");
                string apiResponse = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<User>>(apiResponse);
            }
            catch (Exception)
            {
                return RedirectToAction("Offline", "Error");
            }

            if (users.Any(u => u.Email == user))
            {
                TempData["Ready"] = "Ready";
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([Bind("Email,CV")] User user)
        {
            try
            {
                var httpClient = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://localhost:7236/api/Users", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                return RedirectToAction("Offline", "Error");
            }

            TempData["Ready"] = "Ready";
            return RedirectToAction("Index", "Home");
        }
    }
}

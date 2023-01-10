using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MijnCV_CMS.Models;
using Newtonsoft.Json;
using NuGet.Protocol;
using static System.Collections.Specialized.BitVector32;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace MijnCV_CMS.Controllers
{
    public class AccountController : Controller
    {
        public const string SessionKeyReady = "_Ready";

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
                HttpContext.Session.SetInt32(SessionKeyReady, 1);
                return RedirectToAction("Index", "Home");
            }
            HttpContext.Session.Remove(SessionKeyReady);
            HttpContext.Session.Clear();
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

            HttpContext.Session.SetInt32(SessionKeyReady, 1);
            return RedirectToAction("Index", "Home");
        }
    }
}

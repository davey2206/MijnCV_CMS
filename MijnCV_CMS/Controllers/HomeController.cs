using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MijnCV_CMS.Models;
using Newtonsoft.Json;

namespace MijnCV_CMS.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        if (TempData.Peek("Ready") as string != "Ready")
        {
            return RedirectToAction("Index", "Account");
        }

        List<Section> sections = new List<Section>();
        List<Page> pages = new List<Page>();
        try
        {
            using (var httpClient = new HttpClient())
            {
                string name = User.Identity.Name;
                using (var response = await httpClient.GetAsync("https://localhost:7059/api/Sections/CV/" + name))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    sections = JsonConvert.DeserializeObject<List<Section>>(apiResponse);
                }

                using (var response = await httpClient.GetAsync("https://localhost:7059/api/Pages/CV/" + name))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    pages = JsonConvert.DeserializeObject<List<Page>>(apiResponse);
                }
            }
        }
        catch (Exception)
        {
            return RedirectToAction("Offline", "Error");
        }

        ViewData["Sections"] = sections;
        ViewData["Pages"] = pages;
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Add()
    {
        List<Page> pages = new List<Page>();
        try
        {
            using (var httpClient = new HttpClient())
            {
                string name = User.Identity.Name;
                using (var response = await httpClient.GetAsync("https://localhost:7059/api/Pages/CV/" + name))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    pages = JsonConvert.DeserializeObject<List<Page>>(apiResponse);
                }
            }
        }
        catch (Exception)
        {
            return RedirectToAction("Offline", "Error");
        }

        ViewData["Pages"] = pages;

        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddAsync([Bind("CV,Title,Paragraph,Image,Layout,Position,PageID")] Section section)
    {
        try
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(section), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://localhost:7059/api/Sections", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception)
        {
            return RedirectToAction("Offline", "Error");
        }

        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> DeleteAsync(string Id)
    {
        try
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7059/api/Sections/" + Id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
        }
        catch (Exception)
        {
            return RedirectToAction("Offline", "Error");
        }


        return RedirectToAction("Index", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

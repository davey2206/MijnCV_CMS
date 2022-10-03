﻿using Microsoft.AspNetCore.Mvc;
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
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7059/api/Sections"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    sections = JsonConvert.DeserializeObject<List<Section>>(apiResponse);
                }
            }
            ViewData["Sections"] = sections;
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([Bind("CV,Title,Paragraph,Image,Layout,Position,PageID")] Section section)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(section), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:7059/api/Sections", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
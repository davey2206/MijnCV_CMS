using Microsoft.AspNetCore.Mvc;

namespace MijnCV_CMS.Controllers
{
    public class PageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

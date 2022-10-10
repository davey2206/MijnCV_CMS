using Microsoft.AspNetCore.Mvc;

namespace MijnCV_CMS.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Offline()
        {
            return View();
        }
    }
}

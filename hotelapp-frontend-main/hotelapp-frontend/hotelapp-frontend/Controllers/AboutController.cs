using Microsoft.AspNetCore.Mvc;

namespace hotelapp_frontend.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}

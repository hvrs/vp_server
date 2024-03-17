using Microsoft.AspNetCore.Mvc;

namespace vp_server.Controllers
{
    public class DocumentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

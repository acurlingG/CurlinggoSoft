using Microsoft.AspNetCore.Mvc;

namespace CurlinggoSoft.Controllers
{
    public class AdminController : Controller
    {
        // GET: /Admin
        public IActionResult Index()
        {
            return View();
        }
    }
}
using Microsoft.AspNetCore.Mvc;

namespace MySocialPet.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

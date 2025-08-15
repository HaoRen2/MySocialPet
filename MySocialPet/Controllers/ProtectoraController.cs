using Microsoft.AspNetCore.Mvc;

namespace MySocialPet.Controllers
{
    public class ProtectoraController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

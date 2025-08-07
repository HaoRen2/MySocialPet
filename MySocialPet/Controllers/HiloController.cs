using Microsoft.AspNetCore.Mvc;

namespace MySocialPet.Controllers
{
    public class HiloController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

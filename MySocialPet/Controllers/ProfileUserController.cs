using Microsoft.AspNetCore.Mvc;

namespace MySocialPet.Controllers
{
    public class ProfileUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

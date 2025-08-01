using Microsoft.AspNetCore.Mvc;

namespace MySocialPet.Controllers
{
    public class ProfilePetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

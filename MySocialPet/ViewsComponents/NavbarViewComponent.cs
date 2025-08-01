using Microsoft.AspNetCore.Mvc;

namespace MySocialPet.ViewsComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var isAuthenticated = User.Identity.IsAuthenticated;
            var username = isAuthenticated ? User.Identity.Name : null;

            ViewBag.IsAuthenticated = isAuthenticated;
            ViewBag.Username = username;

            return View();
        }
    }
}

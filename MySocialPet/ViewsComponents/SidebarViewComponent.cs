using Microsoft.AspNetCore.Mvc;

namespace MySocialPet.ViewsComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}

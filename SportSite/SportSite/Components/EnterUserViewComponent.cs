using Microsoft.AspNetCore.Mvc;

namespace SportSite.Components
{
    public class EnterUserViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

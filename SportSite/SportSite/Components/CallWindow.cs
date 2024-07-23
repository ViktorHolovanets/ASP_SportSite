using Microsoft.AspNetCore.Mvc;

namespace SportSite.Components
{
    public class CallWindow: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

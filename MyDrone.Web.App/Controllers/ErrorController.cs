using Microsoft.AspNetCore.Mvc;

namespace MyDrone.Web.App.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // AccessDenied sayfasını gösterecek action
        public IActionResult AccessDenied()
        {
            return View(); // Bu sayfa için bir View oluşturabilirsiniz
        }
    }
}

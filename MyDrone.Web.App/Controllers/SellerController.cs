using Microsoft.AspNetCore.Mvc;

namespace MyDrone.Web.App.Controllers
{
    public class SellerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

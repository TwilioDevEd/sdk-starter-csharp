using Microsoft.AspNetCore.Mvc;

namespace TwilioSdkStarterDotnetCore.Web.Controllers
{
    public class VideoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
using Microsoft.AspNetCore.Mvc;

namespace TwilioSdkStarterDotnetCore.Web.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
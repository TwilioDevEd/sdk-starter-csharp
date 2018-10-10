using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace sdk_starter_dotnet_core.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("~/public/index.html");
        }

        public ActionResult Video()
        {
            return View("~/public/video/index.html");
        }

        public ActionResult Chat()
        {
            return View("~/public/chat/index.html");
        }

        public ActionResult Notify()
        {
            return View("~/public/notify/index.html");
        }

        public ActionResult Sync()
        {
            return View("~/public/sync/index.html");
        }

        public ActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}

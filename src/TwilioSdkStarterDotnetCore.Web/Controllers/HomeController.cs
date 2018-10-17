using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TwilioSdkStarterDotnetCore.Web.Models;

namespace TwilioSdkStarterDotnetCore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly TwilioAccount _twilioAccount;
        public HomeController(IOptions<TwilioAccount> twilioAccount)
        {
            _twilioAccount = twilioAccount.Value ?? throw new ArgumentException(nameof(twilioAccount));
        }
        public IActionResult Index()
        {
            // never publicly show your credentials. This is purely to confirm correct setup
            return View(_twilioAccount);
        }

        
    }
}

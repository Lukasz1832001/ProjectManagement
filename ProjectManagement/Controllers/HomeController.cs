using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Models;
using System.Diagnostics;

namespace ProjectManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _user;

        public HomeController(ILogger<HomeController> logger, UserManager<User> user)
        {
            _logger = logger;
            _user = user;
        }

        public IActionResult Index()
        {
            ViewBag.User = _user.GetUserId(HttpContext.User);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
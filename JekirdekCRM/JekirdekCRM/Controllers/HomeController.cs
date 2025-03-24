using System.Diagnostics;
using System.Threading.Tasks;
using JekirdekCRM.Helpers;
using JekirdekCRM.Models;
using JekirdekCRM.Models.DBModels;
using JekirdekCRM.Models.ViewModels;
using JekirdekCRM.Services;
using Microsoft.AspNetCore.Mvc;

namespace JekirdekCRM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;
        private readonly ILogService _logService;

        public HomeController(ILogger<HomeController> logger, IUserService userService, ILogService logService, ICustomerService customerService)
        {
            _logger = logger;
            _userService = userService;
            _logService = logService;
            _customerService = customerService;
        }

        public IActionResult Index()
        {
            var user = SessionHelper.FindUser(HttpContext);
            if (user == null)
                return RedirectToAction("Login", "Home");

            var model = _customerService.GetCustomerDashboardData();
            return View(model);
        }


        public IActionResult Login()
        {
            var user = SessionHelper.FindUser(HttpContext);
            if (user != null)
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userService.GetUserByUsername(model.Username);
            if (user == null || !_userService.VerifyPassword(user, model.Password))
            {
                ModelState.AddModelError("Error", "Giri� bilgileri hatal�, l�tfen tekrar deneyiniz!");
                await _logService.CreateLogAsync(LogTags.Error, $"{model.Username} kullan�c� ad� ile hatal� giri� yap�ld�.");
                return View(model);
            }

            SessionHelper.SetUserSession(HttpContext, user);
            await _logService.CreateLogAsync(LogTags.Login, "Ba�ar�l� giri� yap�ld�.", user.Id);
            return RedirectToAction("Index");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

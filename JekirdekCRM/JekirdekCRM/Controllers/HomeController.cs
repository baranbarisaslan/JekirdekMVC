using System.Diagnostics;
using JekirdekCRM.Helpers;
using JekirdekCRM.Models;
using JekirdekCRM.Models.DBModels;
using JekirdekCRM.Models.ViewModels;
using JekirdekCRM.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace JekirdekCRM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly DatabaseContext context;
        private readonly HomeRepository homeRepository;

        public HomeController(ILogger<HomeController> _logger, DatabaseContext _context, HomeRepository _homeRepository)
        {
            logger = _logger;
            context = _context;
            homeRepository = _homeRepository;
        }

        public IActionResult Index()
        {
            homeRepository.CreateUser("Jekirdek_2", "123123", "+905397113999", "User");
            User? user = SessionHelper.FindUser(HttpContext);

            if (user == null)
                return RedirectToAction("Login");


            var model = new IndexViewModel
            {
                User = user,
            };
            SessionHelper.SetUserSessionAndViewBag(HttpContext, user, ViewData);
            return View(model);
        }

        public IActionResult Login()
        {
            User? user = SessionHelper.FindUser(HttpContext);
            if (user != null)
            {
                if (context.Users.Contains(user))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                User? user = context.Users.Where(a => a.Username == model.Username).FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError("Error", "Giriþ bilgileri hatalý, lütfen tekrar deneyiniz!");
                    string msg = model.Username + " kullanýcý adý ile hatalý giriþ yapýldý.";
                    homeRepository.CreateLog(LogTags.Error, msg);
                    return View(model);
                }
                else
                {
                    string pwd = ManageHashing.SHA256(model.Password, user.Salt);
                    if(user.Password == pwd)
                    {
                        SessionHelper.SetUserSessionAndViewBag(HttpContext, user, ViewData);
                        homeRepository.CreateLog(LogTags.Login, user.Id);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Giriþ bilgileri hatalý, lütfen tekrar deneyiniz!");
                        string msg = model.Username + " kullanýcý adý ile hatalý giriþ yapýldý.";
                        homeRepository.CreateLog(LogTags.Error, msg);
                        return View(model);
                    }
                }
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
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

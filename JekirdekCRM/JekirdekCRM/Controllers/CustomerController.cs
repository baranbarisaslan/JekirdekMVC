using JekirdekCRM.Helpers;
using JekirdekCRM.Models.DBModels;
using JekirdekCRM.Models.ViewModels;
using JekirdekCRM.Services;
using Microsoft.AspNetCore.Mvc;

namespace JekirdekCRM.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;
        private readonly ILogService _logService;

        public CustomerController(
            ILogger<CustomerController> logger,
            ICustomerService customerService,
            ILogService logService)
        {
            _logger = logger;
            _customerService = customerService;
            _logService = logService;
        }

        public IActionResult AddCustomer()
        {
            var user = SessionHelper.FindUser(HttpContext);
            if (user == null)
                return RedirectToAction("Login", "Home");

            if (user.Role != "Admin")
            {
                _logService.CreateLogAsync(LogTags.Error, $"{user.Username} müşteri ekleme bölümüne erişmeye çalıştı!");
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult AddCustomer(CustomerViewModel model)
        {
            try
            {
                var user = SessionHelper.FindUser(HttpContext);
                if (user == null)
                    return RedirectToAction("Login", "Home");

                if (user.Role != "Admin")
                {
                    _logService.CreateLogAsync(LogTags.Error, $"{user.Username} müşteri ekleme bölümüne erişmeye çalıştı!");
                    return RedirectToAction("Index", "Home");
                }

                if (!ModelState.IsValid)
                    return View(model);

                var result = _customerService.AddCustomer(model);

                if (!result.Success)
                {
                    ModelState.AddModelError("Error", result.Message);
                    return View(model);
                }

                _logService.CreateLogAsync(LogTags.DatabaseAction, $"Yeni müşteri eklendi: {model.Email}", user.Id);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logService.CreateLogAsync(LogTags.Exception, $"AddCustomer - {ex.Message}");
                return RedirectToAction("Error", "Home");
            }

        }


        public IActionResult ListCustomers(string? search = "", int page = 1)
        {
            var user = SessionHelper.FindUser(HttpContext);
            if (user == null)
                return RedirectToAction("Login", "Home");

            int pageSize = 10;

            var (customers, totalCount) = _customerService.ListCustomers(search, page, pageSize);

            var model = new ListCustomersViewModel
            {
                SearchText = search,
                Customers = customers,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                PageSize = pageSize
            };

            return View(model);
        }
    }
}

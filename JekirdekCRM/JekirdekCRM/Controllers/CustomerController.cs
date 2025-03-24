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
                return RedirectToAction("ListCustomers", "Customer");
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

        public IActionResult DeleteCustomer(int id)
        {
            var user = SessionHelper.FindUser(HttpContext);
            if (user == null)
                return RedirectToAction("Login", "Home");

            if (user.Role != "Admin")
            {
                _logService.CreateLogAsync(LogTags.Error, $"{user.Username} müşteri silmeye çalıştı!", id);
                return RedirectToAction("ListCustomers", "Customer");
            }

            var result = _customerService.DeleteCustomer(id);
            if (!result.Success)
            {
                _logService.CreateLogAsync(LogTags.Error, $"{user.Username} müşteri silme işlemi gerçekleştirilemedi!");
            }
            return RedirectToAction("ListCustomers");
        }


        [HttpGet]
        public IActionResult EditCustomer(int id)
        {
            var user = SessionHelper.FindUser(HttpContext);
            if (user == null)
                return RedirectToAction("Login", "Home");

            if (user.Role != "Admin")
            {
                _logService.CreateLogAsync(LogTags.Error, $"{user.Username} müşteri düzenlemeye çalıştı!", id);
                return RedirectToAction("ListCustomers", "Customer");
            }

            var customer = _customerService.GetCustomer(id);
            if (customer == null)
                return RedirectToAction("ListCustomers");

            return View(customer);
        }

        [HttpPost]
        public IActionResult EditCustomer(int id, CustomerViewModel model)
        {
            var user = SessionHelper.FindUser(HttpContext);
            if (user == null)
                return RedirectToAction("Login", "Home");

            if (!ModelState.IsValid)
                return View(model);

            var result = _customerService.EditCustomer(id, model);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            return RedirectToAction("ListCustomers");
        }


    }
}

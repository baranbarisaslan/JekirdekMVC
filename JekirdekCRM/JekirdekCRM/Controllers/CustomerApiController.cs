using JekirdekCRM.Models.ViewModels;
using JekirdekCRM.Services;
using Microsoft.AspNetCore.Mvc;

namespace JekirdekCRM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerApiController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerApiController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult GetAllCustomers([FromQuery] string? search, int page = 1, int pageSize = 10)
        {
            var (customers, totalCount) = _customerService.ListCustomers(search, page, pageSize);
            return Ok(new { data = customers, total = totalCount });
        }

        [HttpPost]
        public IActionResult CreateCustomer([FromBody] CustomerViewModel model)
        {
            var result = _customerService.AddCustomer(model);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, [FromBody] CustomerViewModel model)
        {
            var result = _customerService.EditCustomer(id, model);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            var result = _customerService.DeleteCustomer(id);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }

}

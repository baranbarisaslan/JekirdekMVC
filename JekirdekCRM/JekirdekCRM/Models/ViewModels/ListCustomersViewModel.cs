using JekirdekCRM.Models.DBModels;

namespace JekirdekCRM.Models.ViewModels
{
    public class ListCustomersViewModel
    {
        public string? SearchText { get; set; }

        public List<Customer> Customers { get; set; } = new();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}

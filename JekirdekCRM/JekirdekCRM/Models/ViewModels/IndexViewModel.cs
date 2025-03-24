using JekirdekCRM.Models.DBModels;

namespace JekirdekCRM.Models.ViewModels
{
    public class IndexViewModel
    {
        public int TotalCustomerCount { get; set; }
        public int TodayCustomerCount { get; set; }
        public int ThisMonthCustomerCount { get; set; }

        public List<RegionCount> CustomersByRegion { get; set; } = new();
        public List<MonthlyCustomerCount> CustomersByMonth { get; set; } = new();
    }


    public class RegionCount
    {
        public string Region { get; set; }
        public int Count { get; set; }
    }

    public class MonthlyCustomerCount
    {
        public string Month { get; set; } // örn: "2024-11"
        public int Count { get; set; }
    }
}

namespace IceCreamSalesReport.Models
{
    public class MonthlyPopularItemStats
    {
        public string Month { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
        public int MinimumOrder { get; set; }
        public int MaximumOrder { get; set; }
        public double AverageOrder { get; set; }
    }
}

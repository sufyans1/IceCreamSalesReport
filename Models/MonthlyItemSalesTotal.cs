namespace IceCreamSalesReport.Models
{
    public class MonthlyItemSalesTotal
    {
        public string Month { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public decimal TotalSales { get; set; }
    }
}

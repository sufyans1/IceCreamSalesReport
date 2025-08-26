namespace IceCreamSalesReport.Models
{
    public class SalesRecord
    {
        public DateTime Date { get; set; }
        public string SKU { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}

namespace IceCreamSalesReport.Services
{
    public class ReportPrinterService
    {
        public static async Task PrintReportsAsync(string filePath)
        {
            var salesService = new IceCreamSalesReportService(filePath);

            // Total Sales
            var totalSales = await salesService.GetTotalSalesAsync();
            Console.WriteLine("==== Total Sales ===");
            Console.WriteLine("Total Sales: " + totalSales.ToString());
            Console.WriteLine();



            // Monthly Totals
            var monthlyTotals = await salesService.GetMonthlySalesTotalAsync();
            Console.WriteLine("==== Monthly Sales Totals ====");
            foreach (var m in monthlyTotals)
            {
                Console.WriteLine($"{Convert.ToDateTime(m.Month).ToString("MMM yyyy")} : {m.TotalSales}");
            }
            Console.WriteLine();

            // Top item per month
            var topItems = await salesService.GetMonthlyTopItemSalesTotalAsync();
            Console.WriteLine("==== Monthly Top Items ====");
            foreach (var t in topItems)
            {
                Console.WriteLine($"{Convert.ToDateTime(t.Month).ToString("MMM yyyy")}: {t.Sku} sold {t.TotalSales} units");
            }
            Console.WriteLine();



            // Stats for top item of the month
            var popularItemStats = await salesService.GetMonthlyPopularItemStats();
            Console.WriteLine("==== Popular Item Stats ====");
            foreach (var s in popularItemStats)
            {
                Console.WriteLine(@$"{Convert.ToDateTime(s.Month).ToString("MMM yyyy")} : SKU={s.Sku},  Min={s.MinimumOrder},  Max={s.MaximumOrder}, Total={s.TotalQuantity}, Avg={s.AverageOrder:0.00}");
            }

            Console.WriteLine();
        }
    }
}


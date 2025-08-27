using IceCreamSalesReport.Models;

namespace IceCreamSalesReport.Services
{
    public class IceCreamSalesReportService
    {
        private readonly string _filePath;
        public IceCreamSalesReportService(string filePath)
        {
            _filePath = filePath;
        }



        public async Task<decimal> GetTotalSalesAsync()
        {
            var salesRecords = await GetSalesRecordsAsync();
            decimal total = 0;
            foreach (var salesRecord in salesRecords)
            {
                total += salesRecord.TotalPrice;
            }
            return total;
        }

        public async Task<List<MonthlyItemSalesTotal>> GetMonthlyTopItemSalesTotalAsync()
        {
            var monthlyItemSalesTotal = new Dictionary<string, Dictionary<string, decimal>>();
            var salesRecords = await GetSalesRecordsAsync();

            foreach (var sale in salesRecords)
            {
                string monthKey = sale.Date.ToString("yyyy-MM");

                if (!monthlyItemSalesTotal.ContainsKey(monthKey))
                    monthlyItemSalesTotal[monthKey] = new Dictionary<string, decimal>();

                if (!monthlyItemSalesTotal[monthKey].ContainsKey(sale.SKU))
                    monthlyItemSalesTotal[monthKey][sale.SKU] = 0;

                monthlyItemSalesTotal[monthKey][sale.SKU] += sale.TotalPrice;
            }

            var monthlyTopItemSalesTotal = new List<MonthlyItemSalesTotal>();

            foreach (var month in monthlyItemSalesTotal.Keys)
            {
                var items = monthlyItemSalesTotal[month];

                string topSku = string.Empty;
                decimal topValue = decimal.MinValue;

                foreach (var item in items)
                {
                    if (item.Value > topValue)
                    {
                        topValue = item.Value;
                        topSku = item.Key;
                    }
                }


                monthlyTopItemSalesTotal.Add(new MonthlyItemSalesTotal
                {
                    Month = month,
                    Sku = topSku,
                    TotalSales = topValue
                });
            }

            return monthlyTopItemSalesTotal;
        }

        public async Task<List<MonthlySalesTotal>> GetMonthlySalesTotalAsync()
        {
            var monthWiseTotals = new Dictionary<string, decimal>();
            var salesRecords = await GetSalesRecordsAsync();

            foreach (var sale in salesRecords)
            {
                // assuming that the sales data might contains diffrent year, currently its only 2019
                string monthKey = sale.Date.ToString("yyyy-MM");

                if (!monthWiseTotals.ContainsKey(monthKey))
                    monthWiseTotals[monthKey] = 0;

                monthWiseTotals[monthKey] += sale.TotalPrice;
            }

            return monthWiseTotals.Select(x => new MonthlySalesTotal
            {
                Month = x.Key,
                TotalSales = x.Value

            }).ToList();
        }

        public async Task<List<SalesRecord>> GetSalesRecordsAsync()
        {
            var sales = new List<SalesRecord>();

            if (!File.Exists(_filePath))
                throw new FileNotFoundException("Invalid file path", _filePath);

            var lines = await File.ReadAllLinesAsync(_filePath);

            for (int i = 1; i < lines.Length; i++)
            {
                var parts = lines[i].Split(',');

                if (parts.Length < 5) continue;

                sales.Add(new SalesRecord
                {
                    Date = DateTime.Parse(parts[0]),
                    SKU = parts[1].Trim(),
                    UnitPrice = decimal.Parse(parts[2]),
                    Quantity = int.Parse(parts[3]),
                    TotalPrice = decimal.Parse(parts[4])
                });
            }

            return sales;
        }

    }
}

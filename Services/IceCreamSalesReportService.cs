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

        public async Task<List<MonthlyPopularItemStats>> GetMonthlyPopularItemStats()
        {
            var sales = await GetSalesRecordsAsync();
            var monthlyPopularItemStats = new List<MonthlyPopularItemStats>();

            var monthlySales = new Dictionary<string, List<SalesRecord>>();
            foreach (var record in sales)
            {
                string month = record.Date.ToString("yyyy-MM");
                if (!monthlySales.ContainsKey(month))
                    monthlySales[month] = new List<SalesRecord>();
                monthlySales[month].Add(record);
            }


            foreach (var month in monthlySales.Keys)
            {
                var itemTotals = new Dictionary<string, int>();

                string topSku = string.Empty;
                int topQuantity = int.MinValue;

                foreach (var record in monthlySales[month])
                {
                    if (!itemTotals.ContainsKey(record.Sku))
                        itemTotals[record.Sku] = 0;

                    itemTotals[record.Sku] += record.Quantity;

                    if (itemTotals[record.Sku] > topQuantity)
                    {
                        topQuantity = itemTotals[record.Sku];
                        topSku = record.Sku;
                    }
                }


                int minimumOrder = monthlySales[month][0].Quantity;
                int maximumOrder = monthlySales[month][0].Quantity;
                int totalOrders = 0;
                int totalUnits = 0;

                foreach (var record in monthlySales[month])
                {
                    if (record.Sku == topSku)
                    {
                        int qty = record.Quantity;
                        if (qty < minimumOrder) minimumOrder = qty;
                        if (qty > maximumOrder) maximumOrder = qty;
                        totalUnits += qty;
                        totalOrders++;
                    }
                }

                double averageOrder = totalOrders > 0 ? (double)totalUnits / totalOrders : 0;

                monthlyPopularItemStats.Add(new MonthlyPopularItemStats
                {
                    Month = month,
                    Sku = topSku,
                    TotalQuantity = topQuantity,
                    MinimumOrder = minimumOrder,
                    MaximumOrder = maximumOrder,
                    AverageOrder = averageOrder
                });
            }

            return monthlyPopularItemStats;
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

                if (!monthlyItemSalesTotal[monthKey].ContainsKey(sale.Sku))
                    monthlyItemSalesTotal[monthKey][sale.Sku] = 0;

                monthlyItemSalesTotal[monthKey][sale.Sku] += sale.TotalPrice;
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

        private async Task<List<SalesRecord>> GetSalesRecordsAsync()
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
                    Sku = parts[1].Trim(),
                    UnitPrice = decimal.Parse(parts[2]),
                    Quantity = int.Parse(parts[3]),
                    TotalPrice = decimal.Parse(parts[4])
                });
            }

            return sales;
        }

    }
}

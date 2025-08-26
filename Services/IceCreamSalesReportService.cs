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
            return salesRecords.Sum(record => record.TotalPrice);
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

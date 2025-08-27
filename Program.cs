using IceCreamSalesReport.Helper;
using IceCreamSalesReport.Services;

var salesService = new IceCreamSalesReportService(AppSettings.SalesReportPath);


var totalSales = await salesService.GetTotalSalesAsync();

var monthlySalesTotal = await salesService.GetMonthlySalesTotalAsync();

var monthlyTopItemSalesTotal = await salesService.GetMonthlyTopItemSalesTotalAsync();

var monnthlyTopItemStats = await salesService.GetMonthlyPopularItemStats();

var number = 1;

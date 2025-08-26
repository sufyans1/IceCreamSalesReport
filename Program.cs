using IceCreamSalesReport.Helper;
using IceCreamSalesReport.Services;

var salesService = new IceCreamSalesReportService(AppSettings.SalesReportPath);

var a = await salesService.GetSalesRecordsAsync();
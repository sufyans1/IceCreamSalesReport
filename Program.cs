using IceCreamSalesReport.Helper;
using IceCreamSalesReport.Services;

await ReportPrinterService.PrintReportsAsync(AppSettings.SalesReportPath);
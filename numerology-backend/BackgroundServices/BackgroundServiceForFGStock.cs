//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using OmsSolution.Services;
//using System;
//using System.Data;
//using System.Threading;
//using System.Threading.Tasks;

//namespace OmsSolution.BackgroundServices
//{
//    public class BackgroundServiceForFGStock : BackgroundService
//    {

//        private readonly ILogger<ProductAddJobService> _logger;
//        private IStockService _service;
//        private readonly IServiceProvider _serviceProvider;
//        public BackgroundServiceForFGStock(ILogger<ProductAddJobService> logger, IServiceProvider serviceProvider)
//        {
//            _logger = logger;
//            _serviceProvider = serviceProvider;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            while (!stoppingToken.IsCancellationRequested)
//            {
//                using (var scope = _serviceProvider.CreateScope())
//                {
//                    var _service = scope.ServiceProvider.GetRequiredService<IStockService>();
//                    DataTable dt = await _service.FGInspectionGetLastId("1");
//                    if (dt.Rows.Count > 0)
//                    {
//                        DataTable dt2 = await _service.FGInspectionGetDataForOms(dt.Rows[0][0].ToString());

//                        await _service.InsertFGInspectionData(dt2);
//                    }
//                    _logger.LogInformation("Job is running at: {time}", DateTimeOffset.Now);
//                }
//                await Task.Delay(TimeSpan.FromHours(2), stoppingToken);
//            }
//        }

        
//    }
//}

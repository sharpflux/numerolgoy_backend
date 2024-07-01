//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using OmsSolution.Entities;
//using OmsSolution.Models;
//using OmsSolution.Services;
//using System;
//using System.Data;
//using System.Threading;
//using System.Threading.Tasks;

//namespace OmsSolution.BackgroundServices
//{
//    public class ProductAddJobService : BackgroundService
//    {
//        private readonly ILogger<ProductAddJobService> _logger;
//        private IStockService _service;
//        private readonly IServiceProvider _serviceProvider;

//        public ProductAddJobService(ILogger<ProductAddJobService> logger, IServiceProvider serviceProvider)
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
//                    //var productService = scope.ServiceProvider.GetRequiredService<IProductsService>();
//                    var stockService = scope.ServiceProvider.GetRequiredService<IStockService>();

//                    DataTable lastIdResult = await stockService.ProductsGetLastId("1");

//                    if (lastIdResult.Rows.Count > 0)
//                    {
//                        string lastProductId = lastIdResult.Rows[0][0].ToString();

//                        DataTable productMasterResult = await stockService.ProductMasterGetForOms(lastProductId);

//                        if (productMasterResult.Rows.Count > 0)
//                        {
//                            foreach (DataRow row in productMasterResult.Rows)
//                            {
                              
//                                ProductRequest productRequest = new ProductRequest
//                                {
//                                    ProductId = 0,
//                                    ProductName = row["ProductName"].ToString(),
//                                    ShortDescription = row["ProductName"].ToString(),
//                                    Description = row["ProductName"].ToString(),
//                                    CategoryID = Convert.ToInt32(row["CategoryID"].ToString()),
//                                    SKU="NA",
//                                    DefaultPrice = Convert.ToDecimal(row["ServiceCharge"].ToString()),
//                                    ShowWebSite=true,
//                                    UserId=1,
//                                    ImsMappingId= Convert.ToInt32(row["ProductID"].ToString())
//                                };

//                                ProductMeta productMeta = new ProductMeta
//                                {
//                                    MetaTitle = "",
//                                    MetaKeywords = "",
//                                    MetaDescription = "",
//                                };


//                                //await productService.ProductsInsertUpdate(productRequest, productMeta);
//                            }
//                        }
//                    }

//                    _logger.LogInformation("Job is running at: {time}", DateTimeOffset.Now);
//                }
//                await Task.Delay(TimeSpan.FromHours(2), stoppingToken);
//            }
//        }
//    }
//}

using coffeesoft.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OmsSolution.Helpers;
using OmsSolution.Services;
using OmsSolution.Utilities;
using Service;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Services;

using NumerologystSolution.Services;
using numerology_backend.Services;

namespace OmsSolution
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            SqlDBHelperOld.CONNECTION_STRING = ConfigurationExtensions.GetConnectionString(this.Configuration, "DefaultConnection");
            SqlDBHelper.CONNECTION_STRING = ConfigurationExtensions.GetConnectionString(this.Configuration, "DefaultConnection");
            ImsSQLDBHelper.CONNECTION_STRING = ConfigurationExtensions.GetConnectionString(this.Configuration, "ImsDB");
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
                     options.AddPolicy("DefaultCorsPolicy", builder => builder
                   .AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod())
            );
            services.AddControllersWithViews();
            services.AddControllers();
            //   services.AddCors();
            services.AddMvc();
            services.AddControllers()
            .AddJsonOptions(options =>
             {
              options.JsonSerializerOptions.MaxDepth = 64; // Set the maximum depth
                 // Other serialization options can be configured here
             });

            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<LoginDAL>();
            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MemoryBufferThreshold = int.MaxValue;
            });          
            services.AddTransient<IWebHostEnvironment>(provider => provider.GetService<Microsoft.AspNetCore.Hosting.IHostingEnvironment>() as IWebHostEnvironment);
            services.AddTransient<EmailSender>();
            services.AddHttpContextAccessor();
            //services.AddScoped<IStockService, StockService>();
            services.AddScoped<StockDBHelperForBg>();
            services.AddTokenAuthentication(Configuration);
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<ClientDBHelper>();
            services.AddScoped<IPlanetService, PlanetService>();
            services.AddScoped<PlanetDBHelper>();
            services.AddScoped<IRemediesService, RemediesService>();
            services.AddScoped<RemediesDBHelper>();
            services.AddScoped<INumerologyBNDNService, NumerologyBNDNService>();
            services.AddScoped<NumerologyBNDNDBHelper>();
            services.AddScoped<IPredictionService, PredictionService>();
            services.AddScoped<PredictionDBHelper>();
            services.AddScoped<INumerologyTitleService, NumerologyTitleService>();
            services.AddScoped<NumerologyTitleDBHelper>();
            services.AddScoped<IMissingNumberService, MissingNumberService>();
            services.AddScoped<MissingNumberDBHelper>();
            services.AddScoped<ILuckyUnluckyNumbersService, LuckyUnluckyService>();
            services.AddScoped<LuckyUnluckyDBHelper>();
            services.AddScoped<IVechileNumberService, VechileNumberService>();
            services.AddScoped<VechileNumberDBHelper>();
            services.AddScoped<IDOBCalculateService, DOBCalculateService>();
            services.AddScoped<DOBCalculateDBHelper>();
            services.AddScoped<IRepetitiveService, RepetitiveService>();
            services.AddScoped<RepetitiveDBHelper>();
            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MemoryBufferThreshold = int.MaxValue;
            });       
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseRouting();
            app.UseCors("DefaultCorsPolicy");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllers();
            });

        }
    }
}

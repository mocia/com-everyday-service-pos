using AutoMapper;
using Com.Danliris.Service.Inventory.Lib;
using Com.Danliris.Service.Inventory.Lib.Helpers;
using Com.Danliris.Service.Inventory.Lib.Services;
using Com.Bateeq.Service.Pos.Lib.Services.SalesDocReturnService;
using Com.Bateeq.Service.Pos.Lib.Services.SalesDocService;
using Com.Bateeq.Service.Pos.Lib.Interfaces;
using Com.Bateeq.Service.Pos.Lib.Services;
using Com.Bateeq.Service.Pos.Lib.Services.DiscountService;
using Com.Bateeq.Service.Pos.Lib.Services.SalesDocService;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;

namespace Com.Danliris.Service.Inventory.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void RegisterEndpoint()
        {
            APIEndpoint.Core = Configuration.GetValue<string>("CoreEndpoint") ?? Configuration["CoreEndpoint"];
            APIEndpoint.Warehouse = Configuration.GetValue<string>("WarehouseEndpoint") ?? Configuration["WarehouseEndpoint"];
            APIEndpoint.Production = Configuration.GetValue<string>("ProductionEndpoint") ?? Configuration["ProductionEndpoint"];
            APIEndpoint.Purchasing = Configuration.GetValue<string>("PurchasingEndpoint") ?? Configuration["PurchasingEndpoint"];
            APIEndpoint.Sales = Configuration.GetValue<string>("SalesEndpoint") ?? Configuration["SalesEndpoint"];
        }

        public void RegisterFacades(IServiceCollection services)
        {
            //services
            //    .AddTransient<FPReturnInvToPurchasingFacade>();
            //.AddTransient<FpRegradingResultDocsReportFacade>()
            //.AddTransient<InventoryDocumentFacade>()
            //.AddTransient<InventoryMovementFacade>()
            //.AddTransient<InventorySummaryFacade>()
            //.AddTransient<InventoryMovementReportFacade>()
            //.AddTransient<InventorySummaryReportFacade>();
        }

        public void RegisterServices(IServiceCollection services)
        {
            services
                //.AddScoped<MaterialDistributionNoteService>()
                //.AddTransient<MaterialDistributionNoteItemService>()
                //.AddTransient<StockTransferNoteService>()
                //.AddTransient<StockTransferNote_ItemService>()
                //.AddTransient<MaterialDistributionNoteDetailService>()
                //.AddTransient<FpRegradingResultDetailsDocsService>()
                //.AddTransient<FpRegradingResultDocsService>()
                //.AddTransient<FPReturnInvToPurchasingService>()
                //.AddTransient<FPReturnInvToPurchasingDetailService>()
                //.AddTransient<IStockTransferNoteService, NewStockTransferNoteService>()
                //.AddTransient<IMaterialRequestNoteService, NewMaterialRequestNoteService>()
                //.AddTransient<IMaterialDistributionService, NewMaterialDistributionNoteService>()
                //.AddTransient<IFpRegradingResultDocsService, NewFpRegradingResultDocsService>()
                //.AddTransient<IInventoryDocumentService, InventoryDocumentService>()
                //.AddTransient<IInventoryMovementService, InventoryMovementService>()
                .AddTransient<ISalesDocReturnService, SalesDocReturnService>()
                .AddTransient<ISalesDocService,SalesDocService>()
                .AddTransient<IDiscountService, DiscountService>()
                .AddScoped<IIdentityService, IdentityService>()
                .AddScoped<IValidateService, ValidateService>()
                .AddScoped<IHttpService, HttpService>()
                //.AddScoped<IdentityService>()
                .AddScoped<HttpClientService>()
                .AddScoped<ValidateService>()
                .AddScoped<IHttpClientService, HttpClientServices>();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection") ?? Configuration["DefaultConnection"];

            services
                .AddDbContext<PosDbContext>(options => options.UseSqlServer(connectionString))
                .AddApiVersioning(options =>
                {
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                });

            //services.Configure<MongoDbSettings>(options =>
            //    {
            //        options.ConnectionString = Configuration.GetConnectionString("MongoConnection") ?? Configuration["MongoConnection"];
            //        options.Database = Configuration.GetConnectionString("MongoDatabase") ?? Configuration["MongoDatabase"];
            //    });

            //services.AddSingleton<IMongoClient, MongoClient>(_ => new MongoClient(Configuration.GetConnectionString("MongoConnection") ?? Configuration["MongoConnection"]));

            //services.AddTransient<IMongoDbContext, MongoDbMigrationContext>();
            //services.AddTransient<IInventoryDocumentIntegrationService, InventoryDocumentIntegrationService>();
            //services.AddTransient<IInventoryDocumentMongoRepository, InventoryDocumentMongoRepository>();


            this.RegisterServices(services);
            this.RegisterFacades(services);
            this.RegisterEndpoint();

            var Secret = Configuration.GetValue<string>("Secret") ?? Configuration["Secret"];
            var Key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = false,
                        IssuerSigningKey = Key
                    };
                });

            services
                .AddMvcCore()
                .AddAuthorization()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .AddJsonFormatters();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddCors(options => options.AddPolicy("InventoryPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders("Content-Disposition", "api-version", "content-length", "content-md5", "content-type", "date", "request-id", "response-time");
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<PosDbContext>();
                context.Database.Migrate();
            }
            app.UseAuthentication();
            app.UseCors("InventoryPolicy");
            app.UseMvc();
        }
    }
}

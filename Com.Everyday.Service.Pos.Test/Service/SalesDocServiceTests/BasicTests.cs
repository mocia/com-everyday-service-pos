using Com.Danliris.Service.Inventory.Lib;
using Com.Danliris.Service.Inventory.Lib.Services;
using Com.Everyday.Service.Pos.Lib.Interfaces;
using Com.Everyday.Service.Pos.Lib.Services.SalesDocService;
using Com.Everyday.Service.Pos.Lib.ViewModels.SalesDoc;
using Com.Everyday.Service.Pos.Test.DataUtil.SalesDocDataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace Com.Everyday.Service.Pos.Test.Service.SalesDocServiceTests
{
    public class BasicTests
    {
        private const string ENTITY = "SalesDocs";
        //private PurchasingDocumentAcceptanceDataUtil pdaDataUtil;
        //private readonly IIdentityService identityService;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private PosDbContext _dbContext(string testName)
        {
            var serviceProvider = new ServiceCollection()
              .AddEntityFrameworkInMemoryDatabase()
              .BuildServiceProvider();

            DbContextOptionsBuilder<PosDbContext> optionsBuilder = new DbContextOptionsBuilder<PosDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .UseInternalServiceProvider(serviceProvider);

            PosDbContext dbContext = new PosDbContext(optionsBuilder.Options);

            return dbContext;
        }

        private SalesDocDataUtil _dataUtil(SalesDocService service)
        {
            return new SalesDocDataUtil(service);
        }

        private Mock<IServiceProvider> GetServiceProvider()
        {
            HttpResponseMessage message = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            message.Content = new StringContent("{\"apiVersion\":\"1.0\",\"statusCode\":200,\"message\":\"Ok\",\"data\":[{\"Id\":7,\"code\":\"USD\",\"rate\":13700.0,\"date\":\"2018/10/20\"}],\"info\":{\"count\":1,\"page\":1,\"size\":1,\"total\":2,\"order\":{\"date\":\"desc\"},\"select\":[\"Id\",\"code\",\"rate\",\"date\"]}}");
            var HttpClientService = new Mock<IHttpClientService>();
            HttpClientService
                .Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(message);

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(IdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test" });

            serviceProvider
              .Setup(s => s.GetService(typeof(IIdentityService)))
              .Returns(new IdentityService() { TimezoneOffset = 1, Token = "token", Username = "username" });

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new SalesDocIHttpService());


            return serviceProvider;
        }

        [Fact]
        public async void Should_Success_Get_Data()
        {
            var service = new SalesDocService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service).GetTestData();
            var Response = service.ReadModel("code",1, 25, "{}","", "{}", "username");
            Assert.NotEmpty(Response.Item1);

            //Dictionary<string, string> order = new Dictionary<string, string>()
            //{
            //    {"No", "asc" }
            //};
            //var response2 = service.ReadModel(1, 25, JsonConvert.SerializeObject(order), null, data.Name, "{}");
            //Assert.NotEmpty(response2.Data);
        }

        [Fact]
        public async void Should_Success_Get_Data_By_Id()
        {
            var service = new SalesDocService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service).GetTestData();
            var Response = service.ReadModelById(data.Id);
            Assert.NotNull(Response);
        }

        [Fact]
        public async void Should_Success_Get_Data_By_Code()
        {
            var service = new SalesDocService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service).GetTestData();
            var Response = service.ReadModelByCode(data.Code,data.StoreCode);
            Assert.NotNull(Response);
        }

        [Fact]
        public async void Should_Success_Create_Data()
        {
            var service = new SalesDocService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = _dataUtil(service).GetNewData();
            var Response = await service.Create(data);
            Assert.NotNull(Response);
        }

        //[Fact]
        //public void Should_No_Error_Validate_Data()
        //{
        //    var service = new MachineSpinningService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
        //    var vm = _dataUtil(service).GetDataToValidate(_dbContext(GetCurrentMethod()));
        //    Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
        //    serviceProvider.
        //        Setup(x => x.GetService(typeof(CoreDbContext)))
        //        .Returns(_dbContext(GetCurrentMethod()));
        //    ValidationContext validationDuplicateContext = new ValidationContext(vm, serviceProvider.Object, null);
        //    Assert.True(vm.Validate(validationDuplicateContext).Count() == 0);
        //}

        //[Fact]
        //public async void Should_No_Error_Validate_Data_Duplicate_Name()
        //{
        //    var service = new MachineSpinningService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
        //    var model = _dataUtil(service).GetNewData(_dbContext(GetCurrentMethod()));
        //    var Response = await service.CreateAsync(model);
        //    var vm = _dataUtil(service).GetDataToValidate(_dbContext(GetCurrentMethod()));
        //    vm.Name = model.Name;
        //    vm.No = model.No;
        //    vm.UnitName = model.UnitName;
        //    Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
        //    serviceProvider.
        //        Setup(x => x.GetService(typeof(CoreDbContext)))
        //        .Returns(_dbContext(GetCurrentMethod()));
        //    ValidationContext validationDuplicateContext = new ValidationContext(vm, serviceProvider.Object, null);
        //    Assert.True(vm.Validate(validationDuplicateContext).Count() > 0);
        //}

        [Fact]
        public void Should_Success_Validate_All_Null_Data()
        {
            var vm = new SalesDocViewModel() {
                salesDetail=new SalesDetail()
            };

            Assert.True(vm.Validate(null).Count() > 0);
        }

       
    }
}

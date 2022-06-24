using Com.Danliris.Service.Inventory.Lib;
using Com.Danliris.Service.Inventory.Lib.Services;
using Com.Everyday.Service.Pos.Lib.Interfaces;
using Com.Everyday.Service.Pos.Lib.Services.SalesDocReturnService;
using Com.Everyday.Service.Pos.Lib.Services.SalesDocService;
using Com.Everyday.Service.Pos.Test.DataUtil.SalesDocDataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace Com.Everyday.Service.Pos.Test.Service.SalesDocReturnServiceTests
{
    public class BasicTests
    {
        private const string ENTITY = "SalesDocs";

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
        public async void Should_Success_ReturnReport()
        {
            var salesDoc = new SalesDocService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            Mock<ISalesDocService> mockSalesDoc = new Mock<ISalesDocService>();

            var data = _dataUtil(salesDoc).GetNewData();

            var service = new SalesDocReturnService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()), mockSalesDoc.Object);

            var create = await salesDoc.Create(data);

            var Response = service.SalesReturnReport("code", DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now, "0");
            Assert.NotNull(Response);
        }

    }
}

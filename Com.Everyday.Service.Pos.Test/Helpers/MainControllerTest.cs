using Com.Danliris.Service.Inventory.Lib.Helpers;
using Com.Danliris.Service.Inventory.Lib.Interfaces;
using Com.Danliris.Service.Inventory.Test.Interfaces;
using Com.Moonlay.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Inventory.Test.Helpers
{
    public abstract class MainControllerTest<TModel, TFacade, TViewModel, TDataUtil>
        where TModel : StandardEntity, IValidatableObject, new()
        where TDataUtil : BaseDataUtil<TModel, TFacade>, IEmptyData<TViewModel>
        where TViewModel : BaseViewModel<TModel>
    {
        private const string MediaType = "application/json";
        private readonly string URI;
        private readonly List<string> CreateValidationAttributes;
        private readonly List<string> UpdateValidationAttributes;

        protected TestServerFixture TestFixture { get; set; }

        public MainControllerTest(TestServerFixture fixture, string URI, List<string> CreateValidationAttributes, List<string> UpdateValidationAttributes)
        {
            TestFixture = fixture;
            this.URI = URI;
            this.CreateValidationAttributes = CreateValidationAttributes;
            this.UpdateValidationAttributes = UpdateValidationAttributes;
        }

        protected HttpClient Client
        {
            get { return this.TestFixture.Client; }
        }

        protected TDataUtil DataUtil
        {
            get { return (TDataUtil)this.TestFixture.Service.GetService(typeof(TDataUtil)); }
        }

        [SkippableFact]
        public async Task Should_Success_Get_All_Data()
        {
            Skip.If(!typeof(TFacade).GetInterfaces().Contains(typeof(IReadable)), "Not Readable");

            var response = await this.Client.GetAsync(URI);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var json = response.Content.ReadAsStringAsync().Result;
            Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json.ToString());

            Assert.True(result.ContainsKey("apiVersion"));
            Assert.True(result.ContainsKey("message"));
            Assert.True(result.ContainsKey("data"));
            Assert.True(result["data"].GetType().Name.Equals("JArray"));
        }

        [SkippableFact]
        public async Task Should_Success_Get_Data_By_Id()
        {
            Skip.If(!typeof(TFacade).GetInterfaces().Contains(typeof(IReadByIdable<TModel>)), "Not Read By Id able");

            TModel Model = await DataUtil.GetTestData();
            TViewModel ViewModel = (TViewModel)Activator.CreateInstance(typeof(TViewModel), Model);

            var response = await this.Client.GetAsync(string.Concat(URI, "/", ViewModel.Id));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var json = response.Content.ReadAsStringAsync().Result;
            Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json.ToString());

            Assert.True(result.ContainsKey("apiVersion"));
            Assert.True(result.ContainsKey("message"));
            Assert.True(result.ContainsKey("data"));
            Assert.True(result["data"].GetType().Name.Equals("JObject"));
        }

        [SkippableFact]
        public async Task Should_Success_Create_Data()
        {
            Skip.If(!typeof(TFacade).GetInterfaces().Contains(typeof(ICreateable<TModel>)), "Not Createable");

            TViewModel ViewModel = (TViewModel)Activator.CreateInstance(typeof(TViewModel), DataUtil.GetNewData());

            var response = await this.Client.PostAsync(URI, new StringContent(JsonConvert.SerializeObject(ViewModel).ToString(), Encoding.UTF8, MediaType));
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseLocationHeader = await this.Client.GetAsync(response.Headers.Location.OriginalString);
            Assert.Equal(HttpStatusCode.OK, responseLocationHeader.StatusCode);

            var json = responseLocationHeader.Content.ReadAsStringAsync().Result;
            Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json.ToString());

            Assert.True(result.ContainsKey("apiVersion"));
            Assert.True(result.ContainsKey("message"));
            Assert.True(result.ContainsKey("data"));
            Assert.True(result["data"].GetType().Name.Equals("JObject"));
        }

        /*
        [Fact]
        public async Task Should_Success_Update_Data()
        {
            Skip.If(!typeof(TFacade).GetInterfaces().Contains(typeof(IUpdateable)), "Not Updateable");

            TModel Model = await DataUtil.GetTestData();
            TViewModel ViewModel = (TViewModel)Activator.CreateInstance(typeof(TViewModel), Model);

            var response = await this.Client.PutAsync(string.Concat(URI, "/", ViewModel.Id), new StringContent(JsonConvert.SerializeObject(ViewModel).ToString(), Encoding.UTF8, MediaType));
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        */

        [SkippableFact]
        public async Task Should_Success_Delete_Data()
        {
            Skip.If(!typeof(TFacade).GetInterfaces().Contains(typeof(IDeleteable)), "Not Deleteable");

            TModel Model = await DataUtil.GetTestData();
            TViewModel ViewModel = (TViewModel)Activator.CreateInstance(typeof(TViewModel), Model);

            var response = await this.Client.DeleteAsync(string.Concat(URI, "/", ViewModel.Id));
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [SkippableFact]
        public async Task Should_Error_Create_With_Empty_Data()
        {
            Skip.If(!typeof(TFacade).GetInterfaces().Contains(typeof(ICreateable<TModel>)), "Not Createable");

            TViewModel ViewModel = DataUtil.GetEmptyData();

            var response = await this.Client.PostAsync(URI, new StringContent(JsonConvert.SerializeObject(ViewModel).ToString(), Encoding.UTF8, MediaType));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var json = response.Content.ReadAsStringAsync().Result;
            Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json.ToString());

            Assert.True(result.ContainsKey("apiVersion"));
            Assert.True(result.ContainsKey("message"));
            Assert.True(result.ContainsKey("error"));
            Assert.True(result["error"].GetType().Name.Equals("JObject"));

            JObject error = (JObject)result["error"];
            foreach (string arg in CreateValidationAttributes)
            {
                var prop = error.Properties().FirstOrDefault(c => c.Name.Equals(arg, StringComparison.CurrentCultureIgnoreCase));
                Assert.NotNull(prop);
            }
        }

        /*
        [SkippableFact]
        public async Task Should_Error_Update_With_Empty_Data()
        {
            Skip.If(!typeof(TFacade).GetInterfaces().Contains(typeof(IUpdateable)), "Not Updateable");

            TViewModel EmptyData = DataUtil.GetEmptyData();
            TModel Model = await DataUtil.GetTestData();
            TViewModel ViewModel = (TViewModel)Activator.CreateInstance(typeof(TViewModel), Model);
            EmptyData.Id = ViewModel.Id;

            var response = await this.Client.PutAsync(string.Concat(URI, "/", EmptyData.Id), new StringContent(JsonConvert.SerializeObject(EmptyData).ToString(), Encoding.UTF8, MediaType));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var json = response.Content.ReadAsStringAsync().Result;
            Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json.ToString());

            Assert.True(result.ContainsKey("apiVersion"));
            Assert.True(result.ContainsKey("message"));
            Assert.True(result.ContainsKey("error"));
            Assert.True(result["error"].GetType().Name.Equals("JObject"));

            JObject error = (JObject)result["error"];
            foreach (string arg in UpdateValidationAttributes)
            {
                var prop = error.Properties().FirstOrDefault(c => c.Name.Equals(arg, StringComparison.CurrentCultureIgnoreCase));
                Assert.NotNull(prop);
            }
        }
        */

        [SkippableFact]
        public async Task Should_Return_404_Get_Data_With_Unknown_Id()
        {
            Skip.If(!typeof(TFacade).GetInterfaces().Contains(typeof(IReadByIdable<TModel>)), "Not Read By Id able");

            var response = await this.Client.GetAsync(string.Concat(URI, "/", 0));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        /*
        [SkippableFact]
        public async Task Should_Return_400_Update_Data_With_Invalid_Id()
        {
            Skip.If(!typeof(TFacade).GetInterfaces().Contains(typeof(IUpdateable)), "Not Updateable");

            TModel Model = await DataUtil.GetTestData();
            TViewModel ViewModel = (TViewModel)Activator.CreateInstance(typeof(TViewModel), Model);

            var response = await this.Client.PutAsync(string.Concat(URI, "/", 0), new StringContent(JsonConvert.SerializeObject(ViewModel).ToString(), Encoding.UTF8, MediaType));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        */

        [SkippableFact]
        public async Task Should_Return_404_Delete_Data_With_Unknown_Id()
        {
            Skip.If(!typeof(TFacade).GetInterfaces().Contains(typeof(IDeleteable)), "Not Deleteable");

            var response = await this.Client.DeleteAsync(string.Concat(URI, "/", 0));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}

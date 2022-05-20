using Com.Danliris.Service.Inventory.Lib.Interfaces;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Inventory.Test.Helpers
{
    public abstract class BasicFacadeTest<TFacade, TModel, TDataUtil>
        where TDataUtil : BaseDataUtil<TModel, TFacade>
        where TModel : StandardEntity, IValidatableObject
    {
        private IServiceProvider serviceProvider { get; set; }
        private readonly List<string> Keys;

        public BasicFacadeTest(ServiceProviderFixture fixture, List<string> keys)
        {
            serviceProvider = fixture.ServiceProvider;
            Keys = keys;
        }

        protected TFacade Facade
        {
            get { return (TFacade)this.serviceProvider.GetService(typeof(TFacade)); }
        }

        protected TDataUtil DataUtil
        {
            get { return (TDataUtil)this.serviceProvider.GetService(typeof(TDataUtil)); }
        }

        [SkippableFact]
        public async void Should_Success_Create_Data()
        {
            Skip.If(!typeof(TFacade).GetInterfaces().Contains(typeof(ICreateable<TModel>)), "Not Createable");

            TModel Data = DataUtil.GetNewData();
            int AffectedRows = await (this.Facade as ICreateable<TModel>).Create(Data);

            Assert.True(AffectedRows > 0);
        }

        /*
        [SkippableFact]
        public async void Should_Success_Update_Data()
        {
            Skip.If(!typeof(TFacade).GetInterfaces().Contains(typeof(IUpdateable<TModel>)), "Not Updateable");

            TModel Data = await DataUtil.GetTestData();
            int AffectedRows = await (this.Facade as IUpdateable<TModel>).UpdateModel(Data.Id, Data);

            Assert.True(AffectedRows > 0);
        }
        */

        [SkippableFact]
        public async void Should_Error_Create_Data_With_Same_Keys()
        {
            Skip.If(Keys.Count == 0 || !typeof(TFacade).GetInterfaces().Contains(typeof(ICreateable<TModel>)), "No Keys or Not Createable");

            try
            {
                TModel Data = await DataUtil.GetTestData();
                Data.Id = 0;

                await (this.Facade as ICreateable<TModel>).Create(Data);
            }
            catch (ServiceValidationExeption ex)
            {
                foreach (string Key in Keys)
                {
                    ValidationResult result = ex.ValidationResults.FirstOrDefault(r => r.MemberNames.Contains(Key, StringComparer.CurrentCultureIgnoreCase));
                    Assert.NotNull(result);
                }
            }
        }

        /*
        [SkippableFact]
        public async void Should_Error_Update_Data_With_Same_Keys()
        {
            Skip.If(Keys.Count == 0 || !typeof(TFacade).GetInterfaces().Contains(typeof(IUpdateable<TModel>)), "No Keys or Not Updateable");

            try
            {
                TModel Data = await DataUtil.GetTestData();
                TModel UpdateData = await DataUtil.GetTestData();

                foreach (string Key in this.Keys)
                {
                    string Value = (string)Data.GetType().GetProperty(Key).GetValue(Data, null);
                    UpdateData.GetType().GetProperty(Key).SetValue(UpdateData, Value);
                }

                await (this.Facade as IUpdateable).Update(UpdateData.Id, UpdateData);
            }
            catch (ServiceValidationExeption ex)
            {
                foreach (string Key in Keys)
                {
                    ValidationResult result = ex.ValidationResults.FirstOrDefault(r => r.MemberNames.Contains(Key, StringComparer.CurrentCultureIgnoreCase));
                    Assert.NotNull(result);
                }
            }
        }
        */

        [SkippableFact]
        public async void Should_Success_Delete_Data()
        {
            Skip.If(!typeof(TFacade).GetInterfaces().Contains(typeof(IDeleteable)), "Not Deleteable");

            TModel Data = await DataUtil.GetTestData();
            int AffectedRows = await (this.Facade as IDeleteable).Delete(Data.Id);

            Assert.True(AffectedRows > 0);
        }

        [SkippableFact]
        public async void Should_Success_Get_Data()
        {
            Skip.If(!typeof(TFacade).GetInterfaces().Contains(typeof(IReadable)), "Not Readable");

            TModel Data = await DataUtil.GetTestData();
            var Response = (this.Facade as IReadable).Read();

            Assert.NotEqual(Response.Item1.Count, 0);
        }

        [SkippableFact]
        public async void Should_Success_Get_Data_By_Id()
        {
            Skip.If(!typeof(TFacade).GetInterfaces().Contains(typeof(IReadByIdable<TModel>)), "Not Read By Id able");

            TModel Data = await DataUtil.GetTestData();

            var Response = await (this.Facade as IReadByIdable<TModel>).ReadById(Data.Id);

            Assert.True(!Response.Equals(null));
        }
    }
}

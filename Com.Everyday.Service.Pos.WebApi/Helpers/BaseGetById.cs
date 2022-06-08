using Com.Danliris.Service.Inventory.Lib.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Inventory.WebApi.Helpers
{
    public class BaseGetById<TModel, TViewModel, TFacade> : Controller
        where TFacade : IReadByIdable<TModel>
    {
        private readonly TFacade facade;
        private readonly string apiVersion;

        public BaseGetById(TFacade facade, string apiVersion)
        {
            this.facade = facade;
            this.apiVersion = apiVersion;
        }

        public async Task<ActionResult> GetById(int id)
        {
            var model = await facade.ReadById(id);

            if (model == null)
            {
                Dictionary<string, object> ResultNotFound =
                        new ResultFormatter(apiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                return NotFound(ResultNotFound);
            }

            try
            {
                return Ok(new
                {
                    apiVersion = apiVersion,
                    data = (TViewModel)Activator.CreateInstance(typeof(TViewModel), model),
                    message = "Ok",
                    statusCode = 200
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(apiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
    }
}

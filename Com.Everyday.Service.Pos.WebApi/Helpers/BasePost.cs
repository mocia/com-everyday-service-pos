using Com.Danliris.Service.Inventory.Lib.Helpers;
using Com.Danliris.Service.Inventory.Lib.Interfaces;
using Com.Danliris.Service.Inventory.Lib.Services;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Inventory.WebApi.Helpers
{
    public class BasePost<TModel, TViewModel, TFacade> : Controller
        where TModel : StandardEntity
        where TFacade : ICreateable<TModel>
        where TViewModel : BaseViewModel<TModel>
    {
        private readonly TFacade facade;
        private readonly string apiVersion;
        private readonly ValidateService validateService;
        private readonly string requestPath;

        public BasePost(TFacade facade, string apiVersion, ValidateService validateService, string requestPath)
        {
            this.facade = facade;
            this.apiVersion = apiVersion;
            this.validateService = validateService;
            this.requestPath = requestPath;
        }

        public async Task<ActionResult> Post(TViewModel viewModel)
        {
            try
            {
                this.validateService.Validate(viewModel);

                TModel model = viewModel.ToModel();

                await facade.Create(model);

                Dictionary<string, object> Result =
                    new ResultFormatter(apiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                    .Ok();
                return Created(String.Concat(this.requestPath, "/", model.Id), Result);
            }
            catch (ServiceValidationExeption e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(apiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
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

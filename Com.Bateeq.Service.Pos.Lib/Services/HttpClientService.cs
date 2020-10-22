using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Com.Danliris.Service.Inventory.Lib.Services
{
    public class HttpClientService : HttpClient
    {
        public HttpClientService(IdentityService identityService)
        {
            this.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", identityService.Token);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using request_web.WebService;

namespace request_web.StaticClass
{
    public class WcfClass
    {
        private static readonly RequestWebServiceClient _wcfService = new RequestWebServiceClient();

    }
}
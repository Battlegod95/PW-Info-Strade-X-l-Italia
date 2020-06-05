using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace PW_Info_Strade_X_l_Italia___Azure_Function
{
    public static class SignalRConnection
    {
        [FunctionName("SignalRConnection")]
        public static SignalRConnectionInfo Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [SignalRConnectionInfo(HubName = "broadcast")] SignalRConnectionInfo info,
            ILogger log) => info;
    }
}

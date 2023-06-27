using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorApp.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class IndexVideoFunction
    {
        private readonly ILogger _logger;

        public IndexVideoFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<IndexVideoFunction>();
        }

        [Function("IndexVideo")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            IndexVideoModel model = await req.ReadFromJsonAsync<IndexVideoModel>();
            var requestUrl = Environment.GetEnvironmentVariable("url_ladevindexvideo");
            HttpClient httpClient = new HttpClient();
            var logicAppResponse = await httpClient.PostAsJsonAsync(requestUrl, model);
            logicAppResponse.EnsureSuccessStatusCode();
            var logicAppResponseModel = await logicAppResponse.Content.ReadFromJsonAsync<IndexVideoResponseModel>();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(logicAppResponseModel);

            return response;
        }
    }

}

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class GenerateVideoEditAccessTokenFunction
    {
        private readonly ILogger _logger;

        public GenerateVideoEditAccessTokenFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GenerateVideoEditAccessTokenFunction>();
        }

        [Function("GenerateVideoEditAccessToken")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var requestUrl = Environment.GetEnvironmentVariable("url_ladevgetvitoken");
            HttpClient httpClient = new HttpClient();
            var laResult = await httpClient.GetStringAsync(requestUrl);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync(laResult);
            return response;
        }
    }
}

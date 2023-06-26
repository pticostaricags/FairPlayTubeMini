using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
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
            var requestUrl = Environment.GetEnvironmentVariable("url-ladevindexvideo");
            HttpClient httpClient = new HttpClient();
            var logicAppResponse = await httpClient.PostAsJsonAsync(requestUrl, model);
            logicAppResponse.EnsureSuccessStatusCode();
            var logicAppResponseModel = await logicAppResponse.Content.ReadFromJsonAsync<IndexVideoResponseModel>();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(logicAppResponseModel);

            return response;
        }
    }

    public class IndexVideoModel
    {
        public string VideoFileName { get; set; }
        public string VideoSourceUrl { get; set; }
    }

    public class IndexVideoResponseModel
    {
        public string accountId { get; set; }
        public string id { get; set; }
        public object partition { get; set; }
        public object externalId { get; set; }
        public object metadata { get; set; }
        public string name { get; set; }
        public object description { get; set; }
        public DateTime created { get; set; }
        public DateTime lastModified { get; set; }
        public DateTime lastIndexed { get; set; }
        public string privacyMode { get; set; }
        public string userName { get; set; }
        public bool isOwned { get; set; }
        public bool isBase { get; set; }
        public bool hasSourceVideoFile { get; set; }
        public string state { get; set; }
        public string moderationState { get; set; }
        public string reviewState { get; set; }
        public string processingProgress { get; set; }
        public int durationInSeconds { get; set; }
        public string thumbnailVideoId { get; set; }
        public string thumbnailId { get; set; }
        public object[] searchMatches { get; set; }
        public string indexingPreset { get; set; }
        public string streamingPreset { get; set; }
        public string sourceLanguage { get; set; }
        public string[] sourceLanguages { get; set; }
        public string personModelId { get; set; }
    }


}

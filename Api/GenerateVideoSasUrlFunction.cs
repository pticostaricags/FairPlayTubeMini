using System;
using System.Net;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using BlazorApp.Shared;
using BlazorApp.Shared.Helpers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class GenerateVideoSasUrlFunction
    {
        private readonly ILogger _logger;

        public GenerateVideoSasUrlFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GenerateVideoSasUrlFunction>();
        }

        [Function("GenerateVideoSasUrl")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            var videoFileName = query["videoFileName"];
            var userId = query["userId"];
            var fileRelativePath = UserBlobsHelper.GetBlobRelativePath(userId, videoFileName);
            var blobStorageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            BlobServiceClient blobServiceClient = new(blobStorageConnectionString);
            var blobClient = blobServiceClient.GetBlobContainerClient("videos")
                    .GetBlobClient(fileRelativePath);
            var sasUrl = blobClient.GenerateSasUri(
                Azure.Storage.Sas.BlobSasPermissions.All, DateTimeOffset.UtcNow.AddDays(1));
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync<GenerateVideoSasUrlResponseModel>(
                new GenerateVideoSasUrlResponseModel()
                {
                    VideoSasUrl = sasUrl.ToString()
                }
                );

            return response;
        }
    }
}

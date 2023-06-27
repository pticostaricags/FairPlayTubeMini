using System;
using System.Net;
using BlazorApp.Shared;
using System.Net.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Api
{
    public class SearchVideosFunction
    {
        private readonly ILogger _logger;

        public SearchVideosFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SearchVideosFunction>();
        }

        [Function("SearchVideos")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            var pageNumber = query["pageNumber"];
            var searchTerm = query["searchTerm"];
            SearchVideosModel model = new SearchVideosModel()
            {
                PageNumber = Convert.ToInt32(pageNumber),
                SearchTerm = searchTerm
            };
            var requestUrl = Environment.GetEnvironmentVariable("url_ladevsearchvideos");
            HttpClient httpClient = new HttpClient();
            var logicAppResponse = await httpClient.PostAsJsonAsync(requestUrl, model);
            logicAppResponse.EnsureSuccessStatusCode();
            var logicAppResponseModel = await logicAppResponse.Content.ReadFromJsonAsync<SearchVideosResponseModel>();
            //requestUrl = Environment.GetEnvironmentVariable("url_ladevgetvideothumbnail");
            //foreach (var singleResult in logicAppResponseModel.results)
            //{
            //    var getThumbnailResponse = await httpClient.PostAsJsonAsync(requestUrl, new 
            //    {
            //        VideoId= singleResult.id,
            //        VideoThumbnailId=singleResult.thumbnailId
            //    });
            //    if (getThumbnailResponse.IsSuccessStatusCode)
            //    {
            //        singleResult.thumbnailBytes = await getThumbnailResponse.Content.ReadAsByteArrayAsync();
            //    }
            //}
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(logicAppResponseModel);

            return response;
        }
    }
}

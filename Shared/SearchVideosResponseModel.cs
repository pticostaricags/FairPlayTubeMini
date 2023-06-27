using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BlazorApp.Shared
{

    public class SearchVideosResponseModel
    {
        public Result[] results { get; set; }
        public Nextpage nextPage { get; set; }
    }

    public class Nextpage
    {
        public int pageSize { get; set; }
        public int skip { get; set; }
        public bool done { get; set; }
    }

    public class Result
    {
        public byte[] thumbnailBytes { get; set; }
        private const string location = "eastus";
        public string publicPlayerUrl => $"https://www.videoindexer.ai/embed/player/{accountId}/{id}" +
        $"?locale={CultureInfo.CurrentUICulture.TwoLetterISOLanguageName}&location={location}&autoplay=false";
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

using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System.Collections.Generic;
using System.Linq;

namespace JoelScottFitness.YouTube.Client
{
    public class YouTubeClient : IYouTubeClient
    {
        public void GetVideos(long limit)
        {
            YouTubeService yt = new YouTubeService(new BaseClientService.Initializer() { ApiKey = "AIzaSyAgf8J3106URkifq8DpiE3M0iueH_KCHT0" });
            List<string> videos = new List<string>();
            var searchListRequest = yt.Search.List("snippet");
            searchListRequest.MaxResults = limit;
            searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
            searchListRequest.ChannelId = "UCubxF1muUJF5xH7yLe_KfFg";
            var searchListResult = searchListRequest.Execute();
            foreach (var item in searchListResult.Items)
            {
                videos.Add("https://www.youtube.com/watch?v=" + item.Id.VideoId);
            }
        }
    }
}

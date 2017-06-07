using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using JoelScottFitness.YouTube.Models;
using System;
using System.Collections.Generic;

namespace JoelScottFitness.YouTube.Client
{
    public class YouTubeClient : IYouTubeClient
    {
        private const string apiKey = "AIzaSyAgf8J3106URkifq8DpiE3M0iueH_KCHT0";
        private const string channelId = "UCubxF1muUJF5xH7yLe_KfFg";

        public IEnumerable<YouTubeVideo> GetVideos(long limit)
        {
            var videos = new List<YouTubeVideo>();

            try
            {
                YouTubeService yt = new YouTubeService(new BaseClientService.Initializer() { ApiKey = apiKey });
                List<string> videoList = new List<string>();

                // configure the search query
                var searchListRequest = yt.Search.List("snippet");
                searchListRequest.MaxResults = limit;
                searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
                searchListRequest.ChannelId = channelId;

                // call the youtube api
                var searchListResult = searchListRequest.Execute();

                // iterate the results
                foreach (var item in searchListResult.Items)
                {
                    videos.Add(new YouTubeVideo()
                    {
                        VideoId = item.Id.VideoId,
                        Description = item.Snippet.Description,
                    });

                    //videoList.Add("https://www.youtube.com/watch?v=" + item.Id.VideoId);
                }
            }
            catch(Exception ex)
            {
                // TODO handle exception
                string err = ex.Message;
            }

            return videos;
        }
    }
}

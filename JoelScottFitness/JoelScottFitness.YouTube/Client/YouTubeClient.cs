using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using JoelScottFitness.YouTube.Models;
using JoelScottFitness.YouTube.Properties;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JoelScottFitness.YouTube.Client
{
    public class YouTubeClient : IYouTubeClient
    {
        private ILog logger = LogManager.GetLogger(nameof(YouTubeClient));

        private const string apiKey = "AIzaSyAgf8J3106URkifq8DpiE3M0iueH_KCHT0";
        private const string channelId = "UCubxF1muUJF5xH7yLe_KfFg";

        private IList<YouTubeVideo> videos;
        private DateTime? lastRefreshed;

        public IEnumerable<YouTubeVideo> GetVideos(int limit)
        {
            try
            {
                if (videos == null || !videos.Any()
                 || lastRefreshed == null || lastRefreshed < DateTime.UtcNow.AddMinutes(-Settings.Default.RefreshPeriodMinutes))
                {
                    videos = new List<YouTubeVideo>();
                    lastRefreshed = DateTime.UtcNow;

                    YouTubeService yt = new YouTubeService(new BaseClientService.Initializer() { ApiKey = apiKey });
                    List<string> videoList = new List<string>();

                    // configure the search query
                    var searchListRequest = yt.Search.List("snippet");
                    searchListRequest.MaxResults = Settings.Default.DefaultVideoLimit;
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
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Warn($"Failed to retrieve youtube videos, error details '{ex.Message}'.");
            }

            return videos.Take(limit);
        }
    }
}

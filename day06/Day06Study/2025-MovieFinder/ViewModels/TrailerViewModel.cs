using _2025_MovieFinder.Helpers;
using _2025_MovieFinder.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace _2025_MovieFinder.ViewModels
{
    public partial class TrailerViewModel : ObservableObject
    {
        private readonly IDialogCoordinator dialogCoordinator;

        public TrailerViewModel(IDialogCoordinator coordinator, string movieTitle)
        {
            this.dialogCoordinator = coordinator;
            MovieTitle = movieTitle;
            YoutubeUri = "https://www.youtube.com";
            SearchYoutubeApi();
        }


        private string _movieTitle;

        public string MovieTitle
        {
            get => _movieTitle;
            set => SetProperty(ref _movieTitle, value);
        }

        private ObservableCollection<YoutubeItem> _youtubeItems;

        public ObservableCollection<YoutubeItem> YoutubeItems 
        { 
            get => _youtubeItems; 
            set => SetProperty(ref _youtubeItems, value); 
        }
        private YoutubeItem _selectedYoutube;

        public YoutubeItem SelectedYoutube
        {
            get => _selectedYoutube;
            set => SetProperty(ref _selectedYoutube, value);
        }
        private string _youtubeUri;
        public string YoutubeUri
        {
            get => _youtubeUri;
            set => SetProperty(ref _youtubeUri, value);
        }

        // YouTube Data API v3 호출
        private async void SearchYoutubeApi()
        {
            await LoadDataCollection();
        }

        private async Task LoadDataCollection()
        {
            var service = new YouTubeService(
                new BaseClientService.Initializer()
                {
                    ApiKey = "AIzaSyDTk-iV9rGpVBCkpPQTjX9NxXrhEOpqgkU",
                    ApplicationName = this.GetType().ToString()
                }
            );
            var req = service.Search.List("snippet");
            req.Q = $"{MovieTitle} 예고편";   // 영화이름 Official Trailer API 검색
            req.Order = SearchResource.ListRequest.OrderEnum.Relevance;
            req.Type = "video";
            req.MaxResults = 10;

            ObservableCollection<YoutubeItem> youtubeItems = new ObservableCollection<YoutubeItem>();

            var res = await req.ExecuteAsync(); // YouTube API 서버에 요청된 값을 실행하고 결과를 비동기로 return
            foreach (var item in res.Items)
            {
                youtubeItems.Add(new YoutubeItem
                {
                    Title = item.Snippet.Title,
                    ChannelTitle = item.Snippet.ChannelTitle,
                    URL = $"https://youtube.com/watch?v={item.Id.VideoId}", // Youtube Play Link
                    Author = item.Snippet.ChannelId,
                    Thumbnail = new BitmapImage(new Uri(item.Snippet.Thumbnails.Default__.Url, UriKind.RelativeOrAbsolute))
                });
            }
            YoutubeItems = youtubeItems;
            Common.LOGGER.Info($"{MovieTitle}의 예고편 {YoutubeItems.Count} 건 조회 완료");

        }

        [RelayCommand]
        public async Task YoutubeDoubleClick()
        {
            YoutubeUri = SelectedYoutube.URL;
        }
    }
}

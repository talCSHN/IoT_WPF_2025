using _2025_MovieFinder.Helpers;
using _2025_MovieFinder.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace _2025_MovieFinder.ViewModels
{
    public partial class MoviesViewModel : ObservableObject
    {
        private readonly IDialogCoordinator dialogCoordinator;
        public MoviesViewModel(IDialogCoordinator coordinator)
        {
            this.dialogCoordinator = coordinator;
            Common.LOGGER.Info("MovieFinder2025 Start");

            PosterUri = new Uri("/No_Picture.png", UriKind.RelativeOrAbsolute);
        }

        private string _movieName;

        public string MovieName { 
            get => _movieName;
            set => SetProperty(ref _movieName, value); 
        }

        private ObservableCollection<MovieItem> _movieItems;
        public ObservableCollection<MovieItem> MovieItems { 
            get => _movieItems; 
            set => SetProperty(ref _movieItems, value);
        }

        private MovieItem _selectedMovieItem;
        public MovieItem SelectedMovieItem
        {
            get => _selectedMovieItem;
            set 
            {
                SetProperty(ref _selectedMovieItem, value);
                Common.LOGGER.Info($"Selected Movie Item : {value.Poster_path}");
                PosterUri = new Uri($"{_base_url}{value.Poster_path}", UriKind.RelativeOrAbsolute);
            } 
        }

        private string _base_url = "https://image.tmdb.org/t/p/w300_and_h450_bestv2";

        private Uri _posterUri; 
        public Uri PosterUri { 
            get => _posterUri; 
            set => SetProperty(ref _posterUri, value);
        }

        [RelayCommand]
        public async Task SearchMovie()
        {
            //await this.dialogCoordinator.ShowMessageAsync(this, "영화 검색", MovieName);
            if (string.IsNullOrEmpty(MovieName))
            {
                await this.dialogCoordinator.ShowMessageAsync(this, "영화 검색", "영화 제목 입력");
                return;
            }

            var controller = await dialogCoordinator.ShowProgressAsync(this, "대기중", "검색 중...");
            controller.SetIndeterminate();
            SearchMovie(MovieName);
            await Task.Delay(500);
            await controller.CloseAsync();
        }

        private async void SearchMovie(string movieName)
        {
            string tmdb_apiKey = "ad3884608731e93c48ae539f6fa5a37a";   // TMDB에서 신청한 API키
            string encoding_movieName = HttpUtility.UrlEncode(movieName, Encoding.UTF8);    // 입력한 한글을 UTF-8로 변경
            string openApiUri = $"https://api.themoviedb.org/3/search/movie?api_key={tmdb_apiKey}" +
                                $"&language=ko-KR&page=1&include_adult=false&query={encoding_movieName}";
            //Debug.WriteLine(openApiUri);
            Common.LOGGER.Info($"TMDB URI : {openApiUri}");
            string result = string.Empty;

            // OpenAPI 실행할 웹 객체. WebRequest, WebResponse -> Deprecated: 추후 삭제될 예정
            //WebRequest req = null;
            //WebResponse res = null;
            HttpClient client = new HttpClient();
            ObservableCollection<MovieItem> movieItems = new ObservableCollection<MovieItem>();
            //Task<MovieSearchResponse?> response;
            //HttpResponseMessage response;
            string reader; // 응답 결과를 담는 객체

            try
            {
                //response = await client.GetAsync(openApiUri);
                var response = await client.GetFromJsonAsync<MovieSearchResponse>(openApiUri);
               
                //result = await response.Content.ReadAsStringAsync();
                foreach (var movie in response.Results)
                {
                    Common.LOGGER.Info($"{movie.Title} {movie.Release_date.ToString("yyyy-MM-dd")}");
                    movieItems.Add(movie);
                }
                //Common.LOGGER.Info($"API RESULT : {result}");
               
                //await this.dialogCoordinator.ShowMessageAsync(this, "오류", "API요청 실패");
                //Common.LOGGER.Info($"API 요청 실패");
                
            }
            catch (Exception ex)
            {
                await this.dialogCoordinator.ShowMessageAsync(this, "예외" ,ex.Message);
                Common.LOGGER.Fatal(ex.Message);
            }
            MovieItems = movieItems;    // View에 가져갈 속성에 데이터 할당
        }

        [RelayCommand]
        public async Task MovieItemDoubleClick()
        {
            var currentMovie = SelectedMovieItem;
            if (currentMovie != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{currentMovie.Original_title} ({currentMovie.Release_date.ToString("yyyy-MM-dd")})\n");
                sb.Append($"평점 : {currentMovie.Vote_average.ToString("F2")}\n");
                sb.Append("\n" + currentMovie.Overview);
                await this.dialogCoordinator.ShowMessageAsync(this, currentMovie.Title, sb.ToString());
            }
        }
    }
}

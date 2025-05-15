using _2025_MovieFinder.Helpers;
using _2025_MovieFinder.Models;
using _2025_MovieFinder.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.Serialization.DataContracts;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Threading;

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
            
            CurrDateTime = DateTime.Now.ToString("yyyy-MM-dd");

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (sender, e) =>
            {
                CurrDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Debug.WriteLine($"[DEBUG] {CurrDateTime}");
            };
            _timer.Start();

        }
        // 시계작업

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

        // ViewModel 내에서만 사용
        private string _base_url = "https://image.tmdb.org/t/p/w300_and_h450_bestv2";

        private Uri _posterUri; 
        public Uri PosterUri { 
            get => _posterUri; 
            set => SetProperty(ref _posterUri, value);
        }
        private readonly DispatcherTimer _timer; 

        private string _currDateTime;
        public string CurrDateTime
        {
            get => _currDateTime;
            set => SetProperty(ref _currDateTime, value);
        }

        private string _searchResult;
        public string SearchResult
        {
            get => _searchResult;
            set => SetProperty(ref _searchResult, value);
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
                    //Common.LOGGER.Info($"{movie.Title} {movie.Release_date.ToString("yyyy-MM-dd")}");
                    movieItems.Add(movie);
                }
                SearchResult = $"영화검색 건수 : {response.Total_results}건";
                Common.LOGGER.Info($"{MovieName} {SearchResult} 검색 완료");

                //await this.dialogCoordinator.ShowMessageAsync(this, "오류", "API요청 실패");
                //Common.LOGGER.Info($"API 요청 실패");

            }
            catch (Exception ex)
            {
                await this.dialogCoordinator.ShowMessageAsync(this, "예외" ,ex.Message);
                Common.LOGGER.Fatal(ex.Message);
                SearchResult = $"오류 발생";
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

                Common.LOGGER.Info($"{currentMovie} 상세 정보 조회 완료");
                await this.dialogCoordinator.ShowMessageAsync(this, currentMovie.Title, sb.ToString());
            }
        }

        [RelayCommand]
        public async Task AddFavoriteMovies()
        {
            //await this.dialogCoordinator.ShowMessageAsync(this, "즐겨찾기 추가", "즐겨찾기 추가");
            if (SelectedMovieItem == null)
            {
                await this.dialogCoordinator.ShowMessageAsync(this, "즐겨찾기 추가", "추가할 영화 선택");
                return;
            }
            try
            {
                var query = @"INSERT INTO movieitems
                                     (id, adult, backdrop_path, original_language, original_title, overview,
                                     popularity, poster_path, release_date, title, vote_average, vote_count)
                              VALUES
                                     (@id, @adult, @backdrop_path, @original_language, @original_title, @overview,
                                     @popularity,  @poster_path, @release_date, @title, @vote_average, @vote_count)"; 
                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", SelectedMovieItem.Id);
                    cmd.Parameters.AddWithValue("@adult", SelectedMovieItem.Adult);
                    cmd.Parameters.AddWithValue("@backdrop_path", SelectedMovieItem.Backdrop_path);
                    cmd.Parameters.AddWithValue("@original_language", SelectedMovieItem.Original_language);
                    cmd.Parameters.AddWithValue("@original_title", SelectedMovieItem.Title);
                    cmd.Parameters.AddWithValue("@overview", SelectedMovieItem.Overview);
                    cmd.Parameters.AddWithValue("@popularity", SelectedMovieItem.Popularity);
                    cmd.Parameters.AddWithValue("@poster_path", SelectedMovieItem.Poster_path);
                    cmd.Parameters.AddWithValue("@release_date", SelectedMovieItem.Release_date);
                    cmd.Parameters.AddWithValue("@title", SelectedMovieItem.Title);
                    cmd.Parameters.AddWithValue("@vote_average", SelectedMovieItem.Vote_average);
                    cmd.Parameters.AddWithValue("@vote_count", SelectedMovieItem.Vote_count);

                    var resultCount = cmd.ExecuteNonQuery();
                    if (resultCount > 0)
                    {
                        await this.dialogCoordinator.ShowMessageAsync(this, "즐겨찾기 추가", "즐겨찾기 추가 성공");
                    }
                    else
                    {
                        await this.dialogCoordinator.ShowMessageAsync(this, "즐겨찾기 추가", "즐겨찾기 추가 실패");
                    }
                }

            }
            catch (MySqlException ex)
            {
                if (ex.Message.ToUpper().Contains("DUPLICATE ENTRY"))
                {
                    Common.LOGGER.Info($"{SelectedMovieItem.Title} 즐겨찾기 추가 실패");
                    await this.dialogCoordinator.ShowMessageAsync(this, "에러", "이미 즐겨찾기 추가된 영화입니다.");
                }
                else
                {
                    await this.dialogCoordinator.ShowMessageAsync(this, "에러", ex.Message);
                }
            }
            catch (Exception ex)
            {
                await this.dialogCoordinator.ShowMessageAsync(this, "에러", ex.Message);
                Common.LOGGER.Fatal(ex.Message);
            }
        }
        [RelayCommand]
        public async Task ViewFavoriteMovies()
        {
            //await this.dialogCoordinator.ShowMessageAsync(this, "즐겨찾기 보기", "즐겨찾기 보기");
            ObservableCollection<MovieItem> movieItems = new ObservableCollection<MovieItem>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();
                    var query = @"SELECT id, adult, backdrop_path, original_language, original_title, overview,
                                          popularity, poster_path, release_date, title, vote_average, vote_count
                                    FROM movieitems";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        movieItems.Add(new MovieItem{
                            Id = reader.GetInt32("id"),
                            Adult = reader.GetBoolean("adult"),
                            //Backdrop_path = reader["backdrop_path"] == DBNull.Value? "" : reader.GetString("backdrop_path"),
                            Backdrop_path = reader.IsDBNull(2)? String.Empty : reader.GetString("backdrop_path"),
                            Original_language = reader.GetString("original_language"),
                            Original_title = reader.GetString("original_title"),
                            Overview = reader.GetString("overview"),
                            Popularity = reader.GetDouble("popularity"),
                            Poster_path = reader.GetString("poster_path"),
                            Release_date = reader.GetDateTime("release_date"),
                            Title = reader.GetString("title"),
                            Vote_average = reader.GetDouble("vote_average"),
                            Vote_count = reader.GetInt32("vote_count"),
                        });
                    }
                }
                MovieItems = movieItems;
                SearchResult = $"즐겨찾기 영화 건수 : {MovieItems.Count}건";
                Common.LOGGER.Info($"검색 완료");
            }
            catch (Exception ex)
            {
                await this.dialogCoordinator.ShowMessageAsync(this, "에러", ex.Message);
                Common.LOGGER.Fatal(ex.Message);
            }
        }
        [RelayCommand]
        public async Task DelFavoriteMovies()
        {
            //await this.dialogCoordinator.ShowMessageAsync(this, "즐겨찾기 삭제", "즐겨찾기 삭제");
            if (SelectedMovieItem == null)
            {
                await this.dialogCoordinator.ShowMessageAsync(this, "즐겨찾기 삭제", "삭제할 영화 선택");
                return;
            }
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();
                    var query = "DELETE FROM movieitems WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", SelectedMovieItem.Id);

                    var resultCount = cmd.ExecuteNonQuery();
                    if (resultCount > 0)
                    {
                        await this.dialogCoordinator.ShowMessageAsync(this, "즐겨찾기 삭제", "즐겨찾기 삭제 성공");
                    }
                    else
                    {
                        await this.dialogCoordinator.ShowMessageAsync(this, "즐겨찾기 삭제", "즐겨찾기 삭제 실패");
                    }
                }
            }
            catch (Exception ex)
            {
                await this.dialogCoordinator.ShowMessageAsync(this, "에러", ex.Message);
                Common.LOGGER.Fatal(ex.Message);
            }
            await ViewFavoriteMovies();   // 삭제 후 즐찾 다시보기
        }
        [RelayCommand]
        public async Task ViewMovieTrailer()
        {
            //await this.dialogCoordinator.ShowMessageAsync(this, "예고편 보기", "예고편 보기");
            if (SelectedMovieItem == null)
            {
                await this.dialogCoordinator.ShowMessageAsync(this, "예고편 보기", "예고편 실행할 영화 선택");
                return;
            }
            var movieTitle = SelectedMovieItem.Title;
            //MessageBox.Show(movieTitle);

            var viewModel = new TrailerViewModel(Common.DIALOGCOORDINATOR, movieTitle);
            viewModel.MovieTitle = movieTitle;
            var view = new TrailerView 
            {
                DataContext = viewModel,
            };
            view.Owner = Application.Current.MainWindow;
            Common.LOGGER.Info($"{SelectedMovieItem.Title} PV 영상 보기");
            view.ShowDialog();
            
        }
    }
}

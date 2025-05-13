using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using WpfBookRentalShop01.Helpers;
using WpfBookRentalShop01.Models;

namespace WpfBookRentalShop01.ViewModels
{
    public partial class BookGenreViewModel : ObservableObject
    {
        private readonly IDialogCoordinator dialogCoordinator;  // MainViewModel과 동일

        private ObservableCollection<Genre> _genres;
        public ObservableCollection<Genre> Genres
        {
            get => _genres;
            set => SetProperty(ref _genres, value);
        }

        private Genre _selectedGenre;
        public Genre SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                SetProperty(ref _selectedGenre, value);
                _isUpdate = true;  // 수정할 상태
            }
        }

        private bool _isUpdate;

        public BookGenreViewModel(IDialogCoordinator coordinator)
        {
            this.dialogCoordinator = coordinator;
            InitVariable();
            LoadGridFromDb();
        }

        private void InitVariable()
        {
            SelectedGenre = new Genre();
            SelectedGenre.Names = string.Empty;
            SelectedGenre.Division = string.Empty;
            _isUpdate = false; // 신규 상태
        }

        private async void LoadGridFromDb()
        {
            try
            {
                //string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
                string query = "SELECT division, names FROM divtbl";

                ObservableCollection<Genre> genres = new ObservableCollection<Genre>();

                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var division = reader.GetString("division");
                        var names = reader.GetString("names");

                        genres.Add(new Genre
                        {
                            Division = division,
                            Names = names
                        });
                    }
                }
                Genres = genres;
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error(ex.Message);
                //MessageBox.Show(ex.Message);
                await this.dialogCoordinator.ShowMessageAsync(this, "오류", ex.Message);
            }
            Common.LOGGER.Info("책 장르 데이터 로드");
        }

        // SetInitCommand, SaveDataCommand, DelDataCommand
        [RelayCommand]
        public void SetInit()
        {
            // SelectedGenre = null; // 위험한 행동
            InitVariable();
        }

        [RelayCommand]
        public async void SaveData()
        {
            // 신규 추가/ 기존 데이터 수정
            //Debug.WriteLine(SelectedGenre.Names);
            //Debug.WriteLine(SelectedGenre.Division);
            //Debug.WriteLine(_isUpdate);
            try
            {
                //string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
                string query = string.Empty;
                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();
                    if (_isUpdate)  // 기존 데이터 수정
                    {
                        query = "UPDATE divtbl SET names = @names WHERE division = @division";
                    }
                    else            // 신규 등록
                    {
                        query = "INSERT INTO divtbl VALUES (@division, @names)";
                    }
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@division", SelectedGenre.Division);
                    cmd.Parameters.AddWithValue("@names", SelectedGenre.Names);

                    var resultCnt = cmd.ExecuteNonQuery();
                    if (resultCnt > 0)
                    {
                        Common.LOGGER.Info("책 장르 데이터 저장 성공");

                        //MessageBox.Show("저장 성공");
                        await this.dialogCoordinator.ShowMessageAsync(this, "저장", "저장 성공");
                    }
                    else
                    {
                        Common.LOGGER.Warn("책 장르 데이터 저장 실패");
                        //MessageBox.Show("저장 실패");
                        await this.dialogCoordinator.ShowMessageAsync(this, "저장", "저장 실패");
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error(ex.Message);
                //MessageBox.Show(ex.Message);
                await this.dialogCoordinator.ShowMessageAsync(this, "오류", ex.Message);
            }

            LoadGridFromDb();   // 저장 끝난 후 다시 DB 내용 그리드에 그리기
        }

        [RelayCommand]
        public async void DelData()
        {
            if (_isUpdate == false)
            {
                //MessageBox.Show("선택된 데이터가 아니면 삭제할 수 없습니다.");
                await this.dialogCoordinator.ShowMessageAsync(this, "삭제할 데이터 선택", "삭제할 데이터 선택");

                return;
            }

            var result = await this.dialogCoordinator.ShowMessageAsync(this, "삭제 여부", "삭제하시겠습니까?", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Negative)
            {
                return;
            }

            try
            {
                //string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
                string query = "DELETE FROM divtbl WHERE division = @division";

                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@division", SelectedGenre.Division);

                    int resultCnt = cmd.ExecuteNonQuery(); // 한건 삭제가되면 resultCnt = 1, 안지워지면 resultCnt = 0

                    if (resultCnt > 0)
                    {
                        Common.LOGGER.Info($"책 장르 {SelectedGenre.Division} {SelectedGenre.Names} 삭제 성공");
                        //MessageBox.Show("삭제성공");
                        await this.dialogCoordinator.ShowMessageAsync(this, "삭제", "삭제 성공");

                    }
                    else
                    {
                        Common.LOGGER.Warn("책 장르 데이터 삭제 실패");
                        //MessageBox.Show("삭제실패");
                        await this.dialogCoordinator.ShowMessageAsync(this, "삭제", "삭제 실패");

                    }
                }
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error(ex.Message);
                //MessageBox.Show(ex.Message);
                await this.dialogCoordinator.ShowMessageAsync(this, "오류", ex.Message);
            }
            LoadGridFromDb();
        }
    }
}
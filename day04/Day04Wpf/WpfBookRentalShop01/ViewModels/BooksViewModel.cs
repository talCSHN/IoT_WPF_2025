﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBookRentalShop01.Helpers;
using WpfBookRentalShop01.Models;

namespace WpfBookRentalShop01.ViewModels
{
    public partial class BooksViewModel : ObservableObject
    {
        private readonly IDialogCoordinator dialogCoordinator;

        public ObservableCollection<KeyValuePair<string, string>> Divisions { get; set; }

        private ObservableCollection<Book> _books;
        public ObservableCollection<Book> Books
        {
            get => _books;
            set => SetProperty(ref _books, value);
        }

        private Book _selectedBook;
        public Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                SetProperty(ref _selectedBook, value);
                _isUpdated = true;
            }
        }

        private bool _isUpdated;
        public BooksViewModel(IDialogCoordinator coordinator)
        {
            this.dialogCoordinator = coordinator;
            InitVariable();
            LoadGridFromDb();
            LoadControlFromDb();
        }

        private void InitVariable()
        {
            SelectedBook = new Book
            {
                Idx = 0,
                Author = string.Empty,
                Division = string.Empty,
                Names = string.Empty,
                ReleaseDate = DateTime.Now,
                ISBN = string.Empty,
                Price = 0
            };
            _isUpdated = false; // 신규 상태
        }

        private void LoadControlFromDb()
        {
            // 사용 쿼리
            string query = "SELECT division, names FROM divtbl";

            // Dictionary나 KeyValuePair 둘 다 상관 x
            ObservableCollection<KeyValuePair<string, string>> divisions = new ObservableCollection<KeyValuePair<string, string>>();

            // 3. DB연결, 명령, 리더
            using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();   // 데이터 가져올 때

                    while (reader.Read())
                    {
                        var division = reader.GetString("division");
                        var names = reader.GetString("names");

                        divisions.Add(new KeyValuePair<string, string>(division, names));
                    }
                }
                catch (MySqlException ex)
                {

                }
            }   // conn.Close() 자동 실행됨
            Divisions = divisions;
        }
        private async void LoadGridFromDb()
        {
            try
            {
                //string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
                string query = "SELECT b.idx, b.author, b.division, b.names, b.releaseDate, b.isbn, b.price, d.names as dNames  FROM bookstbl b, divtbl d WHERE b.Division = d.Division ORDER BY b.Idx";

                ObservableCollection<Book> books = new ObservableCollection<Book>();

                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var idx = reader.GetInt32("Idx");
                        var author = reader.GetString("author");
                        var dnames = reader.GetString("dNames");
                        var division = reader.GetString("division");
                        var names = reader.GetString("names");
                        var releaseDate = reader.GetDateTime("releaseDate");
                        var isbn = reader.GetString("isbn");
                        var price = reader.GetInt32("price");

                        books.Add(new Book
                        {
                            Idx = idx,
                            Author = author,
                            DNames = dnames,
                            Division = division,
                            Names = names,
                            ReleaseDate = releaseDate,
                            ISBN = isbn,
                            Price = price,
                        });
                    }
                }
                Books = books;  // View에 바인딩
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error(ex.Message);
                //MessageBox.Show(ex.Message);
                await this.dialogCoordinator.ShowMessageAsync(this, "오류", ex.Message);
            }
            Common.LOGGER.Info("멤버 데이터 로드");
        }
        [RelayCommand]
        public void SetInit()
        {
            InitVariable();
        }
        [RelayCommand]
        public async void SaveData()
        {
            try
            {
                string query = string.Empty;
                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();
                    if (_isUpdated)
                    {
                        query = @"UPDATE bookstbl
                                     SET author = @author,
                                         division = @division,
                                         names = @names,
                                         releaseDate = @releaseDate,
                                         isbn = @isbn,
                                         price = @price
                                   WHERE idx = @idx";
                    }
                    else
                    {
                        query = @"INSERT INTO bookstbl (author, division, names, releaseDate, isbn, price)
                                                 VALUES (@author, @division, @names, @releaseDate, @isbn, @price);";
                    }
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@author", SelectedBook.Author);
                    //cmd.Parameters.AddWithValue("@d.names", SelectedBook.DNames);
                    cmd.Parameters.AddWithValue("@division", SelectedBook.Division);
                    cmd.Parameters.AddWithValue("@names", SelectedBook.Names);
                    cmd.Parameters.AddWithValue("@releaseDate", SelectedBook.ReleaseDate);
                    cmd.Parameters.AddWithValue("@isbn", SelectedBook.ISBN);
                    cmd.Parameters.AddWithValue("@price", SelectedBook.Price);
                    // 업데이트일 때만 @idx 필요
                    if (_isUpdated)
                    {
                        cmd.Parameters.AddWithValue("@idx", SelectedBook.Idx);
                    }
                    var resultCount = cmd.ExecuteNonQuery();
                    if (resultCount > 0)
                    {
                        Common.LOGGER.Info("책 데이터 저장 성공");
                        await this.dialogCoordinator.ShowMessageAsync(this, "저장", "저장 성공");
                    }
                    else
                    {
                        Common.LOGGER.Warn("책 데이터 저장 실패");
                        await this.dialogCoordinator.ShowMessageAsync(this, "저장", "저장 실패");
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error(ex.Message);
                await this.dialogCoordinator.ShowMessageAsync(this, "오류", ex.Message);
            }
            LoadGridFromDb();
        }

        [RelayCommand]
        public async void DelData()
        {
            if (!_isUpdated)
            {
                await this.dialogCoordinator.ShowMessageAsync(this, "삭제", "데이터 선택");
                return;
            }
            var result = await this.dialogCoordinator.ShowMessageAsync(this, "삭제 여부", "삭제하시겠습니까?", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Negative)
            {
                return; // Cancel 하면 메서드 빠져나감
            }
            try
            {
                string query = "DELETE FROM bookstbl WHERE idx = @idx";
                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idx", SelectedBook.Idx);

                    int resultCount = cmd.ExecuteNonQuery();
                    if (resultCount > 0)
                    {
                        Common.LOGGER.Info($"책 데이터 {SelectedBook.Idx} / {SelectedBook.Names} 삭제 완료");
                        await this.dialogCoordinator.ShowMessageAsync(this, "삭제", "삭제 성공");
                    }
                    else
                    {
                        Common.LOGGER.Warn("책 데이터 삭제 실패");
                        await this.dialogCoordinator.ShowMessageAsync(this, "삭제", "삭제 실패");
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error(ex.Message);
                await this.dialogCoordinator.ShowMessageAsync(this, "삭제", ex.Message);
            }
            LoadGridFromDb();

        }
    }
}

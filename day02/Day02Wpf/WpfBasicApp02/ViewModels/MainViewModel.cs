using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using WpfBasicApp02.Models;

namespace WpfBasicApp02.ViewModels
{
    public class MainViewModel : Conductor<object>
    {
        private IDialogCoordinator _dialogCoordinator;  // 메시지박스, 다이얼로그 실행을 위한 변수
        public ObservableCollection<KeyValuePair<string, string>> Divisions { get; set; }
        public ObservableCollection<Book> Books { get; set; }
        private Book _selectedBook;
        public Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                NotifyOfPropertyChange(() => SelectedBook);
            }
        }
        public MainViewModel()
        {
            _dialogCoordinator = new DialogCoordinator();
            LoadControlFromDb();
            LoadGridFromDb();
        }

        private void LoadControlFromDb()
        {
            // 1. DB연결문자열(필수)
            string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
            // 2. 사용 쿼리
            string query = "SELECT division, names FROM divtbl";

            // Dictionary나 KeyValuePair 둘 다 상관 x
            ObservableCollection<KeyValuePair<string, string>> divisions = new ObservableCollection<KeyValuePair<string, string>>();

            // 3. DB연결, 명령, 리더
            using (MySqlConnection conn = new MySqlConnection(connectionString))
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
            NotifyOfPropertyChange(() => Divisions);    // Caliburn.Micro가 제공하는 메서드
        }

        private void LoadGridFromDb()
        {
            // 1. DB연결문자열(필수)
            string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";

            // 2. 사용 쿼리
            string query = @"SELECT b.Idx, b.Author, b.Division, b.Names, b.ReleaseDate, b.ISBN, b.Price, d.Names AS dNames
                               FROM bookstbl AS b, divtbl AS d
                              WHERE b.Division = d.Division
                              ORDER BY b.Idx;";

            ObservableCollection<Book> books = new ObservableCollection<Book>();

            // 3. DB연결, 명령, 리더
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        books.Add(new Book
                        {
                            Idx = reader.GetInt32("Idx"),
                            Division = reader.GetString("Division"),
                            DNames = reader.GetString("dNames"),
                            Names = reader.GetString("Names"),
                            Author = reader.GetString("Author"),
                            ISBN = reader.GetString("ISBN"),
                            ReleaseDate = reader.GetDateTime("ReleaseDate"),
                            Price = reader.GetInt32("Price"),
                        });
                    }
                }
                catch (MySqlException ex)
                {

                }
            }   // conn.Close() 자동 실행됨
            Books = books;
            NotifyOfPropertyChange(() => Books);
        }
        public async void DoAction()
        {
            await _dialogCoordinator.ShowMessageAsync(this, "데이터로드", "로드");
        }
    }
}

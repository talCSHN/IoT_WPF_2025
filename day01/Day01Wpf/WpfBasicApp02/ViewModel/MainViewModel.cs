using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBasicApp02.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            LoadControlFromDb();
            LoadGridFromDb();
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

            // 3. DB연결, 명령, 리더
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                }
                catch (MySqlException ex)
                {

                }
            }   // conn.Close() 자동 실행됨
        }

        private void LoadControlFromDb()
        {
            // 1. DB연결문자열(필수)
            string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
            // 2. 사용 쿼리
            string query = "SELECT division, names FROM divtbl";

            // Dictionary나 KeyValuePair 둘 다 상관 x
            List<KeyValuePair<string, string>> divisions = new List<KeyValuePair<string, string>>();

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
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

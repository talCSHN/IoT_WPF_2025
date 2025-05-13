using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class MembersViewModel : ObservableObject
    {
        private readonly IDialogCoordinator dialogCoordinator;

        private ObservableCollection<Member> _members;
        public ObservableCollection<Member> Members { 
            get => _members; 
            set => SetProperty(ref _members, value);
        }

        private Member _selectedMember;
        public Member SelectedMember { 
            get => _selectedMember;
            set {
                SetProperty(ref _selectedMember, value);
                _isUpdated = true;
            } 
        }

        private bool _isUpdated;
        //public bool IsUpdated { 
        //    get => _isUpdated;
        //    set {
        //            SetProperty(ref _isUpdated, value);
                      
        //        }
        //}

        public MembersViewModel(IDialogCoordinator coordinator)
        {
            this.dialogCoordinator = coordinator;
            InitVariable();
            LoadGridFromDb();
        }

        private void InitVariable()
        {
            SelectedMember = new Member
            {
                Idx = 0,
                Names = string.Empty,
                Levels = string.Empty,
                Addr = string.Empty,
                Mobile = string.Empty,
                Email = string.Empty,
            };
            _isUpdated = false; // 신규 상태

        }

        private async void LoadGridFromDb()
        {
            try
            {
                //string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
                string query = "SELECT idx, names, levels, addr, mobile, email FROM membertbl";

                ObservableCollection<Member> members = new ObservableCollection<Member>();

                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var idx = reader.GetInt32("Idx");
                        var names = reader.GetString("names");
                        var levels = reader.GetString("levels");
                        var addr = reader.GetString("addr");
                        var mobile = reader.GetString("mobile");
                        var email = reader.GetString("email");

                        members.Add(new Member
                        {
                            Idx = idx,
                            Names = names,
                            Levels = levels,
                            Addr = addr,
                            Mobile = mobile,
                            Email = email,
                        });
                    }
                }
                Members = members;  // View에 바인딩
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
                        query = @"UPDATE membertbl
                                     SET names = @names,
                                         levels = @levels,
                                         addr = @addr,
                                         mobile = @mobile,
                                         email = @email
                                   WHERE idx = @idx";
                    }
                    else
                    {
                        query = @"INSERT INTO membertbl (names, levels, addr, mobile, email)
                                                 VALUES (@names, @levels, @addr, @mobile, @email);";
                    }
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@names", SelectedMember.Names);
                    cmd.Parameters.AddWithValue("@levels", SelectedMember.Levels);
                    cmd.Parameters.AddWithValue("@addr", SelectedMember.Addr);
                    cmd.Parameters.AddWithValue("@mobile", SelectedMember.Mobile);
                    cmd.Parameters.AddWithValue("@email", SelectedMember.Email);
                    // 업데이트일 때만 @idx 필요
                    if (_isUpdated)
                    {
                        cmd.Parameters.AddWithValue("@idx", SelectedMember.Idx);
                    }
                    var resultCount = cmd.ExecuteNonQuery();
                    if (resultCount > 0)
                    {
                        Common.LOGGER.Info("회원 데이터 저장 성공");
                        await this.dialogCoordinator.ShowMessageAsync(this, "저장", "저장 성공");
                    }
                    else
                    {
                        Common.LOGGER.Warn("회원 데이터 저장 실패");
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
                string query = "DELETE FROM membertbl WHERE idx = @idx";
                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idx", SelectedMember.Idx);

                    int resultCount = cmd.ExecuteNonQuery();
                    if (resultCount > 0)
                    {
                        Common.LOGGER.Info($"멤버 데이터 {SelectedMember.Idx} / {SelectedMember.Names} 삭제 완료");
                        await this.dialogCoordinator.ShowMessageAsync(this, "삭제", "삭제 성공");
                    }
                    else
                    {
                        Common.LOGGER.Warn("멤버 데이터 삭제 실패");
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

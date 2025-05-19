using BusanRestaurantApp.Helpers;
using BusanRestaurantApp.Models;
using BusanRestaurantApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace BusanRestaurantApp.ViewModels
{
    public partial class BusanMatjibViewModel : ObservableObject
    {
        IDialogCoordinator dialogCoordinator;

        public BusanMatjibViewModel(IDialogCoordinator coordinator)
        {
            dialogCoordinator = coordinator;
            PageNo = 1; NumOfRows = 10;

            GetDataFromOpenAPI();
        }
        private ObservableCollection<BusanItem> _busanItems;
        
        

        public ObservableCollection<BusanItem> BusanItems { 
            get => _busanItems; 
            set => SetProperty(ref _busanItems, value); 
        }
        private int _pageNo;
        public int PageNo { 
            get => _pageNo; 
            set => SetProperty(ref _pageNo, value); 
        }
        private int _numOfRows;

        public int NumOfRows { 
            get => _numOfRows; 
            set => SetProperty(ref _numOfRows, value); 
        }

        private BusanItem _selectedIndividualItem;
        public BusanItem SelectedIndividualItem
        {
            get => _selectedIndividualItem;
            set => SetProperty(ref _selectedIndividualItem, value);
        }

        [RelayCommand]
        public async Task ShowDetails()
        {
            var viewModel = new GoogleMapViewModel();
            var view = new GoogleMapView
            {
                DataContext = viewModel,
            };
            viewModel.SelectedIndividualItem = SelectedIndividualItem;  // 메인창에 있는 선택된 아이템을 그대로 구글맵 화면에 전달
            view.Owner = Application.Current.MainWindow;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Common.LOGGER.Info("구글맵 오픈");
            Common.LOGGER.Info($"위도/경도 : {SelectedIndividualItem.Lat} {SelectedIndividualItem.Lng}");
            view.ShowDialog();
        }

        [RelayCommand]
        public void OnPangeNoChanged(int value)
        {
            MessageBox.Show("Test");
        }

        [RelayCommand]
        private async Task GetDataFromOpenAPI()
        {
            string baseUri = "http://apis.data.go.kr/6260000/FoodService/getFoodKr?";
            string myServiceKey = "hPEKJXpajImUbM7ToBXQyIKmXff1FJ0b7uXN0HLnN1ptsrbBHeVGQ8s0QKmD2p8KuwIYjYbfVghQmqJ4PFBWcQ%3D%3D";
            // serviceKey=hPEKJXpajImUbM7ToBXQyIKmXff1FJ0b7uXN0HLnN1ptsrbBHeVGQ8s0QKmD2p8KuwIYjYbfVghQmqJ4PFBWcQ%3D%3D&pageNo=1&numOfRows=100&resultType=json
            StringBuilder strUri = new StringBuilder();
            strUri.Append($"serviceKey={myServiceKey}&");
            strUri.Append($"pageNo={PageNo}&");
            strUri.Append($"numOfRows={NumOfRows}&");
            strUri.Append($"resultType=json");
            string totalOpenApi = $"{baseUri}{strUri.ToString()}";
            Common.LOGGER.Info(totalOpenApi);

            HttpClient client = new HttpClient();
            ObservableCollection<BusanItem> busanItems = new ObservableCollection<BusanItem>();

            try
            {
                var response = await client.GetStringAsync(totalOpenApi);
                Common.LOGGER.Info("식당데이터 로드 성공");

                // Newtonsoft.Json으로 Json 데이터 처리
                var jsonResult = JObject.Parse(response);
                var message = jsonResult["getFoodKr"]["header"]["message"];
                //await this.dialogCoordinator.ShowMessageAsync(this, "결과 메시지", message.ToString());
                var status = Convert.ToString(jsonResult["getFoodKr"]["header"]["code"]); // code : 00 이면 성공
                JsonSerializerOptions options = new JsonSerializerOptions { IncludeFields = true };
                if (status == "00")
                {
                    var item = jsonResult["getFoodKr"]["item"];
                    var jsonArray = item as JArray;

                    foreach (var subitem in jsonArray)
                    {
                        //Common.LOGGER.Info(subitem.ToString());
                        busanItems.Add(new BusanItem
                        {
                            //Uc_Seq = JsonSerializer.Deserialize<BusanItem>(subitem["UC_SEQ"], options),
                            Uc_Seq = Convert.ToInt32(subitem["UC_SEQ"]),
                            Main_Title = Convert.ToString(subitem["MAIN_TITLE"]),
                            Gugun_Nm = Convert.ToString(subitem["GUGUN_NM"]),
                            Lat = Convert.ToDouble(subitem["LAT"]),
                            Lng = Convert.ToDouble(subitem["LNG"]),
                            Place = Convert.ToString(subitem["PLACE"]),
                            Title = Convert.ToString(subitem["TITLE"]),
                            SubTitle = Convert.ToString(subitem["SUBTITLE"]),
                            Addr1 = Convert.ToString(subitem["ADDR1"]).Replace("\n", ""),
                            Addr2 = Convert.ToString(subitem["ADDR2"]),
                            Cntct_Tel = Convert.ToString(subitem["CNTCT_TEL"]),
                            Homepage_Url = Convert.ToString(subitem["HOMEPAGE_URL"]),
                            Usage_Day_Week_And_Time = Convert.ToString(subitem["USAGE_DAY_WEEK_AND_TIME"]),
                            Rprsntv_Menu = Convert.ToString(subitem["RPRSNTV_MENU"]).Replace("\n", ""),
                            Main_Img_Normal = Convert.ToString(subitem["MAIN_IMG_NORMAL"]),
                            Main_Img_Thumb = Convert.ToString(subitem["MAIN_IMG_THUMB"]),
                            ItemCntnts = Convert.ToString(subitem["ITEMCNTNTS"]),
                        });
                    }
                    BusanItems = busanItems;
                }
            }
            catch (Exception ex)
            {
                await this.dialogCoordinator.ShowMessageAsync(this, "에러", ex.Message);
                Common.LOGGER.Fatal(ex.Message);
            }
        }
    }
}

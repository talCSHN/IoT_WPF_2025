using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfSmartHomeApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        // 온도
        private double _homeTemp;
        public double HomeTemp
        {
            get => _homeTemp;
            set => SetProperty(ref _homeTemp, value);
        }

        // 습도
        private double _homeHumid;
        public double HomeHumid
        {
            get => _homeHumid;
            set => SetProperty(ref _homeHumid, value);
        }

        // 사람 인지
        [ObservableProperty]
        private string detectResult;

        // 사람 인지여부
        [ObservableProperty]
        private bool isDetectOn;

        [ObservableProperty]
        private string rainResult;

        // 사람 인지여부
        [ObservableProperty]
        private bool isRainOn;

        [ObservableProperty]
        private string airConResult;

        // 사람 인지여부
        [ObservableProperty]
        private bool isAirConOn;

        [ObservableProperty]
        private string lightResult;

        // 사람 인지여부
        [ObservableProperty]
        private bool isLightOn;

        // LoadedCommand에서 앞에 On붙고 Command 삭제
        [RelayCommand]
        public void OnLoaded()
        {
            HomeTemp = 27;
            HomeHumid = 30;

            DetectResult = "Human Access Detected";
            IsDetectOn = true;
            RainResult = "Raining";
            IsRainOn = true;
            AirConResult = "Air Conditioner ON";
            IsAirConOn = true;
            LightResult = "Light ON";
            IsLightOn = true;
        }
    }
}

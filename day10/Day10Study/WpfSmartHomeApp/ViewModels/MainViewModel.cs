using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MQTTnet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfSmartHomeApp.Helpers;
using WpfSmartHomeApp.Models;

namespace WpfSmartHomeApp.ViewModels
{
    public partial class MainViewModel : ObservableObject, IDisposable
    {
        // readonly는 생성자에서만 값 할당 가능
        private readonly DispatcherTimer _timer;

        [ObservableProperty]
        private string? currDateTime;

        // MQTT용 필드
        private string TOPIC;
        private IMqttClient mqttClient;
        private string BROKERHOST;

        public MainViewModel()
        {
            CurrDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (sender, e) =>
            {
                CurrDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            };
            _timer.Start();
        }
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
        public async Task OnLoaded()
        {
            //// 테스트용 더미데이터
            //HomeTemp = 27;
            //HomeHumid = 30;

            //DetectResult = "Human Access Detected";
            //IsDetectOn = true;
            //RainResult = "Raining";
            //IsRainOn = true;
            //AirConResult = "Air Conditioner ON";
            //IsAirConOn = true;
            //LightResult = "Light ON";
            //IsLightOn = true;

            // MQTT 접속 ~ 실행
            TOPIC = "pknu/sh01/data";   // publish, subscribe 시 가장 중요
            BROKERHOST = "210.119.12.52";   // SmartHome MQTT Broker IP

            var mqttFactory = new MqttClientFactory();
            mqttClient = mqttFactory.CreateMqttClient();

            // MQTT 클라이언트 접속 설정변수
            var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(BROKERHOST)
                                                                  .WithCleanSession(true)
                                                                  .Build();

            // MQTT 접속 확인 이벤트 메서드 선언
            mqttClient.ConnectedAsync += MqttClient_ConnectedAsync;

            // MQTT 구독메시지 확인 메서드 선언
            mqttClient.ApplicationMessageReceivedAsync += MqttClient_ApplicationMessageReceivedAsync;

            await mqttClient.ConnectAsync(mqttClientOptions);   // MQTT Broker 접속


        }

        private Task MqttClient_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            var topic = arg.ApplicationMessage.Topic;   // pknu/sh01/data
            var payload = arg.ApplicationMessage.ConvertPayloadToString();  // byte -> UTF-8 문자열 변환

            var realtimeData = JsonConvert.DeserializeObject<SensingInfo>(payload);
            Common.LOGGER.Info(@$"Light : {realtimeData.L} / Rain : {realtimeData.R} / Temperature : {realtimeData.T}
                                 Humid : {realtimeData.H} / Fan : {realtimeData.F} / Vulernability : {realtimeData.V}
                                 RealLight : {realtimeData.RL} / ChaimBell : {realtimeData.CB}");
            HomeTemp = realtimeData.T;
            HomeHumid = realtimeData.H;

            IsDetectOn = realtimeData.V == "ON"? true : false;
            DetectResult = realtimeData.V == "ON" ? "Detection State" : "Normal State";

            IsLightOn = realtimeData.RL == "ON"? true: false;
            LightResult = realtimeData.RL == "ON"? "LIGHT ON": "LIGHT OFF";

            IsAirConOn = realtimeData.F == "ON" ? true : false;
            AirConResult = realtimeData.F == "ON" ? "ON" : "OFF";

            IsRainOn = realtimeData.R <= 350? true : false;
            RainResult = realtimeData.R <= 350 ? "RAINING" : "NO Rain";

            return Task.CompletedTask;  // 구독 종료 알려주는 리턴
        }

        // MQTT 접속 확인 이벤트
        private async Task MqttClient_ConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            Common.LOGGER.Info($"{arg}");
            Common.LOGGER.Info($"MQTT BROKER 접속 성공");
            await mqttClient.SubscribeAsync(TOPIC);
        }

        public void Dispose()
        {
            // TODO : 나중에 리소스 해제 추가
        }
    }
}

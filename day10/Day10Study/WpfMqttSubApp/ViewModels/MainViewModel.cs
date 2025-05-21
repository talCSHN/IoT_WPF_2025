using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using MQTTnet;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfMqttSubApp.Models;

namespace WpfSmartHomeApp.ViewModels
{
    // BrokerHost, DatabaseHost
    // ConnectBroker, ConnectDatabase
    public partial class MainViewModel : ObservableObject, IDisposable
    {
        private readonly string TOPIC;
        private IMqttClient mqttClient;
        private readonly IDialogCoordinator dialogCoordinator;
        [ObservableProperty]
        private string logText;

        [ObservableProperty]
        private string brokerHost;

        [ObservableProperty]
        private string databaseHost;

        private readonly DispatcherTimer timer;
        private int lineCounter = 1;    // TODO : 나중에 텍스트가 너무 많아져서 느려지면 초기화시 사용

        private string connString = string.Empty;
        //private readonly MySqlConnection connection;
        private MySqlConnection connection;


        public MainViewModel(IDialogCoordinator coordinator)
        {
            this.dialogCoordinator = coordinator;

            BrokerHost = "210.119.12.52";   // 강사 PC
            DatabaseHost = "210.119.12.54"; // 내 PC
            TOPIC = "pknu/sh01/data";

            connection = new MySqlConnection(); // 예외처리용

            // RichTextBox 테스트용
            //timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick += (sender, e) =>
            //{
            //    // RichTextBox 추가 내용
            //    LogText += $"Log [{DateTime.Now:HH:mm:ss}] - {lineCounter++}\n";
            //    Debug.WriteLine(LogText);
            //};
            //timer.Start();
        }

        

        private async Task ConnectMqttBroker()
        {
            // MQTT 클라이언트 생성
            var mqttFactory = new MqttClientFactory();
            mqttClient = mqttFactory.CreateMqttClient();

            // MQTT 클라이언트 접속 설정
            var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(BrokerHost)
                                                                  .WithCleanSession(true)
                                                                  .Build();

            // MQTT 접속 후 이벤트 처리
            mqttClient.ConnectedAsync += async e =>
            {
                LogText += "MQTT Broker Connected\n";
                // 연결 이후 구독(Subscribe)
                await mqttClient.SubscribeAsync(TOPIC);
            };

            // MQTT 구독메시지 로그 출력
            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                var topic = e.ApplicationMessage.Topic;
                var payload = e.ApplicationMessage.ConvertPayloadToString();    // byte 데이터를 UTF-8 문자열로 변환

                // json으로 변경
                var data = JsonConvert.DeserializeObject<SensingInfo>(payload);
                Console.WriteLine($"{data.L} / {data.R} / {data.T} / {data.H}");

                SaveSensingData(data);

                LogText += $"LINE NUMBER : {lineCounter++}\n";
                LogText += $"PAYLOAD\n{payload}\n--------------------------------------------------------\n";

                return Task.CompletedTask;
            };

            await mqttClient.ConnectAsync(mqttClientOptions);   // MQTT 서버 접속

        }

        private async Task SaveSensingData(SensingInfo data)
        {
            string query = @"INSERT INTO sensingdatas
                                         (sensing_dt, light, rain, temp, humid, fan, vul, real_light, chaim_bell)
                                    VALUES
                                        (now(), @light,  @rain, @temp, @humid, @fan, @vul, @real_light, @chaim_bell);";
            try
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    using var cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@light", data.L);
                    cmd.Parameters.AddWithValue("@rain", data.R);
                    cmd.Parameters.AddWithValue("@temp", data.T);
                    cmd.Parameters.AddWithValue("@humid", data.H);
                    cmd.Parameters.AddWithValue("@fan", data.F);
                    cmd.Parameters.AddWithValue("@vul", data.V);
                    cmd.Parameters.AddWithValue("@real_light", data.RL);
                    cmd.Parameters.AddWithValue("@chaim_bell", data.CB);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                // TODO
            }
            
        }

        [RelayCommand]
        public async Task ConnectBroker()
        {
            if (string.IsNullOrEmpty(BrokerHost))
            {
                await this.dialogCoordinator.ShowMessageAsync(this, "브로커 연결", "브로커 호스트 없음");
                return;
            }
            // MQTT 브로커에 접속해서 데이터 가져오기
            ConnectMqttBroker();
        }


        private async Task ConnectDBServer()
        {
            try
            {
                connection = new MySqlConnection(connString);
                connection.Open();
                LogText += $"{DatabaseHost} DB Server Connection Success | State : {connection.State}\n";
            }
            catch (Exception ex)
            {
                LogText += $"{DatabaseHost} DB Server Connection failed : {ex.Message}\n";
            }
        }
        [RelayCommand]
        public async Task ConnectDatabase()
        {
            if (string.IsNullOrEmpty(DatabaseHost))
            {
                await this.dialogCoordinator.ShowMessageAsync(this, "DB 연결", "DB 호스트 없음");
                return;
            }
            connString = $"Server={DatabaseHost};Database=smarthome;Uid=root;Pwd=12345;Charset=utf8";

            await ConnectDBServer();
        }

        public void Dispose()
        {
            // 리소스 해제를 명시적으로 처리하는 기능 추가
            connection?.Close();    // DB 접속 해제
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfBasicApp01.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        // NLog 객체 생성
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region 속성영역
        private string _greeting;
        public string Greeting { 
            get => _greeting;
            set => SetProperty(ref _greeting, value);   // CommunityToolkit.Mvvm 핵심. C++ & == ref
        }

        private string _currentTime;
        public string CurrentTime { 
            get => _currentTime; 
            set => SetProperty(ref _currentTime, value);
        }
        private readonly DispatcherTimer _timer;    // ViewModel 내에서만 사용

        #endregion
        public MainViewModel()
        {
            _logger.Info("ViewModel 시작");

            Greeting = "Hello, WPF MVVM";
            
            CurrentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);  // 1초마다 변경
            //_timer.Tick += _timer_Tick;
            _timer.Tick += (sender, e) =>
            {
                CurrentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Debug.WriteLine($"[DEBUG] {CurrentTime}");
                _logger.Info($"[DEBUG] {CurrentTime}");
            };
            _timer.Start(); // 타이머 시작
        }
    }
}

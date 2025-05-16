using MahApps.Metro.Controls.Dialogs;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusanRestaurantApp.Helpers
{
    public class Common
    {
        // NLog 인스턴스
        public static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();

        // DB연결문자열을 한군데 저장
        //public static readonly string CONNSTR = "Server=127.0.0.1;Database=moviefinder;Uid=root;Pwd=12345;Charset=utf8";

        // MahApps.Metro 다이얼로그
        public static IDialogCoordinator DIALOGCOORDINATOR;
    }
}

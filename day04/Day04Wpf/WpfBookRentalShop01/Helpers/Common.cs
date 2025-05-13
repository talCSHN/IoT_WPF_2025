using MahApps.Metro.Controls.Dialogs;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBookRentalShop01.Helpers
{
    // 프로젝트 내에서 같이 쓸 수 있는 공통 클래스
    public class Common
    {
        // NLog 인스턴스 생성
        public static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();
        // DB 연결 문자열을 한 군데에 지정
        public static readonly string CONNSTR = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
        // MahApps.Metro 형태 다이얼로그 코디네이터
        public static IDialogCoordinator DIALOGCODINATOR;
    }
}

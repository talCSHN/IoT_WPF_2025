# IoT_WPF_2025
WPF Study

## 1일차
- Windows Presentation Foundation
    - 미흡한 WinForms 디자인, 속도 개선, `개발과 디자인 분리` 개선을 위해 만들어진 MS 프레임워크
    - 화면디자인을 XML 기반의 xaml

### WPF DB 바인딩
1. 프로젝트 생성 - [디자인](./day01/Day01Wpf/WpfBasicApp01/MainWindow.xaml), [소스](./day01/Day01Wpf/WpfBasicApp01/MainWindow.xaml.cs)
2. NuGet 패키지에서 `MahApps.Metro(디자인)` 라이브러리 설치
3. 디자인 영역
    - App.xaml
    - MainWindow.xaml
    - MainWindow.xaml.cs의 기반클래스를 변경하는 것 - 디자인
4. UI구현
5. DB연결 사전준비
    - NuGet 패키지에서 `MySQL.Data` 라이브러리 설치
6. DB 연결
    1. DB 연결 문자열(ConnectionString) : DB 종류마다 연결 문자열 포맷 다름. 무조건 있어야 함
    2. 쿼리 : 실행할 쿼리(보통 SELECT, INSERT, UPDATE, DELETE)
    3. 데이터 담을 객체 : 리스트 형식
    4. DB 연결 객체(`SqlConnection`) : 연결 문자열 처리하는 객체. DB연결, 연결 해제, 연결 확인...
        - DB 종류별로 MySqlConnection, SqlConnection, OracleConnection...
    5. DB 명령 객체(`SqlCommand`) : 쿼리 컨트롤하는 객체
        - ExecuteReader() : SELECT문 실행, 결과 데이터를 담는 메서드
        - ExecuteScalar() : SELECT문 중 count() 등 함수로 1개의 데이터만 1row/1column 데이터만 가져오는 메서드 
        - ExecuteNonQuery() : INSERT, UPDATE, DELETE문과 같이 transaction이 발생하는 쿼리 실행에 사용하는 메서드
    6. 데이터어댑터(`SqlDataAdapter`) : DB 연결 이후 데이터 처리를 쉽게 도와주는 인터페이스 객체
        - DB 명령 객체처럼 쿼리를 직접 실행할 필요 없음
        - DataTable, DataSet 객체에 Fill() 메서드로 자동으로  채워줌
        - 거의 SELECT문에만 사용
    7. DB 데이터리더(`SqlDataReader`)
        - DataReader : ExecuteReader()로 가져온 데이터 조작하는 객체

7. 실행 결과

    <img src="./Image/wpf0001.png" width="600">

8. MahApps.Metro 방식 다이얼로그 처리

    <img src="./Image/wpf0004.png" width="600">

9. 여기까지 개발한 방식은` 전통적인 C# 윈앱 개발`과 차이 없음

### WPF MVVM
- [디자인 패턴](https://ko.wikipedia.org/wiki/%EC%86%8C%ED%94%84%ED%8A%B8%EC%9B%A8%EC%96%B4_%EB%94%94%EC%9E%90%EC%9D%B8_%ED%8C%A8%ED%84%B4)
- [참고](https://refactoring.guru/ko/design-patterns/classification)
    - 소프트웨어 공학에서 공통적으로 발생하는 문제를 재사용 가능하게 해결한 방식들
    - 반복적으로 되풀이되는 개발디자인의 문제를 해결하도록 맞춤화시킨 양식(템플릿)
    - 여러 디자인패턴 중 아키텍쳐패턴, 그 중 디자인과 개발을 분리해 개발할 수 있는 패턴 준비
        - MV* : MVC, MVP, MVVM...

- **MVC** : Model-View-Controller 패턴
    - 사용자 인터페이스(View)와 비즈니스 로직(Controller, Model)을 분리해서 앱 개발
    - 디자이너에게 최소한의 개발에 참여시킴
    - 개발자는 디자인 고려하지 않고 개발만 할 수 있음
    - 사용자는 Controller에게 요청
    - Controller가 Model에게 데이터 요청
    - Model이 DB에서 데이터 가져와 Controller로 전달
    - Controller는 데이터를 비즈니스로직에 따라서 처리하고 View로 전달
    - View는 데이터를 화면에 뿌려주고, 화면상에서 처리할 것을 모두 마친 후 사용자에게 응답

    - 구조는 복잡, 각 부분별 개발코드는 간결
    - Spring, ASP.NET, django 등 웹개발 아키텍처패턴 표준으로 사용

    <img src="./Image/wpf0002.png" width="600">

- MVP : Model-View-Presenter 패턴
    - MVC 패턴에서 파생됨
    - Presenter : Supervising Controller라고 부름

- **MVVM** - Model-View-ViewModel
    - MVC 패턴에서 파생됨
    - 마크업언어로 GUI 코드 구현하는 아키텍처
    - 사용자는 View로 접근(MVC와 차이점)
    - ViewModel이 Controller 역할(비즈니스로직 처리)
    - Model은 DB요청, 응답
    - 연결방식이 MVC와 다름
    - 전통적인 C# 방식은 사용자가 이벤트 발생시키기 때문에 발생 시기를 바로 알 수 있음
    - MVVM 방식은 C#이 변화를 주시하고 있어야 함. 상태가 바뀌면 변화를 줘야 함

    <img src="./Image/wpf0003.png" width="600">

### WPF MVVM 연습
1. 프로젝트 생성 - [디자인](./day01/Day01Wpf/WpfBasicApp02/View/MainWindow.xaml), [소스](./day01/Day01Wpf/WpfBasicApp02/ViewModel/MainViewModel.cs)
2. WPF DB 바인딩 연습 시 사용한 UI 그대로 복사
3. Model, View, ViewModel 폴더 생성
4. MainWindow.xaml을 View로 이동
5. App.xaml StartupUri 수정
6. Model 폴더 내 Book 클래스 생성
    - INotifyPropertyChanged 인터페이스 : 객체 내의 어떤 속성값이 변경되면 상태를 C#에게 알려주는 기능
    - PropertyChangedEventHandler 이벤트 생성
7. ViewModel 폴더 내 MainViewModel 클래스 생성
    - INotifyPropertyChanged 인터페이스 구현
    - OnPropertyChanged 이벤트핸들러 메서드 구현
8. MainView.xaml에 ViewModel 연결

    ```xml
        xmlns:vm="clr-namespace:WpfBasicApp02.ViewModel"
        DataContext="{DynamicResource MainVM}"
        ...   
    <mah:MetroWindow.Resources>
        <!-- MainViewModel을 가져와서 사용하겠다!! -->
        <vm:MainViewModel x:Key="MainVM" />
    </mah:MetroWindow.Resources>
    ```
9. MainView.xaml 컨트롤에 바인딩 작업
    - 전통적인 C# 방식 : x:Name 사용(비하인드 사용 필요), 마우스이벤트 추가
    
    ```xml
    <!-- UI 컨트롤 구성 -->
    <DataGrid x:Name="GrdBooks" 
            Grid.Row="0" Grid.Column="0" Margin="5" 
            AutoGenerateColumns="False" IsReadOnly="True" 
            MouseDoubleClick="GrdBooks_MouseDoubleClick">
    <DataGrid.Columns>
    <DataGridTextColumn Binding="{Binding Idx}" Header="순번" />
    ```

    - WPF MVVM 바인딩 방식 : 전부 Binding 사용

    ```xml
    <!-- UI 컨트롤 구성 -->
    <DataGrid Grid.Row="0" Grid.Column="0" Margin="5" 
            AutoGenerateColumns="False" IsReadOnly="True"
            ItemsSource="{Binding Books}"
            SelectedItem="{Binding SelectedBook, Mode=TwoWay}">
    <DataGrid.Columns>
    <DataGridTextColumn Binding="{Binding Idx}" Header="순번" />
    ```

10. 실행 결과

    <img src="./Image/wpf0005.png" width="600">

- MVVM 장단점
    - View <-> ViewModel간 데이터 자동 연동
    - 로직 분리로 구조가 명확
    - 팀프로젝트 개발 시 역할 분담이 확실
    - 테스트, 유지보수 쉬움
    - 구조가 복잡. 디버깅 어려움
    - 스케일이 커짐


## 2일차

### MVVM Framework
- MVVM 개발 자체가 어려움. 초기 개발 시 MVVM 템플릿 만드는데 시간 많이 소요됨
- 조금 쉽게 개발하고자 3rd party에서 개발한 MVVM 프레임워크 사용
- 종류
    - `Prism` : MS계열에서 직접 개발. 대규모 앱 개발시 사용. 모듈화 잘 되어있음. 커뮤니티 활발
        - 진입장벽 높음
    - `Caliburn.Micro` : 경량화된 프레임워크. 쉽게 개발할 수 있음. Xaml 바인딩 생략 가능. 커뮤니티 감소 추세
        - [공식사이트](https://caliburnmicro.com/)
        - [GitHub](https://github.com/Caliburn-Micro/Caliburn.Micro)
        - MahApps.Metro에서 사용 중
        - 디버깅 어려움
        - [문제]MahApps.Metro의 메시지박스 다이얼로그 구현이 안 됨
    - `MVVM Light Toolkit` : 가장 가벼운 MVVM 입문용. 쉬운 Command 지원. 개발 종료
        - 확장성이 떨어짐
    - **`CommunityToolkit.Mvvm`** : MS 공식 경량 MVVM. 단순, 빠름. 커뮤니티 활발
        - NotifyPropertyChanged 사용할 필요 없음
        - 모듈 기능이 없음
    - `ReactiveUI` : Rx기반 MVVM. 비동기, 스트림처리 강력. 커뮤니티 활발
        - 최신 기술
        - 진입장벽 높음

### Caliburn.Micro 학습
1. WPF 프로젝트 생성
2. NuGet 패키지 Caliburn.Micro 검색 후 설치
3. App.xaml의 StartupUri 삭제 - [소스](./day02/Day02Wpf/WpfBasicApp01/App.xaml)
4. Models, Views, ViewModels(이름 똑같아야 함) 폴더 생성
5. MainViewModel 클래스 생성 - [소스](./day02/Day02Wpf/WpfBasicApp01/ViewModels/MainViewModel.cs)
    - MainView에 속하는 ViewModel은 반드시 **`MainViewModel`** 이라는 이름 써야함
6. MainWindow.xaml을 View로 이동
7. MainWindow를 MainView로 이름 변경
8. Bootstrapper 클래스 생성, 작성 - [소스](./day02/Day02Wpf/WpfBasicApp01/Bootstrapper.cs)
9. App.xaml에서 Resource 추가
10. MahApps.Metro UI 적용

    <img src="./Image/wpf0006.png" width="600">

### Caliburn.Micro MVVM 연습
1. WPF 프로젝트 생성 - [소스](./day02/Day02Wpf/WpfBasicApp02/ViewModels/MainViewModel.cs)
2. 필요 라이브러리 설치
    - MySQL.Data
    - MahApps.Metro
    - MahApps.Metro.IconPacks
    - Caliburn.Micro
3. Models, Views, ViewModels로 폴더 생성
4. 이전 작업 소스코드 복사, 네임스페이스 변경

    <img src="./Image/wpf0007.png" width="600">

## 3일차

### CommunityToolkit.Mvvm
1. WPF 프로젝트 생성
2. 필요 라이브러리 설치
    - CommunityToolkit.Mvvm
    - MahApps.Metro
    - MahApps.Metro.IconPacks
3. Models, Views, ViewModels 폴더 생성
4. MainWindow.xaml 삭제
5. App.xaml StartupUrl 삭제
6. Views/MainView.xaml 생성
7. ViewModels/MainViewModel.cs 생성
8. App.xaml Startup 이벤트 추가
    - App.xaml.cs 로직 추가
9. App.xaml MahApps.Metro 관련 리소스 추가
10. MainView MetroWindow로 변경

    <img src="./Image/wpf0008.png" width="600">

### Log 라이브러리
- 개발한 앱, 솔루션의 현재상태를 계속 모니터링하는 기능
- Log 사용법
    - 직접 코딩 방식
    - 로그 라이브러리 사용 방식
- Log 라이브러리
    - NLog : 가볍고 쉬움. 빠름. 데스크탑 개발 시 사용
    - Serilog : 어려움. 빠름. 웹개발 시 사용
    - Log4net : Java의 로그를 .NET으로 이전. 느림. 웹개발 시 사용
    - ZLogger : 제일 최신(2021). 초고속. 게임서버 개발 시 사용

### NLog 라이브러리 사용
1. NuGet 패키지 > NLog, NLog.Schema 설치
2. 새 항목 >  XML 파일 > NLog.config 생성
3. Info < Debug < Warn < Error < Fatal
4. NLog.config를 출력 디렉토리로 복사
5. Debug, Trace는 출력이 안 됨
6. Info, Warn, Error, Fatal 사용 권장

    <img src="./Image/wpf0009.png" width="600">

### DB연결 CRUD 연습
1. WPF 프로젝트 생성
2. NuGet 패키지 필요 라이브러리 설치
    - CommunityToolkit.Mvvm
    - MahApps.Metro / MahApps.Metro.IconPacks
    - MySql.Data
    - NLog
3. Model, Views, ViewModels 폴더 생성
4. App.xaml 초기화 작업
5. MainView.xaml, MainViewModel 메인화면 MVVM 작업
    - 메뉴 작업
    - ContentControl 추가
6. 하위 사용자컨트롤 작업
    - BookGenre(View, ViewModel)
    - Books(View, ViewModel)
7. Models > Genre(DivisionTbl) 모델 작업
8. BookGenreViewModel DB처리

    https://github.com/user-attachments/assets/85c36757-55f1-42b2-8e41-ea97f3f61947

## 4일차

#### DB연결 CRUD 연습 시 추가 필요 사항
- [ ] NLog로 각 기능 동작 시 로그 남기기. 공통화 작업
- [ ] DB 쿼리 모델로 이전
- [ ] 연결문자열 Common으로 이전
- [ ] MahApps.Metro 메시지 형태로 변경

## 5일차

## 6일차

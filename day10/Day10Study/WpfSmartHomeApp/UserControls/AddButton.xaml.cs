using System.Windows;
using System.Windows.Controls;

namespace WpfSmartHomeApp.UserControls
{
    /// <summary>
    /// AddButton.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AddButton : UserControl
    {
        public AddButton()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(AddButton));
    }
}

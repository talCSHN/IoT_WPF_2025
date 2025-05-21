using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfSmartHomeApp.UserControls
{
    /// <summary>
    /// Card.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Card : UserControl
    {
        public Card()
        {
            InitializeComponent();
        }

        // 원래 없는 속성을 사용자가 추가하는 방법
        // 의존 속성(Dependency Property)
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(Card));

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(Card));

        public bool IsHorizontal
        {
            get { return (bool)GetValue(IsHorizontalProperty); }
            set { SetValue(IsHorizontalProperty, value); }
        }

        public static readonly DependencyProperty IsHorizontalProperty =
            DependencyProperty.Register("IsHorizontal", typeof(string), typeof(Card));

        public ImageSource ImageOn
        {
            get { return (ImageSource)GetValue(ImageOnProperty); }
            set { SetValue(ImageOnProperty, value); }
        }
        
        public static readonly DependencyProperty ImageOnProperty =
            DependencyProperty.Register("ImageOn", typeof(ImageSource), typeof(Card));

        public ImageSource ImageOff
        {
            get { return (ImageSource)GetValue(ImageOffProperty); }
            set { SetValue(ImageOffProperty, value); }
        }

        public static readonly DependencyProperty ImageOffProperty =
            DependencyProperty.Register("ImageOff", typeof(ImageSource), typeof(Card));
    }
}

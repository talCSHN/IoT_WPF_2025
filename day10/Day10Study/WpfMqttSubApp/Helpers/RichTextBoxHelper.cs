using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;

namespace WpfMqttSubApp.Helpers
{
    // RichTextBox를 MVVM에서 데이터 바인딩하려면 RichTextBoxHelper 클래스의 BindableDocument 속성을 추가적으로 만들어야함
    // 추후 분석 필요
    public static class RichTextBoxHelper
    {
        // 바인딩할 문자열 프로퍼티, BindableDocument
        public static readonly DependencyProperty BindableDocumentProperty =
            DependencyProperty.RegisterAttached(
                "BindableDocument",
                typeof(string),
                typeof(RichTextBoxHelper),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBindableDocumentChanged));

        // 속성 Get 처리
        public static string GetBindableDocument(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableDocumentProperty);
        }
        // 속성 Set 처리
        public static void SetBindableDocument(DependencyObject obj, string value)
        {
            obj.SetValue(BindableDocumentProperty, value);
        }
        // 속성값 변경되었을 때 이벤트 처리
        private static void OnBindableDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RichTextBox richTextBox)
            {
                // 기존 문서 클리어
                richTextBox.Document.Blocks.Clear();
                // 새 문자열을 포함하는 Paragraph 추가
                richTextBox.Document.Blocks.Add(new Paragraph(new Run(e.NewValue as string ?? string.Empty)));
            }
        }
    }
}

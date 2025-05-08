using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBasicApp02.Model
{
    public class Book : INotifyPropertyChanged
    {
        public int Idx {  get; set; }
        public string Division { get; set; }
        public string DNames { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Price { get; set; }

        // 위의 여덟 개의 값이 기존 상태에서 변경되면 발생하는 이벤트
        public event PropertyChangedEventHandler? PropertyChanged;  // 사용자의 클릭 등으로 발생하는 이벤트가 아님
    }
}

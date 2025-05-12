using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBookRentalShop01.ViewModels
{
    class BooksViewModel : ObservableObject
    {
        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
        public BooksViewModel()
        {
            Message = "책 관리 화면";
        }
    }
}

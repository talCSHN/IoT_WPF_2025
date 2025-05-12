using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBookRentalShop01.Models
{
    public class Genre : ObservableObject
    {
        private string _division;
        private string _names;

        public string Division { 
            get => _division; 
            set => SetProperty(ref _division, value);
        }
        public string Names { 
            get => _names; 
            set => SetProperty(ref _names, value);
        }
    }
}

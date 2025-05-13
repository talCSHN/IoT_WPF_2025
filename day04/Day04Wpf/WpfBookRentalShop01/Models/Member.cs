using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBookRentalShop01.Models
{
    public class Member : ObservableObject
    {
        private int _idx;
        private string _names;
        private string _levels;
        private string _addr;
        private string _mobile;
        private string _email;

        public int Idx { 
            get => _idx; 
            set => SetProperty(ref _idx, value);
        }
        public string Names { 
            get => _names; 
            set => SetProperty(ref _names, value);
        }
        public string Levels { 
            get => _levels; 
            set => SetProperty(ref _levels, value);
        }
        public string Addr { 
            get => _addr; 
            set => SetProperty(ref _addr, value);
        }
        public string Mobile { 
            get => _mobile;
            set => SetProperty(ref _mobile, value);
        }
        public string Email { 
            get => _email; 
            set => SetProperty(ref _email, value);
        }
    }
}

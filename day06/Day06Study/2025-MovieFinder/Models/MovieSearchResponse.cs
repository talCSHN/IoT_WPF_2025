using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2025_MovieFinder.Models
{
    public class MovieSearchResponse : ObservableObject
    {
        private int _page;
        private List<MovieItem> _results;
        private int _total_pages;
        private int _total_results;

        public int Page { 
            get => _page; 
            set => SetProperty(ref _page, value); 
        }
        public List<MovieItem> Results { 
            get => _results; 
            set => SetProperty(ref _results, value);
        }
        public int Total_pages { 
            get => _total_pages; 
            set => SetProperty(ref _total_pages, value);
        }
        public int Total_results { 
            get => _total_results; 
            set => SetProperty(ref _total_results, value); 
        }
    }
}

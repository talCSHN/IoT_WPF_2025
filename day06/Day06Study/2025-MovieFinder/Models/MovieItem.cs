using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2025_MovieFinder.Models
{
    public class MovieItem : ObservableObject
    {
        /*
         {
          "adult": false,
          "backdrop_path": "/l33oR0mnvf20avWyIMxW02EtQxn.jpg",
          "genre_ids": [12, 18, 878],
          "id": 157336,
          "original_language": "en",
          "original_title": "Interstellar",
          "overview": "세계 각국의 정부와...",
          "popularity": 36.3951,
          "poster_path": "/evoEi8SBSvIIEveM3V6nCJ6vKj8.jpg",
          "release_date": "2014-11-05",
          "title": "인터스텔라",
          "video": false,
          "vote_average": 8.455,
          "vote_count": 37092
        },
        */
        private bool _adult;
        private string _backdrop_path;  // 사용 안 함
        private List<int> _genre_ids;      // 사용 안 함
        private int _id;
        private string _original_language;
        private string _original_title;
        private string _overview;
        private double _popularity;
        private string _poster_path;
        private DateTime _release_date;
        private string _title;
        private bool _video;
        private double _vote_average;
        private int _vote_count;

        public bool Adult { 
            get => _adult; 
            set => SetProperty(ref _adult, value);
        }
        public string Backdrop_path { 
            get => _backdrop_path; 
            set => SetProperty(ref _backdrop_path, value);
        }
        public List<int> Genre_ids { 
            get => _genre_ids; 
            set => SetProperty(ref _genre_ids, value);
        }
        public int Id { 
            get => _id; 
            set => SetProperty(ref _id, value); 
        }
        public string Original_language { 
            get => _original_language; 
            set => SetProperty(ref _original_language, value); 
        }
        public string Original_title { 
            get => _original_title; 
            set => SetProperty(ref _original_title, value); 
        }
        public string Overview { 
            get => _overview; 
            set => SetProperty(ref _overview, value); 
        }
        public double Popularity { 
            get => _popularity; 
            set => SetProperty(ref _popularity, value); 
        }
        public string Poster_path { 
            get => _poster_path; 
            set => SetProperty(ref _poster_path, value); 
        }
        public DateTime Release_date { 
            get => _release_date; 
            set => SetProperty(ref _release_date, value); 
        }
        public string Title { 
            get => _title; 
            set => SetProperty(ref _title, value);  
        }
        public bool Video { 
            get => _video; 
            set => SetProperty(ref _video, value);
        }
        public double Vote_average { 
            get => _vote_average; 
            set => SetProperty(ref _vote_average, value); 
        }
        public int Vote_count { 
            get => _vote_count; 
            set => SetProperty(ref _vote_count, value); 
        }
    }
}

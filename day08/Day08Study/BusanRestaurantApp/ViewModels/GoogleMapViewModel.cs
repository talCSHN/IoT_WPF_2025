using BusanRestaurantApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusanRestaurantApp.ViewModels
{
    public class GoogleMapViewModel : ObservableObject
    {
        private BusanItem _selectedIndividualItem;

        public BusanItem SelectedIndividualItem
        {
            get => _selectedIndividualItem;
            set { 
                SetProperty(ref _selectedIndividualItem, value);
                RestaurantLocation = $"https://google.com/maps/place/{SelectedIndividualItem.Lat},{SelectedIndividualItem.Lng}";
            }
        }

        private string _restaurantLocation;


        public string RestaurantLocation
        {
            get => _restaurantLocation;
            set => SetProperty(ref _restaurantLocation, value);
        }
        public GoogleMapViewModel()
        {
            RestaurantLocation = string.Empty;
        }

        

    }
}

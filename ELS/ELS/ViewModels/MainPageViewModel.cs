using ELS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ELS.ViewModels
{
    class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Command OpenGoogleMap { get; set; }
        public MainPageViewModel()
        {
            Locations = new ObservableCollection<Location>();
            // kind of seed
            for (int i = 1; i < 6; i++)
            {
                Locations.Add(new Location
                {
                    Id = i,
                    MapLink = "https://google.com",
                    Name = $"Ward {i}",
                    SubName = $"Ward {(i + 4)}"
                });
                App.Database.SaveItemAsync(Locations[i -1]).GetAwaiter().GetResult();
            }


            OpenGoogleMap = new Command(GetMapLink);
            var data = App.Database.GetItemsAsync().GetAwaiter().GetResult();
            foreach (var item in data)
            {
                Locations.Add(item);
            }
        }

        private void GetMapLink()
        {
            throw new NotImplementedException();
        }

        private ObservableCollection<Location> _locations;
        public ObservableCollection<Location> Locations
        {
            get { return _locations; }
            set
            {
                _locations = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Locations)));
            }
        }


        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                var args = new PropertyChangedEventArgs(nameof(Name));
                PropertyChanged?.Invoke(this, args);
            }
        }

        private Location _selectedLocation;

        public Location SelectedLocation
        {
            get { return _selectedLocation; }
            set 
            { 
                _selectedLocation = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedLocation)));
            }
        }

    }
}

using ELS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ELS
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {

        #region INotify 
        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Data From Json comes to these props
        public ObservableCollection<District> Districts { get; set; }
        public ObservableCollection<SubDistrict> SubDistricts { get; set; }
        public ObservableCollection<House> Houses { get; set; }


        private District _selectedDistrict;

        public District SelectedDistrict
        {
            get { return _selectedDistrict; }
            set { _selectedDistrict = value; }
        }

        private SubDistrict _selectedSubDistrict;

        public SubDistrict SelectedSubDistrict
        {
            get { return _selectedSubDistrict; }
            set { _selectedSubDistrict = value; }
        }

        private House _selectedHouse;

        public House SelectedHouse
        {
            get 
            {
                return _selectedHouse; 
            }
            set 
            {
                if (SelectedHouse == null)
                {
                    CanClick = false;
                }
                _selectedHouse = value; 
            }
        }


        #endregion

        #region Error Handling and Reactivity
        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(nameof(Message)); }
        }

        // TODO: this guy is never been used.
        private string _errMessage;

        public string ErrMessage
        {
            get { return _errMessage; }
            set { _errMessage = value; OnPropertyChanged(nameof(ErrMessage)); }
        }

        private bool _canClick;

        public bool CanClick
        {
            get { return _canClick; }
            set { _canClick = value; OnPropertyChanged(nameof(CanClick)); }
        }

        private string _mapUrl;

        public string MapUrl
        {
            get { return _mapUrl; }
            set { _mapUrl = value; }
        }


        #endregion

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            PopulateDistricts();
            CanClick = false;
        }

        // Get all districts from Json
        private void PopulateDistricts()
        {
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("ELS.Data.District.json");

            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();

                List<District> districts = JsonConvert.DeserializeObject<List<District>>(json);
                Districts = new ObservableCollection<District>(districts);
                districtPicker.ItemsSource = Districts;
            }
        }

        // Get Sub-District by selected District from Json
        private void PopulateSubDistricts(int districtId)
        {
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("ELS.Data.SubDistrict.json");

            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();

                List<SubDistrict> subDistricts = JsonConvert.DeserializeObject<List<SubDistrict>>(json);
                SubDistricts = new ObservableCollection<SubDistrict>(subDistricts);
                subDistrictPicker.ItemsSource = SubDistricts.Where(x => x.DistrictId == districtId).ToList();
            }
        }

        // Get Houses by selected Sub-District from Json
        private void PopulateHouses(int subDistrictId)
        {
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("ELS.Data.House.json");

            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();

                List<House> houses = JsonConvert.DeserializeObject<List<House>>(json);
                Houses = new ObservableCollection<House>(houses);
                var dataSource = Houses.Where(x => x.SubDistrictId == subDistrictId).ToList();
                if (dataSource.Count() == 0)
                {
                    Message = $"There is no available houses in {SelectedSubDistrict.Name}";
                }
                else
                {
                    Message = "Select House";
                }
                housePicker.ItemsSource = dataSource;
            }
        }

        // Selected District Event
        private void districtPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateSubDistricts(SelectedDistrict.Id);
            // TODO: there is a bug here.
            // when i select district for the second time, button is enabled
        }

        // Selected Sub-District Event
        private void subDistrictPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedSubDistrict == null)
            {
                housePicker.ItemsSource = null;
            }
            else
            {
                PopulateHouses(SelectedSubDistrict.Id);
            }
        }

        // Selected House Event
        private void housePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedHouse != null)
            {
                CanClick = true;
            }
        }

        // Navigate to Google Map by Selected House
        private void Button_Clicked(object sender, EventArgs e)
        {
            if (SelectedHouse == null)
            {
                ErrMessage = "Please select a specific house";
            }
            else
            {
                MapUrl = SelectedHouse.Location;
                Launcher.OpenAsync(MapUrl).Wait();
            }
        }
    }
}
